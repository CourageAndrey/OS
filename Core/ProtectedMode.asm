;##############################;
;#                            #;
;# Переход в защищённый режим #;
;#                            #;
;##############################;

;=============;
;= Константы =;
;=============;
	; !раскомментировать эту строку при компиляции этого файла отдельно от остальных
	; include "BIOS.asm"

	protectedModeStackBaseAddress	equ 0x8000 ; начальный адрес памяти стека
	protectedModeCodeBaseAddress	equ 0x100 ; начальный адрес памяти кода
	protectedModeCodeSize			equ protectedModeCodeEnd - protectedModeCodeBaseAddress

	protectedModeSelectorCode32	equ 0x08
	protectedModeSelectorData	equ 0x10

	protectedConstControlRegisterPort92	equ 0x92
	protectedConstBitA20Enable			equ 2
	protectedConstBitProtectedMode		equ 1
	protectedConstRealTimeClockPort		equ 0x70
	protectedConstBitsRtcDisable		equ 0x80

	messageProtectedModeEntered db "Protected mode is on."
	messageProtectedModeEnteredEnd:


;===========;
;= Макросы =;
;===========;
macro defineCodeDescriptor32 baseAddress, sizeLimit
{
	DefineSegmentDescriptor32 baseAddress, sizeLimit, DescriptorBitGranularityPage, DescriptorBitSize32, 0, 0, DescriptorBitPresent, DescriptorPrivilege0, DescriptorObjectSystem, DescriptorTypeCode, 0, DescriptorCodeReadable, 0
}

macro defineDataDescriptor32 baseAddress, sizeLimit
{
	DefineSegmentDescriptor32 baseAddress, sizeLimit, DescriptorBitGranularityPage, DescriptorBitSize32, 0, 0, DescriptorBitPresent, DescriptorPrivilege0, DescriptorObjectSystem, DescriptorTypeData, DescriptorDataAddressesUsual, DescriptorDataReadWrite, 0
}

macro defineStackDescriptor32 baseAddress, sizeLimit
{
	DefineSegmentDescriptor32 baseAddress, sizeLimit, DescriptorBitGranularityPage, DescriptorBitSize32, 0, 0, DescriptorBitPresent, DescriptorPrivilege0, DescriptorObjectSystem, DescriptorTypeData, DescriptorDataAddressesStack, DescriptorDataReadWrite, 0
}

;===============;
;= Точка входа =;
;===============;
ProtectedModeEntry:
	; открытие линии A20  (для включения 32-битной адресации)
	in al, protectedConstControlRegisterPort92
	or al, protectedConstBitA20Enable
	out protectedConstControlRegisterPort92, al

	; вычисление линейного адреса точки входа в защищённый режим
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, protectedModeEntryPoint
	mov [protectedModeEntryOffset], eax ; protectedModeEntryOffset = CS * 16 + protectedModeEntryPoint

	; вычисление линейного адреса GDT
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, GDT
	mov dword [GDTR + 2], eax
	lgdt fword [GDTR] ; загрузка регистра GDTR

	; запрет прерываний
	cli
	; отключение прерываний от часов реального времени
	in al, protectedConstRealTimeClockPort
	or al, protectedConstBitsRtcDisable
	out protectedConstRealTimeClockPort, al
	; включение защищённого режима через регистр CR0
	mov eax, cr0
	or al, protectedConstBitProtectedMode
	mov cr0, eax

	; загрузка нового селектора в CS
	db 0x66, 0xEA ; JMP FAR
	protectedModeEntryOffset dd protectedModeEntryPoint
	dw protectedModeSelectorCode32

	; глобальная таблица дескрипторов
	align 8
GDT:
	protectedDescriptorNull		db SegmentDescriptorLength dup (0)
	protectedDescriptorCode32:	defineCodeDescriptor32 0x0, 0xFFFFFFFF
	protectedDescriptorData32:	defineDataDescriptor32 0x0, 0xFFFFFFFF
	protectedDescriptorCode64	db 0x0, 0x0, 0x0, 0x0, 0x0, 10011000b, 00100000b, 0x0
	protectedDescriptorData64	db 0x0, 0x0, 0x0, 0x0, 0x0, 10010000b, 00100000b, 0x0
	GDT_size					equ $-GDT

label GDTR fword
	dw GDT_size-1
	dd ?

use32 ; весь дальнейший код - 32x-разрядный
protectedModeEntryPoint:
	; загрузка сегментных регистров созданными селекторами
	mov ax, protectedModeSelectorData
	mov ds, ax
	mov es, ax
	mov ss, ax
	mov esp, protectedModeStackBaseAddress

	call delta
	delta:
	pop ebx ; в EBX выталкивается адрес строки кода call delta
	add ebx, protectedModeCodeStart-delta ; и прибавляется разница, чтобы EBX смотрел на ProtectedModeCodeStart

	mov esi, ebx ; из памяти по адресу ProtectedModeCodeStart (уже загружена)
	mov edi, protectedModeCodeBaseAddress ; в память по адресу ProtectedModeCodeBaseAddress (где будет выполняться код)
	mov ecx, protectedModeCodeSize ; скопировать весь код между метками ProtectedModeCodeEnd и ProtectedModeCodeStart
	rep movsb

	mov eax, protectedModeCodeBaseAddress
	jmp eax

;------------------------;
;----- Вывод строки -----;
;------------------------;
; * ESI - адрес строки	-;
; * ECX - длина строки	-;
; * BL - координата X	-;
; * BH - координата Y	-;
; * AH - цвет			-;
;------------------------;
protectedProcWriteString:
	push eax
	push ebx
	push ecx
	push edi
	push esi

	; установка адреса видеопамяти
	mov edi, eax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add eax, VideoMemoryAddressProtected
	xchg edi, eax

	; посимвольный вывод
	@@:
		test ecx, ecx
		jz @f
		lodsb
		stosw
		dec ecx
		jmp @b

	@@:
	pop esi
	pop edi
	pop ecx
	pop ebx
	pop eax
	ret

protectedModeCodeStart:
org protectedModeCodeBaseAddress
	; сообщение о входе в защищённый режим
	mov ah, ColorForeLime
	mov esi, messageProtectedModeEntered
	mov ecx, messageProtectedModeEnteredEnd-messageProtectedModeEntered
	mov ebx, 0x0200 ; начало третьей строки
	call protectedProcWriteString

	include "LongMode.asm"
protectedModeCodeEnd:
;##############################;
;#                            #;
;# ������� � ���������� ����� #;
;#                            #;
;##############################;

;=============;
;= ��������� =;
;=============;
	; !����������������� ��� ������ ��� ���������� ����� ����� �������� �� ���������
	; include "BIOS.asm"

	protectedModeStackBaseAddress	equ 0x8000 ; ��������� ����� ������ �����
	protectedModeCodeBaseAddress	equ 0x100 ; ��������� ����� ������ ����
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
;= ������� =;
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
;= ����� ����� =;
;===============;
ProtectedModeEntry:
	; �������� ����� A20  (��� ��������� 32-������ ���������)
	in al, protectedConstControlRegisterPort92
	or al, protectedConstBitA20Enable
	out protectedConstControlRegisterPort92, al

	; ���������� ��������� ������ ����� ����� � ���������� �����
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, protectedModeEntryPoint
	mov [protectedModeEntryOffset], eax ; protectedModeEntryOffset = CS * 16 + protectedModeEntryPoint

	; ���������� ��������� ������ GDT
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, GDT
	mov dword [GDTR + 2], eax
	lgdt fword [GDTR] ; �������� �������� GDTR

	; ������ ����������
	cli
	; ���������� ���������� �� ����� ��������� �������
	in al, protectedConstRealTimeClockPort
	or al, protectedConstBitsRtcDisable
	out protectedConstRealTimeClockPort, al
	; ��������� ����������� ������ ����� ������� CR0
	mov eax, cr0
	or al, protectedConstBitProtectedMode
	mov cr0, eax

	; �������� ������ ��������� � CS
	db 0x66, 0xEA ; JMP FAR
	protectedModeEntryOffset dd protectedModeEntryPoint
	dw protectedModeSelectorCode32

	; ���������� ������� ������������
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

use32 ; ���� ���������� ��� - 32x-���������
protectedModeEntryPoint:
	; �������� ���������� ��������� ���������� �����������
	mov ax, protectedModeSelectorData
	mov ds, ax
	mov es, ax
	mov ss, ax
	mov esp, protectedModeStackBaseAddress

	call delta
	delta:
	pop ebx ; � EBX ������������� ����� ������ ���� call delta
	add ebx, protectedModeCodeStart-delta ; � ������������ �������, ����� EBX ������� �� ProtectedModeCodeStart

	mov esi, ebx ; �� ������ �� ������ ProtectedModeCodeStart (��� ���������)
	mov edi, protectedModeCodeBaseAddress ; � ������ �� ������ ProtectedModeCodeBaseAddress (��� ����� ����������� ���)
	mov ecx, protectedModeCodeSize ; ����������� ���� ��� ����� ������� ProtectedModeCodeEnd � ProtectedModeCodeStart
	rep movsb

	mov eax, protectedModeCodeBaseAddress
	jmp eax

;------------------------;
;----- ����� ������ -----;
;------------------------;
; * ESI - ����� ������	-;
; * ECX - ����� ������	-;
; * BL - ���������� X	-;
; * BH - ���������� Y	-;
; * AH - ����			-;
;------------------------;
protectedProcWriteString:
	push eax
	push ebx
	push ecx
	push edi
	push esi

	; ��������� ������ �����������
	mov edi, eax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add eax, VideoMemoryAddressProtected
	xchg edi, eax

	; ������������ �����
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
	; ��������� � ����� � ���������� �����
	mov ah, ColorForeLime
	mov esi, messageProtectedModeEntered
	mov ecx, messageProtectedModeEnteredEnd-messageProtectedModeEntered
	mov ebx, 0x0200 ; ������ ������� ������
	call protectedProcWriteString

	include "LongMode.asm"
protectedModeCodeEnd:
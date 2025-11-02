;########################;
;#                      #;
;# Enter Protected Mode #;
;#                      #;
;########################;

;=============;
;= Constants =;
;=============;
	; ! Not duplicating definitions because by this point BIOS.asm is already included
	; include "BIOS.asm"

	protectedModeStackBaseAddress	equ 0x8000 ; base address of stack segment
	protectedModeCodeBaseAddress	equ 0x100 ; base address of code segment
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
	messageCpuidSupported db "CPUID is supported."
	messageCpuidSupportedEnd:
	messageCpuidNotSupported db "CPUID is not supported. Exiting..."
	messageCpuidNotSupportedEnd:
	messageCpuidExtendedSupported db "Extended CPUID instruction are supported."
	messageCpuidExtendedSupportedEnd:
	messageCpuidExtendedNotSupported db "Extended CPUID instruction are not supported. Exiting..."
	messageCpuidExtendedNotSupportedEnd:
	messageLongModeNotSupported db "x64 mode is not supported. Exiting..."
	messageLongModeNotSupportedEnd:

;==========;
;= Macros =;
;==========;
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
;= Entry Point =;
;===============;
ProtectedModeEntry:
	; enable A20 gate (for 32-bit addressing)
	in al, protectedConstControlRegisterPort92
	or al, protectedConstBitA20Enable
	out protectedConstControlRegisterPort92, al

	; calculate absolute address of entry point for protected mode
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, protectedModeEntryPoint
	mov [protectedModeEntryOffset], eax ; protectedModeEntryOffset = CS * 16 + protectedModeEntryPoint

	; calculate absolute address of GDT
	xor eax, eax
	mov ax, cs
	shl eax, 4
	add eax, GDT
	mov dword [GDTR + 2], eax
	lgdt fword [GDTR] ; load GDTR register

	; disable interrupts
	cli
	; disable interrupts from real-time clock
	in al, protectedConstRealTimeClockPort
	or al, protectedConstBitsRtcDisable
	out protectedConstRealTimeClockPort, al
	; set protected mode bit in CR0
	mov eax, cr0
	or al, protectedConstBitProtectedMode
	mov cr0, eax

	; perform far jump to load CS
	db 0x66, 0xEA ; JMP FAR
	protectedModeEntryOffset dd protectedModeEntryPoint
	dw protectedModeSelectorCode32

	; Global Descriptor Table
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

use32 ; all following code is 32-bit
protectedModeEntryPoint:
	; load all segment registers for protected mode
	mov ax, protectedModeSelectorData
	mov ds, ax
	mov es, ax
	mov ss, ax
	mov esp, protectedModeStackBaseAddress

	call delta
	delta:
	pop ebx ; EBX now contains address of "call delta" instruction
	add ebx, protectedModeCodeStart-delta ; adjust so EBX points to ProtectedModeCodeStart

	mov esi, ebx ; source is ProtectedModeCodeStart (its current location)
	mov edi, protectedModeCodeBaseAddress ; destination is ProtectedModeCodeBaseAddress (where we want to relocate code)
	mov ecx, protectedModeCodeSize ; copy entire block between ProtectedModeCodeEnd and ProtectedModeCodeStart
	rep movsb

	mov eax, protectedModeCodeBaseAddress
	jmp eax

;----------------------------;
;- Write string             -;
; * ESI - string address    -;
; * ECX - string length     -;
; * BL - X coordinate       -;
; * BH - Y coordinate       -;
; * AH - color              -;
;----------------------------;
protectedProcWriteString:
	push eax
	push ebx
	push ecx
	push edi
	push esi

	; calculate video memory address
	mov edi, eax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add eax, VideoMemoryAddressProtected
	xchg edi, eax

	; output the string
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
	; display message that we're in protected mode
	mov ah, ColorForeLime
	mov esi, messageProtectedModeEntered
	mov ecx, messageProtectedModeEnteredEnd-messageProtectedModeEntered
	mov ebx, 0x0200 ; third line of screen
	call protectedProcWriteString

	; execute CPUID with EAX=0 (request vendor string)
	xor eax, eax
	cpuid ; Execute CPUID - if unsupported would cause #UD, but that won't happen on Pentium+

	; display message that CPUID is supported
	mov ah, ColorForeLime
	mov esi, messageCpuidSupported
	mov ecx, messageCpuidSupportedEnd - messageCpuidSupported
	mov ebx, 0x0300 ; fourth line of screen
	call protectedProcWriteString

	; check if CPUID instruction is supported
	pushfd				; push EFLAGS
	pop eax				; pop into EAX
	mov ebx, eax		; save original EFLAGS
	xor eax, 0x200000	; flip bit 21 (ID bit)
	push eax			; push modified EFLAGS
	popfd				; pop back to EFLAGS
	pushfd				; push EFLAGS again
	pop eax				; pop back to EAX
	xor eax, ebx		; compare with original
	and eax, 0x200000	; check if bit 21 changed
	push ebx			; restore original EFLAGS
	popfd

	; check result
	test eax, eax
	jnz @cpuidSupported ; if bit didn't change, CPUID not supported

	mov esi, messageCpuidNotSupported
	mov ecx, messageCpuidNotSupportedEnd - messageCpuidNotSupported
	mov ebx, 0x0300 ; fourth line of screen
@protectedModeError:
	mov ah, ColorForeRed
	call protectedProcWriteString

	cli
	jmp $ ; HALT

	@cpuidSupported:
	; check if extended CPUID functions are supported
	mov eax, 0x80000000
	cpuid
	cmp eax, 0x80000001
	jnb @cpuidExtendedSupported

	mov esi, messageCpuidExtendedNotSupported
	mov ecx, messageCpuidExtendedNotSupportedEnd - messageCpuidExtendedNotSupported
	mov ebx, 0x0400 ; fifth line of screen
	jmp @protectedModeError

	@cpuidExtendedSupported:
	; display message that extended CPUID instructions are supported
	mov ah, ColorForeLime
	mov esi, messageCpuidExtendedSupported
	mov ecx, messageCpuidExtendedSupportedEnd - messageCpuidExtendedSupported
	mov ebx, 0x0400 ; fifth line of screen
	call protectedProcWriteString

	; check if long mode is supported
	mov eax, 0x80000001
	cpuid
	; Check bit 29 of EDX (LM bit) for Long Mode support
	test edx, (1 shl 29)
	jz @longModeNotSupported ; if bit is clear, Long Mode not supported

	; Long Mode is supported, continue execution
	jmp @longModeSupported

	@longModeNotSupported:
	mov ah, ColorForeRed
	mov esi, messageLongModeNotSupported
	mov ecx, messageLongModeNotSupportedEnd - messageLongModeNotSupported
	mov ebx, 0x0500 ; sixth line of screen
	call protectedProcWriteString

	cli
	jmp $ ; HALT

	@longModeSupported:

	cli
	jmp $ ; HALT

	include "LongMode.asm"
protectedModeCodeEnd:
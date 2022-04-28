;###################################;
;#                                 #;
;# Переход в режим длинных адресов #;
;#                                 #;
;###################################;

;=============;
;= Константы =;
;=============;
	longModeSelectorCode	equ SegmentDescriptorLength*3
	longModeSelectorData	equ SegmentDescriptorLength*4

	longModeAddressPML4	equ 0x1FC000
	longModeAddressPDPE	equ 0x1FD000
	longModeAddressPDE	equ 0x1FE000

	msrFuncEFER	equ 0xC0000080
	bitCR4_PAE	equ 5
	bitPG		equ 31
	bitEFER_LME	equ 8

	bitPresentOrWrite	equ 3
	bitPDE_PS			equ 010000000b

;===============;
;= Точка входа =;
;===============;
	mov eax, cr4
	bts eax, bitCR4_PAE
	mov cr4, eax

	mov dword [longModeAddressPDE],		bitPDE_PS or bitPresentOrWrite
	mov dword [longModeAddressPDE+4],	0
	mov dword [longModeAddressPDPE],	longModeAddressPDE or bitPresentOrWrite
	mov dword [longModeAddressPDPE+4],	0
	mov dword [longModeAddressPML4],	longModeAddressPDPE or bitPresentOrWrite
	mov dword [longModeAddressPML4+4],	0

	mov eax, longModeAddressPML4
	mov cr3, eax

	mov ecx, msrFuncEFER
	rdmsr
	bts eax, bitEFER_LME
	wrmsr

	mov eax, cr0
	bts eax, bitPG
	mov cr0, eax

	jmp longModeSelectorCode:LongModeEntry

use64 ; весь дальнейший код - 64x-разрядный

LongModeEntry:
	; перезагрузка всех сегментных регистров
	mov ax, longModeSelectorData
	mov ds, ax
	mov es, ax
	mov fs, ax
	mov gs, ax
	;mov ax, longModeSelectorStack
	;mov ss, ax

	; предварительная очистка всех регистров
	xor rax, rax
	mov rbx, rax
	mov rcx, rax
	mov rdx, rax
	mov rsi, rax
	mov rdi, rax
	;mov rsp, rax
	;mov rbp, rax
	mov r8,  rax
	mov r9,  rax
	mov r10, rax
	mov r11, rax
	mov r12, rax
	mov r13, rax
	mov r14, rax
	mov r15, rax

include 'UserCode.ASM'
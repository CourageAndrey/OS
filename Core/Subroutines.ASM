;########################################;
;#                                      #;
;# ������������, ���������� � long mode #;
;#                                      #;
;########################################;
	; !����������������� ��� ������ ��� ���������� ����� ����� �������� �� ���������
	; include "BIOS.asm"
	; use64

;------------------------;
;----- ����� ������ -----;
;------------------------;
; * RSI - ����� ������	-;
; * RCX - ����� ������	-;
; * BL - ���������� X	-;
; * BH - ���������� Y	-;
; * AH - ����			-;
;------------------------;
procWriteString:
	push rax rbx rcx rdi rsi

	; ��������� ������ �����������
	mov rdi, rax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add rax, VideoMemoryAddressProtected
	xchg rdi, rax

	; ������������ �����
	@@:
		test rcx, rcx
		jz @f
		lodsb
		stosw
		dec rcx
		jmp @b

	@@:
	pop rsi rdi rcx rbx rax
	ret
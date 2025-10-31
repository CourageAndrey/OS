;######################################;
;#                                    #;
;# Subroutines available in long mode #;
;#                                    #;
;######################################;
	; ! Not duplicating definitions because by this point BIOS.asm is already included
	; include "BIOS.asm"
	; use64

;----------------------------;
;- Write string             -;
;----------------------------;
; * RSI - string address    -;
; * RCX - string length     -;
; * BL - X coordinate       -;
; * BH - Y coordinate       -;
; * AH - color              -;
;----------------------------;
procWriteString:
	push rax rbx rcx rdi rsi

	; calculate video memory address
	mov rdi, rax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add rax, VideoMemoryAddressProtected
	xchg rdi, rax

	; output the string
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
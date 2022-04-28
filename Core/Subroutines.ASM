;########################################;
;#                                      #;
;# Подпрограммы, работающие в long mode #;
;#                                      #;
;########################################;
	; !раскомментировать эти строки при компиляции этого файла отдельно от остальных
	; include "BIOS.asm"
	; use64

;------------------------;
;----- Вывод строки -----;
;------------------------;
; * RSI - адрес строки	-;
; * RCX - длина строки	-;
; * BL - координата X	-;
; * BH - координата Y	-;
; * AH - цвет			-;
;------------------------;
procWriteString:
	push rax rbx rcx rdi rsi

	; установка адреса видеопамяти
	mov rdi, rax
	mov al, 80
	mul bh
	xor bh, bh
	add ax, bx
	shl ax, 1
	add rax, VideoMemoryAddressProtected
	xchg rdi, rax

	; посимвольный вывод
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
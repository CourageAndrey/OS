;################;
;#              #;
;# Тестовый код #;
;#              #;
;################;
	; !раскомментировать эти строки при компил€ции этого файла отдельно от остальных
	; include "BIOS.asm"
	; use64

	; сообщение о входе в режим длинных адресов
	mov ah, ColorForeLime
	mov rsi, messageLongModeEntered
	mov rcx, messageLongModeEnteredEnd-messageLongModeEntered
	mov rbx, 0x0300 ; начало четвёртой строки
	call procWriteString

	; зацикливание
	jmp $

messageLongModeEntered db "Long mode is on."
messageLongModeEnteredEnd:

include 'Subroutines.ASM'
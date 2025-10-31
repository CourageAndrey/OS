;################;
;#              #;
;# User Code    #;
;#              #;
;################;
	; ! Not duplicating definitions because by this point BIOS.asm is already included
	; include "BIOS.asm"
	; use64

	; display message that we're in long mode
	mov ah, ColorForeLime
	mov rsi, messageLongModeEntered
	mov rcx, messageLongModeEnteredEnd-messageLongModeEntered
	mov rbx, 0x0300 ; fourth line of screen
	call procWriteString

	; infinite loop
	jmp $

messageLongModeEntered db "Long mode is on."
messageLongModeEnteredEnd:

include 'Subroutines.ASM'
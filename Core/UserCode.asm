;################;
;#              #;
;# �������� ��� #;
;#              #;
;################;
	; !����������������� ��� ������ ��� ���������� ����� ����� �������� �� ���������
	; include "BIOS.asm"
	; use64

	; ��������� � ����� � ����� ������� �������
	mov ah, ColorForeLime
	mov rsi, messageLongModeEntered
	mov rcx, messageLongModeEnteredEnd-messageLongModeEntered
	mov rbx, 0x0300 ; ������ ��������� ������
	call procWriteString

	; ������������
	jmp $

messageLongModeEntered db "Long mode is on."
messageLongModeEnteredEnd:

include 'Subroutines.ASM'
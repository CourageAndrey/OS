;#######################;
;#                     #;
;# ��������� ��������� #;
;#                     #;
;#######################;
	StartAddress equ 0x7C00
org StartAddress
	jmp word BootLoaderEntry

;=============;
;= ��������� =;
;=============;
	 ; ! ��� ��� ���� ���� ���������� ������ - ������ � ���������� BIOS.asm �������������� �� �����.
	 include "BIOS.asm"

	 loaderConstSectorPerTrackEnhanced	equ 0xFF	; �������� (���������� �������� �� �������), ����������� �� ��, ��� ������ ����� ������������ � ������� Enhanced Disk Service, � �� ����������
	 loaderConstSectorsToRead			equ 0x01	; ���������� ��������, �������� �� ���� ���
	 loaderConstSectorsReadAttempt		equ 0x03	; ���������� ������� ������ �������
	 loaderConstSectorsOfBootLoader		equ 2		; ! ���������� ��������, ��������� �� ����������� = CEIL(����������������������������� / �������������)
	 loaderConstDiskSegmentMask			equ 111111b	; ����� ��� ����������� ���������� �������� �� �������
	 loaderConstDiskStructureAddress	equ 0x600	; ����� � ������, � ������� ����� ����������� ������ � ����� (������ �� ������� ����� �����)

	 ; ����� ��������� ���������
	 loaderMessageSuccessfullLength 	 equ loaderMessageSuccessfullEnd - loaderMessageSuccessfull
	 loaderMessageDiskErrorLength		 equ loaderMessageDiskErrorEnd - loaderMessageDiskError
	 loaderMessageDiskEnhancedEnabledLength  equ loaderMessageDiskEnhancedEnabledEnd - loaderMessageDiskEnhancedEnabled
	 loaderMessageDiskEnhancedDisabledLength equ loaderMessageDiskEnhancedDisabledEnd - loaderMessageDiskEnhancedDisabled

;==========;
;= ������ =;
;==========;
	 loaderVarDiskId			db ? ; ������������� �����, � �������� �������������� ��������
	 loaderVarSectorPerTrack	dw ? ; ���������� �������� �� ������� -//-
	 loaderVarHeadCount			db ? ; ���������� ������� -//-
	 loaderVarDiskAddressPacket	EddPacket32 ; ����� Enhanced Disk Service, ����������� ����, � �������� �������������� ��������
	 ; ������������ ��������� ������
	 loaderMessageSuccessfull db 'Disk reading has been completed.'
	 loaderMessageSuccessfullEnd:
	 loaderMessageDiskError db 'Error: disk reading has failed! Press any key for reboot...'
	 loaderMessageDiskErrorEnd:
	 loaderMessageDiskEnhancedEnabled db 'Enhanced disk functions are available.'
	 loaderMessageDiskEnhancedEnabledEnd:
	 loaderMessageDiskEnhancedDisabled db 'Enhanced disk functions are disabled.'
	 loaderMessageDiskEnhancedDisabledEnd:

;================;
;= ������������ =;
;================;
;----------------;
;- ������������ -;
;----------------;
loaderProcReboot:
	jmp RebootAddress:0

;-------------------------------;
;- ������ ������� � ���������� -;
;-------------------------------;
loaderProcReadKey:
	push ax
	mov ah, FunctionKeyboardReadkey
	int InterruptKeyboardIo
	pop ax
	ret

;----------------------------;
;------- ����� ������ -------;
; * DS:SI - ����� ������	-;
; * CX - ����� ������		-;
; * DI - �������� �������	-;
; * AH - ����				-;
;----------------------------;
loaderProcWriteString:
	push ax bx cx es di si

	; ��������� ������ �����������
	mov bx, VideoMemoryAddressReal
	mov es, bx

	; ������������ �����
	@@:
		test cx, cx
		jz @f
		lodsb
		stosw
		dec cx
		jmp @b
	@@:
	pop si di es cx bx ax
	ret

;----------------------------;
;------ ������ ������� ------;
; * DX:AX - ����� �������	-;
; * ES:DI - ���� ������		-;
;----------------------------;
loaderLoadSector:
	; �������� �� ������� �������� Enhanced Disk Service
	cmp byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	je @loadSectorEx

	push ax bx cx dx si
	div [loaderVarSectorPerTrack]
	mov cl, dl ; ������� = ����� �������
	inc cl ; �������, � ������� �� ����� ����������, ������������� �� �������
	div [loaderVarHeadCount]
	mov dh, ah ; ������� = ����� �������
	mov ch, al ; ������� = ����� �������
	mov dl, [loaderVarDiskId]
	mov bx, di ; ���������� ����� � ��� � ES
	mov al, loaderConstSectorsToRead
	mov si, loaderConstSectorsReadAttempt

	@@:
		mov ah, FunctionDiskRead
		int InterruptDiskIo ; ������� ������
		jnc @f ; �� ������ => ����� �� ������������
		mov ah, FunctionDiskReset
		int InterruptDiskIo ; ����� �����
		dec si
		jnz @b ; ���������� ��� ��������� ���

	; ������ - ����������� ������� ������
	jmp @showError

	@@:
	pop si dx cx bx ax
	ret

	@loadSectorEx:
		push ax dx si
		mov byte [loaderVarDiskAddressPacket.LoadSectors], loaderConstSectorsToRead
		mov word [loaderVarDiskAddressPacket.BufferOffset], di
		push es
		pop word [loaderVarDiskAddressPacket.BufferSegment]
		mov word [loaderVarDiskAddressPacket.NumberSector + 0], ax
		mov word [loaderVarDiskAddressPacket.NumberSector + 2], dx
		mov word [loaderVarDiskAddressPacket.NumberSector + 4], 0
		mov word [loaderVarDiskAddressPacket.NumberSector + 6], 0

		mov ah, FunctionDiskReadEnhanced
		mov dl, [loaderVarDiskId]
		mov si, loaderConstDiskStructureAddress ; ��������� ����� ���������
		int InterruptDiskIo
		jc @showError ; ��� HDD � USB ��� ������������� ����������� ������������ ������� ������
		pop si dx ax
	ret

;===============;
;= ����� ����� =;
;===============;
BootLoaderEntry:
	; ��������� ���������� �����������
	mov ah, FunctionVideoSetMode
	mov al, VideoModeTextColor_80_25
	int InterruptVideo

	; ��������� ���������� ���������
	cli
	jmp 0:@f
	@@:
	mov ax, cs
	mov ds, ax
	mov ss, ax
	mov sp, $$
	sti

	;  ������ ������� ������������ �����
	mov [loaderVarDiskId], dl

	; ������� ��������� ������������ �������
	mov ah, FunctionDiskCheckEnhanced
	mov bh, BootSignatureByte0
	mov bl, BootSignatureByte1
	int InterruptDiskIo
	jc @f ; �������� ������ ���������� -> ������ �������� ����������� ����������
	mov byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	jmp @@diskRead ; ���������� ������ ����������� ����������

	@@: ; ������ ��������� ������ ���������� (��� FDD)
		mov ah, ColorForeYellow
		mov cx, loaderMessageDiskEnhancedDisabledLength
		mov si, loaderMessageDiskEnhancedDisabled
		xor di, di ; ������ ������ ������
		call loaderProcWriteString

		mov ah, FunctionDiskParams
		push es
		int InterruptDiskIo
		pop es
		jc @showError
		inc dh ; ������ ��� ������ ������ ������� ���������� �� ����
		mov [loaderVarHeadCount], dh
		and cx, loaderConstDiskSegmentMask ; ����� ������ 6 ������� ��� CX
		mov [loaderVarSectorPerTrack], cx

	; ���������� � ������� ����������� ��������
	@@diskRead:
		mov ah, ColorForeLime
		mov cx, loaderMessageDiskEnhancedEnabledLength
		mov si, loaderMessageDiskEnhancedEnabled
		xor di, di
		call loaderProcWriteString

		xor dx, dx
		xor ax, ax
		mov es, dx
		mov di, StartAddress
		mov cx, loaderConstSectorsOfBootLoader
		@@:
			inc ax
			add di, SectorLength
			call loaderLoadSector
			loop @b

		mov ah, ColorForeLime
		mov cx, loaderMessageSuccessfullLength
		mov si, loaderMessageSuccessfull
		mov di, 80*2*1 ; ������ ������ ������
		call loaderProcWriteString

		jmp ProtectedModeEntry

	; ����� ��������� �� �������� �������
	@showError:
		mov ah, ColorForeRed
		mov cx, loaderMessageDiskErrorLength
		mov si, loaderMessageDiskError
		mov di, 80*2*1 ; ������ ������ ������
		call loaderProcWriteString

	; �������� ���������� ������� �������
	@readKey:
	call loaderProcReadKey

	; ������������
	jmp loaderProcReboot

;============================;
;= ������������ � ��������� =;
;============================;
rb (SectorLength - BootSignatureLength) - ($ - $$)
db BootSignatureByte0, BootSignatureByte1

include "ProtectedMode.asm"
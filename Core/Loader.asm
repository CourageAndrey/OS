;###############;
;#             #;
;# Boot Loader #;
;#             #;
;###############;
	StartAddress equ 0x7C00
org StartAddress
	jmp word BootLoaderEntry

;=============;
;= Constants =;
;=============;
	 ; ! This file must be first in the include chain - otherwise constants from BIOS.asm won't be available.
	 include "BIOS.asm"

	 loaderConstSectorPerTrackEnhanced	equ 0xFF	; marker value (sectors per track), indicating that disk will be read using Enhanced Disk Service, not legacy mode
	 loaderConstSectorsToRead			equ 0x01	; number of sectors to read at once
	 loaderConstSectorsReadAttempt		equ 0x03	; number of read attempts for each sector
	 loaderConstSectorsOfBootLoader		equ 2		; ! number of sectors occupied by bootloader = CEIL(bootloader_size_in_bytes / sector_size_in_bytes)
	 loaderConstDiskSegmentMask			equ 111111b	; mask to extract sector count per track
	 loaderConstDiskStructureAddress	equ 0x600	; memory address where disk structure will be placed (must be before boot sector start)

	 ; message string lengths
	 loaderMessageSuccessfullLength 		 equ loaderMessageSuccessfullEnd - loaderMessageSuccessfull
	 loaderMessageDiskErrorLength			 equ loaderMessageDiskErrorEnd - loaderMessageDiskError
	 loaderMessageDiskEnhancedEnabledLength	 equ loaderMessageDiskEnhancedEnabledEnd - loaderMessageDiskEnhancedEnabled
	 loaderMessageDiskEnhancedDisabledLength equ loaderMessageDiskEnhancedDisabledEnd - loaderMessageDiskEnhancedDisabled

;========;
;= Data =;
;========;
	 loaderVarDiskId			db ? ; ID of the disk from which boot is performed
	 loaderVarSectorPerTrack	dw ? ; number of sectors per track for the same disk
	 loaderVarHeadCount			db ? ; number of heads for the same disk
	 loaderVarDiskAddressPacket	EddPacket32 ; Enhanced Disk Service packet, describing the disk from which boot is performed
	 ; predefined text messages
	 loaderMessageSuccessfull db 'Disk reading has been completed.'
	 loaderMessageSuccessfullEnd:
	 loaderMessageDiskError db 'Error: disk reading has failed! Press any key for reboot...'
	 loaderMessageDiskErrorEnd:
	 loaderMessageDiskEnhancedEnabled db 'Enhanced disk functions are available.'
	 loaderMessageDiskEnhancedEnabledEnd:
	 loaderMessageDiskEnhancedDisabled db 'Enhanced disk functions are disabled.'
	 loaderMessageDiskEnhancedDisabledEnd:

;==============;
;= Procedures =;
;==============;

;--------------;
;- Reboot PC  -;
;--------------;
loaderProcReboot:
	jmp RebootAddress:0

;------------------------------;
;- Wait for key press         -;
;------------------------------;
loaderProcReadKey:
	push ax
	mov ah, FunctionKeyboardReadkey
	int InterruptKeyboardIo
	pop ax
	ret

;----------------------------;
;- Write string             -;
; * DS:SI - string address  -;
; * CX - string length      -;
; * DI - screen offset      -;
; * AH - color              -;
;----------------------------;
loaderProcWriteString:
	push ax bx cx es di si

	; setup video memory segment
	mov bx, VideoMemoryAddressReal
	mov es, bx

	; output the string
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
;- Load disk sector         -;
; * DX:AX - sector number   -;
; * ES:DI - destination buf -;
;----------------------------;
loaderLoadSector:
	; check if Enhanced Disk Service is available
	cmp byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	je @loadSectorEx

	push ax bx cx dx si
	div [loaderVarSectorPerTrack]
	mov cl, dl ; remainder = sector number
	inc cl ; sectors we want to load are numbered starting from one
	div [loaderVarHeadCount]
	mov dh, ah ; remainder = head number
	mov ch, al ; quotient = track number
	mov dl, [loaderVarDiskId]
	mov bx, di ; destination address is in DI and ES
	mov al, loaderConstSectorsToRead
	mov si, loaderConstSectorsReadAttempt

	@@:
		mov ah, FunctionDiskRead
		int InterruptDiskIo ; try to read
		jnc @f ; no error => exit from retry loop
		mov ah, FunctionDiskReset
		int InterruptDiskIo ; reset disk
		dec si
		jnz @b ; repeat for remaining attempts

	; error - cannot read sector
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
		mov si, loaderConstDiskStructureAddress ; packet address
		int InterruptDiskIo
		jc @showError ; for HDD and USB this rarely fails but we still check for errors
		pop si dx ax
	ret

;===============;
;= Entry Point =;
;===============;
BootLoaderEntry:
	; setup video adapter
	mov ah, FunctionVideoSetMode
	mov al, VideoModeTextColor_80_25
	int InterruptVideo

	; setup segment registers
	cli
	jmp 0:@f
	@@:
	mov ax, cs
	mov ds, ax
	mov ss, ax
	mov sp, $$
	sti

	; save boot disk ID
	mov [loaderVarDiskId], dl

	; try to use enhanced disk functions
	mov ah, FunctionDiskCheckEnhanced
	mov bh, BootSignatureByte0
	mov bl, BootSignatureByte1
	int InterruptDiskIo
	jc @f ; enhanced mode not available -> use legacy CHS mode
	mov byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	jmp @@diskRead ; enhanced mode is available

	@@: ; get disk parameters in legacy mode (for FDD)
		mov ah, ColorForeYellow
		mov cx, loaderMessageDiskEnhancedDisabledLength
		mov si, loaderMessageDiskEnhancedDisabled
		xor di, di ; first line of screen
		call loaderProcWriteString

		mov ah, FunctionDiskParams
		push es
		int InterruptDiskIo
		pop es
		jc @showError
		inc dh ; because BIOS returns max head index minus one
		mov [loaderVarHeadCount], dh
		and cx, loaderConstDiskSegmentMask ; keep only lower 6 bits of CX
		mov [loaderVarSectorPerTrack], cx

	; proceed to reading remaining sectors
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
		mov di, 80*2*1 ; second line of screen
		call loaderProcWriteString

		jmp ProtectedModeEntry

	; show error message when disk reading fails
	@showError:
		mov ah, ColorForeRed
		mov cx, loaderMessageDiskErrorLength
		mov si, loaderMessageDiskError
		mov di, 80*2*1 ; second line of screen
		call loaderProcWriteString

	; wait for any key press
	@readKey:
	call loaderProcReadKey

	; reboot
	jmp loaderProcReboot

;============================;
;= Boot Signature & Padding =;
;============================;
rb (SectorLength - BootSignatureLength) - ($ - $$)
db BootSignatureByte0, BootSignatureByte1

include "ProtectedMode.asm"
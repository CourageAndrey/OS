;##########################################;
;#                                        #;
;# Routines to make work with BIOS easier #;
;#                                        #;
;##########################################;

;=============;
;= Constants =;
;=============;
	; address to JMP in order to reboot computer
	RebootAddress equ 0xFFFF

	; boot disk signature
	BootSignatureLength	equ 0x02 ; length
	BootSignatureByte0	equ 0x55 ; byte 1
	BootSignatureByte1	equ 0xAA ; byte 2

	; disk sector length = 512 bytes
	SectorLength equ 0x200

	; video RAM memory address
	VideoMemoryAddressReal		equ 0xB800	; for Real mode 
	VideoMemoryAddressProtected	equ 0xB8000	; for Protected mode

	; interrupts
	InterruptZeroDivision	equ 0x00 ; zero division
	InterruptDebugStep		equ 0x01 ; step-by-step debug
	InterruptUnmasked		equ 0x02 ; unmasked
	InterruptDebugPoint		equ 0x03 ; debugger breakpoint
	InterruptOverflow		equ 0x04 ; math overflow
	InterruptPrintScreen	equ 0x05 ; print screen
	InterruptTimer			equ 0x08 ; timer
	InterruptKeyboard		equ 0x09 ; keyboard
	InterruptFloppy			equ 0x0E ; floppy disk
	InterruptVideo			equ 0x10 ; video adapter subsystem
	InterruptHardware		equ 0x11 ; installed hardware list
	InterruptMemory			equ 0x12 ; size of available memory
	InterruptDiskIo			equ 0x13 ; disk I/O
	InterruptSerialIo		equ 0x14 ; serial port I/O
	InterruptAt				equ 0x15 ; extended AT service
	InterruptKeyboardIo		equ 0x16 ; keyboard I/O
	InterruptPrinterIo		equ 0x17 ; printer I/O
	InterruptRomBasic		equ 0x18 ; ROM-BASIC
	InterruptPost			equ 0x19 ; POST
	InterruptTimerIo		equ 0x1A ; timer I/O
	InterruptKeyboardInt	equ 0x1B ; Ctrl + Break press handler
	InterruptUserTimer		equ 0x1C ; user timer interrupt
	InterruptVideoParams	equ 0x1D ; video parameters
	InterruptFloppy			equ 0x1E ; floppy disk parameters
	InterruptSymbols		equ 0x1F ; graphic symbols

	; Int 0x10 (video) functions
	FunctionVideoSetMode equ 0x00 ; set video mode (clears screen)

	; Int 0x13 (disk) functions
	FunctionDiskReset			equ 0x00 ; reset disk
	FunctionDiskRead			equ 0x02 ; read sector
	FunctionDiskParams			equ 0x08 ; get disk parameters
	FunctionDiskCheckEnhanced	equ 0x41 ; check if Enhanced Disk Service is available
	FunctionDiskReadEnhanced	equ 0x42 ; read using Enhanced Disk Service

	; Int 0x16 (keyboard) functions
	FunctionKeyboardReadkey equ 0x00 ; read character

	; available video modes
	VideoModeTextColor_80_25 equ 0x03 ; text, colored (4 bits for character + 4 for background), 80 columns x 25 rows

	; colors
	ColorForeBlack			equ 0x00 ; foreground is black
	ColorForeNavy			equ 0x01 ; foreground is navy
	ColorForeGreen			equ 0x02 ; foreground is green
	ColorForeDarkCyan		equ 0x03 ; foreground is dark cyan
	ColorForeDarkRed		equ 0x04 ; foreground is dark red
	ColorForeDarkMagenta	equ 0x05 ; foreground is dark magenta
	ColorForeBrown			equ 0x06 ; foreground is brown
	ColorForeSilver			equ 0x07 ; foreground is silver
	ColorForeDarkGray		equ 0x08 ; foreground is dark gray
	ColorForeBlue			equ 0x09 ; foreground is blue
	ColorForeLime			equ 0x0A ; foreground is lime
	ColorForeCyan			equ 0x0B ; foreground is cyan
	ColorForeRed			equ 0x0C ; foreground is red
	ColorForeMagenta		equ 0x0D ; foreground is magenta
	ColorForeYellow			equ 0x0E ; foreground is yellow
	ColorForeWhite			equ 0x0F ; foreground is white
	ColorBackBlack			equ 0x00 ; background is black
	ColorBackNavy			equ 0x10 ; background is navy
	ColorBackGreen			equ 0x20 ; background is green
	ColorBackDarkCyan		equ 0x30 ; background is dark cyan
	ColorBackDarkRed 		equ 0x40 ; background is dark red
	ColorBackDarkMagenta	equ 0x50 ; background is dark magenta
	ColorBackBrown			equ 0x60 ; background is brown
	ColorBackSilver			equ 0x70 ; background is silver
	ColorBackDarkGray		equ 0x80 ; background is dark gray
	ColorBackBlue			equ 0x90 ; background is blue
	ColorBackLime			equ 0xA0 ; background is lime
	ColorBackCyan			equ 0xB0 ; background is cyan
	ColorBackRed			equ 0xC0 ; background is red
	ColorBackMagenta		equ 0xD0 ; background is magenta
	ColorBackYellow			equ 0xE0 ; background is yellow
	ColorBackWhite			equ 0xF0 ; background is white

	; length of segment descriptor
	SegmentDescriptorLength equ 0x08

;==============;
;= Structures =;
;==============;

; Enhanced Disk Service package
struc EddPacket32
{
	.Size			db 0x10	; size (is always equal to 16 bytes)
	.Reserved1		db 0	; reserved field 1
	.LoadSectors	db ?	; number of sectors
	.Reserved2		db 0	; reserved field 2
	.BufferOffset	dw ?	; memory buffer offset
	.BufferSegment	dw ?	; memory buffer segment
	.NumberSector	dq ?	; sector number
}

;==========;
;= Macros =;
;==========;
macro DefineSegmentDescriptor32 baseAddress, sizeLimit, bitGranularity, bitDefaultSize, bitL, bitAVL, bitPresent, privilegieLevel, bitSystem, bitExecutable, bit10, bit9, bitAccessed
{
	dw (sizeLimit and 0xFFFF) ; limit, bytes 00..15
	dw (baseAddress and 0xFFFF) ; base, bytes 00..15
	db ((baseAddress and 0xFF0000) shr 16) ; base, bytes 16..23
	db (((bitPresent and 1b) shl 7) or ((privilegieLevel and 11b) shl 5) or ((bitSystem and 1b) shl 4) or ((bitExecutable and 1b) shl 3) or ((bit10 and 1b) shl 2) or ((bit9 and 1b) shl 1) or ((bitAccessed and 1b))) ; first byte of attributes
	db (((bitGranularity and 1b) shl 7) or ((bitDefaultSize and 1b) shl 6) or ((bitL and 1b) shl 5) or ((bitAVL and 1b) shl 4) or ((sizeLimit and 0xF0000) shr 16)) ; second 4 bits of attributes and limit, bytes 16..19
	db ((baseAddress and 0xFF000000) shr 24) ; base, bytes 24..31
}

	; descriptor properties values
	DescriptorBitSize16				equ 0
	DescriptorBitSize32				equ 1
	DescriptorBitPresent			equ 1
	DescriptorBitAbsent				equ 0
	DescriptorBitGranularityByte	equ 0
	DescriptorBitGranularityPage	equ 1
	DescriptorPrivilege0			equ 0
	DescriptorPrivilege1			equ 1
	DescriptorPrivilege2			equ 2
	DescriptorPrivilege3			equ 3
	DescriptorObjectSystem			equ 1
	DescriptorObjectUsual			equ 0
	DescriptorTypeCode				equ 1
	DescriptorTypeData				equ 0
	DescriptorDataAddressesUsual	equ 0
	DescriptorDataAddressesStack	equ 1
	DescriptorCodeExecutable		equ 0
	DescriptorCodeReadable			equ 1
	DescriptorDataReadOnly			equ 0
	DescriptorDataReadWrite			equ 1
;##########################################;
;#                                        #;
;# ��������� ��� ���������� ������ � BIOS #;
;#                                        #;
;##########################################;

;=============;
;= ��������� =;
;=============;
	; �����, �� �������� ����������� ������, ����� ��������� ������������ ����������
	RebootAddress equ 0xFFFF

	; ��������� ������������ �����
	BootSignatureLength	equ 0x02 ; �����
	BootSignatureByte0	equ 0x55 ; ���� 1
	BootSignatureByte1	equ 0xAA ; ���� 2

	; ����� ������� (�������) = 512 ����
	SectorLength equ 0x200

	; ����� �����������
	VideoMemoryAddressReal	    equ 0xB800	; � �������� ������
	VideoMemoryAddressProtected equ 0xB8000 ; � ���������� ������

	; ����������
	InterruptZeroDivision	equ 0x00 ; ������� �� ����
	InterruptDebugStep		equ 0x01 ; ��������� �������
	InterruptUnmasked		equ 0x02 ; �������������
	InterruptDebugPoint		equ 0x03 ; ����� �������
	InterruptOverflow		equ 0x04 ; ������������
	InterruptPrintScreen	equ 0x05 ; ������ ������
	InterruptTimer			equ 0x08 ; ������
	InterruptKeyboard		equ 0x09 ; ����������
	InterruptFloppy			equ 0x0E ; �������
	InterruptVideo			equ 0x10 ; �����-����������
	InterruptHardware		equ 0x11 ; ������ ������������
	InterruptMemory			equ 0x12 ; ������ ������������ ������
	InterruptDiskIo			equ 0x13 ; �������� in/out
	InterruptSerialIo		equ 0x14 ; ��������������� ���� in/out
	InterruptAt				equ 0x15 ; ����������� ������ AT
	InterruptKeyboardIo		equ 0x16 ; ���������� in/out
	InterruptPrinterIo		equ 0x17 ; ������� in/out
	InterruptRomBasic		equ 0x18 ; ROM-BASIC
	InterruptPost			equ 0x19 ; POST
	InterruptTimerIo		equ 0x1A ; ������ in/out
	InterruptKeyboardInt	equ 0x1B ; �������� ������� Ctrl + Break
	InterruptUserTimer		equ 0x1C ; ���������������� ���������� �� �������
	InterruptVideoParams	equ 0x1D ; �����-���������
	InterruptFloppy			equ 0x1E ; ��������� ������
	InterruptSymbols		equ 0x1F ; ������� �������

	; ������� ��� 0x10 ���������� (�����)
	FunctionVideoSetMode equ 0x00 ; ��������� �����-������ (� �������� ������)

	; ������� ��� 0x13 ���������� (����)
	FunctionDiskReset			equ 0x00 ; ����� �����
	FunctionDiskRead			equ 0x02 ; ������ �������
	FunctionDiskParams			equ 0x08 ; ��������� ���������� �����
	FunctionDiskCheckEnhanced	equ 0x41 ; �������� ����������� Enhanced Disk Service
	FunctionDiskReadEnhanced	equ 0x42 ; ������ � ������� Enhanced Disk Service

	; ������� ��� 0x16 ���������� (����������)
	FunctionKeyboardReadkey equ 0x00 ; ���������� �������

	; ��������� �����-������
	VideoModeTextColor_80_25 equ 0x03 ; ���������, ������� (4 ���� ������ + 4 ���), ����� 80 �������� � 25 �����

	; �����
	ColorForeBlack			equ 0x00 ; ������ ������
	ColorForeNavy			equ 0x01 ; ������ �����
	ColorForeGreen			equ 0x02 ; ������ ������
	ColorForeDarkCyan		equ 0x03 ; ������ ����-���������
	ColorForeDarkRed		equ 0x04 ; ������ ����-������
	ColorForeDarkMagenta	equ 0x05 ; ������ ����-���������
	ColorForeBrown			equ 0x06 ; ������ ����������
	ColorForeSilver			equ 0x07 ; ������ �����������
	ColorForeDarkGray		equ 0x08 ; ������ ����-�����
	ColorForeBlue			equ 0x09 ; ������ �������
	ColorForeLime			equ 0x0A ; ������ ���������
	ColorForeCyan			equ 0x0B ; ������ ������-���������
	ColorForeRed			equ 0x0C ; ������ �������
	ColorForeMagenta		equ 0x0D ; ������ ������-���������
	ColorForeYellow			equ 0x0E ; ������ �����
	ColorForeWhite			equ 0x0F ; ������ �����
	ColorBackBlack			equ 0x00 ; ��� ������
	ColorBackNavy			equ 0x10 ; ��� �����
	ColorBackGreen			equ 0x20 ; ��� ������
	ColorBackDarkCyan		equ 0x30 ; ��� ����-���������
	ColorBackDarkRed 		equ 0x40 ; ��� ����-������
	ColorBackDarkMagenta	equ 0x50 ; ��� ����-���������
	ColorBackBrown			equ 0x60 ; ��� ����������
	ColorBackSilver			equ 0x70 ; ��� �����������
	ColorBackDarkGray		equ 0x80 ; ��� ����-�����
	ColorBackBlue			equ 0x90 ; ��� �������
	ColorBackLime			equ 0xA0 ; ��� ���������
	ColorBackCyan			equ 0xB0 ; ��� ������-���������
	ColorBackRed			equ 0xC0 ; ��� �������
	ColorBackMagenta		equ 0xD0 ; ��� ������-���������
	ColorBackYellow			equ 0xE0 ; ��� �����
	ColorBackWhite			equ 0xF0 ; ��� �����

	; ����� ����������� �����������
	SegmentDescriptorLength equ 0x08

;=============;
;= ��������� =;
;=============;

; ����� ��� Enhanced Disk Service (����������, ��������, ������� ������ ��������� ���������)
struc EddPacket32
{
	.Size			db 0x10	; ������ ������ 16 ����
	.Reserved1		db 0	; ����������������� ���� 1
	.LoadSectors	db ?	; ���������� ��������
	.Reserved2		db 0	; ����������������� ���� 2
	.BufferOffset	dw ?	; �������� ������ � ������
	.BufferSegment	dw ?	; ������� ������ � ������
	.NumberSector	dq ?	; ����� �������
}

;===========;
;= ������� =;
;===========;
macro DefineSegmentDescriptor32 baseAddress, sizeLimit, bitGranularity, bitDefaultSize, bitL, bitAVL, bitPresent, privilegieLevel, bitSystem, bitExecutable, bit10, bit9, bitAccessed
{
	dw (sizeLimit and 0xFFFF) ; �����, ���� 00..15
	dw (baseAddress and 0xFFFF) ; ����, ���� 00..15
	db ((baseAddress and 0xFF0000) shr 16) ; ����, ���� 16..23
	db (((bitPresent and 1b) shl 7) or ((privilegieLevel and 11b) shl 5) or ((bitSystem and 1b) shl 4) or ((bitExecutable and 1b) shl 3) or ((bit10 and 1b) shl 2) or ((bit9 and 1b) shl 1) or ((bitAccessed and 1b))) ; ������ ���� ���������
	db (((bitGranularity and 1b) shl 7) or ((bitDefaultSize and 1b) shl 6) or ((bitL and 1b) shl 5) or ((bitAVL and 1b) shl 4) or ((sizeLimit and 0xF0000) shr 16)) ; ������ 4 ���� ��������� � �����, ���� 16..19
	db ((baseAddress and 0xFF000000) shr 24) ; ����, ���� 24..31
}

	; ��������� ��������� �����������
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
;##########################################;
;#                                        #;
;# Константы для облегчения работы с BIOS #;
;#                                        #;
;##########################################;

;=============;
;= Константы =;
;=============;
	; адрес, по которому совершается прыжок, чтобы выполнить перезагрузку компьютера
	RebootAddress equ 0xFFFF

	; сигнатура загрузочного диска
	BootSignatureLength	equ 0x02 ; длина
	BootSignatureByte0	equ 0x55 ; байт 1
	BootSignatureByte1	equ 0xAA ; байт 2

	; длина сектора (дискеты) = 512 байт
	SectorLength equ 0x200

	; адрес видеопамяти
	VideoMemoryAddressReal	    equ 0xB800	; в реальном режиме
	VideoMemoryAddressProtected equ 0xB8000 ; в защищённом режиме

	; прерывания
	InterruptZeroDivision	equ 0x00 ; деление на ноль
	InterruptDebugStep		equ 0x01 ; пошаговая отладка
	InterruptUnmasked		equ 0x02 ; немаскируемое
	InterruptDebugPoint		equ 0x03 ; точка отладки
	InterruptOverflow		equ 0x04 ; переполнение
	InterruptPrintScreen	equ 0x05 ; печать экрана
	InterruptTimer			equ 0x08 ; таймер
	InterruptKeyboard		equ 0x09 ; клавиатура
	InterruptFloppy			equ 0x0E ; дискета
	InterruptVideo			equ 0x10 ; видео-подсистема
	InterruptHardware		equ 0x11 ; список оборудования
	InterruptMemory			equ 0x12 ; размер используемой памяти
	InterruptDiskIo			equ 0x13 ; дисковый in/out
	InterruptSerialIo		equ 0x14 ; последовательнй порт in/out
	InterruptAt				equ 0x15 ; расширенный сервис AT
	InterruptKeyboardIo		equ 0x16 ; клавиатура in/out
	InterruptPrinterIo		equ 0x17 ; принтер in/out
	InterruptRomBasic		equ 0x18 ; ROM-BASIC
	InterruptPost			equ 0x19 ; POST
	InterruptTimerIo		equ 0x1A ; таймер in/out
	InterruptKeyboardInt	equ 0x1B ; обработа нажатия Ctrl + Break
	InterruptUserTimer		equ 0x1C ; пользовательское прерывание по таймеру
	InterruptVideoParams	equ 0x1D ; видео-параметры
	InterruptFloppy			equ 0x1E ; параметры дискет
	InterruptSymbols		equ 0x1F ; символы графики

	; функции для 0x10 прерывания (видео)
	FunctionVideoSetMode equ 0x00 ; установка видео-режима (с очисткой экрана)

	; функции для 0x13 прерывания (диск)
	FunctionDiskReset			equ 0x00 ; сброс диска
	FunctionDiskRead			equ 0x02 ; чтение сектора
	FunctionDiskParams			equ 0x08 ; получение параметров диска
	FunctionDiskCheckEnhanced	equ 0x41 ; проверка доступности Enhanced Disk Service
	FunctionDiskReadEnhanced	equ 0x42 ; чтение с помощью Enhanced Disk Service

	; функции для 0x16 прерывания (клавиатура)
	FunctionKeyboardReadkey equ 0x00 ; считывание символа

	; доступные видео-режимы
	VideoModeTextColor_80_25 equ 0x03 ; текстовый, цветной (4 бита символ + 4 фон), экран 80 столбцов х 25 строк

	; цвета
	ColorForeBlack			equ 0x00 ; символ чёрный
	ColorForeNavy			equ 0x01 ; символ синий
	ColorForeGreen			equ 0x02 ; символ зелёный
	ColorForeDarkCyan		equ 0x03 ; символ тёмно-бирюзовый
	ColorForeDarkRed		equ 0x04 ; символ тёмно-красый
	ColorForeDarkMagenta	equ 0x05 ; символ тёмно-пурпурный
	ColorForeBrown			equ 0x06 ; символ коричневый
	ColorForeSilver			equ 0x07 ; символ серебристый
	ColorForeDarkGray		equ 0x08 ; символ тёмно-серый
	ColorForeBlue			equ 0x09 ; символ голубой
	ColorForeLime			equ 0x0A ; символ салатовый
	ColorForeCyan			equ 0x0B ; символ светло-бирюзовый
	ColorForeRed			equ 0x0C ; символ красный
	ColorForeMagenta		equ 0x0D ; символ светло-пурпурный
	ColorForeYellow			equ 0x0E ; символ жёлтый
	ColorForeWhite			equ 0x0F ; символ белый
	ColorBackBlack			equ 0x00 ; фон чёрный
	ColorBackNavy			equ 0x10 ; фон синий
	ColorBackGreen			equ 0x20 ; фон зелёный
	ColorBackDarkCyan		equ 0x30 ; фон тёмно-бирюзовый
	ColorBackDarkRed 		equ 0x40 ; фон тёмно-красый
	ColorBackDarkMagenta	equ 0x50 ; фон тёмно-пурпурный
	ColorBackBrown			equ 0x60 ; фон коричневый
	ColorBackSilver			equ 0x70 ; фон серебристый
	ColorBackDarkGray		equ 0x80 ; фон тёмно-серый
	ColorBackBlue			equ 0x90 ; фон голубой
	ColorBackLime			equ 0xA0 ; фон салатовый
	ColorBackCyan			equ 0xB0 ; фон светло-бирюзовый
	ColorBackRed			equ 0xC0 ; фон красный
	ColorBackMagenta		equ 0xD0 ; фон светло-пурпурный
	ColorBackYellow			equ 0xE0 ; фон жёлтый
	ColorBackWhite			equ 0xF0 ; фон белый

	; длина сегментного дескриптора
	SegmentDescriptorLength equ 0x08

;=============;
;= Структуры =;
;=============;

; пакет для Enhanced Disk Service (показывает, например, сколько данных требуется прочитать)
struc EddPacket32
{
	.Size			db 0x10	; размер всегда 16 байт
	.Reserved1		db 0	; зарезервированное поле 1
	.LoadSectors	db ?	; количество секторов
	.Reserved2		db 0	; зарезервированное поле 2
	.BufferOffset	dw ?	; смещение буфера в памяти
	.BufferSegment	dw ?	; сегмент буфера в памяти
	.NumberSector	dq ?	; номер сектора
}

;===========;
;= Макросы =;
;===========;
macro DefineSegmentDescriptor32 baseAddress, sizeLimit, bitGranularity, bitDefaultSize, bitL, bitAVL, bitPresent, privilegieLevel, bitSystem, bitExecutable, bit10, bit9, bitAccessed
{
	dw (sizeLimit and 0xFFFF) ; лимит, биты 00..15
	dw (baseAddress and 0xFFFF) ; база, биты 00..15
	db ((baseAddress and 0xFF0000) shr 16) ; база, биты 16..23
	db (((bitPresent and 1b) shl 7) or ((privilegieLevel and 11b) shl 5) or ((bitSystem and 1b) shl 4) or ((bitExecutable and 1b) shl 3) or ((bit10 and 1b) shl 2) or ((bit9 and 1b) shl 1) or ((bitAccessed and 1b))) ; первый байт атрибутов
	db (((bitGranularity and 1b) shl 7) or ((bitDefaultSize and 1b) shl 6) or ((bitL and 1b) shl 5) or ((bitAVL and 1b) shl 4) or ((sizeLimit and 0xF0000) shr 16)) ; вторых 4 бита атрибутов и лимит, биты 16..19
	db ((baseAddress and 0xFF000000) shr 24) ; база, биты 24..31
}

	; константы настройки дескриптора
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
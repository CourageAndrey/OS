;#######################;
;#                     #;
;# Начальный загрузчик #;
;#                     #;
;#######################;
	StartAddress equ 0x7C00
org StartAddress
	jmp word BootLoaderEntry

;=============;
;= Константы =;
;=============;
	 ; ! Так как этот файл собирается первым - строку с включением BIOS.asm комментировать не нужно.
	 include "BIOS.asm"

	 loaderConstSectorPerTrackEnhanced	equ 0xFF	; значение (количество секторов на дорожку), указывающее на то, что чтение стоит осуществлять с помощью Enhanced Disk Service, а не посекторно
	 loaderConstSectorsToRead			equ 0x01	; количество секторов, читаемое за один раз
	 loaderConstSectorsReadAttempt		equ 0x03	; количество попыток чтения сектора
	 loaderConstSectorsOfBootLoader		equ 2		; ! количество секторов, следующих за загрузчиком = CEIL(РазмерБинарникаОстальногоКода / РазмерСектора)
	 loaderConstDiskSegmentMask			equ 111111b	; маска для определения количества секторов на дорожку
	 loaderConstDiskStructureAddress	equ 0x600	; адрес в памяти, в который будут выгружаться данные о диске (выбран по желанию левой пятки)

	 ; длины выводимых сообщений
	 loaderMessageSuccessfullLength 	 equ loaderMessageSuccessfullEnd - loaderMessageSuccessfull
	 loaderMessageDiskErrorLength		 equ loaderMessageDiskErrorEnd - loaderMessageDiskError
	 loaderMessageDiskEnhancedEnabledLength  equ loaderMessageDiskEnhancedEnabledEnd - loaderMessageDiskEnhancedEnabled
	 loaderMessageDiskEnhancedDisabledLength equ loaderMessageDiskEnhancedDisabledEnd - loaderMessageDiskEnhancedDisabled

;==========;
;= Данные =;
;==========;
	 loaderVarDiskId			db ? ; идентификатор диска, с которого осуществляется загрузка
	 loaderVarSectorPerTrack	dw ? ; количество секторов на дорожке -//-
	 loaderVarHeadCount			db ? ; количество головок -//-
	 loaderVarDiskAddressPacket	EddPacket32 ; пакет Enhanced Disk Service, описывающий диск, с которого осуществляется загрузка
	 ; используемые текстовые строки
	 loaderMessageSuccessfull db 'Disk reading has been completed.'
	 loaderMessageSuccessfullEnd:
	 loaderMessageDiskError db 'Error: disk reading has failed! Press any key for reboot...'
	 loaderMessageDiskErrorEnd:
	 loaderMessageDiskEnhancedEnabled db 'Enhanced disk functions are available.'
	 loaderMessageDiskEnhancedEnabledEnd:
	 loaderMessageDiskEnhancedDisabled db 'Enhanced disk functions are disabled.'
	 loaderMessageDiskEnhancedDisabledEnd:

;================;
;= Подпрограммы =;
;================;
;----------------;
;- Перезагрузка -;
;----------------;
loaderProcReboot:
	jmp RebootAddress:0

;-------------------------------;
;- Чтение символа с клавиатуры -;
;-------------------------------;
loaderProcReadKey:
	push ax
	mov ah, FunctionKeyboardReadkey
	int InterruptKeyboardIo
	pop ax
	ret

;----------------------------;
;------- Вывод строки -------;
; * DS:SI - адрес строки	-;
; * CX - длина строки		-;
; * DI - смещение символа	-;
; * AH - цвет				-;
;----------------------------;
loaderProcWriteString:
	push ax bx cx es di si

	; установка адреса видеопамяти
	mov bx, VideoMemoryAddressReal
	mov es, bx

	; посимвольный вывод
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
;------ Чтение сектора ------;
; * DX:AX - номер сектора	-;
; * ES:DI - куда читать		-;
;----------------------------;
loaderLoadSector:
	; проверка на наличие рабочего Enhanced Disk Service
	cmp byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	je @loadSectorEx

	push ax bx cx dx si
	div [loaderVarSectorPerTrack]
	mov cl, dl ; остаток = номер сектора
	inc cl ; сектора, в отличии от всего остального, отсчитываются от единицы
	div [loaderVarHeadCount]
	mov dh, ah ; остаток = номер головки
	mov ch, al ; частное = номер дорожки
	mov dl, [loaderVarDiskId]
	mov bx, di ; сегментная часть и так в ES
	mov al, loaderConstSectorsToRead
	mov si, loaderConstSectorsReadAttempt

	@@:
		mov ah, FunctionDiskRead
		int InterruptDiskIo ; попытка чтения
		jnc @f ; всё хорошо => выход из подпрограммы
		mov ah, FunctionDiskReset
		int InterruptDiskIo ; сброс диска
		dec si
		jnz @b ; попытаться ещё несколько раз

	; ошибка - закончились попытки чтения
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
		mov si, loaderConstDiskStructureAddress ; Начальный адрес структуры
		int InterruptDiskIo
		jc @showError ; для HDD и USB нет необходимости производить многократные попытки чтения
		pop si dx ax
	ret

;===============;
;= Точка входа =;
;===============;
BootLoaderEntry:
	; установка требуемого видеорежима
	mov ah, FunctionVideoSetMode
	mov al, VideoModeTextColor_80_25
	int InterruptVideo

	; настройка сегментных регистров
	cli
	jmp 0:@f
	@@:
	mov ax, cs
	mov ds, ax
	mov ss, ax
	mov sp, $$
	sti

	;  чтение свойств загрузочного диска
	mov [loaderVarDiskId], dl

	; попытка включения расширенного сервиса
	mov ah, FunctionDiskCheckEnhanced
	mov bh, BootSignatureByte0
	mov bl, BootSignatureByte1
	int InterruptDiskIo
	jc @f ; дисковый сервис недоступен -> старый механизм определения параметров
	mov byte[loaderVarSectorPerTrack], loaderConstSectorPerTrackEnhanced
	jmp @@diskRead ; пропускаем старое определение параметров

	@@: ; старая процедура чтения параметров (для FDD)
		mov ah, ColorForeYellow
		mov cx, loaderMessageDiskEnhancedDisabledLength
		mov si, loaderMessageDiskEnhancedDisabled
		xor di, di ; начало первой строки
		call loaderProcWriteString

		mov ah, FunctionDiskParams
		push es
		int InterruptDiskIo
		pop es
		jc @showError
		inc dh ; потому что отсчёт номера головки начинается от нуля
		mov [loaderVarHeadCount], dh
		and cx, loaderConstDiskSegmentMask ; нужны только 6 младших бит CX
		mov [loaderVarSectorPerTrack], cx

	; считывание с помощью расширенных функиций
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
		mov di, 80*2*1 ; начало второй строки
		call loaderProcWriteString

		jmp ProtectedModeEntry

	; вывод сообщения об дисковых ошибках
	@showError:
		mov ah, ColorForeRed
		mov cx, loaderMessageDiskErrorLength
		mov si, loaderMessageDiskError
		mov di, 80*2*1 ; начало второй строки
		call loaderProcWriteString

	; ожидание финального нажатия клавиши
	@readKey:
	call loaderProcReadKey

	; перезагрузка
	jmp loaderProcReboot

;============================;
;= Выравнивание и сигнатура =;
;============================;
rb (SectorLength - BootSignatureLength) - ($ - $$)
db BootSignatureByte0, BootSignatureByte1

include "ProtectedMode.asm"
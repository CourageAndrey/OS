using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public static class Environment
	{
		public static System.Tuple<MemoryWord[], MemoryAddress> LoadSample()
		{
			var memory =  new System.Tuple<MemoryWord[], MemoryAddress>(new MemoryWord[]
			{
				//--- NULL (ZERO pointer) ---
				0000, // 0000

				//--- ObjectDataType ---
				0011, // 0001 DataType > DataTypeDataType
				0000, // 0002 BaseType > #NULL
				0111, // 0003 Fields > Array of object fields
				0103, // 0004 Methods > Empty array of Methods
				0105, // 0005 Properties > Empty array of Properties
				0107, // 0006 Events > Empty array of Events
				0109, // 0007 Constructors > Empty array of Constructors
				0241, // 0008 Name > "Object"
				0000, // 0009
				0000, // 0010

				//--- DataTypeDataType ---
				0011, // 0011 DataType > DataTypeDataType
				0001, // 0012 BaseType > ObjectDataType
				0121, // 0013 Fields > Array of type fields
				0103, // 0014 Methods > Empty array of Methods
				0105, // 0015 Properties > Empty array of Properties
				0107, // 0016 Events > Empty array of Events
				0109, // 0017 Constructors > Empty array of Constructors
				0251, // 0018 Name > "DataType"
				0000, // 0019
				0000, // 0020

				//--- DataTypeMemberDataType ---
				0011, // 0021 DataType > DataTypeDataType
				0001, // 0022 BaseType > ObjectDataType
				0101, // 0023 Fields > Empty array of Fields
				0103, // 0024 Methods > Empty array of Methods
				0105, // 0025 Properties > Empty array of Properties
				0107, // 0026 Events > Empty array of Events
				0109, // 0027 Constructors > Empty array of Constructors
				0261, // 0028 Name > "DataTypeMember"
				0000, // 0029
				0000, // 0030

				//--- DataTypeFieldDataType ---
				0011, // 0031 DataType > DataTypeDataType
				0021, // 0032 BaseType > DataTypeMemberDataType
				0101, // 0033 Fields > Empty array of Fields
				0103, // 0034 Methods > Empty array of Methods
				0105, // 0035 Properties > Empty array of Properties
				0107, // 0036 Events > Empty array of Events
				0109, // 0037 Constructors > Empty array of Constructors
				0281, // 0038 Name > "DataTypeField"
				0000, // 0039
				0000, // 0040

				//--- DataTypeMethodDataType ---
				0011, // 0041 DataType > DataTypeDataType
				0021, // 0042 BaseType > DataTypeMemberDataType
				0101, // 0043 Fields > Empty array of Fields
				0103, // 0044 Methods > Empty array of Methods
				0105, // 0045 Properties > Empty array of Properties
				0107, // 0046 Events > Empty array of Events
				0109, // 0047 Constructors > Empty array of Constructors
				0301, // 0048 Name > "DataTypeMethod"
				0000, // 0049
				0000, // 0050

				//--- DataTypePropertyDataType ---
				0011, // 0051 DataType > DataTypeDataType
				0021, // 0052 BaseType > DataTypeMemberDataType
				0101, // 0053 Fields > Empty array of Fields
				0103, // 0054 Methods > Empty array of Methods
				0105, // 0055 Properties > Empty array of Properties
				0107, // 0056 Events > Empty array of Events
				0109, // 0057 Constructors > Empty array of Constructors
				0321, // 0058 Name > "DataTypeProperty"
				0000, // 0059
				0000, // 0060

				//--- DataTypeEventDataType ---
				0011, // 0061 DataType > DataTypeDataType
				0021, // 0062 BaseType > DataTypeMemberDataType
				0101, // 0063 Fields > Empty array of Fields
				0103, // 0064 Methods > Empty array of Methods
				0105, // 0065 Properties > Empty array of Properties
				0107, // 0066 Events > Empty array of Events
				0109, // 0067 Constructors > Empty array of Constructors
				0341, // 0068 Name > "DataTypeEvent"
				0000, // 0069
				0000, // 0070

				//--- DataTypeConstructorDataType ---
				0011, // 0070 DataType > DataTypeDataType
				0021, // 0071 BaseType > DataTypeMemberDataType
				0101, // 0072 Fields > Empty array of Fields
				0103, // 0074 Methods > Empty array of Methods
				0105, // 0075 Properties > Empty array of Properties
				0107, // 0076 Events > Empty array of Events
				0109, // 0077 Constructors > Empty array of Constructors
				0361, // 0078 Name > "DataTypeConstructor"
				0000, // 0079
				0000, // 0080

				//--- IntegerDataType ---
				0011, // 0081 DataType > DataTypeDataType
				0001, // 0082 BaseType > ObjectDataType
				0101, // 0083 Fields > Empty array of Fields
				0103, // 0084 Methods > Empty array of Methods
				0105, // 0085 Properties > Empty array of Properties
				0107, // 0086 Events > Empty array of Events
				0109, // 0087 Constructors > Empty array of Constructors
				0391, // 0088 Name > "Integer"
				0000, // 0089
				0000, // 0090

				//--- ArrayDataType ---
				0011, // 0091 DataType > DataTypeDataType
				0001, // 0092 BaseType > ObjectDataType
				0131, // 0093 Fields > Array of array fields
				0103, // 0094 Methods > Empty array of Methods
				0105, // 0095 Properties > Empty array of Properties
				0107, // 0096 Events > Empty array of Events
				0109, // 0097 Constructors > Empty array of Constructors
				0401, // 0098 Name > "Array"
				0000, // 0099
				0000, // 0100

				//--- Empty arrays ---
				0091, // 0101 DataType > ArrayDataType // <DataTypeField>
				0000, // 0102 Length
				0091, // 0103 DataType > ArrayDataType // <DataTypeMethod>
				0000, // 0104 Length
				0091, // 0105 DataType > ArrayDataType // <DataTypeProperty>
				0000, // 0106 Length
				0091, // 0107 DataType > ArrayDataType // <DataTypeEvent>
				0000, // 0108 Length
				0091, // 0109 DataType > ArrayDataType // <DataTypeConstructor>
				0000, // 0110 Length

				//--- Array of object fields ---
				0091, // 0111 DataType > ArrayDataType
				0001, // 0112 Length
				0141, // 0113 Object DataType field
				0000, // 0114
				0000, // 0115
				0000, // 0116
				0000, // 0117
				0000, // 0118
				0000, // 0119
				0000, // 0120

				//--- Array of type fields ---
				0091, // 0121 DataType > ArrayDataType
				0006, // 0122 Length
				0151, // 0123 DataType BaseType field
				0161, // 0124 DataType Fields field
				0171, // 0125 DataType Methods field
				0181, // 0126 DataType Properties field
				0191, // 0127 DataType Events field
				0201, // 0128 DataType Constructors field
				0000, // 0129
				0000, // 0130

				//--- Array of array fields ---
				0091, // 0131 DataType > ArrayDataType
				0001, // 0132 Length
				0211, // 0133 Array Length field
				0000, // 0134
				0000, // 0135
				0000, // 0136
				0000, // 0137
				0000, // 0138
				0000, // 0139
				0000, // 0140

				//--- Object DataType field ---
				0031, // 0141 DataType > DataTypeFieldDataType
				0000, // 0142
				0000, // 0143
				0000, // 0144
				0000, // 0145
				0000, // 0146
				0000, // 0147
				0000, // 0148
				0000, // 0149
				0000, // 0150

				//--- DataType BaseType field ---
				0031, // 0151 DataType > DataTypeFieldDataType
				0000, // 0152
				0000, // 0153
				0000, // 0154
				0000, // 0155
				0000, // 0156
				0000, // 0157
				0000, // 0158
				0000, // 0159
				0000, // 0160

				//--- DataType Fields field ---
				0031, // 0161 DataType > DataTypeFieldDataType
				0000, // 0162
				0000, // 0163
				0000, // 0164
				0000, // 0165
				0000, // 0166
				0000, // 0167
				0000, // 0168
				0000, // 0169
				0000, // 0170

				//--- DataType Methods field ---
				0031, // 0171 DataType > DataTypeFieldDataType
				0000, // 0172
				0000, // 0173
				0000, // 0174
				0000, // 0175
				0000, // 0176
				0000, // 0177
				0000, // 0178
				0000, // 0179
				0000, // 0180

				//--- DataType Properties field ---
				0031, // 0181 DataType > DataTypeFieldDataType
				0000, // 0182
				0000, // 0183
				0000, // 0184
				0000, // 0185
				0000, // 0186
				0000, // 0187
				0000, // 0188
				0000, // 0189
				0000, // 0190

				//--- DataType Events field ---
				0031, // 0191 DataType > DataTypeFieldDataType
				0000, // 0192
				0000, // 0193
				0000, // 0194
				0000, // 0195
				0000, // 0196
				0000, // 0197
				0000, // 0198
				0000, // 0199
				0000, // 0200

				//--- DataType Constructors field ---
				0031, // 0201 DataType > DataTypeFieldDataType
				0000, // 0202
				0000, // 0203
				0000, // 0204
				0000, // 0205
				0000, // 0206
				0000, // 0207
				0000, // 0208
				0000, // 0209
				0000, // 0210

				//--- Array Length field ---
				0031, // 0211 DataType > DataTypeFieldDataType
				0000, // 0212
				0000, // 0213
				0000, // 0214
				0000, // 0215
				0000, // 0216
				0000, // 0217
				0000, // 0218
				0000, // 0219
				0000, // 0220

				//--- CharDataType ---
				0011, // 0221 DataType > DataTypeDataType
				0001, // 0222 BaseType > ObjectDataType
				0101, // 0223 Fields > Empty array of Fields
				0103, // 0224 Methods > Empty array of Methods
				0105, // 0225 Properties > Empty array of Properties
				0107, // 0226 Events > Empty array of Events
				0109, // 0227 Constructors > Empty array of Constructors
				0411, // 0228 Name > "Char"
				0000, // 0229
				0000, // 0230

				//--- StringDataType ---
				0011, // 0231 DataType > DataTypeDataType
				0091, // 0232 BaseType > ArrayDataType
				0101, // 0233 Fields > Empty array of Fields
				0103, // 0234 Methods > Empty array of Methods
				0105, // 0235 Properties > Empty array of Properties
				0107, // 0236 Events > Empty array of Events
				0109, // 0237 Constructors > Empty array of Constructors
				0421, // 0238 Name > "String"
				0000, // 0239
				0000, // 0240

				//--- String "Object" ---
				0000, // 0241
				0000, // 0242
				0000, // 0243
				0000, // 0244
				0000, // 0245
				0000, // 0246
				0000, // 0247
				0000, // 0248
				0000, // 0249
				0000, // 0250

				//--- String "DataType" ---
				0000, // 0251
				0000, // 0252
				0000, // 0253
				0000, // 0254
				0000, // 0255
				0000, // 0256
				0000, // 0257
				0000, // 0258
				0000, // 0259
				0000, // 0260

				//--- String "DataTypeMember" ---
				0000, // 0261
				0000, // 0262
				0000, // 0263
				0000, // 0264
				0000, // 0265
				0000, // 0266
				0000, // 0267
				0000, // 0268
				0000, // 0269
				0000, // 0270

				0000, // 0271
				0000, // 0272
				0000, // 0273
				0000, // 0274
				0000, // 0275
				0000, // 0276
				0000, // 0277
				0000, // 0278
				0000, // 0279
				0000, // 0280

				//--- String "DataTypeField" ---
				0000, // 0281
				0000, // 0282
				0000, // 0283
				0000, // 0284
				0000, // 0285
				0000, // 0286
				0000, // 0287
				0000, // 0288
				0000, // 0289
				0000, // 0290

				0000, // 0291
				0000, // 0292
				0000, // 0293
				0000, // 0294
				0000, // 0295
				0000, // 0296
				0000, // 0297
				0000, // 0298
				0000, // 0299
				0000, // 0300

				//--- String "DataTypeMethod" ---
				0000, // 0301
				0000, // 0302
				0000, // 0303
				0000, // 0304
				0000, // 0305
				0000, // 0306
				0000, // 0307
				0000, // 0308
				0000, // 0309
				0000, // 0310

				0000, // 0311
				0000, // 0312
				0000, // 0313
				0000, // 0314
				0000, // 0315
				0000, // 0316
				0000, // 0317
				0000, // 0318
				0000, // 0319
				0000, // 0320

				//--- String "DataTypeProperty" ---
				0000, // 0321
				0000, // 0322
				0000, // 0323
				0000, // 0324
				0000, // 0325
				0000, // 0326
				0000, // 0327
				0000, // 0328
				0000, // 0329
				0000, // 0330

				0000, // 0331
				0000, // 0332
				0000, // 0333
				0000, // 0334
				0000, // 0335
				0000, // 0336
				0000, // 0337
				0000, // 0338
				0000, // 0339
				0000, // 0340

				//--- String "DataTypeEvent" ---
				0000, // 0341
				0000, // 0342
				0000, // 0343
				0000, // 0344
				0000, // 0345
				0000, // 0346
				0000, // 0347
				0000, // 0348
				0000, // 0349
				0000, // 0350

				0000, // 0351
				0000, // 0352
				0000, // 0353
				0000, // 0354
				0000, // 0355
				0000, // 0356
				0000, // 0357
				0000, // 0358
				0000, // 0359
				0000, // 0360

				//--- String "DataTypeConstructor" ---
				0000, // 0361
				0000, // 0362
				0000, // 0363
				0000, // 0364
				0000, // 0365
				0000, // 0366
				0000, // 0367
				0000, // 0368
				0000, // 0369
				0000, // 0370

				0000, // 0371
				0000, // 0372
				0000, // 0373
				0000, // 0374
				0000, // 0375
				0000, // 0376
				0000, // 0377
				0000, // 0378
				0000, // 0379
				0000, // 0380

				0000, // 0381
				0000, // 0382
				0000, // 0383
				0000, // 0384
				0000, // 0385
				0000, // 0386
				0000, // 0387
				0000, // 0388
				0000, // 0389
				0000, // 0390

				//--- String "Integer" ---
				0000, // 0391
				0000, // 0392
				0000, // 0393
				0000, // 0394
				0000, // 0395
				0000, // 0396
				0000, // 0397
				0000, // 0398
				0000, // 0399
				0000, // 0400

				//--- String "Array" ---
				0000, // 0401
				0000, // 0402
				0000, // 0403
				0000, // 0404
				0000, // 0405
				0000, // 0406
				0000, // 0407
				0000, // 0408
				0000, // 0409
				0000, // 0410

				//--- String "Char" ---
				0000, // 0411
				0000, // 0412
				0000, // 0413
				0000, // 0414
				0000, // 0415
				0000, // 0416
				0000, // 0417
				0000, // 0418
				0000, // 0419
				0000, // 0420

				//--- String "String" ---
				0000, // 0421
				0000, // 0422
				0000, // 0423
				0000, // 0424
				0000, // 0425
				0000, // 0426
				0000, // 0427
				0000, // 0428
				0000, // 0429
				0000, // 0430
			}, 1);

			writeString(memory.Item1, 0241, "Object");
			writeString(memory.Item1, 0251, "DataType");
			writeString(memory.Item1, 0261, "DataTypeMember");
			writeString(memory.Item1, 0281, "DataTypeField");
			writeString(memory.Item1, 0301, "DataTypeMethod");
			writeString(memory.Item1, 0321, "DataTypeProperty");
			writeString(memory.Item1, 0341, "DataTypeEvent");
			writeString(memory.Item1, 0361, "DataTypeConstructor");
			writeString(memory.Item1, 0391, "Integer");
			writeString(memory.Item1, 0401, "Array");
			writeString(memory.Item1, 0411, "Char");
			writeString(memory.Item1, 0421, "String");

			return memory;
		}

		private static void writeString(MemoryWord[] array, MemoryOffset address, string text)
		{
			array[address] = 0231; // data type = String
			array[address + 1] = (MemoryWord) text.Length;
			int index = 0;
			foreach (char c in text)
			{
				array[address + 2 + index] = c;
				index++;
			}
		}
	}
}

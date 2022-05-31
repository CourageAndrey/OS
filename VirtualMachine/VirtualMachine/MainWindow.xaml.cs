using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			var memory = new Core.Memory(new MemoryWord[]
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
				0000, // 0008
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
				0000, // 0018
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
				0000, // 0028
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
				0000, // 0038
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
				0000, // 0048
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
				0000, // 0058
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
				0000, // 0068
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
				0000, // 0078
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
				0000, // 0088
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
				0000, // 0098
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
				0000, // 0228
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
				0000, // 0238
				0000, // 0239
				0000, // 0240
			}, 1);

			var text = new System.Text.StringBuilder();
			var array = memory.ObjectDataType.DataType.Fields;

			for (MemoryWord f = 0; f < array.Length.Value; f++)
			{
				text.AppendLine(array[(int) f].Tag);
			}

			Content = text;
		}
	}
}

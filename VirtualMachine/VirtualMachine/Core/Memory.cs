using System.Linq;

using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Memory
	{
		public const MemoryAddress RequiredSize = 431;

		#region Properties

		public readonly MemoryWord[] Cells;

		public MemoryAddress Size
		{ get { return Cells.Length; } }

		internal readonly System.Collections.Generic.IDictionary<MemoryAddress, Object> Objects;

		internal readonly System.Collections.Generic.IDictionary<string, DataType> Types;

		public readonly DataType
			ObjectDataType,

			DataTypeDataType,

			DataTypeMemberDataType,
			DataTypeFieldDataType,
			DataTypeMethodDataType,
			DataTypePropertyDataType,
			DataTypeEventDataType,
			DataTypeConstructorDataType,

			IntegerDataType,
			ArrayDataType,
			CharDataType,
			StringDataType;

		#endregion

		#region Constructors

		public Memory(MemoryWord[] cells, MemoryAddress objectDataTypeAddress)
		{
			// check and set cells
			if (cells == null)
			{
				throw new System.ArgumentNullException(nameof(cells));
			}
			else if (cells.Length < RequiredSize)
			{
				throw new System.ArgumentException($"{RequiredSize} memory cells required.", nameof(cells));
			}
			else if (cells[0] != 0)
			{
				throw new System.ArgumentException("First cell must contain 0 in order to represent NULL/ZERO.", nameof(cells));
			}
			else
			{
				Cells = cells;
			}

			// check root address
			if (objectDataTypeAddress < 0 || objectDataTypeAddress >= Size)
			{
				throw new System.ArgumentException("Invalid Object type address.", nameof(objectDataTypeAddress));
			}
			else if (objectDataTypeAddress == 0)
			{
				throw new System.NullReferenceException("Object type points to NULL.");
			}

			// prepare objects
			Objects = new System.Collections.Generic.Dictionary<MemoryAddress, Object>();

			// load root Object data type
			Objects[objectDataTypeAddress] = ObjectDataType = new DataType(this, objectDataTypeAddress);

			// load DataType data type
			MemoryAddress dataTypeDataTypeAddress = (MemoryAddress) cells[objectDataTypeAddress];
			Objects[dataTypeDataTypeAddress] = DataTypeDataType = new DataType(this, dataTypeDataTypeAddress);

			// load Reflection data types
#warning Temporary calculation (while members list is unfinished)
			const MemoryAddress initialObjectsOffset = 10;
			MemoryAddress
				dataTypeMemberDataTypeAddress = dataTypeDataTypeAddress + 1 * initialObjectsOffset,
				dataTypeFieldDataTypeAddress = dataTypeDataTypeAddress + 2 * initialObjectsOffset,
				dataTypeMethodDataTypeAddress = dataTypeDataTypeAddress + 3 * initialObjectsOffset,
				dataTypePropertyDataTypeAddress = dataTypeDataTypeAddress + 4 * initialObjectsOffset,
				dataTypeEventDataTypeAddress = dataTypeDataTypeAddress + 5 * initialObjectsOffset,
				dataTypeConstructorDataTypeAddress = dataTypeDataTypeAddress + 6 * initialObjectsOffset;
			Objects[dataTypeMemberDataTypeAddress] = DataTypeMemberDataType = new DataType(this, dataTypeMemberDataTypeAddress);
			Objects[dataTypeFieldDataTypeAddress] = DataTypeFieldDataType = new DataType(this, dataTypeFieldDataTypeAddress);
			Objects[dataTypeMethodDataTypeAddress] = DataTypeMethodDataType = new DataType(this, dataTypeMethodDataTypeAddress);
			Objects[dataTypePropertyDataTypeAddress] = DataTypePropertyDataType = new DataType(this, dataTypePropertyDataTypeAddress);
			Objects[dataTypeEventDataTypeAddress] = DataTypeEventDataType = new DataType(this, dataTypeEventDataTypeAddress);
			Objects[dataTypeConstructorDataTypeAddress] = DataTypeConstructorDataType = new DataType(this, dataTypeConstructorDataTypeAddress);

			// load int and array data types
			MemoryAddress integerDataTypeAddress = dataTypeDataTypeAddress + 7 * initialObjectsOffset;
			Objects[integerDataTypeAddress] = IntegerDataType = new DataType(this, integerDataTypeAddress);

			MemoryAddress arrayDataTypeAddress = dataTypeDataTypeAddress + 8 * initialObjectsOffset;
			Objects[arrayDataTypeAddress] = ArrayDataType = new DataType(this, arrayDataTypeAddress);

			// load char and string data types
			MemoryAddress charDataTypeAddress = dataTypeDataTypeAddress + 21 * initialObjectsOffset;
			Objects[charDataTypeAddress] = CharDataType = new DataType(this, charDataTypeAddress);

			MemoryAddress stringDataTypeAddress = dataTypeDataTypeAddress + 22 * initialObjectsOffset;
			Objects[stringDataTypeAddress] = StringDataType = new DataType(this, stringDataTypeAddress);

			// load empty arrays
			Array<DataTypeField> emptyArrayOfFields;
			Array<DataTypeMethod> emptyArrayOfMethods;
			Array<DataTypeProperty> emptyArrayOfProperties;
			Array<DataTypeEvent> emptyArrayOfEvents;
			Array<DataTypeConstructor> emptyArrayOfConstructors;
			MemoryAddress emptyArrayOfFieldsAddress = dataTypeDataTypeAddress + 9 * initialObjectsOffset;
			MemoryAddress emptyArrayOfMethodsAddress = emptyArrayOfFieldsAddress + 2;
			MemoryAddress emptyArrayOfPropertiesAddress = emptyArrayOfFieldsAddress + 4;
			MemoryAddress emptyArrayOfEventsAddress = emptyArrayOfFieldsAddress + 6;
			MemoryAddress emptyArrayOfConstructorsAddress = emptyArrayOfFieldsAddress + 8;
			Objects[emptyArrayOfFieldsAddress] = emptyArrayOfFields = new Array<DataTypeField>(this, emptyArrayOfFieldsAddress, DataTypeField.TotalFieldsCountOfDataTypeFieldClass);
			Objects[emptyArrayOfMethodsAddress] = emptyArrayOfMethods = new Array<DataTypeMethod>(this, emptyArrayOfMethodsAddress, DataTypeMethod.TotalFieldsCountOfDataTypeMethodClass);
			Objects[emptyArrayOfPropertiesAddress] = emptyArrayOfProperties = new Array<DataTypeProperty>(this, emptyArrayOfPropertiesAddress, DataTypeProperty.TotalFieldsCountOfDataTypePropertyClass);
			Objects[emptyArrayOfEventsAddress] = emptyArrayOfEvents = new Array<DataTypeEvent>(this, emptyArrayOfEventsAddress, DataTypeEvent.TotalFieldsCountOfDataTypeEventClass);
			Objects[emptyArrayOfConstructorsAddress] = emptyArrayOfConstructors = new Array<DataTypeConstructor>(this, emptyArrayOfConstructorsAddress, DataTypeConstructor.TotalFieldsCountOfDataTypeConstructorClass);

			// load fields arrays
			Array
				objectFieldsArray,
				typeFieldsArray,
				arrayFieldsArray;
			MemoryAddress
				arrayObjectFieldsAddress = dataTypeDataTypeAddress + 10 * initialObjectsOffset,
				arrayTypeFieldsAddress = dataTypeDataTypeAddress + 11 * initialObjectsOffset,
				arrayArrayFieldsAddress = dataTypeDataTypeAddress + 12 * initialObjectsOffset;
			Objects[arrayObjectFieldsAddress] = objectFieldsArray = new Array<DataTypeField>(this, arrayObjectFieldsAddress, DataTypeField.TotalFieldsCountOfDataTypeFieldClass);
			Objects[arrayTypeFieldsAddress] = typeFieldsArray = new Array<DataTypeField>(this, arrayTypeFieldsAddress, DataTypeField.TotalFieldsCountOfDataTypeFieldClass);
			Objects[arrayArrayFieldsAddress] = arrayFieldsArray = new Array<DataTypeField>(this, arrayArrayFieldsAddress, DataTypeField.TotalFieldsCountOfDataTypeFieldClass);

			// load fields
			DataTypeField
				objectDataTypeField,
				dataTypeBaseTypeField,
				dataTypeFieldsField,
				dataTypeMethodsField,
				dataTypePropertiesField,
				dataTypeEventsField,
				dataTypeConstructorsField,
				arrayLengthField;
			MemoryAddress
				objectDataTypeFieldAddress = dataTypeDataTypeAddress + 13 * initialObjectsOffset,
				dataTypeBaseTypeFieldAddress = dataTypeDataTypeAddress + 14 * initialObjectsOffset,
				dataTypeFieldsFieldAddress = dataTypeDataTypeAddress + 15 * initialObjectsOffset,
				dataTypeMethodsFieldAddress = dataTypeDataTypeAddress + 16 * initialObjectsOffset,
				dataTypePropertiesFieldAddress = dataTypeDataTypeAddress + 17 * initialObjectsOffset,
				dataTypeEventsFieldAddress = dataTypeDataTypeAddress + 18 * initialObjectsOffset,
				dataTypeConstructorsFieldAddress = dataTypeDataTypeAddress + 19 * initialObjectsOffset,
				arrayLengthFieldAddress = dataTypeDataTypeAddress + 20 * initialObjectsOffset;
			Objects[objectDataTypeFieldAddress] = objectDataTypeField = new DataTypeField(this, objectDataTypeFieldAddress);
			Objects[dataTypeBaseTypeFieldAddress] = dataTypeBaseTypeField = new DataTypeField(this, dataTypeBaseTypeFieldAddress);
			Objects[dataTypeFieldsFieldAddress] = dataTypeFieldsField = new DataTypeField(this, dataTypeFieldsFieldAddress);
			Objects[dataTypeMethodsFieldAddress] = dataTypeMethodsField = new DataTypeField(this, dataTypeMethodsFieldAddress);
			Objects[dataTypePropertiesFieldAddress] = dataTypePropertiesField = new DataTypeField(this, dataTypePropertiesFieldAddress);
			Objects[dataTypeEventsFieldAddress] = dataTypeEventsField = new DataTypeField(this, dataTypeEventsFieldAddress);
			Objects[dataTypeConstructorsFieldAddress] = dataTypeConstructorsField = new DataTypeField(this, dataTypeConstructorsFieldAddress);
			Objects[arrayLengthFieldAddress] = arrayLengthField = new DataTypeField(this, arrayLengthFieldAddress);

			String
				objectDataTypeName,
				dataTypeDataTypeName,
				dataTypeMemberDataTypeName,
				dataTypeFieldDataTypeName,
				dataTypeMethodDataTypeName,
				dataTypePropertyDataTypeName,
				dataTypeEventDataTypeName,
				dataTypeConstructorDataTypeName,
				integerDataTypeName,
				arrayDataTypeName,
				charDataTypeName,
				stringDataTypeName;
			MemoryAddress
				objectDataTypeNameAddress = dataTypeDataTypeAddress + 23 * initialObjectsOffset,
				dataTypeDataTypeNameAddress = dataTypeDataTypeAddress + 24 * initialObjectsOffset,
				dataTypeMemberDataTypeNameAddress = dataTypeDataTypeAddress + 25 * initialObjectsOffset,
				dataTypeFieldDataTypeNameAddress = dataTypeDataTypeAddress + 27 * initialObjectsOffset,
				dataTypeMethodDataTypeNameAddress = dataTypeDataTypeAddress + 29 * initialObjectsOffset,
				dataTypePropertyDataTypeNameAddress = dataTypeDataTypeAddress + 31 * initialObjectsOffset,
				dataTypeEventDataTypeNameAddress = dataTypeDataTypeAddress + 33 * initialObjectsOffset,
				dataTypeConstructorDataTypeNameAddress = dataTypeDataTypeAddress + 35 * initialObjectsOffset,
				integerDataTypeNameAddress = dataTypeDataTypeAddress + 38 * initialObjectsOffset,
				arrayDataTypeNameAddress = dataTypeDataTypeAddress + 39 * initialObjectsOffset,
				charDataTypeNameAddress = dataTypeDataTypeAddress + 40 * initialObjectsOffset,
				stringDataTypeNameAddress = dataTypeDataTypeAddress + 41 * initialObjectsOffset;
			Objects[objectDataTypeNameAddress] = objectDataTypeName = new String(this, objectDataTypeNameAddress);
			Objects[dataTypeDataTypeNameAddress] = dataTypeDataTypeName = new String(this, dataTypeDataTypeNameAddress);
			Objects[dataTypeMemberDataTypeNameAddress] = dataTypeMemberDataTypeName = new String(this, dataTypeMemberDataTypeNameAddress);
			Objects[dataTypeFieldDataTypeNameAddress] = dataTypeFieldDataTypeName = new String(this, dataTypeFieldDataTypeNameAddress);
			Objects[dataTypeMethodDataTypeNameAddress] = dataTypeMethodDataTypeName = new String(this, dataTypeMethodDataTypeNameAddress);
			Objects[dataTypePropertyDataTypeNameAddress] = dataTypePropertyDataTypeName = new String(this, dataTypePropertyDataTypeNameAddress);
			Objects[dataTypeEventDataTypeNameAddress] = dataTypeEventDataTypeName = new String(this, dataTypeEventDataTypeNameAddress);
			Objects[dataTypeConstructorDataTypeNameAddress] = dataTypeConstructorDataTypeName = new String(this, dataTypeConstructorDataTypeNameAddress);
			Objects[integerDataTypeNameAddress] = integerDataTypeName = new String(this, integerDataTypeNameAddress);
			Objects[arrayDataTypeNameAddress] = arrayDataTypeName = new String(this, arrayDataTypeNameAddress);
			Objects[charDataTypeNameAddress] = charDataTypeName = new String(this, charDataTypeNameAddress);
			Objects[stringDataTypeNameAddress] = stringDataTypeName = new String(this, stringDataTypeNameAddress);

			// tags for debug purpose
			ObjectDataType.Tag = ObjectDataType.Name.ToString();
			DataTypeDataType.Tag = DataTypeDataType.Name.ToString();
			DataTypeMemberDataType.Tag = DataTypeMemberDataType.Name.ToString();
			DataTypeFieldDataType.Tag = DataTypeFieldDataType.Name.ToString();
			DataTypeMethodDataType.Tag = DataTypeMethodDataType.Name.ToString();
			DataTypePropertyDataType.Tag = DataTypePropertyDataType.Name.ToString();
			DataTypeEventDataType.Tag = DataTypeEventDataType.Name.ToString();
			DataTypeConstructorDataType.Tag = DataTypeConstructorDataType.Name.ToString();
			IntegerDataType.Tag = IntegerDataType.Name.ToString();
			ArrayDataType.Tag = ArrayDataType.Name.ToString();
			CharDataType.Tag = CharDataType.Name.ToString();
			StringDataType.Tag = StringDataType.Name.ToString();

			emptyArrayOfFields.Tag = "Empty array of Fields";
			emptyArrayOfMethods.Tag = "Empty array of Methods";
			emptyArrayOfProperties.Tag = "Empty array of Properties";
			emptyArrayOfEvents.Tag = "Empty array of Events";
			emptyArrayOfConstructors.Tag = "Empty array of Constructors";
			objectFieldsArray.Tag = "Array of Object type fields";
			typeFieldsArray.Tag = "Array of DataType type fields";
			arrayFieldsArray.Tag = "Array of Array type fields";

			objectDataTypeField.Tag = "object DataType Field";
			dataTypeBaseTypeField.Tag = "DataType BaseType Field";
			dataTypeFieldsField.Tag = "DataType Fields Field";
			dataTypeMethodsField.Tag = "DataType Methods Field";
			dataTypePropertiesField.Tag = "DataType Properties Field";
			dataTypeEventsField.Tag = "DataType Events Field";
			dataTypeConstructorsField.Tag = "DataType Constructors Field";
			arrayLengthField.Tag = "Array Length Field";

			// types list
			Types = Objects.Values.ToList().OfType<DataType>().ToDictionary(
				type => type.Name.ToString(),
				type => type);
		}

		#endregion

		public ObjectT GetObject<ObjectT>(MemoryAddress address)
			where ObjectT : Object
		{
			if (address < 0 || address >= Size)
			{
				throw new System.Exception("Invalid memory address.");
			}

			if (typeof(Structure).IsAssignableFrom(typeof(ObjectT)))
			{
				if (typeof(ObjectT) == typeof(Integer))
				{
					return new Integer(this, address) as ObjectT;
				}
				else if (typeof(ObjectT) == typeof(Char))
				{
					return new Char(this, address) as ObjectT;
				}
				else
				{
					throw new System.NotSupportedException();
				}
			}
			else
			{
				address = (MemoryAddress) Cells[address];

				if (address == 0)
				{
					return null;
				}

				Object result;
				if (Objects.TryGetValue(address, out result))
				{
					if (typeof(ObjectT).IsAssignableFrom(result.GetType()))
					{
						return result as ObjectT;
					}
					else
					{
						throw new System.InvalidCastException($"Impossible to cast {typeof(ObjectT)} to {result.GetType()}.");
					}
				}
				else
				{
					throw new System.Exception("There is no object with such address.");
				}
			}
		}

		public MemoryAddress GetNextFreeAddress()
		{
			MemoryAddress lastStartAddress = Objects.Keys.Max();
			var lastObject  = Objects[lastStartAddress];
			return lastStartAddress + lastObject.GetDataSize();
		}
	}
}

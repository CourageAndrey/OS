using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Memory
	{
#warning This value has to be calculated.
		public const MemoryAddress RequiredSize = 1;

		#region Properties

		public readonly MemoryWord[] Cells;

		public MemoryAddress Size
		{ get { return Cells.Length; } }

		private readonly System.Collections.Generic.IDictionary<MemoryAddress, Object> _objects;

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
			ArrayDataType;

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
			_objects = new System.Collections.Generic.Dictionary<MemoryAddress, Object>();

			// load root Object data type
			_objects[objectDataTypeAddress] = ObjectDataType = new DataType(this, objectDataTypeAddress);

			// load DataType data type
			MemoryAddress dataTypeDataTypeAddress = (MemoryAddress) cells[objectDataTypeAddress];
			_objects[dataTypeDataTypeAddress] = DataTypeDataType = new DataType(this, dataTypeDataTypeAddress);

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
			_objects[dataTypeMemberDataTypeAddress] = DataTypeMemberDataType = new DataType(this, dataTypeMemberDataTypeAddress);
			_objects[dataTypeFieldDataTypeAddress] = DataTypeFieldDataType = new DataType(this, dataTypeFieldDataTypeAddress);
			_objects[dataTypeMethodDataTypeAddress] = DataTypeMethodDataType = new DataType(this, dataTypeMethodDataTypeAddress);
			_objects[dataTypePropertyDataTypeAddress] = DataTypePropertyDataType = new DataType(this, dataTypePropertyDataTypeAddress);
			_objects[dataTypeEventDataTypeAddress] = DataTypeEventDataType = new DataType(this, dataTypeEventDataTypeAddress);
			_objects[dataTypeConstructorDataTypeAddress] = DataTypeConstructorDataType = new DataType(this, dataTypeConstructorDataTypeAddress);

			// load int and array data types
			MemoryAddress integerDataTypeAddress = dataTypeDataTypeAddress + 7 * initialObjectsOffset;
			_objects[integerDataTypeAddress] = IntegerDataType = new DataType(this, integerDataTypeAddress);

			MemoryAddress arrayDataTypeAddress = dataTypeDataTypeAddress + 8 * initialObjectsOffset;
			_objects[arrayDataTypeAddress] = ArrayDataType = new DataType(this, arrayDataTypeAddress);

			// load empty array
			Array emptyArray;
			MemoryAddress emptyArrayAddress = dataTypeDataTypeAddress + 9 * initialObjectsOffset;
			_objects[emptyArrayAddress] = emptyArray = new Array(this, emptyArrayAddress);

			// load fields arrays
			Array
				objectFieldsArray,
				typeFieldsArray,
				arrayFieldsArray;
			MemoryAddress
				arrayObjectFieldsAddress = dataTypeDataTypeAddress + 10 * initialObjectsOffset,
				arrayTypeFieldsAddress = dataTypeDataTypeAddress + 11 * initialObjectsOffset,
				arrayArrayFieldsAddress = dataTypeDataTypeAddress + 12 * initialObjectsOffset;
			_objects[arrayObjectFieldsAddress] = objectFieldsArray = new Array(this, arrayObjectFieldsAddress);
			_objects[arrayTypeFieldsAddress] = typeFieldsArray = new Array(this, arrayTypeFieldsAddress);
			_objects[arrayArrayFieldsAddress] = arrayFieldsArray = new Array(this, arrayArrayFieldsAddress);

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
			_objects[objectDataTypeFieldAddress] = objectDataTypeField = new DataTypeField(this, objectDataTypeFieldAddress);
			_objects[dataTypeBaseTypeFieldAddress] = dataTypeBaseTypeField = new DataTypeField(this, dataTypeBaseTypeFieldAddress);
			_objects[dataTypeFieldsFieldAddress] = dataTypeFieldsField = new DataTypeField(this, dataTypeFieldsFieldAddress);
			_objects[dataTypeMethodsFieldAddress] = dataTypeMethodsField = new DataTypeField(this, dataTypeMethodsFieldAddress);
			_objects[dataTypePropertiesFieldAddress] = dataTypePropertiesField = new DataTypeField(this, dataTypePropertiesFieldAddress);
			_objects[dataTypeEventsFieldAddress] = dataTypeEventsField = new DataTypeField(this, dataTypeEventsFieldAddress);
			_objects[dataTypeConstructorsFieldAddress] = dataTypeConstructorsField = new DataTypeField(this, dataTypeConstructorsFieldAddress);
			_objects[arrayLengthFieldAddress] = arrayLengthField = new DataTypeField(this, arrayLengthFieldAddress);

			// tags for debug purpose
			ObjectDataType.Tag = "Object DataType";
			DataTypeDataType.Tag = "DataType DataType";
			DataTypeMemberDataType.Tag = "DataTypeMember DataType";
			DataTypeFieldDataType.Tag = "DataTypeField DataType";
			DataTypeMethodDataType.Tag = "DataTypeMethod DataType";
			DataTypePropertyDataType.Tag = "DataTypeProperty DataType";
			DataTypeEventDataType.Tag = "DataTypeEvent DataType";
			DataTypeConstructorDataType.Tag = "DataTypeConstructor DataType";
			IntegerDataType.Tag = "Integer DataType";
			ArrayDataType.Tag = "Array DataType";

			emptyArray.Tag = "Empty array";
			objectFieldsArray.Tag = "Array of fields";
			typeFieldsArray.Tag = "Array of fields";
			arrayFieldsArray.Tag = "Array of fields";

			objectDataTypeField.Tag = "object DataType Field";
			dataTypeBaseTypeField.Tag = "DataType BaseType Field";
			dataTypeFieldsField.Tag = "DataType Fields Field";
			dataTypeMethodsField.Tag = "DataType Methods Field";
			dataTypePropertiesField.Tag = "DataType Properties Field";
			dataTypeEventsField.Tag = "DataType Events Field";
			dataTypeConstructorsField.Tag = "DataType Constructors Field";
			arrayLengthField.Tag = "Array Length Field";
		}

		#endregion

		public ObjectT GetObject<ObjectT>(MemoryAddress address)
			where ObjectT : Object
		{
			if (address < 0 || address >= Size)
			{
				throw new System.Exception("Invalid memory address.");
			}

			if (address == 0)
			{
				return null;
			}

			Object result;
			if (_objects.TryGetValue(address, out result))
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
}

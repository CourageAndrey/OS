using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class Memory
	{
		#region Properties

		public const int FreeMemoryCount = 512;

		public MemoryWord[] Cells
		{ get; private set; }

		public MemoryAddress Size
		{ get { return Cells.Length; } }

		public System.Collections.Generic.IDictionary<MemoryAddress, MemoryObject> Objects { get; }
		public System.Collections.Generic.IDictionary<string, DataType> Types { get; }
		public MemoryAddress NextFreeAddress { get; internal set; }
		public readonly System.Collections.Generic.IDictionary<System.Type, DataType> ConcreteTypesMapping;

		public readonly DataType
			ObjectDataType,

			DataTypeDataType,
		
			DataTypeMemberDataType,
			DataTypeFieldDataType,
			DataTypeMethodDataType,
			DataTypePropertyDataType,
			DataTypeEventDataType,
			DataTypeConstructorDataType,
		
			StructureDataType,

			IntegerDataType,
			ArrayDataType,
			CharDataType,
			StringDataType;

		#endregion

		#region Constructors

		public Memory()
		{
			// memory own variables
			Cells = System.Array.Empty<MemoryWord>();
			Objects = new System.Collections.Generic.Dictionary<MemoryAddress, MemoryObject>();
			Types = new System.Collections.Generic.Dictionary<string, DataType>();
			NextFreeAddress = -1;

			// data types
			Types["Object"] = ObjectDataType = new DataType();
			DataTypeDataType = new DataType(this);

			DataTypeMemberDataType = new DataType(this, "DataTypeMember", ObjectDataType);
			DataTypeFieldDataType = new DataType(this, "DataTypeField", DataTypeMemberDataType);
			DataTypeMethodDataType = new DataType(this, "DataTypeMethod", DataTypeMemberDataType);
			DataTypePropertyDataType = new DataType(this, "DataTypeProperty", DataTypeMemberDataType);
			DataTypeEventDataType = new DataType(this, "DataTypeEvent", DataTypeMemberDataType);
			DataTypeConstructorDataType = new DataType(this, "DataTypeConstructor", DataTypeMemberDataType);

			StructureDataType = new DataType(this, "Structure", ObjectDataType);
			IntegerDataType = new DataType(this, "Integer", StructureDataType, new[] { new DataTypeField("#value") });
			ArrayDataType = new DataType(this, "Array", ObjectDataType, new[] { new DataTypeField("Length") });
			CharDataType = new DataType(this, "Char", StructureDataType, new[] { new DataTypeField("#value") });
			StringDataType = new DataType(this, "String", ArrayDataType);

			ConcreteTypesMapping = new System.Collections.Generic.Dictionary<System.Type, DataType>
			{
				{ typeof(ReferencedObject), ObjectDataType },
				{ typeof(DataType), DataTypeDataType },
				{ typeof(DataTypeField), DataTypeFieldDataType },
				{ typeof(DataTypeMethod), DataTypeMethodDataType },
				{ typeof(DataTypeProperty), DataTypePropertyDataType },
				{ typeof(DataTypeEvent), DataTypeEventDataType },
				{ typeof(DataTypeConstructor), DataTypeConstructorDataType },
				{ typeof(Integer), IntegerDataType },
				{ typeof(Char), CharDataType },
				{ typeof(String), StringDataType },
			};
		}

		#endregion

		public MemoryAddress Allocate(MemoryObject variable)
		{
			if (variable == null)
			{
				throw new System.ArgumentNullException(nameof(variable));
			}

			variable.Address = NextFreeAddress;
			variable.Memory = this;

#warning Separate ref-size and data size
			NextFreeAddress += (MemoryAddress) variable.VariableSize;

			return variable.Address;
		}

		public void Serialize()
		{
			var data = DataType.SerializeDataTypes(this);
			NextFreeAddress = data.Count;
			data.AddRange(new MemoryWord[FreeMemoryCount]);
			Cells = data.ToArray();
		}
	}
}

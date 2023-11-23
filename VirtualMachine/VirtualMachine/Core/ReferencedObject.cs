using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class ReferencedObject : MemoryObject
	{
		#region Properties

		protected internal const MemoryOffset ObjectFieldsCount = 1;
		protected internal const MemoryOffset ObjectFieldsTotalCount = ObjectFieldsCount;
		protected internal const MemoryOffset FieldOffsetType = 0;

		public override MemoryWord ReferencedDataSize
		{ get { return GetDataType().GetDataSize(); } }

		#endregion

		#region Constructors



		#endregion

		public sealed override DataType GetDataType()
		{
			return IsInMemory
				? GetFieldValue<DataType>(FieldOffsetType)
				: null;
		}

		internal sealed override System.Collections.Generic.List<MemoryWord> Serialize(Memory memory, MemoryAddress address)
		{
			memory.Objects[address] = this;

			var data = new System.Collections.Generic.List<MemoryWord>();
			data.AddRange(SerializeReferencedData(memory, address));

			Memory = memory;
			Address = address;

			return data;
		}

		protected virtual System.Collections.Generic.List<MemoryWord> SerializeReferencedData(Memory memory, MemoryAddress address)
		{
			var dataType = this.GetDataTypeNotNull(memory);

			return new System.Collections.Generic.List<MemoryWord> { dataType.IsInMemory ? (MemoryWord) dataType.Address : 0 };
		}

		public override string ToString()
		{
			return GetDataType().ToString();
		}
	}

	internal static class SerializingHelper
	{
		public static DataType GetDataTypeNotNull(this ReferencedObject instance, Memory memory)
		{
			var type = instance.GetDataType();

			if (type == null)
			{
				var types = new System.Collections.Generic.Dictionary<System.Type, DataType>
				{
					{ typeof(DataType), memory.DataTypeDataType },
					{ typeof(DataTypeField), memory.DataTypeFieldDataType },
					{ typeof(DataTypeMethod), memory.DataTypeMethodDataType },
					{ typeof(DataTypeProperty), memory.DataTypePropertyDataType },
					{ typeof(DataTypeEvent), memory.DataTypeEventDataType },
					{ typeof(DataTypeConstructor), memory.DataTypeConstructorDataType },
					{ typeof(Array), memory.ArrayDataType },
					{ typeof(String), memory.StringDataType },
				};
				// NOT NECESSARY: memory.ObjectDataType },
				// NOT NECESSARY: memory.StructureDataType },
				// NOT NECESSARY: memory.DataTypeMemberDataType },
				// NOT NECESSARY: memory.IntegerDataType },
				// NOT NECESSARY: memory.CharDataType },

				var instanceType = instance.GetType();
				if (!types.TryGetValue(instanceType, out type))
				{
					foreach (var typePair in types)
					{
						if (typePair.Key.IsAssignableFrom(instance.GetType()))
						{
							type = typePair.Value;
							break;
						}
					}
				}
			}

			return type;
		}
	}
}

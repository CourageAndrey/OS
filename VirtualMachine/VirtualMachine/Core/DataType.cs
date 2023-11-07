using System;
using System.Linq;

using VirtualMachine.Reflection;

using MemoryAddress = System.Int32;
using MemoryOffset = System.Int32;
using MemoryWord = System.UInt64;

namespace VirtualMachine.Core
{
	public class DataType : ReferencedObject
	{
		#region Properties

		protected const MemoryOffset DataTypeFieldsCount = 7;
		protected const MemoryOffset DataTypeFieldsTotalCount = ObjectFieldsCount + DataTypeFieldsCount;
		protected const MemoryOffset FieldOffsetBaseType = ObjectFieldsCount + 0;
		protected const MemoryOffset FieldOffsetName = ObjectFieldsCount + 1;
		protected const MemoryOffset FieldOffsetFields = ObjectFieldsCount + 2;
		protected const MemoryOffset FieldOffsetMethods = ObjectFieldsCount + 3;
		protected const MemoryOffset FieldOffsetProperties = ObjectFieldsCount + 4;
		protected const MemoryOffset FieldOffsetEvents = ObjectFieldsCount + 5;
		protected const MemoryOffset FieldOffsetConstructors = ObjectFieldsCount + 6;

		private readonly DataType _baseType;
		private readonly String _name;
		private readonly Array<DataTypeField> _fields;
		private readonly Array<DataTypeMethod> _methods;
		private readonly Array<DataTypeProperty> _properties;
		private readonly Array<DataTypeEvent> _events;
		private readonly Array<DataTypeConstructor> _constructors;

		public DataType BaseType
		{
			get
			{
				return IsInMemory
					? GetFieldValue<DataType>(FieldOffsetBaseType)
					: _baseType;
			}
		}

		public String Name
		{
			get
			{
				return IsInMemory
					? GetFieldValue<String>(FieldOffsetName)
					: _name;
			}
		}

		public Array<DataTypeField> Fields
		{
			get
			{
				return IsInMemory
					? GetFieldValue<Array<DataTypeField>>(FieldOffsetFields)
					: _fields;
			}
		}

		public Array<DataTypeMethod> Methods
		{
			get
			{
				return IsInMemory
					? GetFieldValue<Array<DataTypeMethod>>(FieldOffsetMethods)
					: _methods;
			}
		}

		public Array<DataTypeProperty> Properties
		{
			get
			{
				return IsInMemory
					? GetFieldValue<Array<DataTypeProperty>>(FieldOffsetProperties)
					: _properties;
			}
		}

		public Array<DataTypeEvent> Events
		{
			get
			{
				return IsInMemory
					? GetFieldValue<Array<DataTypeEvent>>(FieldOffsetEvents)
					: _events;
			}
		}

		public Array<DataTypeConstructor> Constructors
		{
			get
			{
				return IsInMemory
					? GetFieldValue<Array<DataTypeConstructor>>(FieldOffsetConstructors)
					: _constructors;
			}
		}

		#endregion

		#region Constructors

		private DataType(
			string name,
			DataType baseType,
			DataTypeField[] fields = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException(nameof(name));
			}

			_baseType = baseType;
			_name = new String(Tag = name);
			_fields = new Array<DataTypeField>(fields ?? new DataTypeField[0]);
			_methods = new Array<DataTypeMethod>(new DataTypeMethod[0]);
			_properties = new Array<DataTypeProperty>(new DataTypeProperty[0]);
			_events = new Array<DataTypeEvent>(new DataTypeEvent[0]);
			_constructors = new Array<DataTypeConstructor>(new DataTypeConstructor[0]);
		}

		internal DataType()
			: this("Object", null, new[] { new DataTypeField("#type") })
		{ }

		internal DataType(Memory memory)
			: this("DataType", memory.ObjectDataType, new[]
			{
				new DataTypeField("BaseType"),
				new DataTypeField("Name"),
				new DataTypeField("Fields"),
				new DataTypeField("Methods"),
				new DataTypeField("Properties"),
				new DataTypeField("Events"),
				new DataTypeField("Constructors"),
			})
		{
			memory.Types[Tag] = this;
		}

		internal DataType(Memory memory, string name, DataType baseType, DataTypeField[] fields = null)
			: this(name, baseType, fields)
		{
			memory.Types[name] = this;
		}

		#endregion

		public System.Collections.Generic.ICollection<DataTypeField> GetAllFields()
		{
			var list = new System.Collections.Generic.List<DataTypeField>();
			if (BaseType != null && Tag != "Structure" /* because structures don't have base types */)
			{
				list.AddRange(BaseType.GetAllFields());
			}
			list.AddRange(Fields);
			return list;
		}

		internal static System.Collections.Generic.List<MemoryWord> SerializeDataTypes(Memory memory)
		{
			var data = new System.Collections.Generic.List<MemoryWord>(new MemoryWord[1]); // null-pointer at address [0x00000000]

			MemoryAddress objectTypeAddress = data.Count;
			data.AddRange(memory.ObjectDataType.Serialize(memory, objectTypeAddress));
			MemoryAddress typeTypeAddress = data.Count;
			data.AddRange(memory.DataTypeDataType.Serialize(memory, typeTypeAddress));
			data[objectTypeAddress] = (MemoryWord) typeTypeAddress;
			data[typeTypeAddress] = (MemoryWord) typeTypeAddress;

			foreach (var type in memory.Types.Values.Except(new[] { memory.ObjectDataType, memory.DataTypeDataType }))
			{
				data.AddRange(type.Serialize(memory, data.Count));
			}

			return data;
		}

		protected override System.Collections.Generic.List<MemoryWord> SerializeReferencedData(Memory memory, MemoryAddress address)
		{
			var data = base.SerializeReferencedData(memory, address);

			var nameData = Name.Serialize(memory, address + DataTypeFieldsTotalCount);
			var fieldsData = Fields.Serialize(memory, Name.Address + nameData.Count);
			var methodsData = Methods.Serialize(memory, Fields.Address + fieldsData.Count);
			var propertiesData = Properties.Serialize(memory, Methods.Address + methodsData.Count);
			var eventsData = Events.Serialize(memory, Properties.Address + propertiesData.Count);
			var constructorsData = Constructors.Serialize(memory, Events.Address + eventsData.Count);

			data.Add((MemoryWord) (BaseType != null ? BaseType.Address : 0));
			data.Add((MemoryWord) Name.Address);
			data.Add((MemoryWord) Fields.Address);
			data.Add((MemoryWord) Methods.Address);
			data.Add((MemoryWord) Properties.Address);
			data.Add((MemoryWord) Events.Address);
			data.Add((MemoryWord) Constructors.Address);

			data.AddRange(nameData);
			data.AddRange(fieldsData);
			data.AddRange(methodsData);
			data.AddRange(propertiesData);
			data.AddRange(eventsData);
			data.AddRange(constructorsData);

			return data;
		}
	}

	public static class DataTypeExtensions
	{
		public static MemoryWord GetDataSize(this DataType type)
		{
			return (MemoryWord) type.GetAllFields().Count;
		}
	}
}

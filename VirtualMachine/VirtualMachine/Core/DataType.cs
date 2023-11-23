﻿using System;
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

		protected internal const MemoryOffset DataTypeFieldsCount = 7;
		protected internal const MemoryOffset DataTypeFieldsTotalCount = ObjectFieldsTotalCount + DataTypeFieldsCount;
		protected internal const MemoryOffset FieldOffsetBaseType = ObjectFieldsTotalCount + 0;
		protected internal const MemoryOffset FieldOffsetName = ObjectFieldsTotalCount + 1;
		protected internal const MemoryOffset FieldOffsetFields = ObjectFieldsTotalCount + 2;
		protected internal const MemoryOffset FieldOffsetMethods = ObjectFieldsTotalCount + 3;
		protected internal const MemoryOffset FieldOffsetProperties = ObjectFieldsTotalCount + 4;
		protected internal const MemoryOffset FieldOffsetEvents = ObjectFieldsTotalCount + 5;
		protected internal const MemoryOffset FieldOffsetConstructors = ObjectFieldsTotalCount + 6;

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

			var arrayTypeAddress = (MemoryWord) memory.ArrayDataType.Address;
			var stringTypeAddress = (MemoryWord) memory.StringDataType.Address;
			var fieldTypeAddress = (MemoryWord) memory.DataTypeFieldDataType.Address;
			foreach (var type in memory.Types.Values)
			{
				SetArrayTypeAddress(data, type.Address, arrayTypeAddress);
				SetStringTypeAddress(data, type.Address, stringTypeAddress);
				SetFieldTypeAddress(data, type.Address, fieldTypeAddress);
			}

			return data;
		}

		private static void SetArrayTypeAddress(System.Collections.Generic.IList<MemoryWord> data, MemoryAddress typeAddress, MemoryWord arrayTypeAddress)
		{
			data[(MemoryAddress) data[typeAddress + 3]] = arrayTypeAddress;
			data[(MemoryAddress) data[typeAddress + 4]] = arrayTypeAddress;
			data[(MemoryAddress) data[typeAddress + 5]] = arrayTypeAddress;
			data[(MemoryAddress) data[typeAddress + 6]] = arrayTypeAddress;
			data[(MemoryAddress) data[typeAddress + 7]] = arrayTypeAddress;
		}

		private static void SetStringTypeAddress(System.Collections.Generic.IList<MemoryWord> data, MemoryAddress typeAddress, MemoryWord stringTypeAddress)
		{
			data[(MemoryAddress) data[typeAddress + 2]] = stringTypeAddress;
		}

		private static void SetFieldTypeAddress(System.Collections.Generic.IList<MemoryWord> data, MemoryAddress typeAddress, MemoryWord fieldTypeAddress)
		{
			var arrayAddress = (MemoryAddress) data[typeAddress + 3];
			var arrayLength = data[arrayAddress + 1];
			for (MemoryWord i = 0; i < arrayLength; i++)
			{
				data[(MemoryAddress) data[arrayAddress + 2 + (MemoryAddress) i]] = fieldTypeAddress;
			}
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

		public override string ToString()
		{
			return Name.ToString();
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

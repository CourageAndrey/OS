using VirtualMachine.Core.DataTypes;

namespace VirtualMachine.Core.Reflection
{
	public class Class : Object
	{
		public String Name
		{ get { return _name; } }

		public String Namespace
		{ get { return _namespace; } }

		public String FullName
		{ get { return _namespace.Concat(NamespaceSeparator).Concat(_name); } }

		public Collection<Field> Fields
		{ get { return _fields; } }

		public Collection<Property> Properties
		{ get { return _properties; } }

		public Collection<Method> Methods
		{ get { return _methods; } }

		public Collection<Event> Events
		{ get { return _events; } }

		public Collection<Constructor> Constructors
		{ get { return _constructors; } }

		public Class Ancestor
		{ get { return _ancestor; } }

		public Boolean IsAbstract
		{ get { return _isAbstract; } }

		public Object DefaultValue
		{ get { return _defaultValue; } }

		private readonly String _name;
		private readonly String _namespace;
		private readonly Collection<Field> _fields;
		private readonly Collection<Property> _properties;
		private readonly Collection<Method> _methods;
		private readonly Collection<Event> _events;
		private readonly Collection<Constructor> _constructors;
		private readonly Class _ancestor;
		private readonly Boolean _isAbstract;
		private readonly Object _defaultValue;

		private Class(
			String name,
			String @namespace,
			Collection<Field> fields,
			Collection<Property> properties,
			Collection<Method> methods,
			Collection<Event> events,
			Collection<Constructor> constructors,
			Class ancestor,
			Boolean isAbstract,
			Object defaultValue)
			: base(ClassClass)
		{
			_name = name;
			_namespace = @namespace;
			_fields = fields;
			_properties = properties;
			_methods = methods;
			_events = events;
			_constructors = constructors;
			_ancestor = ancestor;
			_isAbstract = isAbstract;
			_defaultValue = defaultValue;
		}

		public override String ToString()
		{
			return FullName;
		}

		public static readonly Class ObjectClass;
		public static readonly Class ClassClass;
		public static readonly Class ClassMemberClass;
		public static readonly Class FieldClass;
		public static readonly Class PropertyClass;
		public static readonly Class MethodClass;
		public static readonly Class EventClass;
		public static readonly Class ConstructorClass;
		public static readonly Class StringClass;
		public static readonly Class BooleanClass;
		public static readonly Class IntegerClass;
		public static readonly Class CollectionClass;
		public static readonly Class ExceptionClass;

		static Class()
		{
			var emptyFields = new Collection<Field>();
			var emptyMethods = new Collection<Method>();
			var emptyEvents = new Collection<Event>();
			var emptyConstructors = new Collection<Constructor>();

			var booleanProperties = new System.Collections.Generic.List<Property>();
			var classProperties = new System.Collections.Generic.List<Property>();
			var classMemberProperties = new System.Collections.Generic.List<Property>();
			var collectionProperties = new System.Collections.Generic.List<Property>();
			var constructorProperties = new System.Collections.Generic.List<Property>();
			var eventProperties = new System.Collections.Generic.List<Property>();
			var fieldProperties = new System.Collections.Generic.List<Property>();
			var integerProperties = new System.Collections.Generic.List<Property>();
			var methodProperties = new System.Collections.Generic.List<Property>();
			var objectProperties = new System.Collections.Generic.List<Property>();
			var propertyProperties = new System.Collections.Generic.List<Property>();
			var stringProperties = new System.Collections.Generic.List<Property>();
			var exceptionProperties = new System.Collections.Generic.List<Property>();
		
			ObjectClass = new Class(
				new String("Object"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(objectProperties), 
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				null,
				Boolean.False,
				Object.Null);
		
			ClassClass = new Class(
				new String("Class"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(classProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				Object.Null);
			ClassMemberClass = new Class(
				new String("ClassMember"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(classMemberProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.True,
				Object.Null);
			FieldClass = new Class(
				new String("Field"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(fieldProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ClassMemberClass,
				Boolean.False,
				Object.Null);
			PropertyClass = new Class(
				new String("Property"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(propertyProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ClassMemberClass,
				Boolean.False,
				Object.Null);
			MethodClass = new Class(
				new String("Method"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(methodProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ClassMemberClass,
				Boolean.False,
				Object.Null);
			EventClass = new Class(
				new String("Event"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(eventProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ClassMemberClass,
				Boolean.False,
				Object.Null);
			ConstructorClass = new Class(
				new String("Constructor"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(constructorProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ClassMemberClass,
				Boolean.False,
				Object.Null);

			StringClass = new Class(
				new String("String"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(stringProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				String.Empty);
			BooleanClass = new Class(
				new String("Boolean"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(booleanProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				Boolean.False);
			IntegerClass = new Class(
				new String("Integer"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(integerProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				Integer.Zero);

			CollectionClass = new Class(
				new String("Collection"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(collectionProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				Object.Null);

			ExceptionClass = new Class(
				new String("Exception"),
				new String("Core"),
				emptyFields,
				new Collection<Property>(exceptionProperties),
				emptyMethods,
				emptyEvents,
				emptyConstructors,
				ObjectClass,
				Boolean.False,
				Object.Null);

			classProperties.Add(new Property(new String("Name"), ClassClass, StringClass, @class => ((Class) @class).Name, null));
			classProperties.Add(new Property(new String("Namespace"), ClassClass, StringClass, @class => ((Class) @class).Namespace, null));
			classProperties.Add(new Property(new String("FullName"), ClassClass, StringClass, @class => ((Class) @class).FullName, null));
			classProperties.Add(new Property(new String("Fields"), ClassClass, CollectionClass, @class => ((Class) @class).Fields, null));
			classProperties.Add(new Property(new String("Properties"), ClassClass, CollectionClass, @class => ((Class) @class).Properties, null));
			classProperties.Add(new Property(new String("Methods"), ClassClass, CollectionClass, @class => ((Class) @class).Methods, null));
			classProperties.Add(new Property(new String("Events"), ClassClass, CollectionClass, @class => ((Class) @class).Events, null));
			classProperties.Add(new Property(new String("Constructors"), ClassClass, CollectionClass, @class => ((Class) @class).Constructors, null));
			classProperties.Add(new Property(new String("Ancestor"), ClassClass, ClassClass, @class => ((Class) @class).Ancestor, null));
			classProperties.Add(new Property(new String("IsAbstract"), ClassClass, BooleanClass, @class => ((Class) @class).IsAbstract, null));

			classMemberProperties.Add(new Property(new String("Name"), ClassMemberClass, StringClass, member => ((ClassMember) member).Name, null));
			classMemberProperties.Add(new Property(new String("OfClass"), ClassMemberClass, ClassClass, member => ((ClassMember)member).OfClass, null));

			fieldProperties.Add(new Property(new String("DataType"), FieldClass, ClassClass, field => ((Field) field).DataType, null));

			propertyProperties.Add(new Property(new String("DataType"), PropertyClass, ClassClass, property => ((Property) property).DataType, null));

			stringProperties.Add(new Property(new String("Length"), StringClass, IntegerClass, @string => ((String) @string).Length, null));

			collectionProperties.Add(new Property(new String("Count"), CollectionClass, IntegerClass, collection => ((Collection) collection).Count, null));

			exceptionProperties.Add(new Property(new String("Message"), ExceptionClass, StringClass, exception => ((Exception) exception).Message, null));
		}

		public static readonly String NamespaceSeparator = new String(".");

		internal void InitializeFieldValues(System.Collections.Generic.Dictionary<Field, Object> fieldValues)
		{
			if (_ancestor != null)
			{
				_ancestor.InitializeFieldValues(fieldValues);
			}

			foreach (var field in _fields)
			{
				FieldValues[field] = field.OfClass.DefaultValue;
			}
		}
	}
}

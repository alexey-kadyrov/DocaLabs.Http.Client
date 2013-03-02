using System;
using System.Collections;
using System.Collections.Generic;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.Mapping;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable InconsistentNaming

    [Subject(typeof(TypeMap))]
    class when_parsing_type
    {
        static TypeMap type_map;

        Because of =
            () => type_map = new TypeMap(typeof (TestClass));

        It should_map_all_eligible_properties =
            () => type_map.Properties.Count.ShouldEqual(19);

        class TestClass
        {
            public int IntProperty { get; set; }
            public TestEnum EnumProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public string StringProperty { get; set; }
            public byte[] ByteArrayProperty { get; set; }
            public object ObjectProperty { get; set; }
            public TestClass ClassProperty { get; set; }
            public TestStruct StructProperty { get; set; }
            public IEnumerable<string> EnumerableStringProperty { get; set; }

            [SeparatedCollectionConverter]
            public IEnumerable<string> EnumerableStringWithAttributeProperty { get; set; }

            public IEnumerable<int> EnumerableIntProperty { get; set; }
            public List<int> ListIntProperty { get; set; }
            public IList<int> IListIntProperty { get; set; }
            public int[] ArrayIntProperty { get; set; }
            public IEnumerable<Guid> EnumerableGuidProperty { get; set; }
            public IEnumerable<DateTime> EnumerableDateTimeProperty { get; set; }
            public IEnumerable<object> EnumerableObjectProperty { get; set; }
            public IEnumerable EnumerableProperty { get; set; }

            public string NoGetter { set {} }

            [QueryIgnore]
            public string Ignored { get; set; }

            public IEnumerable<int> this[int index]
            {
                get { return null; }
                set { }
            }

            string PrivateProperty { get; set; }

            public static string StaticProperty { get; set; }
        }

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }
    }

    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
}

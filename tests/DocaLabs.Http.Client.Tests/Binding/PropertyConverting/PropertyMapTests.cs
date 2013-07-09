using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable InconsistentNaming

    [Subject(typeof(PropertyMap))]
    class when_converting_type_without_nested_objects
    {
        static PropertyMaps maps;
        static PropertyMap map;
        static TestClass instance;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                IntProperty = 1,
                EnumProperty = TestEnum.Large,
                GuidProperty = Guid.NewGuid(),
            };

            maps = new PropertyMaps(x => true);
            map = maps.Parse(instance);
        };

        Because of =
            () => result = map.Convert(instance);

        It should_map_all_eligible_properties/* =
            () => result.Count.ShouldEqual(12)*/;

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

            public string NoGetter { set { } }

            [RequestUse(RequestUseTargets.Ignore)]
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
            Small = 0,
            Large = 1
        }

        struct TestStruct
        {
            public string Country { get; set; }
            public decimal Price { get; set; }
        }
    }

    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
}

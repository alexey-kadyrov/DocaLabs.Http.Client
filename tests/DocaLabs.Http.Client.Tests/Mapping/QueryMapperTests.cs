using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using DocaLabs.Http.Client.Mapping;
using DocaLabs.Http.Client.Mapping.Attributes;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping
{
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedMember.Local
    // ReSharper disable MemberCanBePrivate.Local
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable NotAccessedField.Local

    [Subject(typeof(QueryMapper))]
    class when_query_mapper_is_used
    {
        static TestClass instance;
        static string query;

        Establish context = () => instance = new TestClass
        {
            IntProperty = 1,
            EnumProperty = TestEnum.Two,
            GuidProperty = Guid.Parse("CCA75C92-B3FB-4A0F-B9A9-D149E7CB4F5D"),
            DateTimeProperty = new DateTime(2013, 2, 17, 11, 5, 0, DateTimeKind.Utc),
            StringProperty = "string-1",
            NullStringProperty = null,
            ByteArrayProperty = new byte[] { 1, 2, 3, 4 },
            ClassForToStringProperty = new ClassForToString(),
            ClassWithCustomConverterProperty = new ClassWithCustomConverter(),
            StructProperty = new TestStruct { Value = 2 },
            EnumerableStringProperty = new [] { "s1", "s2" },
            EnumerableStringWithAttributeProperty = new [] { "ss1", "ss2" },
            ArrayIntProperty = new [] { 3, 4 },
            NoGetter = "hello",
            Ignored = "World",
            PublicField = "field"
        };

        Because of =
            () => query = QueryMapper.ToQueryString(instance);

        It should_create_query_string =
            () => ShouldContainOnly(HttpUtility.ParseQueryString(query));

        class TestClass
        {
            public int IntProperty { get; set; }
            public TestEnum EnumProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public string StringProperty { get; set; }
            public string NullStringProperty { get; set; }
            public byte[] ByteArrayProperty { get; set; }
            public ClassForToString ClassForToStringProperty { get; set; }
            public ClassWithCustomConverter ClassWithCustomConverterProperty { get; set; }
            public TestStruct StructProperty { get; set; }
            public IEnumerable<string> EnumerableStringProperty { get; set; }
            [SeparatedCollectionConverter]
            public IEnumerable<string> EnumerableStringWithAttributeProperty { get; set; }
            public int[] ArrayIntProperty { get; set; }

            public string NoGetter { set { } }

            [QueryIgnore]
            public string Ignored { get; set; }

            public int this[int index]
            {
                get { return 42; }
                set { }
            }

            string PrivateProperty { get; set; }

            public static string StaticProperty { get; set; }

            public string PublicField;
            string _privateField;
            static string _staticField;

            static TestClass()
            {
                StaticProperty = "static";
                _staticField = "static field";
            }

            public TestClass()
            {
                PrivateProperty = "private";
                _privateField = "private field";
            }
        }

        enum TestEnum
        {
            One,
            Two
        }

        struct TestStruct
        {
            public int Value { get; set; }

            public override string ToString()
            {
                return Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        class ClassForToString
        {
            public override string ToString()
            {
                return "ClassForToStringValue";
            }
        }

        class ClassWithCustomConverter : ICustomQueryMapper
        {
            public override string ToString()
            {
                return "ClassWithCustomConverterToString";
            }

            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection
                {
                    { "ClassWithCustomConverterKey", "ClassWithCustomConverterValue"}
                };
            }
        }
    
        static void ShouldContainOnly(NameValueCollection values)
        {
            values.Count.ShouldEqual(12);

            values["IntProperty"].ShouldEqual("1");
            values["EnumProperty"].ShouldEqual("Two");
            values["GuidProperty"].ToUpperInvariant().ShouldEqual("CCA75C92-B3FB-4A0F-B9A9-D149E7CB4F5D");
            values["DateTimeProperty"].ShouldEqual("2013-02-17T11:05:00.000Z");
            values["StringProperty"].ShouldEqual("string-1");
            values["ByteArrayProperty"].ShouldEqual(Convert.ToBase64String(instance.ByteArrayProperty));
            values["ClassForToStringProperty"].ShouldEqual("ClassForToStringValue");
            values["ClassWithCustomConverterKey"].ShouldEqual("ClassWithCustomConverterValue");
            values["StructProperty"].ShouldEqual("2");
            values["EnumerableStringProperty"].ShouldEqual("s1,s2");
            values["EnumerableStringWithAttributeProperty"].ShouldEqual("ss1|ss2");
            values["ArrayIntProperty"].ShouldEqual("3,4");
        }
    }

    [Subject(typeof(QueryMapper))]
    class when_query_mapper_is_used_with_null_instance
    {
        static string query;

        Because of =
            () => query = QueryMapper.ToQueryString(null);

        It should_return_empty_string =
            () => query.ShouldBeEmpty();
    }

    [Subject(typeof(QueryMapper))]
    class when_query_mapper_is_used_on_type_wich_implement_custom_converter
    {
        static TestClass instance;
        static string query;

        Establish context = () => instance = new TestClass
        {
            Value = "Hello World!"
        };

        Because of =
            () => query = QueryMapper.ToQueryString(instance);

        It should_return_empty_string =
            () => query.ShouldEqual("Key1=Value1&Key2=Value2");

        class TestClass : ICustomQueryMapper
        {
            public string Value { get; set; }

            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection
                {
                    {"Key1", "Value1"},
                    {"Key2", "Value2"}
                };
            }
        }
    }

    // ReSharper restore NotAccessedField.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore MemberCanBePrivate.Local
    // ReSharper restore UnusedMember.Local
    // ReSharper restore ValueParameterNotUsed
}

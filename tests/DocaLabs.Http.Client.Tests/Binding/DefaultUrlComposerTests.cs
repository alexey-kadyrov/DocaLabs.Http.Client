using System;
using DocaLabs.Http.Client.Binding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_model_without_serialization_hints
    {
        static Uri base_url;
        static TestModel model;
        static string url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");

            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => url = new DefaultUrlComposer().Compose(model, base_url);

        It should_not_modify_authority_and_path_and_should_add_input_model_part =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local

    //    // ReSharper disable ValueParameterNotUsed
    //    // ReSharper disable UnusedMember.Local
    //    // ReSharper disable MemberCanBePrivate.Local
    //    // ReSharper disable UnusedParameter.Local
    //    // ReSharper disable UnusedAutoPropertyAccessor.Local
    //    // ReSharper disable NotAccessedField.Local

    //    [Subject(typeof(QueryMapper))]
    //    class when_query_mapper_is_used
    //    {
    //        static TestClass instance;
    //        static string query;

    //        Establish context = () => instance = new TestClass
    //        {
    //            IntProperty = 1,
    //            EnumProperty = TestEnum.Two,
    //            GuidProperty = Guid.Parse("CCA75C92-B3FB-4A0F-B9A9-D149E7CB4F5D"),
    //            DateTimeProperty = new DateTime(2013, 2, 17, 11, 5, 0, DateTimeKind.Utc),
    //            StringProperty = "string-1",
    //            NullStringProperty = null,
    //            ByteArrayProperty = new byte[] { 1, 2, 3, 4 },
    //            ClassForToStringProperty = new ClassForToString(),
    //            ClassWithCustomConverterProperty = new ClassWithCustomConverter(),
    //            StructProperty = new TestStruct { Value = 2 },
    //            EnumerableStringProperty = new [] { "s1", "s2" },
    //            EnumerableStringWithAttributeProperty = new [] { "ss1", "ss2" },
    //            ArrayIntProperty = new [] { 3, 4 },
    //            NoGetter = "hello",
    //            Ignored = "World",
    //            PublicField = "field"
    //        };

    //        Because of =
    //            () => query = QueryMapper.ToQueryString(instance);

    //        It should_create_query_string =
    //            () => ShouldContainOnly(HttpUtility.ParseQueryString(query));

    //        class TestClass
    //        {
    //            public int IntProperty { get; set; }
    //            public TestEnum EnumProperty { get; set; }
    //            public Guid GuidProperty { get; set; }
    //            public DateTime DateTimeProperty { get; set; }
    //            public string StringProperty { get; set; }
    //            public string NullStringProperty { get; set; }
    //            public byte[] ByteArrayProperty { get; set; }
    //            public ClassForToString ClassForToStringProperty { get; set; }
    //            public ClassWithCustomConverter ClassWithCustomConverterProperty { get; set; }
    //            public TestStruct StructProperty { get; set; }
    //            public IEnumerable<string> EnumerableStringProperty { get; set; }
    //            [SeparatedCollectionConverter]
    //            public IEnumerable<string> EnumerableStringWithAttributeProperty { get; set; }
    //            public int[] ArrayIntProperty { get; set; }

    //            public string NoGetter { set { } }

    //            [QueryIgnore]
    //            public string Ignored { get; set; }

    //            public int this[int index]
    //            {
    //                get { return 42; }
    //                set { }
    //            }

    //            string PrivateProperty { get; set; }

    //            public static string StaticProperty { get; set; }

    //            public string PublicField;
    //            string _privateField;
    //            static string _staticField;

    //            static TestClass()
    //            {
    //                StaticProperty = "static";
    //                _staticField = "static field";
    //            }

    //            public TestClass()
    //            {
    //                PrivateProperty = "private";
    //                _privateField = "private field";
    //            }
    //        }

    //        enum TestEnum
    //        {
    //            One,
    //            Two
    //        }

    //        struct TestStruct
    //        {
    //            public int Value { get; set; }

    //            public override string ToString()
    //            {
    //                return Value.ToString(CultureInfo.InvariantCulture);
    //            }
    //        }

    //        class ClassForToString
    //        {
    //            public override string ToString()
    //            {
    //                return "ClassForToStringValue";
    //            }
    //        }

    //        class ClassWithCustomConverter : ICustomQueryMapper
    //        {
    //            public override string ToString()
    //            {
    //                return "ClassWithCustomConverterToString";
    //            }

    //            public NameValueCollection ToParameterDictionary()
    //            {
    //                return new NameValueCollection
    //                {
    //                    { "ClassWithCustomConverterKey", "ClassWithCustomConverterValue"}
    //                };
    //            }
    //        }

    //        static void ShouldContainOnly(NameValueCollection values)
    //        {
    //            values.Count.ShouldEqual(12);

    //            values["IntProperty"].ShouldEqual("1");
    //            values["EnumProperty"].ShouldEqual("Two");
    //            values["GuidProperty"].ToUpperInvariant().ShouldEqual("CCA75C92-B3FB-4A0F-B9A9-D149E7CB4F5D");
    //            values["DateTimeProperty"].ShouldEqual("2013-02-17T11:05:00.000Z");
    //            values["StringProperty"].ShouldEqual("string-1");
    //            values["ByteArrayProperty"].ShouldEqual(Convert.ToBase64String(instance.ByteArrayProperty));
    //            values["ClassForToStringProperty"].ShouldEqual("ClassForToStringValue");
    //            values["ClassWithCustomConverterKey"].ShouldEqual("ClassWithCustomConverterValue");
    //            values["StructProperty"].ShouldEqual("2");
    //            values["EnumerableStringProperty"].ShouldEqual("s1,s2");
    //            values["EnumerableStringWithAttributeProperty"].ShouldEqual("ss1|ss2");
    //            values["ArrayIntProperty"].ShouldEqual("3,4");
    //        }
    //    }

    //    [Subject(typeof(QueryMapper))]
    //    class when_query_mapper_is_used_with_null_instance
    //    {
    //        static string query;

    //        Because of =
    //            () => query = QueryMapper.ToQueryString(null);

    //        It should_return_empty_string =
    //            () => query.ShouldBeEmpty();
    //    }

    //    [Subject(typeof(QueryMapper))]
    //    class when_query_mapper_is_used_on_type_wich_implement_custom_converter
    //    {
    //        static TestClass instance;
    //        static string query;

    //        Establish context = () => instance = new TestClass
    //        {
    //            Value = "Hello World!"
    //        };

    //        Because of =
    //            () => query = QueryMapper.ToQueryString(instance);

    //        It should_return_empty_string =
    //            () => query.ShouldEqual("Key1=Value1&Key2=Value2");

    //        class TestClass : ICustomQueryMapper
    //        {
    //            public string Value { get; set; }

    //            public NameValueCollection ToParameterDictionary()
    //            {
    //                return new NameValueCollection
    //                {
    //                    {"Key1", "Value1"},
    //                    {"Key2", "Value2"}
    //                };
    //            }
    //        }
    //    }

    //    // ReSharper restore NotAccessedField.Local
    //    // ReSharper restore UnusedAutoPropertyAccessor.Local
    //    // ReSharper restore UnusedParameter.Local
    //    // ReSharper restore MemberCanBePrivate.Local
    //    // ReSharper restore UnusedMember.Local
    //    // ReSharper restore ValueParameterNotUsed

    //    // ReSharper disable UnusedAutoPropertyAccessor.Local
    //    // ReSharper disable ClassNeverInstantiated.Local
    //    // ReSharper disable UnusedMember.Local

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_without_input_model_and_type_without_serialization_hints
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/product/red/");

    //            client = new TestClient(base_url);

    //            model = new TestModel { Value = "Hello World!" };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_add_input_model_part =
    //            () => url.ToString().ShouldEqual("http://foo.bar/product/red/?Value=Hello+World!");

    //        class TestModel
    //        {
    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_with_some_input_model_part_and_type_without_serialization_hints
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/product/red?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel { Value = "Hello World!" };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/product/red?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_ordered_path_and_query
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red category",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_named_path_and_query
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red category",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            [NamedRequestPath]
    //            public string ProductCategory { get; set; }

    //            [NamedRequestPath]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_named_and_ordered_path_and_query
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red category",
    //                ViewType = "small",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42/small?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            [NamedRequestPath]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(1)]
    //            public string ViewType { get; set; }

    //            [NamedRequestPath]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_with_user_password_existing_input_model_part_and_fragment
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://user1:password1@foo.bar/product/red?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

    //            client = new TestClient(base_url);

    //            model = new TestModel { Value = "Hello World!" };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_user_password_authority_path_existing_qyery_part_and_fragment_and_should_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://user1:password1@foo.bar/product/red?keepMe=Yes&Value=Hello+World!#keepMeAsWell/andMe?andMeToo");

    //        class TestModel
    //        {
    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_to_create_ordered_path_where_empty_value_follow_non_emtpy
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://user1:password1@foo.bar/product/?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                Value1 = "Hello World",
    //                Value2 = "",
    //                Value3 = ""
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_user_password_authority_path_existing_qyery_part_and_fragment_and_should_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://user1:password1@foo.bar/product/Hello%20World?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string Value1 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public string Value2 { get; set; }

    //            [OrderedRequestPath(3)]
    //            public string Value3 { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_to_create_named_path_where_empty_value_follow_non_emtpy
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red category",
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            [NamedRequestPath]
    //            public string ProductCategory { get; set; }

    //            [NamedRequestPath]
    //            public string ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_with_model_instance_marked_as_ignore
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_url =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

    //        [IgnoreInRequest]
    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_with_client_instance_marked_as_ignore
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                ProductCategory = "red",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_not_modify_url =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        [IgnoreInRequest]
    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_with_null_model_instance
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(null, client, base_url);

    //        It should_not_modify_url =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        [IgnoreInRequest]
    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_with_null_client_instance
    //    {
    //        static Uri base_url;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            model = new TestModel
    //            {
    //                ProductCategory = "red",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, null, base_url);

    //        It should_still_modify_url =
    //            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red/42?keepMe=Yes&Value=Hello+World!");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_for_local_file
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("file:///c:/root");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                SubFolder1 = "red",
    //                SubFolder2 = 42,
    //                File = "readme.txt"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_create_full_url =
    //            () => url.AbsoluteUri.ShouldEqual("file:///c:/root/red/42/readme.txt");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string SubFolder1 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int SubFolder2 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public string File { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_for_url_for_unc_file
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Uri url;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("file://file-server/root");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                SubFolder1 = "red",
    //                SubFolder2 = 42,
    //                File = "read me.txt"
    //            };
    //        };

    //        Because of =
    //            () => url = UrlBuilder.Compose(model, client, base_url);

    //        It should_create_full_url =
    //            () => url.AbsoluteUri.ShouldEqual("file://file-server/root/red/42/read%20me.txt");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string SubFolder1 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int SubFolder2 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public string File { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_with_null_base_url
    //    {
    //        static TestClient client;
    //        static TestModel model;
    //        static Exception exception;

    //        Establish context = () =>
    //        {
    //            client = new TestClient(new Uri("http://contoso.com/"));

    //            model = new TestModel
    //            {
    //                ProductCategory = "red",
    //                ProductId = 42,
    //                Value = "Hello World!"
    //            };
    //        };

    //        Because of =
    //            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, null));

    //        It should_throw_argument_null_exception =
    //            () => exception.ShouldBeOfType<ArgumentNullException>();

    //        It should_report_base_url_argument =
    //            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("baseUrl");

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string ProductCategory { get; set; }

    //            [OrderedRequestPath(2)]
    //            public int ProductId { get; set; }

    //            public string Value { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_and_exepction_is_thrown
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Exception exception;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel();
    //        };

    //        Because of =
    //            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, base_url));

    //        It should_throw_unrecoverable_http_client_exception =
    //            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

    //        It should_wrap_original_exception =
    //            () => exception.InnerException.InnerException.Message.ShouldEqual("You got me!");

    //        class TestModel
    //        {
    //            public string Value { get { throw new Exception("You got me!"); } }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    [Subject(typeof(UrlBuilder))]
    //    class when_url_builder_is_used_to_create_ordered_path_where_non_empty_value_follow_empty
    //    {
    //        static Uri base_url;
    //        static TestClient client;
    //        static TestModel model;
    //        static Exception exception;

    //        Establish context = () =>
    //        {
    //            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

    //            client = new TestClient(base_url);

    //            model = new TestModel
    //            {
    //                Value1 = "",
    //                Value2 = "red",
    //                Value3 = null
    //            };
    //        };

    //        Because of =
    //            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, base_url));

    //        It should_throw_unrecoverable_http_client_exception =
    //            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

    //        It should_not_wrap_original_exception_as_it_is_already_unrecoverable_http_client_exception =
    //            () => exception.InnerException.ShouldBeNull();

    //        class TestModel
    //        {
    //            [OrderedRequestPath(1)]
    //            public string Value1 { get; set; }

    //            [OrderedRequestPath(2)]
    //            public string Value2 { get; set; }

    //            [OrderedRequestPath(3)]
    //            public string Value3 { get; set; }
    //        }

    //        class TestClient : HttpClient<TestModel, string>
    //        {
    //            public TestClient(Uri baseUrl)
    //                : base(baseUrl)
    //            {
    //            }
    //        }
    //    }

    //    // ReSharper restore UnusedMember.Local
    //    // ReSharper restore ClassNeverInstantiated.Local
    //    // ReSharper restore UnusedAutoPropertyAccessor.Local
}

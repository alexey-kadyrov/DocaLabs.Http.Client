using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(DefaultResponseBinder))]
    class when_default_response_binder_is_instantiated
    {
        static DefaultResponseBinder binder;

        Because of =
            () => binder = new DefaultResponseBinder();

        It should_instantiate_three_deserializers =
            () => binder.Providers.Count.ShouldEqual(3);

        It should_instantiate_json_deserializer =
            () => binder.Providers.ShouldContain(x => x is DeserializeFromJsonAttribute);

        It should_instantiate_xml_deserializer =
            () => binder.Providers.ShouldContain(x => x is DeserializeFromXmlAttribute);

        It should_instantiate_plain_text_deserializer_as_the_last_in_the_list =
            () => binder.Providers[2].ShouldBeOfType<DeserializeFromPlainTextAttribute>();
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_setting_collection_of_deserialization_providers
    {
        static DefaultResponseBinder binder;
        static Mock<IResponseDeserializationProvider> deserialization_provider;

        Establish context = () =>
        {
            binder = new DefaultResponseBinder();
            deserialization_provider = new Mock<IResponseDeserializationProvider>();
        };

        Because of = () => binder.Providers = new List<IResponseDeserializationProvider>
        {
            deserialization_provider.Object
        };

        It should_set_the_collection_to_the_specified_value =
            () => binder.Providers.ShouldContainOnly(deserialization_provider.Object);
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_setting_collection_of_deserialization_providers_to_null
    {
        static DefaultResponseBinder binder;
        static Exception exception;

        Establish context = 
            () => binder = new DefaultResponseBinder();

        Because of =
            () => exception = Catch.Exception(() => { binder.Providers = null; });

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_setting_and_getting_collection_of_deserialization_concurrently
    {
        static DefaultResponseBinder binder;
        static int count;

        Establish context = () => binder = new DefaultResponseBinder
        {
            Providers = new List<IResponseDeserializationProvider> { new Mock<IResponseDeserializationProvider>().Object }
        };

        Because of = () => Parallel.For(0, 1000000, i =>
        {
            if (i%2 == 0)
            {
                var pp = binder.Providers;
                pp.Count.ShouldEqual(1);
            }
            else
                binder.Providers = new List<IResponseDeserializationProvider> { new Mock<IResponseDeserializationProvider>() .Object };

            Interlocked.Increment(ref count);
        });

        It should_do_all_setting_getting_without_exception =
            () => count.ShouldEqual(1000000);
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_with_null_context
    {
        static DefaultResponseBinder binder;
        static Exception exception;

        Establish context =
            () => binder = new DefaultResponseBinder();

        Because of =
            () => exception = Catch.Exception(() => binder.Read(null, new Mock<WebRequest>().Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_context_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("context");
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_stream_and_with_deserialization_attributes_on_client : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Stream));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.Read(binding_context, mock_request.Object);

        It should_return_data_deserialized_by_the_attribute =
            () => data.ShouldBeOfType<int>();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, Stream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_class_derived_from_stream_with_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.Read(binding_context, mock_request.Object);

        It should_return_data_deserialized_by_the_attribute =
            () => data.ShouldBeOfType<int>();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Model : MemoryStream
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_class_derived_from_stream_without_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(MemoryStream));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => exception = Catch.Exception(() => binder.Read(binding_context, mock_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        class Client : HttpClient<string, MemoryStream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_http_response_stream_and_with_deserialization_attributes_on_client : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(HttpResponseStream));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.Read(binding_context, mock_request.Object);

        It should_return_data_deserialized_by_the_attribute =
            () => data.ShouldBeOfType<int>();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, HttpResponseStream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_void_type_and_with_deserialization_attributes_on_client : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static object value;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(VoidType));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => value = binder.Read(binding_context, mock_request.Object);

        It should_return_data_deserialized_by_the_attribute =
            () => value.ShouldBeOfType<int>();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, VoidType>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_type_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_type_and_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        [TestResponseDeserialization(ReturnType = typeof(object))]
        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static int data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(int));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (int)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, int>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_from_json : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("{Value:\"Hello World!\"}")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_properties =
            () => data.Value.ShouldEqual("Hello World!");

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_from_xml : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("<Model><Value>Hello World!</Value></Model>")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_properties =
            () => data.Value.ShouldEqual("Hello World!");

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_from_plain_text : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static string data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(string));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (string)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_value =
            () => data.ShouldEqual("Hello World!");

        class Client : HttpClient<string, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_as_plain_text_when_content_type_is_unknown_but_the_return_type_is_string : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static string data;

        Establish context = () =>
        {
            Setup("who-knows/what; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(string));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (string)binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_value =
            () => data.ShouldEqual("Hello World!");

        class Client : HttpClient<string, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_from_octet_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static byte[] data;

        Establish context = () =>
        {
            Setup("application/octet-stream", new MemoryStream(new byte[] {1, 2, 3, 4 }));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(byte[]));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (byte[])binder.Read(binding_context, mock_request.Object);

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_value =
            () => data.ShouldEqual(new byte[] { 1, 2, 3, 4 });

        class Client : HttpClient<string, byte[]>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_from_unknwon_encoding_type : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            Setup("who-knows/what; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Model));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => exception = Catch.Exception(() => binder.Read(binding_context, mock_request.Object));

        It should_throw_http_client_exceptionl =
            () => exception.ShouldBeOfType<HttpClientException>();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_http_response_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static HttpResponseStream stream;

        Cleanup after =
            () => stream.Dispose();

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(HttpResponseStream));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => stream = (HttpResponseStream)binder.Read(binding_context, mock_request.Object);

        It should_return_stream =
            () => stream.ShouldBeOfType<HttpResponseStream>();

        It should_return_stream_wiath_all_data =
            () => stream.ReadAsString(Encoding.UTF8).ShouldEqual("Hello World!");

        class Client : HttpClient<string, HttpResponseStream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Stream stream;

        Cleanup after =
            () => stream.Dispose();

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(Stream));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => stream = (Stream)binder.Read(binding_context, mock_request.Object);

        It should_return_stream =
            () => stream.ShouldBeOfType<Stream>();

        It should_return_stream_wiath_all_data =
            () => stream.ReadAsString(Encoding.UTF8).ShouldEqual("Hello World!");

        class Client : HttpClient<string, Stream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_void_type : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static object value;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null, typeof(string), typeof(VoidType));
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => value = binder.Read(binding_context, mock_request.Object);

        It should_return_void_type =
            () => value.ShouldEqual(VoidType.Value);

        class Client : HttpClient<string, VoidType>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_with_null_context
    {
        static DefaultResponseBinder binder;
        static Exception exception;
        static string result;

        Establish context =
            () => binder = new DefaultResponseBinder();

        Because of = () => exception = Catch.Exception(() =>
        {
            result = binder.ReadAsync<string>(null, new Mock<WebRequest>().Object).Result;
        });

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_context_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("context");
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_stream_and_with_deserialization_attributes_on_client : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Stream), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Stream>(binding_context, mock_request.Object).Result;

        It should_return_data_deserialized_by_the_attribute =
            () => data.ShouldBeOfType<MemoryStream>();

        [TestResponseDeserialization(ReturnType = typeof(MemoryStream))]
        class Client : HttpClient<string, Stream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_class_derived_from_stream_without_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(MemoryStream), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => exception = Catch.Exception(() => binder.ReadAsync<MemoryStream>(binding_context, mock_request.Object).Wait());

        It should_throw_http_client_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        class Client : HttpClient<string, MemoryStream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_derived_from_stream_and_with_deserialization_attributes_on_client : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Model>(binding_context, mock_request.Object).Result;

        It should_return_data_deserialized_by_the_attribute =
            () => data.ShouldBeOfType<Model>();

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model : MemoryStream
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_type_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Model>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldBeOfType<Model>();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_type_and_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Model>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        [TestResponseDeserialization(ReturnType = typeof(object))]
        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static object data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(int), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<int>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldBeOfType<int>();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, int>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_from_json : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("{Value:\"Hello World!\"}")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Model>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_properties =
            () => data.Value.ShouldEqual("Hello World!");

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_from_xml : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("<Model><Value>Hello World!</Value></Model>")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<Model>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_properties =
            () => data.Value.ShouldEqual("Hello World!");

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_as_plain_text_when_content_type_is_unknown_but_the_return_type_is_string : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static string data;

        Establish context = () =>
        {
            Setup("who-knows/what; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(string), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<string>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_value =
            () => data.ShouldEqual("Hello World!");

        class Client : HttpClient<string, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_from_octet_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static byte[] data;

        Establish context = () =>
        {
            Setup("application/octet-stream", new MemoryStream(new byte[] { 1, 2, 3, 4 }));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(byte[]), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = binder.ReadAsync<byte[]>(binding_context, mock_request.Object).Result;

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        It should_deserialize_value =
            () => data.ShouldEqual(new byte[] { 1, 2, 3, 4 });

        class Client : HttpClient<string, byte[]>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_from_unknwon_encoding_type : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            Setup("who-knows/what; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Model), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => exception = Catch.Exception(() => binder.ReadAsync<Model>(binding_context, mock_request.Object).Wait());

        It should_throw_http_client_exceptionl =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_http_response_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static HttpResponseStream stream;

        Cleanup after =
            () => stream.Dispose();

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(HttpResponseStream), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => stream = binder.ReadAsync<HttpResponseStream>(binding_context, mock_request.Object).Result;

        It should_return_stream =
            () => stream.ShouldBeOfType<HttpResponseStream>();

        It should_return_stream_wiath_all_data =
            () => stream.ReadAsString(Encoding.UTF8).ShouldEqual("Hello World!");

        class Client : HttpClient<string, HttpResponseStream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_stream : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static Stream stream;

        Cleanup after =
            () => stream.Dispose();

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(Stream), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => stream = binder.ReadAsync<Stream>(binding_context, mock_request.Object).Result;

        It should_return_stream =
            () => stream.ShouldBeOfType<Stream>();

        It should_return_stream_wiath_all_data =
            () => stream.ReadAsString(Encoding.UTF8).ShouldEqual("Hello World!");

        class Client : HttpClient<string, Stream>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_asynchronously_reading_for_void_type : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static AsyncBindingContext binding_context;
        static object value;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new AsyncBindingContext(new Client(), null, null, null, typeof(string), typeof(VoidType), CancellationToken.None);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => value = binder.ReadAsync<VoidType>(binding_context, mock_request.Object).Result;

        It should_return_void_type =
            () => value.ShouldEqual(VoidType.Value);

        class Client : HttpClient<string, VoidType>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    class default_response_binder_test_context
    {
        protected static Mock<WebRequest> mock_request;
        protected static Mock<WebResponse> mock_response;

        protected static void Setup(string contentType, Stream stream)
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(stream);
            mock_response.Object.ContentType = contentType;
            mock_response.Object.ContentLength = stream.Length;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
            mock_request.Setup(x => x.GetResponseAsync()).Returns(Task.FromResult(mock_response.Object));
        }
    }

    class TestResponseDeserializationAttribute : ResponseDeserializationAttribute
    {
        public Type ReturnType { get; set; }

        public override object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            return Activator.CreateInstance(ReturnType);
        }
    }

    public class Model
    {
        public string Value { get; set; }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}

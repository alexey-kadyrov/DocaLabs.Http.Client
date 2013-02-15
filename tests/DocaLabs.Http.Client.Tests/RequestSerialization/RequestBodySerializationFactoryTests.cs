using System;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.RequestSerialization;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.RequestSerialization
{
    [Subject(typeof(RequestBodySerializationFactory))]
    class when_http_client_and_query_and_property_of_query_have_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_attribute_that_is_defined_on_the_query_class =
            () => serializer.ShouldBeOfType<SerializeAsJsonAttribute>();

        [SerializeAsJson]
        class Query
        {
            public string Id { get; set; }

            [SerializeAsForm]
            public string Name { get; set; }
        }

        [SerializeAsXml]
        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient() : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_http_client_and_property_of_query_have_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_attribute_that_is_defined_on_the_property_of_the_query_class =
            () => serializer.ShouldBeOfType<SerializeAsFormAttribute>();

        class Query
        {
            public string Id { get; set; }

            [SerializeAsForm]
            public string Name { get; set; }
        }

        [SerializeAsXml]
        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_only_query_has_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_attribute_that_is_defined_on_the_query_class =
            () => serializer.ShouldBeOfType<SerializeAsJsonAttribute>();

        [SerializeAsJson]
        class Query
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_only_http_client_has_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_attribute_that_is_defined_on_the_http_client_class =
            () => serializer.ShouldBeOfType<SerializeAsXmlAttribute>();

        class Query
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        [SerializeAsXml]
        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_only_property_of_query_have_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_attribute_that_is_defined_on_the_property_of_the_query_class =
            () => serializer.ShouldBeOfType<SerializeAsFormAttribute>();

        class Query
        {
            public string Id { get; set; }

            [SerializeAsForm]
            public string Name { get; set; }
        }

        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_no_one_has_request_serialization_attribute
    {
        // ReSharper disable UnusedMember.Local
        static IRequestSerialization serializer;

        Because of =
            () => serializer = RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query());

        It should_return_null =
            () => serializer.ShouldBeNull();

        class Query
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_http_clinet_is_null
    {
        // ReSharper disable UnusedMember.Local
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => RequestBodySerializationFactory.GetSerializer(null, new Query()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_http_client_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("httpClient");

        class Query
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        // ReSharper restore UnusedMember.Local
    }

    [Subject(typeof(RequestBodySerializationFactory))]
    class when_called_from_different_threads_concurrently
    {
        // ReSharper disable UnusedMember.Local
        static int counter;
        static int current_concurrency;
        static int achieved_concurrency;
        static object locker;

        Establish context =
            () => locker = new object();

        Because of = () =>
        {
            Parallel.For(0, 500000, i =>
            {
                var concurrencyLevel = Interlocked.Increment(ref current_concurrency);

                lock (locker)
                {
                    if (achieved_concurrency < concurrencyLevel)
                        achieved_concurrency = concurrencyLevel;
                }

                if (RequestBodySerializationFactory.GetSerializer(new TestHttpClient(), new Query()) == null)
                    Interlocked.Increment(ref counter);

                Interlocked.Decrement(ref current_concurrency);
            });

            Console.WriteLine(@"Achieved concurrency of: {0}", achieved_concurrency);
        };

        It should_not_fail_any_call =
            () => counter.ShouldEqual(500000);

        class Query
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        class TestHttpClient : HttpClient<Query, string>
        {
            public TestHttpClient()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
        // ReSharper restore UnusedMember.Local
    }
}

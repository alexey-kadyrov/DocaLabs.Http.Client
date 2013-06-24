//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using DocaLabs.Http.Client.ResponseDeserialization;
//using DocaLabs.Http.Client.Tests._Utils;
//using Machine.Specifications;
//using It = Machine.Specifications.It;

//namespace DocaLabs.Http.Client.Tests.ResponseDeserialization
//{
//    [Subject(typeof(DefaultResponseReader))]
//    class when_response_parser_is_used_in_default_configuration
//    {
//        It should_have_four_providers =
//            () => DefaultResponseReader.Providers.Count.ShouldEqual(4);

//        It should_have_json_xml_plain_text_and_image_deserializers = () => DefaultResponseReader.Providers.ShouldMatch(x =>
//            x[0].GetType() == typeof(JsonResponseDeserializer) &&
//            x[1].GetType() == typeof(XmlResponseDeserializer) &&
//            x[2].GetType() == typeof(DeserializeFromPlainTextAttribute) &&
//            x[3].GetType() == typeof(ImageResponseDeserializer));
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_setting_a_new_collection_of_providers_and_parsing_for_types_that_can_be_deserialized : response_deserialization_test_context
//    {
//        static IList<IResponseDeserializationProvider> original_providers;

//        Cleanup after_each =
//            () => DefaultResponseReader.Providers = original_providers;

//        Establish context = () =>
//        {
//            Setup("application/json; charset=utf-8", new MemoryStream());

//            original_providers = DefaultResponseReader.Providers;

//            DefaultResponseReader.Providers = new List<IResponseDeserializationProvider>
//            {
//                new FirstDeserializer(),
//                new SecondDeserializer(),
//                new ThirdDeserializer()
//            };
//        };

//        It should_have_three_providers =
//            () => DefaultResponseReader.Providers.Count.ShouldEqual(3);

//        It should_have_configured_deserializers = () => DefaultResponseReader.Providers.ShouldMatch(x =>
//            x[0].GetType() == typeof(FirstDeserializer) &&
//            x[1].GetType() == typeof(SecondDeserializer) &&
//            x[2].GetType() == typeof(ThirdDeserializer));

//        It should_be_able_to_deserialize_using_first_provider =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof(FirstResult)).ShouldBeOfType<FirstResult>();

//        It should_be_able_to_deserialize_using_second_provider =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof(SecondResult)).ShouldBeOfType<SecondResult>();

//        It should_be_able_to_deserialize_using_attribute_instead_of_third_provider =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof(ThirdResult)).ShouldBeOfType<ThirdResult>();

//        It should_return_deafult_void_type_if_result_type_is_void_type =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof (VoidType)).ShouldNotBeTheSameAs(VoidType.Value);

//        It should_throw_unrecoverable_http_client_exception_for_type_which_deserialization_is_unknown =
//            () => Catch.Exception(() => DefaultResponseReader.Parse(mock_request.Object, typeof(ForthResult))).ShouldBeOfType<UnrecoverableHttpClientException>();

//        class FirstResult
//        {
//        }

//        class SecondResult
//        {
//        }

//        [AttributeForThirdDeserializer]
//        class ThirdResult
//        {
//        }

//        class ForthResult
//        {
//        }

//        class FirstDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new FirstResult();
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof (FirstResult);
//            }
//        }

//        class SecondDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new SecondResult();
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof(SecondResult);
//            }
//        }

//        class ThirdDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return null;
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof(ThirdResult);
//            }
//        }

//        class AttributeForThirdDeserializerAttribute : ResponseDeserializationAttribute
//        {
//            public override object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new ThirdResult();
//            }
//        }
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_parsing_response_parsers_are_choosen_in_order_that_they_were_set : response_deserialization_test_context
//    {
//        static IList<IResponseDeserializationProvider> original_providers;
//        static int order;
//        static Result result;

//        Cleanup after_each =
//            () => DefaultResponseReader.Providers = original_providers;

//        Establish context = () =>
//        {
//            order = 1;

//            Setup("application/json; charset=utf-8", new MemoryStream());

//            original_providers = DefaultResponseReader.Providers;

//            DefaultResponseReader.Providers = new List<IResponseDeserializationProvider>
//            {
//                new FirstDeserializer(),
//                new SecondDeserializer(),
//                new ThirdDeserializer()
//            };
//        };

//        Because of =
//            () => result = (Result) DefaultResponseReader.Parse(mock_request.Object, typeof (Result));

//        It should_deserialize =
//            () => result.Value.ShouldEqual(3);


//        class Result
//        {
//            public int Value { get; set; }
//        }

//        class FirstDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new Result { Value = 1 };
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                if(order != 1)
//                    throw new SpecificationException("Should have been called first.");

//                order++;

//                return false;
//            }
//        }

//        class SecondDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new Result { Value = 2 };
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                if (order != 2)
//                    throw new SpecificationException("Should have been called second.");

//                order++;

//                return false;
//            }
//        }

//        class ThirdDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new Result { Value = 3 };
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                if (order != 3)
//                    throw new SpecificationException("Should have been called third.");

//                order++;

//                return resultType == typeof(Result);
//            }
//        }
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_response_parser_is_used_to_deserialize_into_string : response_deserialization_test_context
//    {
//        const string data = "Hello World!";

//        Establish context = 
//            () => Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));

//        It should_be_able_to_deserialize =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof (string)).ShouldEqual(data);
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_response_parser_is_used_to_deserialize_into_byte_array : response_deserialization_test_context
//    {
//        static byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

//        Establish context =
//            () => Setup("application/json; charset=utf-8", new MemoryStream(data));

//        It should_be_able_to_deserialize =
//            () => DefaultResponseReader.Parse(mock_request.Object, typeof(byte[])).ShouldEqual(data);
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_response_parser_is_used_concurrently 
//    {
//        static IList<IResponseDeserializationProvider> original_providers;
//        static WebRequest request;
//        static int count;
//        static object locker;
//        static Random random;

//        Cleanup after_each =
//            () => DefaultResponseReader.Providers = original_providers;

//        Establish context = () =>
//        {
//            original_providers = DefaultResponseReader.Providers;

//            DefaultResponseReader.Providers = new List<IResponseDeserializationProvider>
//            {
//                new FirstDeserializer(),
//                new SecondDeserializer(),
//                new ThirdDeserializer()
//            };

//            count = 0;
//            locker = new object();
//            random = new Random();

//            request = new TestRequest();
//        };

//        Because of = () => Parallel.For(0, 1000000, i =>
//        {
//            int ii;

//            lock (locker)
//            {
//                ii = random.Next(7);
//            }

//            switch (ii)
//            {
//                case 0:
//                    DefaultResponseReader.Providers = new List<IResponseDeserializationProvider>
//                    {
//                        new FirstDeserializer(),
//                        new SecondDeserializer(),
//                        new ThirdDeserializer()
//                    };
//                    break;
//                case 1:
//                    DefaultResponseReader.Providers.Count.ShouldEqual(3);
//                    break;
//                case 2:
//                    DefaultResponseReader.Parse(request, typeof(FirstResult)).ShouldBeOfType<FirstResult>();
//                    break;
//                case 3:
//                    DefaultResponseReader.Parse(request, typeof(SecondResult)).ShouldBeOfType<SecondResult>();
//                    break;
//                case 4:
//                    DefaultResponseReader.Parse(request, typeof(ThirdResult)).ShouldBeOfType<ThirdResult>();
//                    break;
//                case 5:
//                    DefaultResponseReader.Parse(request, typeof(VoidType)).ShouldNotBeTheSameAs(VoidType.Value);
//                    break;
//                case 6:
//                    Catch.Exception(() => DefaultResponseReader.Parse(request, typeof(ForthResult))).ShouldBeOfType<UnrecoverableHttpClientException>();
//                    break;
//            }

//            Interlocked.Increment(ref count);
//        });

//        It should_execute_all_operations =
//            () => count.ShouldEqual(1000000);

//        class FirstResult
//        {
//        }

//        class SecondResult
//        {
//        }

//        [AttributeForThirdDeserializer]
//        class ThirdResult
//        {
//        }

//        class ForthResult
//        {
//        }

//        class FirstDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new FirstResult();
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof(FirstResult);
//            }
//        }

//        class SecondDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new SecondResult();
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof(SecondResult);
//            }
//        }

//        class ThirdDeserializer : IResponseDeserializationProvider
//        {
//            public object Deserialize(HttpResponse response, Type resultType)
//            {
//                return null;
//            }

//            public bool CanDeserialize(HttpResponse response, Type resultType)
//            {
//                return resultType == typeof(ThirdResult);
//            }
//        }

//        class AttributeForThirdDeserializerAttribute : ResponseDeserializationAttribute
//        {
//            public override object Deserialize(HttpResponse response, Type resultType)
//            {
//                return new ThirdResult();
//            }
//        }

//        class TestResponse : WebResponse
//        {
//            public override Stream GetResponseStream()
//            {
//                return new MemoryStream();
//            }  
//        }

//        class TestRequest : WebRequest
//        {
//            public override WebResponse GetResponse()
//            {
//                return new TestResponse();
//            }
//        }
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_parsing_response_for_null_request
//    {
//        static Exception exception;

//        Because of =
//            () => exception = Catch.Exception(() => DefaultResponseReader.Parse(null, typeof (TestTarget)));

//        It should_throw_argument_null_exception =
//            () => exception.ShouldBeOfType<ArgumentNullException>();

//        It should_report_request_parameter =
//            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("request");
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_parsing_response_for_null_result_type : response_deserialization_test_context
//    {
//        static Exception exception;

//        Because of =
//            () => exception = Catch.Exception(() => DefaultResponseReader.Parse(mock_request.Object, null));

//        It should_throw_argument_null_exception =
//            () => exception.ShouldBeOfType<ArgumentNullException>();

//        It should_report_result_type_parameter =
//            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
//    }

//    [Subject(typeof(DefaultResponseReader))]
//    class when_provider_collection_to_null
//    {
//        static Exception exception;

//        Because of =
//            () => exception = Catch.Exception(() => DefaultResponseReader.Providers = null);

//        It should_throw_argument_null_exception =
//            () => exception.ShouldBeOfType<ArgumentNullException>();

//        It should_report_value_parameter =
//            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
//    }
//}

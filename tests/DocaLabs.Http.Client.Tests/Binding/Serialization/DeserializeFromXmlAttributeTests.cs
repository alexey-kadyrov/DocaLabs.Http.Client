using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Result;

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_for_text_xml_media_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_for_text_xml_media_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Result;

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_but_without_charset : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_but_without_charset : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Result;

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_in_deafult_configuration_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data = 
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>

        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_in_deafult_configuration_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Result;

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_with_dtd_processing_set_to_prohibit_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute
            {
                DtdProcessing = DtdProcessing.Prohibit
            };
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_with_dtd_processing_set_to_prohibit_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute
            {
                DtdProcessing = DtdProcessing.Prohibit
            };
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Wait());

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_with_dtd_processing_set_to_parse_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute
            {
                DtdProcessing = DtdProcessing.Parse
            };
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_with_dtd_processing_set_to_parse_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute
            {
                DtdProcessing = DtdProcessing.Parse
            };
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Wait());

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_in_deafult_configuration_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response_stream, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_in_deafult_configuration_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Result;

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Wait());

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, null, CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(null, typeof(TestTarget), CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "deserialization")]
    class when_xml_deserializer_is_used_on_bad_xml_value : response_deserialization_test_context
    {
        const string data = "} : Non XML string : {";
        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response_stream, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "async deserialization")]
    class when_xml_deserializer_is_used_asynchronously_on_bad_xml_value : response_deserialization_test_context
    {
        const string data = "} : Non XML string : {";
        static DeserializeFromXmlAttribute deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.DeserializeAsync(http_response_stream, typeof(TestTarget), CancellationToken.None).Wait());

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response_stream, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromXmlAttribute deserializer;

        Establish context =
            () => deserializer = new DeserializeFromXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_stream_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("responseStream");
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_xml_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_application_xml_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_text_xml_content_type_but_without_charset : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_text_xml_content_type_all_in_capital : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("TEXT/XML", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_application_xml_content_type_all_in_capital : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("APPLICATION/Xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_xml_content_type_but_for_simple_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_non_xml_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("application/json; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new DeserializeFromXmlAttribute();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(DeserializeFromXmlAttribute), "checking that can deserialize")]
    class when_changing_supported_media_types_to_some_garbage : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static string[] original_supported_types;
        static DeserializeFromXmlAttribute deserializer;
        static bool can_deserialize;

        Cleanup after_each =
            () => DeserializeFromXmlAttribute.SupportedTypes = original_supported_types;

        Establish context = () =>
        {
            original_supported_types = DeserializeFromXmlAttribute.SupportedTypes;
            DeserializeFromXmlAttribute.SupportedTypes = new[] { "weird/type" };
            deserializer = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response_stream, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }
}

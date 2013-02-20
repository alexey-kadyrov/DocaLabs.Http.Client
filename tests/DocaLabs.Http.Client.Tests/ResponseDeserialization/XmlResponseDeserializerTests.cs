using System;
using System.IO;
using System.Text;
using System.Xml;
using DocaLabs.Http.Client.ResponseDeserialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.ResponseDeserialization
{
    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_in_deafult_configuration_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data = 
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static XmlResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>

        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_with_dtd_processing_set_to_prohibit_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DtdProcessing original;
        static XmlResponseDeserializer deserializer;
        static Exception exception;

        Cleanup after_each =
            () => XmlResponseDeserializer.DtdProcessing = original;

        Establish context = () =>
        {
            original = XmlResponseDeserializer.DtdProcessing;
            XmlResponseDeserializer.DtdProcessing = DtdProcessing.Prohibit;
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_with_dtd_processing_set_to_parse_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DtdProcessing original;
        static XmlResponseDeserializer deserializer;
        static Exception exception;

        Cleanup after_each =
            () => XmlResponseDeserializer.DtdProcessing = original;

        Establish context = () =>
        {
            original = XmlResponseDeserializer.DtdProcessing;
            XmlResponseDeserializer.DtdProcessing = DtdProcessing.Parse;
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_in_deafult_configuration_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static XmlResponseDeserializer deserializer;
        static TestTarget target;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)deserializer.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static XmlResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static XmlResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    public class when_xml_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static XmlResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new XmlResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(XmlResponseDeserializer), "deserialization")]
    class when_xml_deserializer_is_used_on_bad_xml_value : response_deserialization_test_context
    {
        const string data = "} : Non XML string : {";
        static XmlResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static XmlResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    public class when_xml_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static XmlResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new XmlResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_xml_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_xml_content_type_all_in_capital : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("TEXT/XML", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_be_able_to_deserialize =
            () => can_deserialize.ShouldBeTrue();
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_xml_content_type_but_for_simple_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(string));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_non_xml_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("application/json", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(XmlResponseDeserializer), "checking that can deserialize")]
    class when_xml_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static XmlResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new XmlResponseDeserializer();
            Setup("", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(TestTarget));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }
}

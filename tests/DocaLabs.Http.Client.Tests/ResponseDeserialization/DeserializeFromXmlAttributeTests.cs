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
    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_and_charset_is_not_set_in_content_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static DeserializeFromXmlAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_in_deafult_configuration_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_with_dtd_processing_set_to_prohibit_on_xml_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value1, Value2)> <!ELEMENT Value1 (#PCDATA)> <!ELEMENT Value2 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute attribute;
        static Exception exception;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute { DtdProcessing = DtdProcessing.Prohibit };
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_with_dtd_processing_set_to_parse_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute attribute;
        static Exception exception;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute { DtdProcessing = DtdProcessing.Parse };
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_in_deafult_configuration_on_xml_not_compliant_with_embedded_dtd : response_deserialization_test_context
    {
        const string data =
            "<?xml version='1.0' standalone='yes'?>" +
            "<!DOCTYPE tests [<!ELEMENT TestTarget (Value11, Value22)> <!ELEMENT Value11 (#PCDATA)> <!ELEMENT Value22 (#PCDATA)>]>" +
            "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";

        static DeserializeFromXmlAttribute attribute;
        static TestTarget target;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => target = (TestTarget)attribute.Deserialize(http_response, typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        const string data = "";
        static DeserializeFromXmlAttribute attribute;
        static Exception exception;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_with_null_result_type : response_deserialization_test_context
    {
        const string data = "<TestTarget><Value1>2012</Value1><Value2>Hello World!</Value2></TestTarget>";
        static Exception exception;
        static DeserializeFromXmlAttribute attribute;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    public class when_deserialize_xml_json_attribute_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static DeserializeFromXmlAttribute attribute;

        Establish context =
            () => attribute = new DeserializeFromXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(null, typeof(TestTarget)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(DeserializeFromXmlAttribute))]
    class when_deserialize_from_xml_attribute_is_used_on_bad_xml_value : response_deserialization_test_context
    {
        const string data = "} : Non XML string : {";
        static DeserializeFromXmlAttribute attribute;
        static Exception exception;

        Establish context = () =>
        {
            attribute = new DeserializeFromXmlAttribute();
            Setup("text/xml; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_an_exception =
            () => exception.ShouldNotBeNull();
    }
}

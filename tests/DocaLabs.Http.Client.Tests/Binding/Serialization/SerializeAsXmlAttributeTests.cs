using System;
using System.Text;
using System.Threading;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.Serialization
{
    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_in_default_configuration : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute();
        };

        Because of = 
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_in_default_configuration : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute();
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_text_xml_media_type : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute { MediaType = "text/xml" };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_text_xml_media_type : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute { MediaType = "text/xml" };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("text/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_gzip_content_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object =
            () => ParseDecodedRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetDecodedRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetDecodedRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetDecodedRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_gzip_content_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute { RequestContentEncoding = KnownContentEncodings.Gzip };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_add_content_encoding_request_header =
            () => mock_web_request.Object.Headers.ShouldContain("content-encoding");

        It should_add_gzip_content_encoding =
            () => mock_web_request.Object.Headers["content-encoding"].ShouldEqual(KnownContentEncodings.Gzip);

        It should_serialize_object =
            () => ParseDecodedRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetDecodedRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetDecodedRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetDecodedRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_doc_type : request_serialization_test_context
    {
        // ReSharper disable PossibleNullReferenceException
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                DocTypeName = "testService",
                Pubid = "-//Test//DTDTest testService v2//EN",
                Sysid = "http://dtd.foo.com/testService_v2.dtd"
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldNotBeNull();

        It should_serialize_specified_doctype_name =
            () => GetRequestDataAsXDocument().DocumentType.Name.ShouldEqual("testService");

        It should_serialize_specified_doctype_pubid =
            () => GetRequestDataAsXDocument().DocumentType.PublicId.ShouldEqual("-//Test//DTDTest testService v2//EN");

        It should_serialize_specified_doctype_sysid =
            () => GetRequestDataAsXDocument().DocumentType.SystemId.ShouldEqual("http://dtd.foo.com/testService_v2.dtd");

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
        // ReSharper restore PossibleNullReferenceException
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_doc_type : request_serialization_test_context
    {
        // ReSharper disable PossibleNullReferenceException
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                DocTypeName = "testService",
                Pubid = "-//Test//DTDTest testService v2//EN",
                Sysid = "http://dtd.foo.com/testService_v2.dtd"
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldNotBeNull();

        It should_serialize_specified_doctype_name =
            () => GetRequestDataAsXDocument().DocumentType.Name.ShouldEqual("testService");

        It should_serialize_specified_doctype_pubid =
            () => GetRequestDataAsXDocument().DocumentType.PublicId.ShouldEqual("-//Test//DTDTest testService v2//EN");

        It should_serialize_specified_doctype_sysid =
            () => GetRequestDataAsXDocument().DocumentType.SystemId.ShouldEqual("http://dtd.foo.com/testService_v2.dtd");

        It should_use_tab_identation =
            () => GetRequestData().ShouldContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
        // ReSharper restore PossibleNullReferenceException
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_utf16_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = CharSets.Utf16
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-16");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.Unicode).ShouldContain("\t");

        It should_use_utf16_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-16");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_utf16_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = CharSets.Utf16
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-16");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.Unicode).ShouldContain("\t");

        It should_use_utf16_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-16");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_utf32_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = Encoding.UTF32.WebName
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-32");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.UTF32).ShouldContain("\t");

        It should_use_utf32_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-32");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_utf32_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = Encoding.UTF32.WebName
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-32");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.UTF32).ShouldContain("\t");

        It should_use_utf32_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-32");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_ascii_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = CharSets.Ascii
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=us-ascii");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.ASCII).ShouldContain("\t");

        It should_use_ascii_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("us-ascii");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_ascii_encoding : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Encoding = CharSets.Ascii
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=us-ascii");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_tab_identation =
            () => GetRequestData(Encoding.ASCII).ShouldContain("\t");

        It should_use_ascii_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("us-ascii");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_without_identation : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Indent = false
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_not_use_tab_identation =
            () => GetRequestData().ShouldNotContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_without_identation : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                Indent = false
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_not_use_tab_identation =
            () => GetRequestData().ShouldNotContain("\t");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_redefined_ident_chars : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                IndentChars = "\r\r\r\r\r"
            };
        };

        Because of =
            () => attribute.Serialize(original_object, mock_web_request.Object);

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_specified_chars_for_identation =
            () => GetRequestData().ShouldContain("\r\r\r\r\r");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_redefined_ident_chars : request_serialization_test_context
    {
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute
            {
                IndentChars = "\r\r\r\r\r"
            };
        };

        Because of =
            () => attribute.SerializeAsync(original_object, mock_web_request.Object, CancellationToken.None).Wait();

        It should_set_request_content_type_as_xml =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/xml; charset=utf-8");

        It should_serialize_object =
            () => ParseRequestDataAsXml<TestTarget>().ShouldBeSimilar(original_object);

        It should_not_serialize_doctype_definition =
            () => GetRequestDataAsXDocument().DocumentType.ShouldBeNull();

        It should_use_specified_chars_for_identation =
            () => GetRequestData().ShouldContain("\r\r\r\r\r");

        It should_use_utf8_encoding =
            () => GetRequestDataAsXDocument().Declaration.Encoding.ShouldBeEqualIgnoringCase("utf-8");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_newed
    {
        static SerializeAsXmlAttribute attribute;

        Because of = 
            () => attribute = new SerializeAsXmlAttribute();

        It should_set_encoding_to_utf8 =
            () => attribute.Encoding.ShouldEqual(CharSets.Utf8);

        It should_set_ident_to_true =
            () => attribute.Indent.ShouldBeTrue();

        It should_set_ident_chars_to_tab =
            () => attribute.IndentChars.ShouldEqual("\t");

        It should_set_doc_type_name_to_null =
            () => attribute.DocTypeName.ShouldBeNull();

        It should_set_pubid_to_null =
            () => attribute.Pubid.ShouldBeNull();

        It should_set_sysid_to_null =
            () => attribute.Sysid.ShouldBeNull();

        It should_set_subset_to_null =
            () => attribute.Subset.ShouldBeNull();

        It should_set_request_content_encoding_to_null =
            () => attribute.RequestContentEncoding.ShouldBeNull();
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    class when_serialize_as_xml_attribute_is_used_with_null_object : request_serialization_test_context
    {
        static Exception exception;
        static SerializeAsXmlAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(null, mock_web_request.Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_obj_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("obj");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    class when_serialize_as_xml_attribute_is_used_asynchronously_with_null_object : request_serialization_test_context
    {
        static Exception exception;
        static SerializeAsXmlAttribute attribute;

        Establish context =
            () => attribute = new SerializeAsXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.SerializeAsync(null, mock_web_request.Object, CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_obj_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("obj");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute();
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    public class when_serialize_as_xml_attribute_is_used_asynchronously_with_null_request : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute();
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.SerializeAsync(original_object, null, CancellationToken.None).Wait());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    class when_setting_encoding_in_serialize_as_xml_attribute_to_null
    {
        static SerializeAsXmlAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Encoding = null);

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    class when_setting_encoding_in_serialize_as_xml_attribute_to_empty_string
    {
        static SerializeAsXmlAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SerializeAsXmlAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.Encoding = "");

        It should_throw_argument_null_excpetion =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(SerializeAsXmlAttribute))]
    class when_setting_encoding_in_serialize_as_xml_attribute_to_unknown_charset : request_serialization_test_context
    {
        static Exception exception;
        static TestTarget original_object;
        static SerializeAsXmlAttribute attribute;

        Establish context = () =>
        {
            original_object = new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            attribute = new SerializeAsXmlAttribute { Encoding = "-unknown-charset-" };
        };

        Because of =
            () => exception = Catch.Exception(() => attribute.Serialize(original_object, mock_web_request.Object));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldNotBeNull();
    }
}

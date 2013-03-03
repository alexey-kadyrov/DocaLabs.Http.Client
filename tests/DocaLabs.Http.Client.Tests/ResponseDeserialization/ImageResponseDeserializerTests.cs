using System;
using System.Drawing;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.ResponseDeserialization;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.ResponseDeserialization
{
    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_image_type_on_gif : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Image target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/gif", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.gif"))));
        };

        Because of =
            () => target = (Image)deserializer.Deserialize(http_response, typeof(Image));

        It should_deserialize_image = 
            () => target.ShouldMatch( x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_bitmap_type_on_gif : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Bitmap target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/gif", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.gif"))));
        };

        Because of =
            () => target = (Bitmap)deserializer.Deserialize(http_response, typeof(Bitmap));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_image_type_on_jpeg : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Image target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/jpeg", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.jpg"))));
        };

        Because of =
            () => target = (Image)deserializer.Deserialize(http_response, typeof(Image));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_bitmap_type_on_jpeg : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Bitmap target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/jpeg", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.jpg"))));
        };

        Because of =
            () => target = (Bitmap)deserializer.Deserialize(http_response, typeof(Bitmap));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_image_type_on_tiff : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Image target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/tiff", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        Because of =
            () => target = (Image)deserializer.Deserialize(http_response, typeof(Image));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_bitmap_type_on_tiff : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Bitmap target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/tiff", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        Because of =
            () => target = (Bitmap)deserializer.Deserialize(http_response, typeof(Bitmap));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_image_type_on_png : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Image target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => target = (Image)deserializer.Deserialize(http_response, typeof(Image));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_bitmap_type_on_png : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Bitmap target;

        Cleanup after_each =
            () => target.Dispose();

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => target = (Bitmap)deserializer.Deserialize(http_response, typeof(Bitmap));

        It should_deserialize_image =
            () => target.ShouldMatch(x => x.Width == 32 && x.Height == 32);
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_empty_response_stream : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(new byte[0]));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(Image)));

        It should_throw_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_null_result_type : response_deserialization_test_context
    {
        static Exception exception;
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    public class when_image_deserializer_is_used_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static ImageResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new ImageResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(null, typeof(Image)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_on_bad_image_value : response_deserialization_test_context
    {
        const string data = "} : Bad image data : {";
        static ImageResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(Encoding.UTF8.GetBytes(data)));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(Image)));

        It should_throw_exception =
            () => exception.ShouldNotBeNull();
    }

    [Subject(typeof(ImageResponseDeserializer), "deserialization")]
    class when_image_deserializer_is_used_with_type_other_than_image_or_bitmap : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static Exception exception;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.Deserialize(http_response, typeof(TestTarget)));

        It should_throw_unrecoverable_http_client_exception =
            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_with_null_result_type : response_deserialization_test_context
    {
        static Exception exception;
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(http_response, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    public class when_image_deserializer_is_checking_with_null_response : response_deserialization_test_context
    {
        static Exception exception;
        static ImageResponseDeserializer deserializer;

        Establish context =
            () => deserializer = new ImageResponseDeserializer();

        Because of =
            () => exception = Catch.Exception(() => deserializer.CanDeserialize(null, typeof(Image)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_response_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("response");
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_empty_content_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;
        static bool can_deserialize;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        Because of =
            () => can_deserialize = deserializer.CanDeserialize(http_response, typeof(Image));

        It should_not_be_able_to_deserialize =
            () => can_deserialize.ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_gif_content_type_for_image_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/gif", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.gif"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Image)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_jpeg_content_type_for_image_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/jpeg", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.jpg"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Image)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_tiff_content_type_for_image_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/tiff", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Image)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_png_content_type_for_image_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Image)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_gif_content_type_for_bitmap_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/gif", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.gif"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Bitmap)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_jpeg_content_type_for_bitmap_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/jpeg", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.jpg"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Bitmap)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_tiff_content_type_for_bitmap_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/tiff", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Bitmap)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_png_content_type_for_bitmap_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        It should_be_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Bitmap)).ShouldBeTrue();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_gif_content_type_for_type_other_than_bitmap_or_image : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/gif", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.gif"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_jpeg_content_type_for_type_other_than_bitmap_or_image : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/jpeg", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.jpg"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_tiff_content_type_for_type_other_than_bitmap_or_image : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/tiff", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_png_content_type_for_type_other_than_bitmap_or_image : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("image/png", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(TestTarget)).ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_non_image_content_type_for_bitmap_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("text/plain", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.tif"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Bitmap)).ShouldBeFalse();
    }

    [Subject(typeof(ImageResponseDeserializer), "checking that can deserialize")]
    class when_image_deserializer_is_checking_response_with_non_image_content_type_for_image_type : response_deserialization_test_context
    {
        static ImageResponseDeserializer deserializer;

        Establish context = () =>
        {
            deserializer = new ImageResponseDeserializer();
            Setup("text/plain", new MemoryStream(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Utils/img.png"))));
        };

        It should_be_not_able_to_deserialize =
            () => deserializer.CanDeserialize(http_response, typeof(Image)).ShouldBeFalse();
    }
}

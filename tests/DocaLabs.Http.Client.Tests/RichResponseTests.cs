using System.Net;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(RichResponse<>))]
    class when_response_is_instantiated
    {
        static RichResponse<string> response;

        Because of =
            () => response = new RichResponse<string>(412, "Condition Failed", "Hello World!");

        It should_initialize_status_code =
            () => response.StatusCode.ShouldEqual(412);

        It should_initialize_status_description =
            () => response.StatusDescription.ShouldEqual("Condition Failed");

        It should_initialize_value =
            () => response.Value.ShouldEqual("Hello World!");
    }

    [Subject(typeof(RichResponse<>))]
    class when_response_is_instantiated_with_null_value_for_value_type
    {
        static RichResponse<int> response;

        Because of =
            () => response = new RichResponse<int>(412, "Condition Failed", null);

        It should_initialize_status_code =
            () => response.StatusCode.ShouldEqual(412);

        It should_initialize_status_description =
            () => response.StatusDescription.ShouldEqual("Condition Failed");

        It should_set_value_to_deafult =
            () => response.Value.ShouldEqual(0);
    }

    [Subject(typeof(RichResponse<>))]
    class when_comparing_http_status_code
    {
        static RichResponse<int> response;

        Because of =
            () => response = new RichResponse<int>(409, "Condition Failed", null);

        It should_return_true_if_codes_are_equal =
            () => response.Is(HttpStatusCode.Conflict).ShouldBeTrue();

        It should_return_false_if_codes_are_not_equal =
            () => response.Is(HttpStatusCode.ExpectationFailed).ShouldBeFalse();
    }

    [Subject(typeof(RichResponse<>))]
    class when_comparing_ftp_status_code
    {
        static RichResponse<int> response;

        Because of =
            () => response = new RichResponse<int>(221, "Condition Failed", null);

        It should_return_true_if_codes_are_equal =
            () => response.Is(FtpStatusCode.ClosingControl).ShouldBeTrue();

        It should_return_false_if_codes_are_not_equal =
            () => response.Is(FtpStatusCode.SendUserCommand).ShouldBeFalse();
    }
}

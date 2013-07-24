using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(Response<>))]
    class when_response_is_instantiated
    {
        static Response<string> response;

        Because of =
            () => response = new Response<string>(412, "Condition Failed", "Hello World!");

        It should_initialize_status_code =
            () => response.StatusCode.ShouldEqual(412);

        It should_initialize_status_description =
            () => response.StatusDescription.ShouldEqual("Condition Failed");

        It should_initialize_value =
            () => response.Value.ShouldEqual("Hello World!");
    }

    [Subject(typeof(Response<>))]
    class when_response_is_instantiated_with_null_value_for_value_type
    {
        static Response<int> response;

        Because of =
            () => response = new Response<int>(412, "Condition Failed", null);

        It should_initialize_status_code =
            () => response.StatusCode.ShouldEqual(412);

        It should_initialize_status_description =
            () => response.StatusDescription.ShouldEqual("Condition Failed");

        It should_set_value_to_deafult =
            () => response.Value.ShouldEqual(0);
    }
}

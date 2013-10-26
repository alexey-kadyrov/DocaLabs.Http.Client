using System;
using DocaLabs.Http.Client.Binding;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(ModelBinders))]
    class when_getting_default_request_binder
    {
        It should_return_instance_of_the_default_request_binder_class =
            () => ModelBinders.DefaultRequestBinder.ShouldBeOfType<DefaultRequestBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_getting_default_response_binder
    {
        It should_return_instance_of_the_default_response_binder_class =
            () => ModelBinders.DefaultResponseBinder.ShouldBeOfType<DefaultResponseBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_setting_request_binder
    {
        static Mock<IRequestBinder> binder;
        static IRequestBinder original;

        Cleanup after =
            () => ModelBinders.DefaultRequestBinder = original;

        Establish context = () =>
        {
            original = ModelBinders.DefaultRequestBinder;
            binder = new Mock<IRequestBinder>();
        };

        Because of =
            () => ModelBinders.DefaultRequestBinder = binder.Object;

        It should_return_instance_of_the_set_binder =
            () => ModelBinders.DefaultRequestBinder.ShouldBeTheSameAs(binder.Object);
    }

    [Subject(typeof(ModelBinders))]
    class when_setting_response_binder
    {
        static Mock<IResponseBinder> binder;
        static IResponseBinder original;

        Cleanup after =
            () => ModelBinders.DefaultResponseBinder = original;

        Establish context = () =>
        {
            original = ModelBinders.DefaultResponseBinder;
            binder = new Mock<IResponseBinder>();
        };

        Because of =
            () => ModelBinders.DefaultResponseBinder = binder.Object;

        It should_return_instance_of_the_set_binder =
            () => ModelBinders.DefaultResponseBinder.ShouldBeTheSameAs(binder.Object);
    }

    [Subject(typeof(ModelBinders))]
    class when_getting_request_binder
    {
        static Mock<IRequestBinder> binder;

        Establish context =
            () => binder = new Mock<IRequestBinder>();

        Because of =
            () => ModelBinders.Add(typeof (string), binder.Object);
        
        It should_return_instance_of_the_registered_binder_for_the_type =
            () => ModelBinders.GetRequestBinder(typeof(string)).ShouldBeTheSameAs(binder.Object);

        It should_return_instance_of_the_default_request_binder_class_for_non_registered_type =
            () => ModelBinders.GetRequestBinder(typeof(int)).ShouldBeOfType<DefaultRequestBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_getting_response_binder
    {
        static Mock<IResponseBinder> binder;

        Establish context =
            () => binder = new Mock<IResponseBinder>();

        Because of =
            () => ModelBinders.Add(typeof(string), binder.Object);

        It should_return_instance_of_the_registered_binder_for_the_type =
            () => ModelBinders.GetResponseBinder(typeof(string)).ShouldBeTheSameAs(binder.Object);

        It should_return_instance_of_the_default_response_binder_class_for_non_registered_type =
            () => ModelBinders.GetResponseBinder(typeof(int)).ShouldBeOfType<DefaultResponseBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_getting_asynchronous_response_binder
    {
        static Mock<IAsyncResponseBinder> binder;

        Establish context =
            () => binder = new Mock<IAsyncResponseBinder>();

        Because of =
            () => ModelBinders.Add(typeof(string), binder.Object);

        It should_return_instance_of_the_registered_binder_for_the_type =
            () => ModelBinders.GetAsyncResponseBinder(typeof(string)).ShouldBeTheSameAs(binder.Object);

        It should_return_instance_of_the_default_response_binder_class_for_non_registered_type =
            () => ModelBinders.GetResponseBinder(typeof(int)).ShouldBeOfType<DefaultResponseBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_setting_default_request_binder_to_null
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => { ModelBinders.DefaultRequestBinder = null; });

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("value");

        It should_still_return_instance_of_the_default_request_binder_class_afterwards =
            () => ModelBinders.DefaultRequestBinder.ShouldBeOfType<DefaultRequestBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_setting_default_response_binder_to_null
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => { ModelBinders.DefaultResponseBinder = null; });

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");

        It should_still_return_instance_of_the_default_response_binder_class_afterwards =
            () => ModelBinders.DefaultResponseBinder.ShouldBeOfType<DefaultResponseBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_setting_default_asynchronous_response_binder_to_null
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => { ModelBinders.AsyncDefaultResponseBinder = null; });

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");

        It should_still_return_instance_of_the_default_response_binder_class_afterwards =
            () => ModelBinders.DefaultResponseBinder.ShouldBeOfType<DefaultResponseBinder>();
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_request_binder_for_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(null, new Mock<IRequestBinder>().Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_null_request_binder
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(typeof(string), (IRequestBinder)null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("binder");
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_response_binder_for_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(null, new Mock<IResponseBinder>().Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_asynchronous_response_binder_for_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(null, new Mock<IAsyncResponseBinder>().Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_null_response_binder
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(typeof(string), (IResponseBinder)null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("binder");
    }

    [Subject(typeof(ModelBinders))]
    class when_registering_null_asynchronous_response_binder
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ModelBinders.Add(typeof(string), (IAsyncResponseBinder)null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("binder");
    }
}

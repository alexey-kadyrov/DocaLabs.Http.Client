using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace DocaLabs.Testing.Common.MSpec
{
    // ReSharper disable StaticFieldInGenericType

    public class ExceptionIsNewedUsingDefaultConstructorContext<TException>
        where TException : Exception, new()
    {
        protected static TException exception;

        Because of = 
            () => exception = new TException();
    }

    [Behaviors]
    public class ExceptionIsNewedUsingDefaultConstructorBehaviour
    {
        protected static Exception exception;

        It should_set_message_to_default_value =
            () => exception.Message.ShouldEqual(string.Format("Exception of type '{0}' was thrown.", exception.GetType()));

        It should_set_inner_exception_to_null =
            () => exception.InnerException.ShouldBeNull();
    }

    public class ExceptionIsNewedUsingOverloadConstructorWithMessageContext<TException>
        where TException : Exception, new()
    {
        protected static TException exception;

        Because of =
            () => exception = Activator.CreateInstance(typeof(TException), "my-message") as TException;
    }

    [Behaviors]
    public class ExceptionIsNewedUsingOverloadConstructorWithMessageBehaviour
    {
        protected static Exception exception;

        It should_set_message_to_specfied_value =
            () => exception.Message.ShouldEqual("my-message");

        It should_set_inner_exception_to_null =
            () => exception.InnerException.ShouldBeNull();
    }

    public class ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionContext<TException>
        where TException : Exception, new()
    {
        protected static TException exception;
        protected static Exception inner_exception;

        Establish context =
            () => inner_exception = new Exception();

        Because of =
            () => exception = Activator.CreateInstance(typeof(TException), "my-message", inner_exception) as TException;
    }

    [Behaviors]
    public class ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionBehaviour
    {
        protected static Exception exception;
        protected static Exception inner_exception;

        It should_set_message_to_specfied_value =
            () => exception.Message.ShouldEqual("my-message");

        It should_set_inner_exception_to_spefied_value =
            () => exception.InnerException.ShouldBeTheSameAs(inner_exception);
    }

    public class ExceptionIsSerializedContext<TException>
        where TException : Exception, new()
    {
        protected static ArgumentNullException original_inner_exception;
        protected static TException original_exception;
        protected static TException deserialized_exception;

        Establish context = () =>
        {
            original_inner_exception = new ArgumentNullException(null, @"inner-message");
            original_exception = Activator.CreateInstance(typeof (TException), "my-message", original_inner_exception) as TException;
        };

        Because of = () =>
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.All));

                formatter.Serialize(stream, original_exception);

                stream.Position = 0;

                deserialized_exception = formatter.Deserialize(stream) as TException;
            }
        };
    }

    [Behaviors]
    public class ExceptionIsSerializedBehaviour
    {
        protected static ArgumentNullException original_inner_exception;
        protected static Exception original_exception;
        protected static Exception deserialized_exception;

        It deserialize_exception_should_not_be_the_same_instance_as_the_original =
            () => deserialized_exception.ShouldNotBeTheSameAs(original_exception);

        It deserialized_inner_exception_should_not_be_the_same_instance_as_the_original_inner_exception =
            () => deserialized_exception.InnerException.ShouldNotBeTheSameAs(original_exception.InnerException);

        It deserialize_exception_should_be_the_same_type_as_the_original =
            () => deserialized_exception.ShouldBeOfType(original_exception.GetType());

        It deserialized_inner_exception_should_be_the_same_type_as_the_original_inner_exception =
            () => deserialized_exception.InnerException.ShouldBeOfType<ArgumentNullException>();

        It deserialize_exception_should_have_the_same_message_as_the_original =
            () => deserialized_exception.Message.ShouldEqual("my-message");

        It deserialize_exception_should_have_message_for_inner_exception_the_same_as_in_the_original_inner_exception =
            () => deserialized_exception.InnerException.Message.ShouldEqual("inner-message");
    }

    // ReSharper restore StaticFieldInGenericType
}

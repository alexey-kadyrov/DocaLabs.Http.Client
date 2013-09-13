﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    [Subject(typeof(DefaultJsonDeserializer))]
    class when_default_json_deserializer_is_used
    {
        static DefaultJsonDeserializer deserializer;
        static TestTarget target;

        Establish context = 
            () => deserializer = new DefaultJsonDeserializer();

        Because of =
            () => target = (TestTarget)deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof(TestTarget));

        It should_deserialize_object = () => target.ShouldBeSimilar(new TestTarget
        {
            Value1 = 2012,
            Value2 = "Hello World!"
        });
    }

    [Subject(typeof(DefaultJsonDeserializer))]
    class when_updating_serialization_settings_with_null_type_for_default_json_deserializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => DefaultJsonDeserializer.UpdateSettings(null, new SerializationSettings()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(DefaultJsonDeserializer))]
    class when_updating_serialization_settings_with_null_settings_for_default_json_deserializer
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => DefaultJsonDeserializer.UpdateSettings(typeof(TestTarget), null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_settings_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("settings");
    }

    [Subject(typeof(DefaultJsonDeserializer))]
    class when_default_json_deserializer_is_used_concurrently
    {
        static DefaultJsonDeserializer deserializer;
        static int count;

        Establish context =
            () => deserializer = new DefaultJsonDeserializer();

        Because of = () => Parallel.For(0, 500000, i =>
        {
            var target = (TestTarget) deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof (TestTarget));

            target.ShouldBeSimilar(new TestTarget
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            });

            Interlocked.Increment(ref count);
        });

        It should_deserialize_all_object =
            () => count.ShouldEqual(500000);
    }
}

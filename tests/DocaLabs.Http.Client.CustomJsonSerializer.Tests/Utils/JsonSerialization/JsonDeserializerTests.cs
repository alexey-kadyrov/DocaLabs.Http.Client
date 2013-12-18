using System;
using System.Threading;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [TestClass]
    public class when_json_deserializer_is_used
    {
        static JsonDeserializer _deserializer;
        static Model _model;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _deserializer = new JsonDeserializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _model = (Model)_deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof(Model));
        }

        [TestMethod]
        public void it_should_deserialize_object()
        {
            _model.ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");
        }

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    [TestClass]
    public class when_json_deserializer_is_used_with_null_result_type
    {
        static JsonDeserializer _deserializer;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _deserializer = new JsonDeserializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", null));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_result_type_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("resultType");
        }
    }

    [TestClass]
    public class when_updating_serialization_settings_with_null_type_for_json_deserializer
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => JsonDeserializer.UpdateSettings(null, new JsonSerializerSettings()));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_type_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("type");
        }
    }

    [TestClass]
    public class when_json_deserializer_is_used_with_json_serializer_settings
    {
        static JsonDeserializer deserializer;
        static Exception exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            JsonDeserializer.UpdateSettings(typeof(Model), new JsonSerializerSettings { MaxDepth = 1 });

            deserializer = new JsonDeserializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            exception = Catch.Exception(() => deserializer.Deserialize("{Value:\"Hello World!\", AnotherModel : { \"$type\" : \"InnerModel\", \"InnerValue\" : \"Value42\"}}", typeof(Model)));
        }

        [TestMethod]
        public void it_should_use_that_settings()
        {
            exception.ShouldNotBeNull();
        }

        class Model
        {
            public string Value { get; set; }
            public object AnotherModel { get; set; }
        }

        class InnerModel
        {
            public string InnerValue { get; set; }
        }
    }

    [TestClass]
    public class when_json_deserializer_is_used_concurrently
    {
        static JsonDeserializer _deserializer;
        static int _count;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _deserializer = new JsonDeserializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            var model = (Model)_deserializer.Deserialize("{Value1:2012, Value2:\"Hello World!\"}", typeof(Model));

            model.ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

            Interlocked.Increment(ref _count);
        }

        [TestMethod]
        public void it_should_deserialize_all_object()
        {
            _count.ShouldEqual(10000);
        }

        public class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}

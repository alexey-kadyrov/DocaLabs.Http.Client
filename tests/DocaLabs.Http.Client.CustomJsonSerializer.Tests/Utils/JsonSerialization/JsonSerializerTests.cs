using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using JsonSerializer = DocaLabs.Http.Client.Utils.JsonSerialization.JsonSerializer;

namespace DocaLabs.Http.Client.Tests.Utils.JsonSerialization
{
    [TestClass]
    public class when_json_serializer_is_used
    {
        static Model _model;
        static string _result;
        static JsonSerializer _serializer;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _model = new Model
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            _serializer = new JsonSerializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _serializer.Serialize(_model);
        }

        [TestMethod]
        public void it_should_serialize_object()
        {
            JsonConvert.DeserializeObject<Model>(_result).ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");
        }

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    [TestClass]
    public class when_updating_serialization_settings_with_null_type_for_json_serializer
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => JsonSerializer.UpdateSettings(null, new SerializationSettings()));
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
    public class when_updating_serialization_settings_with_null_settings_for_json_serializer
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => JsonSerializer.UpdateSettings(typeof (Model), null));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_settings_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("settings");
        }

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    [TestClass]
    public class when_json_serializer_is_used_with_customized_json_serializer_settings
    {
        static Model _model;
        static SerializationBinderStub _binder;
        static string _jsonString;
        static JsonSerializer _serializer;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _binder = new SerializationBinderStub();

            JsonSerializer.UpdateSettings(typeof(Model), new SerializationSettings { Settings = new JsonSerializerSettings
            {
                Binder = _binder,
                TypeNameHandling = TypeNameHandling.All
            }});

            _model = new Model
            {
                Value = "Hello World!"
            };

            _serializer = new JsonSerializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _jsonString = _serializer.Serialize(_model);
        }

        [TestMethod]
        public void it_should_serialize_object()
        {
            JsonConvert.DeserializeObject<Model>(_jsonString).Value.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_use_provided_settings()
        {
            _binder.BinToNameCount.ShouldBeEqualOrGreaterThan(0);
        }

        class Model
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_json_serializer_is_used_with_customized_type
    {
        static Model _model;
        static SerializationBinderStub _binder;
        static string _jsonString;
        static JsonSerializer _serializer;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _binder = new SerializationBinderStub();

            JsonSerializer.UpdateSettings(typeof(Model), new SerializationSettings
            {
                Settings = new JsonSerializerSettings
                {
                    Binder = _binder,
                    TypeNameHandling = TypeNameHandling.Auto
                },
                Type = typeof(object)
            });

            _model = new Model
            {
                Value = "Hello World!"
            };

            _serializer = new JsonSerializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _jsonString = _serializer.Serialize(_model);
        }

        [TestMethod]
        public void it_should_serialize_object()
        {
            JsonConvert.DeserializeObject<Model>(_jsonString).Value.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_use_provided_type()
        {
            _binder.BinToNameCount.ShouldBeEqualOrGreaterThan(0);
        }

        class Model
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_json_serializer_is_used_concurrently
    {
        static Model _originalObject;
        static JsonSerializer _serializer;
        static int _count;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _originalObject = new Model
            {
                Value1 = 2012,
                Value2 = "Hello World!"
            };

            _serializer = new JsonSerializer();

            BecauseOf();
        }

        static void BecauseOf()
        {
            Parallel.For(0, 10000, i =>
            {
                var target = _serializer.Serialize(_originalObject);

                JsonConvert.DeserializeObject<Model>(target)
                    .ShouldMatch(x => x.Value1 == 2012 && x.Value2 == "Hello World!");

                Interlocked.Increment(ref _count);
            });
        }

        [TestMethod]
        public void it_should_deserialize_all_object()
        {
            _count.ShouldEqual(10000);
        }

        class Model
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
        }
    }

    class SerializationBinderStub : SerializationBinder
    {
        public int BinToNameCount { get; private set; }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            BinToNameCount++;
            base.BindToName(serializedType, out assemblyName, out typeName);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return null;
        }
    }
}

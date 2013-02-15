using System;
using Moq;
using NUnit.Framework;

namespace DocaLabs.Conversion.Tests
{
    [TestFixture]
    public class CustomConverterTests
    {
        [Test]
        public void CurrentIsOfCustomConverterType()
        {
            Assert.IsNotNull(CustomConverter.Current);
            Assert.IsInstanceOf<CustomConverter>(CustomConverter.Current);
        }

        [Test]
        public void FactoryOfTheCurrentIsOfCustomConverterFactoryType()
        {
            Assert.IsNotNull(CustomConverter.Current.Factory);
            Assert.IsInstanceOf<CustomConverterFactory>(CustomConverter.Current.Factory);
        }

        [Test]
        public void DefaultCtorCreatesInstanceWithFactoryOfCustomConverterFactoryType()
        {
            var target = new CustomConverter();

            Assert.IsNotNull(target.Factory);
            Assert.IsInstanceOf<CustomConverterFactory>(target.Factory);
        }

        [Test]
        public void CtorCreatesInstanceWithSpecifiedFactoryInstance()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            var target = new CustomConverter(mockFactory.Object);

            Assert.IsNotNull(target.Factory);
            Assert.AreSame(mockFactory.Object, target.Factory);
        }

        [Test]
        public void CtorThrowsArgumentNullExceptionWhenPassedNullFactoryInstance()
        {
            var exception = Assert.Catch<ArgumentNullException>(() => new CustomConverter(null));

            Assert.AreEqual("factory", exception.ParamName);
        }

        [Test]
        public void ChangeTypeCallsConverterForRegisteredType()
        {
            Func<object, object> converter = x => "Hello World!";

            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(string)))
                .Returns(converter);

            var target = new CustomConverter(mockFactory.Object);

            var result = target.ChangeType("any value", typeof (string));

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello World!", result);
        }

        [Test]
        public void ChangeTypeUsesFrameworkForNonRegisteredType()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(CustomConverterTests)))
                .Returns<Func<object, object>>(null);

            var target = new CustomConverter(mockFactory.Object);

            var result = target.ChangeType("42", typeof(int));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(42, result);
        }

        [Test]
        public void GenericChangeTypeCallsConverterForRegisteredType()
        {
            Func<object, object> converter = x => "Hello World!";

            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(string)))
                .Returns(converter);

            var target = new CustomConverter(mockFactory.Object);

            var result = target.ChangeType<string>("any value");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello World!", result);
        }

        [Test]
        public void GenericChangeTypeUsesFrameworkForNonRegisteredType()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(CustomConverterTests)))
                .Returns<Func<object, object>>(null);

            var target = new CustomConverter(mockFactory.Object);

            var result = target.ChangeType<int>("42");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(42, result);
        }

        [Test]
        public void GetDefaultValueReturnsDefaultValueForType()
        {
            Assert.AreEqual(null, CustomConverter.GetDefaultValue(typeof(CustomConverterTests)));
            Assert.AreEqual(null, CustomConverter.GetDefaultValue(typeof(string)));
            Assert.AreEqual(0, CustomConverter.GetDefaultValue(typeof(int)));
            Assert.AreEqual(0L, CustomConverter.GetDefaultValue(typeof(long)));
            Assert.AreEqual(Guid.Empty, CustomConverter.GetDefaultValue(typeof(Guid)));
            Assert.AreEqual(DateTime.MinValue, CustomConverter.GetDefaultValue(typeof(DateTime)));
        }

        [Test]
        public void GenericGetDefaultReturnsDefaultValueForType()
        {
            Assert.AreEqual(null, CustomConverter.GetDefault<CustomConverterTests>());
            Assert.AreEqual(null, CustomConverter.GetDefault<string>());
            Assert.AreEqual(0, CustomConverter.GetDefault<int>());
            Assert.AreEqual(0L, CustomConverter.GetDefault<long>());
            Assert.AreEqual(Guid.Empty, CustomConverter.GetDefault<Guid>());
            Assert.AreEqual(DateTime.MinValue, CustomConverter.GetDefault<DateTime>());
        }
    }
}

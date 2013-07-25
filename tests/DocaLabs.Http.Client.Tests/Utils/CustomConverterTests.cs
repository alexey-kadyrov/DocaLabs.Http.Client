using System;
using System.Globalization;
using DocaLabs.Http.Client.Utils;
using Moq;
using NUnit.Framework;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestFixture]
    public class CustomConverterTests
    {
        [TearDown]
        public void TearDown()
        {
            CustomConverter.Current = null;
        }

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
        public void ChangeToStringCallsConverterForRegisteredTypeWhenFormatIsNull()
        {
            Func<object, object> converter = x => "Hello World!";

            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(string)))
                .Returns(converter);

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString(null, CultureInfo.InvariantCulture, "any value");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello World!", result);
        }

        [Test]
        public void ChangeToStringCallsConverterForRegisteredTypeWhenFormatIsEmptyString()
        {
            Func<object, object> converter = x => "Hello World!";

            var mockFactory = new Mock<ICustomConverterFactory>();
            mockFactory.Setup(x => x.GetConverter(typeof(string)))
                .Returns(converter);

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString("", CultureInfo.InvariantCulture, "any value");

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello World!", result);
        }

        [Test]
        public void ChangeToStringUsesStringFormatWhenFormatIsNotEmptyString()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString("{0:X}", CultureInfo.InvariantCulture, 32);

            Assert.IsNotNull(result);
            Assert.AreEqual("20", result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void ChangeToStringUsesStringFormatWhenFormatIsNotEmptyStringButTheCultureIsNull()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString("{0:X}", null, 32);

            Assert.IsNotNull(result);
            Assert.AreEqual("20", result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void ChangeToStringUsesStringFormatWhenFormatIsNotEmptyStringAndNonEnglishCulture()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString("{0:MMM}", new CultureInfo("ru-RU"), new DateTime(2013, 2, 12));

            Assert.IsNotNull(result);
            Assert.AreEqual("фев", result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void ChangeToStringReturnEmptyStringWhenFormatIsNotEmptyStringAndTheValueIsNull()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString("{0:X}", CultureInfo.InvariantCulture, null);

            Assert.IsEmpty(result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void ChangeToStringReturnEmptyStringWhenFormatIsNullAndTheValueIsNull()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString(null, CultureInfo.InvariantCulture, null);

            Assert.IsEmpty(result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void ChangeToStringReturnEmptyStringWhenFormatIsEmptyAndTheValueIsNull()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            CustomConverter.Current = new CustomConverter(mockFactory.Object);

            var result = CustomConverter.ChangeToString(null, CultureInfo.InvariantCulture, null);

            Assert.IsEmpty(result);

            mockFactory.Verify(x => x.GetConverter(typeof(string)), Times.Never());
        }

        [Test]
        public void SettingTheCurrentReturnsTheSetValue()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            var target = new CustomConverter(mockFactory.Object);

            CustomConverter.Current = target;

            Assert.AreSame(target, CustomConverter.Current);
        }

        [Test]
        public void SettingTheCurrentNullStillReturnsNotNullValue()
        {
            var mockFactory = new Mock<ICustomConverterFactory>();

            var target = new CustomConverter(mockFactory.Object);

            CustomConverter.Current = target;

            CustomConverter.Current = null;

            Assert.IsNotNull(CustomConverter.Current);
            Assert.AreNotSame(target, CustomConverter.Current);
        }
    }
}

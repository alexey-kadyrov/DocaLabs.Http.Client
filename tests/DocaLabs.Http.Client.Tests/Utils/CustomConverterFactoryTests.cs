using System;
using System.Globalization;
using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding.Utils;
using NUnit.Framework;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestFixture]
    public class CustomConverterFactoryTests
    {
        [Test]
        public void RegisterConverterAddsNewConverter()
        {
            var target = new CustomConverterFactory();

            Func<object, object> converter = x => null;

            target.RegisterConverter(typeof(CustomConverterFactoryTests), converter);

            var result = target.GetConverter(typeof(CustomConverterFactoryTests));

            Assert.AreSame(converter, result);
        }

        [Test]
        public void RegisterConverterChangesExisitongConverter()
        {
            var target = new CustomConverterFactory();

            Func<object, object> converter1 = x => null;
            Func<object, object> converter2 = x => new object();

            target.RegisterConverter(typeof(CustomConverterFactoryTests), converter1);

            // act here
            target.RegisterConverter(typeof(CustomConverterFactoryTests), converter2);

            var result = target.GetConverter(typeof(CustomConverterFactoryTests));

            Assert.AreSame(converter2, result);
        }

        [Test]
        public void RegisterConverterThrowsArgumentNullExceptionIfTheTypeIsNull()
        {
            var target = new CustomConverterFactory();

            Func<object, object> converter = x => null;

            var exception = Assert.Catch<ArgumentNullException>(() => target.RegisterConverter(null, converter));

            Assert.AreEqual("type", exception.ParamName);
        }

        [Test]
        public void RegisterConverterThrowsArgumentNullExceptionIfTheConverterIsNull()
        {
            var target = new CustomConverterFactory();

            var exception = Assert.Catch<ArgumentNullException>(() => target.RegisterConverter(typeof(CustomConverterFactoryTests), null));

            Assert.AreEqual("converter", exception.ParamName);
        }

        [Test]
        public void RemoveConverterRemovesExistingConverter()
        {
            var target = new CustomConverterFactory();

            Func<object, object> converter = x => null;

            target.RegisterConverter(typeof(CustomConverterFactoryTests), converter);

            var result1 = target.RemoveConverter(typeof (CustomConverterFactoryTests));

            var result2 = target.GetConverter(typeof(CustomConverterFactoryTests));

            Assert.IsNotNull(result1);
            Assert.AreSame(converter, result1);

            Assert.IsNull(result2);
        }

        [Test]
        public void RemoveConverterRetunrsNullForNonRegisteredConverter()
        {
            var target = new CustomConverterFactory();

            var result = target.RemoveConverter(typeof(CustomConverterFactoryTests));

            Assert.IsNull(result);
        }

        [Test]
        public void RemoveConverterThrowsArgumentNullExceptionIfTheTypeIsNull()
        {
            var target = new CustomConverterFactory();

            var exception = Assert.Catch<ArgumentNullException>(() => target.RemoveConverter(null));

            Assert.AreEqual("type", exception.ParamName);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(string))]
        [TestCase(typeof(byte[]))]
        public void GetConverterReturnsRegisteredConverter(Type type)
        {
            var target = new CustomConverterFactory();

            var result = target.GetConverter(type);

            Assert.IsNotNull(result);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void GetConverterReturnsRegisteredConverterForNullableTypes(Type type)
        {
            var target = new CustomConverterFactory();

            var result = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetConverterReturnsNullForNonRegisteredConverter()
        {
            var target = new CustomConverterFactory();

            var result = target.GetConverter(typeof(CustomConverterFactoryTests));

            Assert.IsNull(result);
        }

        [Test]
        // to bool
        [TestCase(true, true)]
        [TestCase(false, false)]
        [TestCase("true", true)]
        [TestCase("1", true)]
        [TestCase(1, true)]
        [TestCase(new byte[] { 1 }, true)]
        [TestCase("false", false)]
        [TestCase("0", false)]
        [TestCase(0, false)]
        [TestCase(new byte[] { 0 }, false)]
        [TestCase(new byte[] { 1 }, true)]
        // to int
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        [TestCase(123, 123)]
        [TestCase(123L, 123)]
        [TestCase(123.0, 123)]
        [TestCase(123.0F, 123)]
        [TestCase("123", 123)]
        [TestCase(-123, -123)]
        [TestCase(-123L, -123)]
        [TestCase(-123.0, -123)]
        [TestCase(-123.0F, -123)]
        [TestCase("-123", -123)]
        [TestCase(new byte[] { 0, 1, 2, 3 }, 50462976)]
        // to long
        [TestCase(true, 1L)]
        [TestCase(false, 0L)]
        [TestCase(89723, 89723L)]
        [TestCase(89723L, 89723L)]
        [TestCase(89723.0, 89723)]
        [TestCase(89723.0F, 89723)]
        [TestCase("89723", 89723L)]
        [TestCase(-89723, -89723)]
        [TestCase(-89723L, -89723L)]
        [TestCase(-89723.0, -89723)]
        [TestCase(-89723.0F, -89723)]
        [TestCase("-89723", -89723L)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 506097522914230528L)]
        // to double
        [TestCase(true, 1.0)]
        [TestCase(false, 0.0)]
        [TestCase(89723, 89723.0)]
        [TestCase(89723L, 89723.0)]
        [TestCase(89723.0, 89723.0)]
        [TestCase(89723.0F, 89723.0)]
        [TestCase("89723", 89723.0)]
        [TestCase(-89723, -89723.0)]
        [TestCase(-89723L, -89723.0)]
        [TestCase(-89723.0, -89723.0)]
        [TestCase(-89723.0F, -89723.0)]
        [TestCase("-89723", -89723.0)]
        [TestCase(new byte[] { 9, 69, 1, 3, 0, 100, 9, 200 }, -1.0799977541937219E+39d)]
        // to string
        [TestCase(true, "true")]
        [TestCase(false, "false")]
        [TestCase(89723, "89723")]
        [TestCase(89723L, "89723")]
        [TestCase(89723.45, "89723.45")]
        [TestCase(89723.67F, "89723.67")]
        [TestCase("89723", "89723")]
        [TestCase(-89723, "-89723")]
        [TestCase(-89723L, "-89723")]
        [TestCase(-89723.89, "-89723.89")]
        [TestCase(-89723.11F, "-89723.11")]
        [TestCase(new byte[] { 9, 69, 1, 3, 0, 100, 9, 200 }, "CUUBAwBkCcg=")]
        public void ChangesTypeToExpectedValue(object testValue, object expected)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(expected.GetType());

            var result = converter(testValue);

            Assert.IsInstanceOf(expected.GetType(), result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        // to bool
        [TestCase(true, true)]
        [TestCase(false, false)]
        [TestCase("true", true)]
        [TestCase("1", true)]
        [TestCase(1, true)]
        [TestCase(new byte[] { 1 }, true)]
        [TestCase("false", false)]
        [TestCase("0", false)]
        [TestCase(0, false)]
        [TestCase(new byte[] { 0 }, false)]
        [TestCase(new byte[] { 1 }, true)]
        // to int
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        [TestCase(123, 123)]
        [TestCase(123L, 123)]
        [TestCase(123.0, 123)]
        [TestCase(123.0F, 123)]
        [TestCase("123", 123)]
        [TestCase(-123, -123)]
        [TestCase(-123L, -123)]
        [TestCase(-123.0, -123)]
        [TestCase(-123.0F, -123)]
        [TestCase("-123", -123)]
        [TestCase(new byte[] { 0, 1, 2, 3 }, 50462976)]
        // to long
        [TestCase(true, 1L)]
        [TestCase(false, 0L)]
        [TestCase(89723, 89723L)]
        [TestCase(89723L, 89723L)]
        [TestCase(89723.0, 89723)]
        [TestCase(89723.0F, 89723)]
        [TestCase("89723", 89723L)]
        [TestCase(-89723, -89723)]
        [TestCase(-89723L, -89723L)]
        [TestCase(-89723.0, -89723)]
        [TestCase(-89723.0F, -89723)]
        [TestCase("-89723", -89723L)]
        [TestCase(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 506097522914230528L)]
        // to double
        [TestCase(true, 1.0)]
        [TestCase(false, 0.0)]
        [TestCase(89723, 89723.0)]
        [TestCase(89723L, 89723.0)]
        [TestCase(89723.0, 89723.0)]
        [TestCase(89723.0F, 89723.0)]
        [TestCase("89723", 89723.0)]
        [TestCase(-89723, -89723.0)]
        [TestCase(-89723L, -89723.0)]
        [TestCase(-89723.0, -89723.0)]
        [TestCase(-89723.0F, -89723.0)]
        [TestCase("-89723", -89723.0)]
        [TestCase(new byte[] { 9, 69, 1, 3, 0, 100, 9, 200 }, -1.0799977541937219E+39d)]
        public void ChangesTypeToExpectedNullableValue(object testValue, object expected)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(expected.GetType()));

            var result = converter(testValue);

            Assert.IsInstanceOf(expected.GetType(), result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(string))]
        [TestCase(typeof(byte[]))]
        public void ConverterReturnsDefaultValueWhenInputValueIsNull(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(type);

            var result = converter(null);

            Assert.AreEqual(CustomConverter.GetDefaultValue(type), result);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void NullableConverterReturnsNullWhenInputValueIsNull(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            var result = converter(null);

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void ConverterThrowsFormatExceptionWhenInputValueIsWhitespaceString(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(type);

            Assert.Catch<FormatException>(() => converter("  "));
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void NullableConverterReturnsNullWhenInputValueIsWhitespaceString(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            var result = converter("  ");

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void ConverterThrowsFormatExceptionWhenInputValueIsEmptyString(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(type);

            Assert.Catch<FormatException>(() => converter(string.Empty));
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        public void NullableConverterReturnsNullWhenInputValueIsEmptyString(Type type)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            var result = converter(string.Empty);

            Assert.IsNull(result);
        }

        [Test]
        [TestCase(typeof(Guid), false)]
        [TestCase(typeof(Guid), 1)]
        [TestCase(typeof(Guid), 14124L)]
        [TestCase(typeof(Guid), 198273.78)]
        [TestCase(typeof(DateTime), false)]
        [TestCase(typeof(DateTime), 1)]
        [TestCase(typeof(DateTime), 14124L)]
        [TestCase(typeof(DateTime), 198273.78)]
        [TestCase(typeof(DateTimeOffset), false)]
        [TestCase(typeof(DateTimeOffset), 1)]
        [TestCase(typeof(DateTimeOffset), 14124L)]
        [TestCase(typeof(DateTimeOffset), 198273.78)]
        [TestCase(typeof(TimeSpan), false)]
        [TestCase(typeof(TimeSpan), 1)]
        [TestCase(typeof(TimeSpan), 198273.78)]
        public void ConverterThrowsInvalidCastExceptionWhenCannotChangeType(Type type, object badValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(type);

            Assert.Catch<InvalidCastException>(() => converter(badValue));
        }

        [Test]
        [TestCase(typeof(Guid), "bad value")]
        [TestCase(typeof(DateTime), "bad value")]
        [TestCase(typeof(DateTimeOffset), "bad value")]
        [TestCase(typeof(TimeSpan), "bad value")]
        [TestCase(typeof(int), "bad value")]
        [TestCase(typeof(long), "bad value")]
        [TestCase(typeof(double), "bad value")]
        [TestCase(typeof(int), "bad value")]
        [TestCase(typeof(bool), "bad value")]
        [TestCase(typeof(decimal), "bad value")]
        public void ConverterThrowsFormatExceptionWhenCannotChangeTypeFromBadlyFormattedString(Type type, object badValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(type);

            Assert.Catch<FormatException>(() => converter(badValue));
        }

        [Test]
        [TestCase(typeof(Guid), false)]
        [TestCase(typeof(Guid), 1)]
        [TestCase(typeof(Guid), 14124L)]
        [TestCase(typeof(Guid), 198273.78)]
        [TestCase(typeof(DateTime), false)]
        [TestCase(typeof(DateTime), 1)]
        [TestCase(typeof(DateTime), 14124L)]
        [TestCase(typeof(DateTime), 198273.78)]
        [TestCase(typeof(DateTimeOffset), false)]
        [TestCase(typeof(DateTimeOffset), 1)]
        [TestCase(typeof(DateTimeOffset), 14124L)]
        [TestCase(typeof(DateTimeOffset), 198273.78)]
        [TestCase(typeof(TimeSpan), false)]
        [TestCase(typeof(TimeSpan), 1)]
        [TestCase(typeof(TimeSpan), 198273.78)]
        public void ConverterThrowsInvalidCastExceptionWhenCannotChangeToNullableType(Type type, object badValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            Assert.Catch<InvalidCastException>(() => converter(badValue));
        }

        [Test]
        [TestCase(typeof(Guid), "bad value")]
        [TestCase(typeof(DateTime), "bad value")]
        [TestCase(typeof(DateTimeOffset), "bad value")]
        [TestCase(typeof(TimeSpan), "bad value")]
        [TestCase(typeof(int), "bad value")]
        [TestCase(typeof(long), "bad value")]
        [TestCase(typeof(double), "bad value")]
        [TestCase(typeof(int), "bad value")]
        [TestCase(typeof(bool), "bad value")]
        [TestCase(typeof(decimal), "bad value")]
        public void ConverterThrowsFormatExceptionWhenCannotChangeTypeToNullableTypeFromBadlyFormattedString(Type type, object badValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(type));

            Assert.Catch<FormatException>(() => converter(badValue));
        }

        #region decimal tests

        [Test]
        public void StringConverterChangesTypeFromDecimal()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("123.45", converter(123.45M));
            Assert.AreEqual("-123.45", converter(-123.45M));
        }

        [Test]
        public void DecimalConverterChangesTypeFromDecimal()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(decimal));

            Assert.AreEqual(123.45M, converter(123.45M));
            Assert.AreEqual(-123.45M, converter(-123.45M));
        }

        [Test]
        public void NullableDecimalConverterChangesTypeFromDecimal()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(decimal?));

            Assert.AreEqual(123.45M, converter(123.45M));
            Assert.AreEqual(-123.45M, converter(-123.45M));
        }

        [Test]
        [TestCase("123.45", "123.45")]
        [TestCase("-123.45", "-123.45")]
        [TestCase(17865L, "17865")]
        [TestCase(-7463L, "-7463")]
        [TestCase(42, "42")]
        [TestCase(-78, "-78")]
        [TestCase(6573.96, "6573.96")]
        [TestCase(-7483.1, "-7483.1")]
        [TestCase(88F, "88")]
        [TestCase(-176.8F, "-176.8")]
        public void DecimalConverterChangesTypeToExpectedValue(object testValue, string expectedValueStr)
        {
            var expectedValue = decimal.Parse(expectedValueStr);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(decimal));

            Assert.AreEqual(expectedValue, converter(testValue));
        }

        [Test]
        [TestCase("123.45", "123.45")]
        [TestCase("-123.45", "-123.45")]
        [TestCase(17865L, "17865")]
        [TestCase(-7463L, "-7463")]
        [TestCase(42, "42")]
        [TestCase(-78, "-78")]
        [TestCase(6573.96, "6573.96")]
        [TestCase(-7483.1, "-7483.1")]
        [TestCase(88F, "88")]
        [TestCase(-176.8F, "-176.8")]
        public void NullableDecimalConverterChangesTypeToExpectedValue(object testValue, string expectedValueStr)
        {
            var expectedValue = decimal.Parse(expectedValueStr);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(decimal?));

            Assert.AreEqual(expectedValue, converter(testValue));
        }

        #endregion

        #region DateTime tests

        [Test]
        public void StringConverterChangesTypeFromUtcDateTime()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Utc);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("2009-10-22T14:59:32.125Z", converter(time));
        }

        [Test]
        public void StringConverterChangesTypeFromLocalDateTime()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Local);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            var localOffset = TimeZoneInfo.Local.GetUtcOffset(time.ToUniversalTime());

            var offset = localOffset.ToString("hhmm");

            offset = localOffset >= TimeSpan.Zero ? "+" + offset : "-" + offset;

            Assert.AreEqual("2009-10-22T14:59:32.125" + offset, converter(time));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromByteArray()
        {
            var time = DateTime.Now;

            var array = BitConverter.GetBytes(time.ToBinary());

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, converter(array));
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromByteArray()
        {
            var time = DateTime.Now;

            var array = BitConverter.GetBytes(time.ToBinary());

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, converter(array));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromDateTime()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Local);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, converter(time));
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromDateTime()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Local);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, converter(time));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromIso8601String()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Local);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, converter(time.ToString(CustomConverterFactory.Iso8601DateTimePattern)));
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromIso8601String()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Local);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, converter(time.ToString(CustomConverterFactory.Iso8601DateTimePattern)));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromRoundtripString()
        {
            var time = DateTime.Now;

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, converter(time.ToString("O", CultureInfo.InvariantCulture)));
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromRoundtripString()
        {
            var time = DateTime.Now;

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, converter(time.ToString("O", CultureInfo.InvariantCulture)));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromUtcString()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Utc);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, converter("2009-10-22T14:59:32.125Z"));
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromUtcString()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Utc);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, converter("2009-10-22T14:59:32.125Z"));
        }

        [Test]
        public void DateTimeConverterChangesTypeFromJavaString()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Utc);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime));

            Assert.AreEqual(time, ((DateTime)(converter("2009-10-22T14:59:32.125+0000"))).ToUniversalTime());
        }

        [Test]
        public void NullableDateTimeConverterChangesTypeFromJavaString()
        {
            var time = new DateTime(2009, 10, 22, 14, 59, 32, 125, DateTimeKind.Utc);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTime?));

            Assert.AreEqual(time, ((DateTime)(converter("2009-10-22T14:59:32.125+0000"))).ToUniversalTime());
        }

        #endregion DateTime tests

        #region DateTimeOffset tests

        [Test]
        public void StringConverterChangesTypeFromDateTimeOffset()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("2009-10-22T14:59:32.000+0000", converter(time));
        }

        [Test]
        public void DateTimeOffsetConverterChangesTypeFromByteArray()
        {
            var time = DateTimeOffset.Now;

            var array = BitConverter.GetBytes(time.DateTime.ToBinary());

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset));

            Assert.AreEqual(time, converter(array));
        }

        [Test]
        public void NullableDateTimeOffsetConverterChangesTypeFromByteArray()
        {
            var time = DateTimeOffset.Now;

            var array = BitConverter.GetBytes(time.DateTime.ToBinary());

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset?));

            Assert.AreEqual(time, converter(array));
        }

        [Test]
        public void DateTimeOffsetConverterChangesTypeFromDateTimeOffset()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset));

            Assert.AreEqual(time, converter(time));
        }

        [Test]
        public void NullableDateTimeOffsetConverterChangesTypeFromDateTimeOffset()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset?));

            Assert.AreEqual(time, converter(time));
        }

        [Test]
        public void DateTimeOffsetConverterChangesTypeFromIso8601String()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, 125, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset));

            Assert.AreEqual(time, converter(time.ToString(CustomConverterFactory.Iso8601DateTimePattern)));
        }

        [Test]
        public void NullableDateTimeOffsetConverterChangesTypeFromIso8601String()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset?));

            Assert.AreEqual(time, converter(time.ToString(CustomConverterFactory.Iso8601DateTimePattern)));
        }

        [Test]
        public void DateTimeOffsetConverterChangesTypeFromRoundtripString()
        {
            var time = DateTimeOffset.Now;

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset));

            Assert.AreEqual(time, converter(time.ToString("O", CultureInfo.InvariantCulture)));
        }

        [Test]
        public void NullableDateTimeOffsetConverterChangesTypeFromRoundtripString()
        {
            var time = DateTimeOffset.Now;

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset?));

            Assert.AreEqual(time, converter(time.ToString("O", CultureInfo.InvariantCulture)));
        }

        [Test]
        public void DateTimeOffsetConverterChangesTypeFromJavaString()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, 125, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset));

            Assert.AreEqual(time, converter("2009-10-22T14:59:32.125+0000"));
        }

        [Test]
        public void NullableDateTimeOffsetConverterChangesTypeFromJavaString()
        {
            var time = new DateTimeOffset(2009, 10, 22, 14, 59, 32, 125, TimeSpan.Zero);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(DateTimeOffset?));

            Assert.AreEqual(time, converter("2009-10-22T14:59:32.125+0000"));
        }

        #endregion DateTimeOffset tests

        #region TimeSpan tests

        [Test]
        public void StringConverterChangesTypeFromTimeSpan()
        {
            var timeSpan = new TimeSpan(3, 12, 53, 45, 765);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("3.12:53:45.7650000", converter(timeSpan));
        }

        [Test]
        public void TimeSpanConverterChangesTypeFromByteArray()
        {
            var timeSpan = TimeSpan.FromSeconds(2000);

            var array = BitConverter.GetBytes(timeSpan.Ticks);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan));

            Assert.AreEqual(timeSpan, converter(array));
        }

        [Test]
        public void NullableTimeSpanConverterChangesTypeFromByteArray()
        {
            var timeSpan = TimeSpan.FromSeconds(2000);

            var array = BitConverter.GetBytes(timeSpan.Ticks);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan?));

            Assert.AreEqual(timeSpan, converter(array));
        }

        [Test]
        public void TimeSpanConverterChangesTypeFromTimeSpan()
        {
            var timeSpan = TimeSpan.FromSeconds(4000);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan));

            Assert.AreEqual(timeSpan, converter(timeSpan));
        }

        [Test]
        public void NullableTimeSpanConverterChangesTypeFromTimeSpan()
        {
            var timeSpan = TimeSpan.FromSeconds(4000);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan?));

            Assert.AreEqual(timeSpan, converter(timeSpan));
        }

        [Test]
        public void TimeSpanConverterChangesTypeFromString()
        {
            var timeSpan = new TimeSpan(3, 12, 53, 45, 765);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan));

            Assert.AreEqual(timeSpan, converter("3.12:53:45.765"));
        }

        [Test]
        public void NullableTimeSpanConverterChangesTypeFromString()
        {
            var timeSpan = new TimeSpan(3, 12, 53, 45, 765);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan?));

            Assert.AreEqual(timeSpan, converter("3.12:53:45.765"));
        }

        [Test]
        public void TimeSpanConverterChangesTypeFromStringInTicks()
        {
            var timeSpan = new TimeSpan(2893746236L);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan));

            Assert.AreEqual(timeSpan, converter("2893746236"));
        }

        [Test]
        public void NullableTimeSpanConverterChangesTypeFromStringInTicks()
        {
            var timeSpan = new TimeSpan(2893746236L);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan?));

            Assert.AreEqual(timeSpan, converter("2893746236"));
        }

        [Test]
        public void TimeSpanConverterChangesTypeFromTicks()
        {
            var timeSpan = new TimeSpan(2893746236L);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan));

            Assert.AreEqual(timeSpan, converter(2893746236L));
        }

        [Test]
        public void NullableTimeSpanConverterChangesTypeFromTicks()
        {
            var timeSpan = new TimeSpan(2893746236L);

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(TimeSpan?));

            Assert.AreEqual(timeSpan, converter(2893746236L));
        }

        #endregion DateTimeOffset tests

        #region Guid tests

        [Test]
        public void StringConverterChangesTypeFromGuid()
        {
            var guid = new Guid("2CB00AE6-AAF1-4E5B-A2C9-9C74009B1CCB");

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("2cb00ae6-aaf1-4e5b-a2c9-9c74009b1ccb", converter(guid));
        }

        [Test]
        public void GuidConverterChangesTypeFromByteArray()
        {
            var expected = Guid.NewGuid();

            var array = expected.ToByteArray();

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid));

            Assert.AreEqual(expected, converter(array));
        }

        [Test]
        public void NullableGuidConverterChangesTypeFromByteArray()
        {
            var expected = Guid.NewGuid();

            var array = expected.ToByteArray();

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid?));

            Assert.AreEqual(expected, converter(array));
        }

        [Test]
        public void GuidConverterChangesTypeFromString()
        {
            var guid = new Guid("2CB00AE6-AAF1-4E5B-A2C9-9C74009B1CCB");

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid));

            Assert.AreEqual(guid, converter(guid.ToString()));
        }

        [Test]
        public void NullableGuidConverterChangesTypeFromString()
        {
            var guid = new Guid("2CB00AE6-AAF1-4E5B-A2C9-9C74009B1CCB");

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid?));

            Assert.AreEqual(guid, converter(guid.ToString()));
        }

        [Test]
        public void GuidConverterChangesTypeFromGuid()
        {
            var guid = Guid.NewGuid();

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid));

            Assert.AreEqual(guid, converter(guid));
        }

        [Test]
        public void NullableGuidConverterChangesTypeFromGuid()
        {
            var guid = Guid.NewGuid();

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Guid?));

            Assert.AreEqual(guid, converter(guid));
        }

        #endregion Guid tests

        #region byte array

        [Test]
        public void ByteArrayConverterChangesTypeToExpectedValueForAllSupportedTypes()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(byte[]));

            Assert.AreEqual(34, BitConverter.ToInt16((byte[])converter(34), 0));
            Assert.AreEqual(42, BitConverter.ToInt32((byte[])converter(42), 0));
            Assert.AreEqual(54123564L, BitConverter.ToInt64((byte[])converter(54123564L), 0));
            Assert.AreEqual(1564.65, BitConverter.ToDouble((byte[])converter(1564.65), 0), double.Epsilon);
            Assert.AreEqual(17, BitConverter.ToChar((byte[])converter((char)17), 0));
            Assert.AreEqual(true, BitConverter.ToBoolean((byte[])converter(true), 0));

            var guid = Guid.NewGuid();
            Assert.AreEqual(guid, new Guid((byte[])converter(guid)));

            var time = DateTime.UtcNow;
            Assert.AreEqual(time, DateTime.FromBinary(BitConverter.ToInt64((byte[])converter(time), 0)));

            var timeOffset = new DateTimeOffset(DateTime.Now);
            Assert.AreEqual(timeOffset, new DateTimeOffset(DateTime.FromBinary(BitConverter.ToInt64((byte[])converter(timeOffset), 0))));

            var strB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World!"));
            Assert.AreEqual("Hello World!", Encoding.UTF8.GetString(((byte[])converter(strB64))));
            
            var timeSpan = TimeSpan.FromSeconds(786124);
            Assert.AreEqual(timeSpan, TimeSpan.FromTicks(BitConverter.ToInt64((byte[])converter(timeSpan), 0)));

            var array = new byte[] { 11, 34, 56, 73, 156, 227, 255, 0, 3, 100, 99 };
            CollectionAssert.AreEqual(array, (byte[])converter(array));
        }

        [Test]
        public void ByteArrayConverterThrowsInvalidCastExceptionForNonSupportedTypes()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(byte[]));

            Assert.Catch<InvalidCastException>(() => converter(Stream.Null));
        }

        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        public void ConvertersThrowInvalidCastExceptionForByteArrayWithWrongLengthToBuiltInTypes(Type convertTo)
        {
            // 11 bytes shouldn't fit any type, it's normally 1, 4, 8, 16
            var array = new byte[] { 11, 34, 56, 73, 156, 227, 255, 0, 3, 100, 99 };

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(convertTo);

            Assert.Catch<InvalidCastException>(() => converter(array));
        }


        [Test]
        [TestCase(typeof(bool))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(double))]
        [TestCase(typeof(Guid))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        public void ConvertersThrowInvalidCastExceptionForByteArrayWithWrongLengthToNullableBuiltInTypes(Type convertTo)
        {
            // 11 bytes shouldn't fit any type, it's normally 1, 4, 8, 16
            var array = new byte[] { 11, 34, 56, 73, 156, 227, 255, 0, 3, 100, 99 };

            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(convertTo));

            Assert.Catch<InvalidCastException>(() => converter(array));
        }

        public void ByteArrayConverterReturnsNullWhenInputValueIsWhitespaceString()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(byte[]));

            Assert.Catch<FormatException>(() => converter("  "));
        }

        public void ByteArrayConverterReturnsNullWhenInputValueIsEmptyString()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(byte[]));

            Assert.Catch<FormatException>(() => converter(string.Empty));
        }

        #endregion byte array

        #region Enums

        public enum HelloWorld
        {
            Hello = 2, World = 5
        }

        public enum EnumWithNoValues
        {
        }

        [Test]
        public void StringConverterChangesTypeFromEnum()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(string));

            Assert.AreEqual("Hello", converter(HelloWorld.Hello));
        }

        [Test]
        [TestCase("Hello", HelloWorld.Hello)]
        [TestCase(2, HelloWorld.Hello)]
        [TestCase(2.0, HelloWorld.Hello)]
        [TestCase(2L, HelloWorld.Hello)]
        [TestCase("2", HelloWorld.Hello)]
        [TestCase("World", HelloWorld.World)]
        [TestCase(5, HelloWorld.World)]
        [TestCase(5.0, HelloWorld.World)]
        [TestCase(5L, HelloWorld.World)]
        [TestCase("5", HelloWorld.World)]
        [TestCase(HelloWorld.World, HelloWorld.World)]
        public void EnumConverterChangesTypesFromSupportedValues(object testValue, HelloWorld expected)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(HelloWorld));

            Assert.AreEqual(expected, converter(testValue));
        }

        [Test]
        [TestCase("Hello", HelloWorld.Hello)]
        [TestCase(2, HelloWorld.Hello)]
        [TestCase(2.0, HelloWorld.Hello)]
        [TestCase(2L, HelloWorld.Hello)]
        [TestCase("2", HelloWorld.Hello)]
        [TestCase("World", HelloWorld.World)]
        [TestCase(5, HelloWorld.World)]
        [TestCase(5.0, HelloWorld.World)]
        [TestCase(5L, HelloWorld.World)]
        [TestCase("5", HelloWorld.World)]
        [TestCase(HelloWorld.Hello, HelloWorld.Hello)]
        public void NullableEnumConverterChangesTypesFromSupportedValues(object testValue, HelloWorld expected)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(typeof(HelloWorld)));

            Assert.AreEqual(expected, converter(testValue));
        }

        [Test]
        [TestCase("Hello", HelloWorld.Hello)]
        [TestCase(2, HelloWorld.Hello)]
        [TestCase(2.0, HelloWorld.Hello)]
        [TestCase(2L, HelloWorld.Hello)]
        [TestCase("World", HelloWorld.World)]
        [TestCase(5, HelloWorld.World)]
        [TestCase(5.0, HelloWorld.World)]
        [TestCase(5L, HelloWorld.World)]
        public void TypesAreChangedFromEnum(object expected, HelloWorld testValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(expected.GetType());

            Assert.AreEqual(expected, converter(testValue));
        }

        [Test]
        [TestCase(2, HelloWorld.Hello)]
        [TestCase(2.0, HelloWorld.Hello)]
        [TestCase(2L, HelloWorld.Hello)]
        [TestCase(5, HelloWorld.World)]
        [TestCase(5.0, HelloWorld.World)]
        [TestCase(5L, HelloWorld.World)]
        public void TypesAreChangedFromEnumUsingNullableConverters(object expected, HelloWorld testValue)
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(Nullable<>).MakeGenericType(expected.GetType()));

            Assert.AreEqual(expected, converter(testValue));
        }

        [Test]
        public void EnumConverterReturnsFirstEnumValueForNullInput()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(HelloWorld));

            Assert.AreEqual(HelloWorld.Hello, converter(null));
        }

        [Test]
        public void EnumConverterThrowsInvalidCastExceptionForNullInputWhenTheEnumDoesNotHaveValues()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(EnumWithNoValues));

            Assert.Catch<InvalidCastException>(() => converter(null));
        }

        [Test]
        public void EnumConverterThrowsInvalidCastExceptionWhenInputValueIsNotDefinedInTheEnum()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(HelloWorld));

            Assert.Catch<InvalidCastException>(() => converter(100));
        }

        [Test]
        public void NullableEnumConverterReturnsDefaultValueForNullInput()
        {
            var target = new CustomConverterFactory();

            var converter = target.GetConverter(typeof(HelloWorld?));

            Assert.IsNull(converter(null));
        }

        [Test]
        public void EnumConverterThrowsInvalidCastExceptionWhenCannotFindConverterForUnderlyingType()
        {
            var target = new CustomConverterFactory();

            target.RemoveConverter(typeof(int)); // it's the underlying type for the HelloWorld enum

            var converter = target.GetConverter(typeof(HelloWorld));

            Assert.Catch<InvalidCastException>(() => converter(2));
        }

        #endregion Enums
    }
}

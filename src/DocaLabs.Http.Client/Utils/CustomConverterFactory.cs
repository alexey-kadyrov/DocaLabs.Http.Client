using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Xml;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// The converter factory. The behaviour can be customised by updating the factory.
    /// </summary>
    public class CustomConverterFactory : ICustomConverterFactory
    {
        /// <summary>
        /// Format which is used for DateTime &amp; DateTimeOffest conversions
        /// </summary>
        public const string Iso8601DateTimePattern = @"yyyy-MM-ddTHH:mm:ss.fffK";

        ConcurrentDictionary<Type, Func<object, object>> Converters { get; set; }

        /// <summary>
        /// Initialize an instance of the CustomConverterFactory with default set of converters
        /// </summary>
        public CustomConverterFactory()
        {
            Converters = new ConcurrentDictionary<Type, Func<object, object>>();

            RegisterConverter(typeof(bool), ToBooleanConverter);
            RegisterConverter(typeof(bool?), ToNullableBooleanConverter);

            RegisterConverter(typeof(DateTime), ToDateTimeConverter);
            RegisterConverter(typeof(DateTime?), ToNullableDateTimeConverter);

            RegisterConverter(typeof(DateTimeOffset), ToDateTimeOffsetConverter);
            RegisterConverter(typeof(DateTimeOffset?), ToNullableDateTimeOffsetConverter);

            RegisterConverter(typeof(TimeSpan), ToTimeSpanConverter);
            RegisterConverter(typeof(TimeSpan?), ToNullableTimeSpanConverter);

            RegisterConverter(typeof(decimal), ToDecimalConverter);
            RegisterConverter(typeof(decimal?), ToNullableDecimalConverter);

            RegisterConverter(typeof(double), ToDoubleConverter);
            RegisterConverter(typeof(double?), ToNullableDoubleConverter);

            RegisterConverter(typeof(Guid), ToGuidConverter);
            RegisterConverter(typeof(Guid?), ToNullableGuidConverter);

            RegisterConverter(typeof(int), ToInt32Converter);
            RegisterConverter(typeof(int?), ToNullableInt32Converter);

            RegisterConverter(typeof(long), ToInt64Converter);
            RegisterConverter(typeof(long?), ToNullableInt64Converter);

            RegisterConverter(typeof(string), ToStringConverter);
            RegisterConverter(typeof(byte[]), ToByteArray);
        }

        /// <summary>
        /// Registers a converter.
        /// </summary>
        /// <param name="type">A type which converter can change type to.</param>
        /// <param name="converter">The converter.</param>
        public void RegisterConverter(Type type, Func<object, object> converter)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(converter == null)
                throw new ArgumentNullException("converter");

            Converters.AddOrUpdate(type, converter, (k, v) => converter);
        }

        /// <summary>
        /// Attempts to remove converter with the specified key. 
        /// </summary>
        /// <param name="type">A type which converter can change type to.</param>
        /// <returns>Removed converter or null if it wasn't found.</returns>
        public Func<object, object> RemoveConverter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Func<object, object> converter;

            return Converters.TryRemove(type, out converter) ? converter : null;
        }

        /// <summary>
        /// Returns a registered converter.
        /// </summary>
        /// <param name="type">The lookup type.</param>
        /// <returns>If there is no a converter for the type it will be null; otherwise the converter.</returns>
        public Func<object, object> GetConverter(Type type)
        {
            Func<object, object> converter;

            Converters.TryGetValue(type, out converter);

            if (converter == null)
            {
                var nullable = false;
                if(type.IsEnum == false)
                {
                    type = Nullable.GetUnderlyingType(type);
                    if (type == null || type.IsEnum == false)
                        return null;

                    nullable = true;
                }

                var enumConverter = new EnumConverter(this, type, nullable);

                RegisterConverter(type, enumConverter.Convert);

                return enumConverter.Convert;
            }

            return converter;
        }

        /// <summary>
        /// Checks whenever the object is null or DBNull
        /// </summary>
        /// <param name="value">Value to be checked.</param>
        /// <returns>True if the value is null or DBNull otherwise false.</returns>
        public static bool IsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }

        static object ToBooleanConverter(object value)
        {
            if (IsNull(value))
                return default(bool);

            var valueAsString = value as string;

            if(valueAsString != null)
                return XmlConvert.ToBoolean(valueAsString.ToLower(CultureInfo.InvariantCulture));

            var valueAsByteArray = value as byte[];

            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 1)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(bool)));

                return BitConverter.ToBoolean(valueAsByteArray, 0);
            }

            return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        static object ToNullableBooleanConverter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 1)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(bool?)));

                    return BitConverter.ToBoolean(valueAsByteArray, 0);
                }

                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }

            valueAsString = valueAsString.Trim();

            return valueAsString.Length == 0
                       ? (object) null
                       : XmlConvert.ToBoolean(valueAsString.ToLower(CultureInfo.InvariantCulture));
        }

        static object ToDateTimeConverter(object value)
        {
            if (IsNull(value))
                return default(DateTime);

            var timeAsString = value as string;

            if (timeAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(DateTime)));

                    return DateTime.FromBinary(BitConverter.ToInt64(valueAsByteArray, 0));
                }

                return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }

            return timeAsString.EndsWith("Z", StringComparison.OrdinalIgnoreCase) 
                        ? DateTime.Parse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal) 
                        : DateTime.Parse(timeAsString, CultureInfo.InvariantCulture);
        }

        static object ToNullableDateTimeConverter(object value)
        {
            if (IsNull(value))
                return null;

            var timeAsString = value as string;

            if (timeAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(DateTime?)));

                    return DateTime.FromBinary(BitConverter.ToInt64(valueAsByteArray, 0));
                }

                return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }

            timeAsString = timeAsString.Trim();

            if (timeAsString.Length == 0)
                return null;

            return timeAsString.EndsWith("Z", StringComparison.OrdinalIgnoreCase)
                       ? DateTime.Parse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
                        : DateTime.Parse(timeAsString, CultureInfo.InvariantCulture);
        }

        static object ToDateTimeOffsetConverter(object value)
        {
            if (IsNull(value))
                return default(DateTimeOffset);

            var timeAsString = value as string;

            if (timeAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(DateTimeOffset)));

                    return new DateTimeOffset(DateTime.FromBinary(BitConverter.ToInt64(valueAsByteArray, 0)));
                }

                if (value is DateTimeOffset)
                    return (DateTimeOffset) value;

                return new DateTimeOffset(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
            }

            return timeAsString.EndsWith("Z", StringComparison.OrdinalIgnoreCase)
                       ? DateTimeOffset.Parse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
                       : DateTimeOffset.Parse(timeAsString, CultureInfo.InvariantCulture);
        }

        static object ToNullableDateTimeOffsetConverter(object value)
        {
            if (IsNull(value))
                return null;

            var timeAsString = value as string;

            if (timeAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(DateTimeOffset?)));

                    return new DateTimeOffset(DateTime.FromBinary(BitConverter.ToInt64(valueAsByteArray, 0)));
                }

                if (value is DateTimeOffset)
                    return (DateTimeOffset)value;

                return new DateTimeOffset(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
            }

            timeAsString = timeAsString.Trim();

            if (timeAsString.Length == 0)
                return null;

            return timeAsString.EndsWith("Z", StringComparison.OrdinalIgnoreCase)
                       ? DateTimeOffset.Parse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
                       : DateTimeOffset.Parse(timeAsString, CultureInfo.InvariantCulture);
        }

        static object ToTimeSpanConverter(object value)
        {
            if (IsNull(value))
                return default(TimeSpan);

            var timeSpanAsString = value as string;

            if (timeSpanAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(TimeSpan)));

                    return new TimeSpan(BitConverter.ToInt64(valueAsByteArray, 0));
                }

                if(value is long)
                    return TimeSpan.FromTicks((long)value);

                if (value is TimeSpan)
                    return (TimeSpan) value;

                throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(TimeSpan)));
            }

            long ticks;
            return long.TryParse(timeSpanAsString, NumberStyles.Number, CultureInfo.InvariantCulture, out ticks) 
                ? TimeSpan.FromTicks(ticks) 
                : TimeSpan.Parse(timeSpanAsString, CultureInfo.InvariantCulture);
        }

        static object ToNullableTimeSpanConverter(object value)
        {
            if (IsNull(value))
                return null;

            var timeSpanAsString = value as string;

            if (timeSpanAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(TimeSpan?)));

                    return new TimeSpan(BitConverter.ToInt64(valueAsByteArray, 0));
                }

                if (value is long)
                    return TimeSpan.FromTicks((long)value);

                if (value is TimeSpan)
                    return (TimeSpan)value;

                throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(TimeSpan?)));
            }

            timeSpanAsString = timeSpanAsString.Trim();

            if (timeSpanAsString.Length == 0)
                return null;

            long ticks;
            return long.TryParse(timeSpanAsString, NumberStyles.Number, CultureInfo.InvariantCulture, out ticks)
                ? TimeSpan.FromTicks(ticks)
                : TimeSpan.Parse(timeSpanAsString, CultureInfo.InvariantCulture);
        }

        static object ToDecimalConverter(object value)
        {
            return IsNull(value) 
                ? default(decimal) 
                : Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        static object ToNullableDecimalConverter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString == null)
                return Convert.ToDecimal(value, CultureInfo.InvariantCulture);

            valueAsString = valueAsString.Trim();

            return valueAsString.Length == 0
                       ? (object) null
                       : Convert.ToDecimal(valueAsString, CultureInfo.InvariantCulture);
        }

        static object ToDoubleConverter(object value)
        {
            if (IsNull(value))
                return default(double);

            var valueAsByteArray = value as byte[];
            
            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 8)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(double)));

                return BitConverter.ToDouble(valueAsByteArray, 0);
            }

            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        static object ToNullableDoubleConverter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(double?)));

                    return BitConverter.ToDouble(valueAsByteArray, 0);
                }

                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }

            valueAsString = valueAsString.Trim();

            return valueAsString.Length == 0
                       ? (object) null
                       : Convert.ToDouble(valueAsString, CultureInfo.InvariantCulture);
        }

        static object ToGuidConverter(object value)
        {
            if (IsNull(value))
                return default(Guid);

            var valueAsString = value as string;

            if (valueAsString != null)
                return new Guid(valueAsString);
            
            if (value is Guid)
                return value;

            var valueAsByteArray = value as byte[];

            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 16)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(Guid)));

                return new Guid(valueAsByteArray);
            }

            throw new InvalidCastException(Resources.Text.input_argument_can_be_only_string_or_guid);
        }

        static object ToNullableGuidConverter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString != null)
            {
                valueAsString = valueAsString.Trim();

                if (valueAsString.Length == 0)
                    return null;

                return new Guid(valueAsString);
            }
            
            if (value is Guid)
                return value;

            var valueAsByteArray = value as byte[];

            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 16)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(Guid?)));

                return new Guid(valueAsByteArray);
            }

            throw new InvalidCastException(Resources.Text.input_argument_can_be_only_string_or_guid);
        }

        static object ToInt32Converter(object value)
        {
            if (IsNull(value))
                return default(int);

            var valueAsByteArray = value as byte[];

            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 4)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(int)));

                return BitConverter.ToInt32(valueAsByteArray, 0);
            }

            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        static object ToNullableInt32Converter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if (valueAsByteArray.Length != 4)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(int?)));

                    return BitConverter.ToInt32(valueAsByteArray, 0);
                }

                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }

            valueAsString = valueAsString.Trim();

            if (valueAsString.Length == 0)
                return null;

            return Convert.ToInt32(valueAsString, CultureInfo.InvariantCulture);
        }

        static object ToInt64Converter(object value)
        {
            if (IsNull(value))
                return default(long);

            var valueAsByteArray = value as byte[];

            if (valueAsByteArray != null)
            {
                if (valueAsByteArray.Length != 8)
                    throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(long)));

                return BitConverter.ToInt64(valueAsByteArray, 0);
            }

            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        static object ToNullableInt64Converter(object value)
        {
            if (IsNull(value))
                return null;

            var valueAsString = value as string;

            if (valueAsString == null)
            {
                var valueAsByteArray = value as byte[];

                if (valueAsByteArray != null)
                {
                    if(valueAsByteArray.Length != 8)
                        throw new InvalidCastException(string.Format(Resources.Text.cannot_convert_from_0_to_1, value.GetType(), typeof(long?)));

                    return BitConverter.ToInt64(valueAsByteArray, 0);
                }

                return Convert.ToInt64(value, CultureInfo.InvariantCulture);
            }

            valueAsString = valueAsString.Trim();

            if (valueAsString.Length == 0)
                return null;

            return Convert.ToInt64(valueAsString, CultureInfo.InvariantCulture);
        }

        static object ToStringConverter(object value)
        {
            if (IsNull(value))
                return null;

            if (value is DateTime)
                return ConvertDateTimeToString((DateTime)value);
            
            if (value is DateTimeOffset)
                return ConvertDateTimeOffsetToString((DateTimeOffset)value);
            
            if (value is bool)
                return XmlConvert.ToString((bool)value);

            var inArray = value as byte[];
            return inArray != null 
                ? Convert.ToBase64String(inArray, Base64FormattingOptions.None) 
                : Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        static object ToByteArray(object value)
        {
            if (IsNull(value))
                return null;

            if (value is bool)
                return BitConverter.GetBytes((bool) value);

            if (value is int)
                return BitConverter.GetBytes((int)value);

            if (value is long)
                return BitConverter.GetBytes((long)value);

            if (value is double)
                return BitConverter.GetBytes((double)value);

            if (value is char)
                return BitConverter.GetBytes((char)value);

            var s = value as string;
            if (s != null)
                return Convert.FromBase64String(s);

            if (value is Guid)
                return ((Guid) value).ToByteArray();

            if(value is DateTime)
                return BitConverter.GetBytes(((DateTime)value).ToBinary());

            if (value is DateTimeOffset)
                return BitConverter.GetBytes(((DateTimeOffset)value).DateTime.ToBinary());

            if (value is TimeSpan)
                return BitConverter.GetBytes(((TimeSpan)value).Ticks);

            var bytes = value as byte[];
            if (bytes != null)
                return bytes.Clone();

            throw new InvalidCastException(string.Format(Resources.Text.type_0_cannot_be_converted_to_byte_array, value.GetType()));
        }

        static string ConvertDateTimeToString(DateTime time)
        {
            var convertedTime = time.ToString(Iso8601DateTimePattern, CultureInfo.InvariantCulture);

            if (convertedTime.Length == 29 && convertedTime[26] == ':')
            {
                // substitute +00:00 by +0000 in order to be compatible with Java, .Net understands this format anyway
                return convertedTime.Substring(0, 26) + convertedTime.Substring(27, 2);
            }

            return convertedTime;
        }

        static string ConvertDateTimeOffsetToString(DateTimeOffset time)
        {
            var convertedTime = time.ToString(Iso8601DateTimePattern, CultureInfo.InvariantCulture);

            if (convertedTime.Length == 29 && convertedTime[26] == ':')
            {
                // substitute +00:00 by +0000 in order to be compatible with Java, .Net understands this format anyway
                return convertedTime.Substring(0, 26) + convertedTime.Substring(27, 2);
            }

            return convertedTime;
        }

        class EnumConverter
        {
            CustomConverterFactory CustomConverterFactory { get; set; }
            Type EnumType { get; set; }
            Type UnderlyingType { get; set; }
            bool IsNullable { get; set; }

            public EnumConverter(CustomConverterFactory customConverterFactory, Type enumType, bool isNullable)
            {
                CustomConverterFactory = customConverterFactory;
                EnumType = enumType;
                UnderlyingType = EnumType.GetEnumUnderlyingType();
                IsNullable = isNullable;
            }

            public object Convert(object value)
            {
                if (value == null)
                {
                    if(IsNullable)
                        return null;

                    var values = Enum.GetValues(EnumType);
                    if (values.Length == 0)
                        throw new InvalidCastException(string.Format(Resources.Text.value_0_is_not_defined_for_enum_1, "<null>", EnumType));

                    return values.GetValue(0);
                }

                var valueAsString = value as string;
                if (valueAsString != null)
                    return Enum.Parse(EnumType, valueAsString, true);

                if (value.GetType() == EnumType)
                    return value;

                var converter = CustomConverterFactory.GetConverter(UnderlyingType);
                if (converter != null)
                {
                    var enumValue = converter(value);
                    if(!Enum.IsDefined(EnumType, enumValue))
                        throw new InvalidCastException(string.Format(Resources.Text.value_0_is_not_defined_for_enum_1, enumValue ?? "<null>", EnumType));

                    return Enum.ToObject(EnumType, enumValue);
                }

                throw new InvalidCastException(string.Format(Resources.Text.unknown_underlying_enum_type_0, UnderlyingType));
            }
        }
    }
}

using System;
using System.Text;

namespace DocaLabs.Http.Client.Binding.Utils
{
    /// <summary>
    /// String type extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// Thanks for the idea: http://stackoverflow.com/a/244933
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue.</returns>
        static public string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder();
            var previousIndex = 0;
            var index = str.IndexOf(oldValue, comparison);

            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;
                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Returns a value indicating wherever the specified value occurs in the string.
        /// </summary>
        static public bool Contains(this string str, string value, StringComparison comparison)
        {
            if(str == null)
                throw new ArgumentNullException("str");

            if(value == null)
                throw new ArgumentNullException("value");

            return str.IndexOf(value, comparison) >= 0;
        }
    }
}

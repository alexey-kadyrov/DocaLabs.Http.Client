using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Represents a collection of associated string keys and collection of string values for each key.
    /// </summary>
    [Serializable]
    public class CustomNameValueCollection : DictionaryList<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public CustomNameValueCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the CustomNameValueCollection can contain.</param>
        public CustomNameValueCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that is empty, has the default initial capacity, and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public CustomNameValueCollection(IEqualityComparer<string> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the CustomNameValueCollection can contain.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public CustomNameValueCollection(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that contains elements copied from the specified IDictionary and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new CustomNameValueCollection.</param>
        public CustomNameValueCollection(IDictionary<string, IList<string>> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that contains elements copied from the specified IDictionary and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionaryList whose elements are copied to the new CustomNameValueCollection.</param>
        public CustomNameValueCollection(IDictionaryList<string, string> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that contains elements copied from the specified IDictionary&lt;string, string> and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new CustomNameValueCollection.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public CustomNameValueCollection(IDictionary<string, IList<string>> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomNameValueCollection class that contains elements copied from the specified IDictionary&lt;string, string> and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="dictionary">The IDictionaryList whose elements are copied to the new CustomNameValueCollection.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public CustomNameValueCollection(IDictionaryList<string, string> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
        }
    }
}

﻿using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines methods to convert a property value to name-value pairs.
    /// </summary>
    public interface IPropertyConverter
    {
        /// <summary>
        /// Converts a property value.
        /// </summary>
        /// <param name="obj">Instance of the object on which the property is defined.</param>
        /// <returns>One key-value pairs where the key would normally be the property name.</returns>
        NameValueCollection Convert(object obj);
    }
}

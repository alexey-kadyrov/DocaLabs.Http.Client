﻿using System;

namespace DocaLabs.Http.Client.Binding.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class QueryHeaderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a name that should be used for the request's header.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets the custom format string that is set by QueryParameterAttribute.
        /// </summary>
        protected string Format { get; private set; }
    }
}

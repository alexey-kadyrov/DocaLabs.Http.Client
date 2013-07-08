﻿using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Specifies additional information about a property for serializing into a web request.
    /// If the mapping is set to the URL's path the name is used as a substitution mask. The mask is case insensitive. 
    /// The Url template can be specified like: http://contoso.com/{propertyName1}/{propertyName2}.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class RequestUseAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets where the property should be mapped, e.g. URL's query, path or request headers
        /// </summary>
        public RequestUseTargets Targets { get; set; }

        /// <summary>
        /// Gets or sets a name that overrides the property name which is used by default.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the custom format string that is used by a property converter.
        /// If the format is non white space string then the converter will use string.Format
        /// to convert the property value. If set the format must include curly brackets.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Initializes a new instance of the RequestUseAttribute to be mapped to the URL's query by default.
        /// </summary>
        public RequestUseAttribute()
        {
            Targets = RequestUseTargets.UrlQuery;
        }

        /// <summary>
        /// Initializes a new instance of the RequestUseAttribute with specified mapping.
        /// </summary>
        public RequestUseAttribute(RequestUseTargets targets)
        {
            Targets = targets;
        }

        /// <summary>
        /// Initializes a new instance of the RequestUseAttribute with specified mapping and name.
        /// </summary>
        public RequestUseAttribute(RequestUseTargets targets, string name)
        {
            Targets = targets;
            Name = name;
        }
    }
}
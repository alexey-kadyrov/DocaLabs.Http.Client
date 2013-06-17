﻿using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Extension.NewtonSoft
{
    /// <summary>
    /// Implements IJsonSerializer using Newtonsoft JsonConvert.
    /// </summary>
    [Export(typeof(IJsonSerializer))]
    public class NewtonSoftDefaultJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
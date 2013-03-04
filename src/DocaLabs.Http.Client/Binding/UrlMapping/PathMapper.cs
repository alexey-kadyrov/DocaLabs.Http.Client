using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class PathMapper
    {
        readonly object _model;
        readonly object _client;
        readonly string _existingPath;

        public PathMapper(object model, object client, Uri baseUrl)
        {
            _existingPath = baseUrl == null ? "" : baseUrl.AbsolutePath;
            _client = client;
            _model = model;
        }

        public string TryMakePath()
        {
            if (_model == null)
                return _existingPath;

            var modelPath = ConvertModelToPath();

            if (string.IsNullOrWhiteSpace(_existingPath))
                return modelPath;

            return string.IsNullOrWhiteSpace(modelPath) 
                ? _existingPath 
                : ConcatenatePathParts(_existingPath, modelPath);
        }

        string ConvertModelToPath()
        {
            var mapper = ClientModelBinders.GetUrlPathMapper(_model.GetType());
                
            var values = mapper.Map(_model, _client);

            ValidatePathValues(values);

            return HttpUtility.UrlPathEncode(string.Join("/", GetNonEmptyValues(values)));
        }

        static void ValidatePathValues(string[] values)
        {
            var isPreviousValueEmpty = false;

            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    isPreviousValueEmpty = true;
                }
                else
                {
                    if(isPreviousValueEmpty)
                        throw new UnrecoverableHttpClientException(string.Format(Resources.Text.path_mapping_is_strictly_positioonal, string.Join(",", values)));
                }
            }
        }

        static IEnumerable<string> GetNonEmptyValues(IEnumerable<string> values)
        {
            return values.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        static string ConcatenatePathParts(string leftPart, string rightPart)
        {
            return leftPart.EndsWith("/")
                       ? leftPart + rightPart
                       : leftPart + "/" + rightPart;
        }
    }
}
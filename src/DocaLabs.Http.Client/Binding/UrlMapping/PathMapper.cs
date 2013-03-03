using System;

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
            var modelPath = ConvertModelToPath();

            if (string.IsNullOrWhiteSpace(_existingPath))
                return modelPath;

            if(string.IsNullOrWhiteSpace(modelPath))
                return _existingPath;

            return _existingPath.EndsWith("/")
                ? _existingPath + modelPath
                : _existingPath + "/" + modelPath;
        }

        string ConvertModelToPath()
        {
            var mapper = ClientModelBinders.GetUrlPathMapper(_model.GetType());
                
            var values = mapper.Map(_model, _client);

            return string.Join("/", values);
        }
    }
}
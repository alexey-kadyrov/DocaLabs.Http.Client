using System;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class QueryMapper
    {
        readonly object _model;
        readonly object _client;
        readonly string _existingQuery;

        public QueryMapper(object model)
            : this(model, null, null)
        {
        }

        public QueryMapper(object model, object client, Uri baseUrl)
        {
            _model = model;
            _client = client;

            _existingQuery = baseUrl == null ? "" : baseUrl.Query;

            if (_existingQuery.StartsWith("?"))
                _existingQuery = _existingQuery.Substring(1);
        }

        public string TryMakeQuery()
        {
            var modelQuery = ConvertModelToQuery();

            if (string.IsNullOrWhiteSpace(_existingQuery))
                return modelQuery;

            if(string.IsNullOrWhiteSpace(modelQuery))
                return _existingQuery;

            return _existingQuery.EndsWith("&")
                ? _existingQuery + modelQuery
                : _existingQuery + "&" + modelQuery;
        }

        string ConvertModelToQuery()
        {
            var mapper = ClientModelBinders.GetUrlQueryMapper(_model.GetType());
                
            var values = mapper.Map(_model, _client);

            return new QueryStringBuilder().Add(values).ToString();
        }
    }
}
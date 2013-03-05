using System;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class QueryMapper
    {
        readonly object _model;
        readonly string _existingQuery;

        public QueryMapper(object model, Uri baseUrl)
        {
            _model = model;
            _existingQuery = GetExistingQuery(baseUrl);
        }

        public string TryMakeQuery()
        {
            if (_model == null)
                return _existingQuery;

            var modelQuery = ConvertModelToQuery();

            if (string.IsNullOrWhiteSpace(_existingQuery))
                return modelQuery;

            return string.IsNullOrWhiteSpace(modelQuery) 
                ? _existingQuery 
                : ConcatenateQueryParts(_existingQuery, modelQuery);
        }

        static string ConcatenateQueryParts(string leftPart, string rightPart)
        {
            return leftPart.EndsWith("&")
                    ? leftPart + rightPart
                    : leftPart + "&" + rightPart;
        }

        static string GetExistingQuery(Uri baseUrl)
        {
            var query = baseUrl == null ? "" : baseUrl.Query;

            return GetQueryWithoutQuestionMark(query);
        }

        string ConvertModelToQuery()
        {
            var mapper = ClientModelBinders.GetUrlQueryMapper(_model.GetType());
                
            var values = mapper.Map(_model);

            return new QueryStringBuilder().Add(values).ToString();
        }

        static string GetQueryWithoutQuestionMark(string query)
        {
            return query.StartsWith("?")
                       ? query.Substring(1)
                       : query;
        }
    }
}
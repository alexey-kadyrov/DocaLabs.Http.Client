using System;
using System.Linq;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class GetUser : IReturn<User>
    {
        public Guid Id  { get; set; }
    }

    public class GetUserService : Service
    {
        public object Get(GetUser request)
        {
            if (Request.RawUrl.Contains("/v1/"))
            {
                return new HttpResult
                {
                    StatusCode = HttpStatusCode.Redirect,
                    Location = Request.RawUrl.Replace("/v1/", "/v2/")
                };
            }

            var user = Users.Data.FirstOrDefault(x => x.Id == request.Id);
            if(user == null)
                throw HttpError.NotFound("User {0} does not exist.".Fmt(request.Id));

            var ifNoneMatch = Request.Headers["If-None-Match"];
            if (!string.IsNullOrWhiteSpace(ifNoneMatch) && ifNoneMatch == Users.ETags[user.Id])
            {
                return new HttpResult
                {
                    StatusCode = HttpStatusCode.NotModified,
                    StatusDescription = "{0} Not Modified".Fmt(request.Id),
                    Headers =
                    {
                        { "Hello", "World!" }
                    }
                };
            }

            return new HttpResult(user)
            {
                Headers =
                {
                    { "ETag", Users.ETags[user.Id] },
                    { "Hello", "World!" }
                }
            };
        }
    }
}

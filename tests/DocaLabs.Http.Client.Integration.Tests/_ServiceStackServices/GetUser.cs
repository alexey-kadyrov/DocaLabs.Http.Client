using System;
using System.Linq;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.Text;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    public class GetUser : IReturn<User>
    {
        public Guid Id  { get; set; }
    }

    [EnableCors(allowedMethods: "GET")]
    public class GetUserService : Service
    {
        public object Get(GetUser request)
        {
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

using System;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    public class GetUser : IReturn<User>
    {
        public Guid Id  { get; set; }
    }

    public class GetUserService : Service
    {
        public object Get(GetUser request)
        {
            if(request.Id == Guid.Empty)
                throw HttpError.NotFound("User {0} does not exist.".Fmt(request.Id));

            return new User
            {
                Id = request.Id,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@foo.bar"
            };
        }
    }
}

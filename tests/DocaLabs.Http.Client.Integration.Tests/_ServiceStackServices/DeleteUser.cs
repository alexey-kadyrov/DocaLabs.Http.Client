using System;
using System.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    public class DeleteUserRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserService : Service
    {
        public object Delete(DeleteUserRequest request)
        {
            var user = Users.Data.FirstOrDefault(x => x.Id == request.Id);
            if (user == null)
                throw HttpError.NotFound("User {0} does not exist.".Fmt(request.Id));

            Users.Data.Remove(user);
            Users.ETags.Remove(request.Id);

            return null;
        }
    }
}

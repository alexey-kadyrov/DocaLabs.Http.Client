using System.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class AddUserRequest : User
    {
    }

    public class AddUserService : Service
    {
        public object Post(AddUserRequest request)
        {
            if (Users.Data.FirstOrDefault(x => x.Id == request.Id) != null)
                throw HttpError.Conflict("User {0} already exist.".Fmt(request.Id));

            Users.Data.Add(request);
            Users.ETags[request.Id] = Users.FakeETag();

            return new HttpResult
            {
                Headers = { { "ETag", Users.ETags[request.Id] } }
            };
        }
    }
}

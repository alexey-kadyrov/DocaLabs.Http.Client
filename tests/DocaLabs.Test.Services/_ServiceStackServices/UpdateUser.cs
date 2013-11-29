using System.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class UpdateUserService : Service
    {
        public object Put(UpdateUserRequest request)
        {
            var user = Users.Data.FirstOrDefault(x => x.Id == request.Id);
            if (user == null)
                throw HttpError.NotFound("User {0} does not exist.".Fmt(request.Id));

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            Users.ETags[user.Id] = "u" + user.Id;

            return new HttpResult
            {
                Headers = { { "ETag", "u" + user.Id } }
            };
        }
    }
}

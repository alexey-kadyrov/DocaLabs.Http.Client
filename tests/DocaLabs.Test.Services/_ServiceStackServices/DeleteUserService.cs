using System.Linq;
using ServiceStack;

namespace DocaLabs.Test.Services._ServiceStackServices
{
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

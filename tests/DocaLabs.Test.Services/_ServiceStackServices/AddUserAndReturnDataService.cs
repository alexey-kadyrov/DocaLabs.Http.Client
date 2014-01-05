using System.Linq;
using ServiceStack;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class AddUserAndReturnDataService : Service
    {
        public object Post(AddUserAndReturnDataRequest request)
        {
            if (Users.Data.FirstOrDefault(x => x.Id == request.Id) != null)
                throw HttpError.Conflict("User {0} already exist.".Fmt(request.Id));

            Users.Data.Add(request);
            Users.ETags[request.Id] = "a" + request.Id;

            return new HttpResult(request)
            {
                Headers = { { "ETag", "a" + request.Id } }
            };
        }
    }
}

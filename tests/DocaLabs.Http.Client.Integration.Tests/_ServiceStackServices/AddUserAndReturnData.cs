﻿using System.Linq;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    public class AddUserAndReturnDataRequest : User
    {
    }

    public class AddUserAndReturnDataService : Service
    {
        public object Post(AddUserAndReturnDataRequest request)
        {
            if (Users.Data.FirstOrDefault(x => x.Id == request.Id) != null)
                throw HttpError.Conflict("User {0} already exist.".Fmt(request.Id));

            Users.Data.Add(request);
            Users.ETags[request.Id] = Users.FakeETag();

            return new HttpResult(request)
            {
                Headers = { { "ETag", Users.ETags[request.Id] } }
            };
        }
    }
}
﻿using System.Linq;
using ServiceStack;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class AddUserService : Service
    {
        public object Post(AddUserRequest request)
        {
            if (Users.Data.FirstOrDefault(x => x.Id == request.Id) != null)
                throw HttpError.Conflict("User {0} already exist.".Fmt(request.Id));

            Users.Data.Add(request);
            Users.ETags[request.Id] = "a" + request.Id;

            return new HttpResult
            {
                Headers = { { "ETag", "a" + request.Id } }
            };
        }
    }
}

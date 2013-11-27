using System;
using ServiceStack.ServiceHost;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    public class GetUserRequest : IReturn<User>
    {
        public Guid Id  { get; set; }
    }
}

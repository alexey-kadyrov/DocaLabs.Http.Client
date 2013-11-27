using System.Collections.Generic;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    static class Users
    {
        public static List<User> Data { get; private set; }
        public static Dictionary<long, string> ETags { get; private set; }

        static Users()
        {
            Data = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@foo.bar"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Michael",
                    LastName = "Goodwill",
                    Email = "michael.goodwill@foo.bar"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Daniel",
                    LastName = "Banbury",
                    Email = "daniel.bunbury@foo.bar"
                }
            };

            ETags = new Dictionary<long, string>
            {
                { Data[0].Id, "i1" },
                { Data[1].Id, "i2" },
                { Data[2].Id, "i3" }
            };
        }
    }
}

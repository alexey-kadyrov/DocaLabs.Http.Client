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
                },
                new User
                {
                    Id = 4,
                    FirstName = "Paul",
                    LastName = "Joseph",
                    Email = "paul.joseph@foo.bar"
                },
                new User
                {
                    Id = 5,
                    FirstName = "Another Paul",
                    LastName = "Joseph",
                    Email = "paul.joseph@foo.bar"
                },
                new User
                {
                    Id = 6,
                    FirstName = "And Another Paul",
                    LastName = "Joseph",
                    Email = "paul.joseph@foo.bar"
                },
                new User
                {
                    Id = 1002,
                    FirstName = "Michael 2",
                    LastName = "Goodwill 2",
                    Email = "michael.goodwill@foo.bar"
                },
                new User
                {
                    Id = 2002,
                    FirstName = "Michael 2",
                    LastName = "Goodwill 2",
                    Email = "michael.goodwill@foo.bar"
                },
                new User
                {
                    Id = 3002,
                    FirstName = "Michael 2",
                    LastName = "Goodwill 2",
                    Email = "michael.goodwill@foo.bar"
                }
            };

            ETags = new Dictionary<long, string>
            {
                { Data[0].Id, "i1" },
                { Data[1].Id, "i2" },
                { Data[2].Id, "i3" },
                { Data[3].Id, "i4" },
                { Data[4].Id, "i5" },
                { Data[5].Id, "i6" },
                { Data[6].Id, "i1002" },
                { Data[7].Id, "i2002" },
                { Data[8].Id, "i3002" }
            };
        }
    }
}

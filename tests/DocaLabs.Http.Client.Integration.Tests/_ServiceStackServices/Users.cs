using System;
using System.Collections.Generic;
using System.Globalization;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    static class Users
    {
        readonly static Random Random;
        public static List<User> Data { get; private set; }
        public static Dictionary<Guid, string> ETags { get; private set; }

        static Users()
        {
            Random = new Random();

            Data = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@foo.bar"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Michael",
                    LastName = "Goodwill",
                    Email = "michael.goodwill@foo.bar"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Daniel",
                    LastName = "Banbury",
                    Email = "daniel.bunbury@foo.bar"
                }
            };

            ETags = new Dictionary<Guid, string>
            {
                { Data[0].Id, FakeETag() },
                { Data[1].Id, FakeETag() }
            };
        }

        public static string FakeETag()
        {
            return DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture) + Random.Next();
        }
    }
}

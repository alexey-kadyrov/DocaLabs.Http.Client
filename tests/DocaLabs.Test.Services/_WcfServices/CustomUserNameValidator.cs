﻿using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace DocaLabs.Test.Services._WcfServices
{
    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            Console.WriteLine("Validating userName:'{0}' password:'{1}'", userName, password);

            if (null == userName || null == password)
                throw new ArgumentNullException();

            if (!(userName == "testUser" && password == "testPassword"))
            {
                throw new SecurityTokenException("Unknown User name or Incorrect Password");
            }
        }
    }
}

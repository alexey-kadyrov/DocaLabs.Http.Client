﻿using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class TestHttpClientBaseTypeWithOnlyDefaultConstructor : HttpClient<TestsQuery, TestResultValue>
    {
        public TestHttpClientBaseTypeWithOnlyDefaultConstructor()
            : base(new Uri("http://foo.bar/"), "my")
        {
        }
    }
}
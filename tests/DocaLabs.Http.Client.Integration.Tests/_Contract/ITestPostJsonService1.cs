﻿using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Integration.Tests._Contract
{
    [SerializeAsJson]
    public interface ITestPostJsonService1
    {
        DataResponse PostData(DataRequest query);
    }
}
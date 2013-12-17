using System;
using DocaLabs.Http.Client;
using DocaLabs.Http.Client.Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests
{
    [TestClass]
    public class ServiceSetup
    {
        [AssemblyInitialize]
        public static void StartServices(TestContext context)
        {
            new HttpClient<VoidType, VoidType>(new Uri("http://localhost:6701/TestServicesProxy/Start")).Execute(VoidType.Value);
        }

        [AssemblyCleanup]
        public static void StopServices()
        {
            new HttpClient<VoidType, VoidType>(new Uri("http://localhost:6701/TestServicesProxy/Stop")).Execute(VoidType.Value);
        }
    }
}

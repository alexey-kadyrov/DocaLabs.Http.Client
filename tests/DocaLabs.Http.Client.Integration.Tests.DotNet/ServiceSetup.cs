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
            new HttpClient<VoidType, VoidType>(null, "startServices").Execute(VoidType.Value);
        }

        [AssemblyCleanup]
        public static void StopServices()
        {
            new HttpClient<VoidType, VoidType>(null, "stopServices").Execute(VoidType.Value);
        }
    }
}

using System;
using DocaLabs.Http.Client;
using DocaLabs.Http.Client.Binding;
#if GENERIC_DOT_NET
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

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

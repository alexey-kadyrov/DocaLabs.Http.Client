using System.ComponentModel.Composition;

namespace DocaLabs.Http.Client.Extension.Test.Example
{
    [Export(typeof(ITestImport1))]
    public class TestImport1 : ITestImport1
    {
    }
}

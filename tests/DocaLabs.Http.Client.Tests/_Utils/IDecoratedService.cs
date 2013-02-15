namespace DocaLabs.Http.Client.Tests._Utils
{
    [InterfaceOnly, ClassAttributeWithFieldsPropertiesAndConstructorArgs("one", Field = "two", Property = "three")]
    public interface IDecoratedService
    {
        TestResult GetResult(TestsQuery query);
    }
}
namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IServiceWithTwoMethods
    {
        TestResultValue GetResult(TestsQuery query);
        TestResultValue GetAnotherResult(TestsQuery query);
    }
}
namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IServiceWithTwoMethods
    {
        TestResult GetResult(TestsQuery query);
        TestResult GetAnotherResult(TestsQuery query);
    }
}
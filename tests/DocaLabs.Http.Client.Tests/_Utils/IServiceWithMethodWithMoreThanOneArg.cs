namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IServiceWithMethodWithMoreThanOneArg
    {
        TestResult GetResult(TestsQuery query, string notOk);
    }
}
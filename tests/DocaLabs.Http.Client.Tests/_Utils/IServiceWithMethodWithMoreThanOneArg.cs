namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IServiceWithMethodWithMoreThanOneArg
    {
        TestResultValue GetResult(TestsQuery query, string notOk);
    }
}
namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IServiceWithMethodWithMoreThanOneArg
    {
        TestResultValue GetResult(int query, string notOk);
    }
}
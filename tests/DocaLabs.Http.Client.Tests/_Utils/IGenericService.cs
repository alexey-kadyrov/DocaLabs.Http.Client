namespace DocaLabs.Http.Client.Tests._Utils
{
    public interface IGenericService<in TQuery, out TResult>
    {
        TResult GetResult(TQuery query);
    }

    public interface IGenericService2<in TQuery, out TResult>
    {
        TResult GetResult(TQuery query);
    }
}
namespace DocaLabs.Http.Client.Binding
{
    public class DefaultMutator : IMutator
    {
        public object Mutate(object queryModel, object httpClient)
        {
            return queryModel;
        }
    }
}

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultMutator : IMutator
    {
        public object Mutate(object model, object client)
        {
            return model;
        }
    }
}

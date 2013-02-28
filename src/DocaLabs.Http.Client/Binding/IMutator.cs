using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IMutator
    {
        object Mutate(object model, object client);
    }
}

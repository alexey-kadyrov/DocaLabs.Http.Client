﻿using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlMapper
    {
        CustomNameValueCollection Map(object model, object client);
    }
}

﻿using System;

namespace DocaLabs.Http.Client.Binding
{
    public interface IStreamTypeChecker
    {
        bool IsSupportedStream(Type type);
    }
}

using System;

namespace Unity.Data.Interface
{
    public interface IDatabaseFactory : IDisposable
    {
        IDataContext Get();
    }
}

using System;
using System.Data.Entity;

namespace Unity.Data.Interface
{
    public interface IDataContext : IDisposable
    {
        Database GetDbContext();
    }
}

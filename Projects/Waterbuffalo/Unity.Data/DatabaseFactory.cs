using Unity.Common.Disposable;
using Unity.Data.Interface;
using System;

namespace Unity.Data
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private IDataContext dataContext;

        public IDataContext Get()
        {
            return dataContext ?? (dataContext = new DatabaseContext());
        }

        protected override void DisposeCore()
        {
            base.DisposeCore();
        }
    }
}

using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Edmx;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework
{
    internal class EFContextFactory : IDisposable
    {
        internal void FixEfProviderServicesProblem()
        {
            var instance = SqlProviderServices.Instance;
        }

        internal TimeTrackerEntities TimeTrackerDBContext
        {
            get
            {
                TimeTrackerEntities context = new TimeTrackerEntities();
                context.Configuration.LazyLoadingEnabled = true;
                context.Configuration.AutoDetectChangesEnabled = false;
                ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
                return context;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

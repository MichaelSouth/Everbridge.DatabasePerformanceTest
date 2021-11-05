using System;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class DatabasePerformanceRepositoryFactory : IDatabasePerformanceRepositoryFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DatabasePerformanceRepositoryFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        IDatabasePerformanceRepository IDatabasePerformanceRepositoryFactory.Create(DatabaseProvider databaseProvider)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.sql:
                    return (IDatabasePerformanceRepository)serviceProvider.GetService(typeof(SqlRepository));
                    break;
                case DatabaseProvider.mongo:
                    return (IDatabasePerformanceRepository)serviceProvider.GetService(typeof(MongoRepository));
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}

using System;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class DatabasePerformanceRepositoryFactory : IDatabasePerformanceRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabasePerformanceRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        IDatabasePerformanceRepository IDatabasePerformanceRepositoryFactory.Create(DatabaseProvider databaseProvider)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.sql:
                    return (IDatabasePerformanceRepository)_serviceProvider.GetService(typeof(SqlRepository));
                    break;
                case DatabaseProvider.mongo:
                    return (IDatabasePerformanceRepository)_serviceProvider.GetService(typeof(MongoRepository));
                    break;
                case DatabaseProvider.elasticsearch:
                    return (IDatabasePerformanceRepository)_serviceProvider.GetService(typeof(ElasticsearchRepository));
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}

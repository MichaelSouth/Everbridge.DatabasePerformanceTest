namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public interface IDatabasePerformanceRepositoryFactory
    {
        IDatabasePerformanceRepository Create(DatabaseProvider databaseProvider);
    }
}

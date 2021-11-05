using System.Threading.Tasks;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public interface IDatabasePerformanceRepository
    {
        void ExecuteTask(DatabasePerformanceTask task);
    }
}

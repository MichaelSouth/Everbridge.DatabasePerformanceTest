using System;
using System.Threading.Tasks;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class SqlRepository : IDatabasePerformanceRepository
    {
        void IDatabasePerformanceRepository.ExecuteTask(DatabasePerformanceTask task)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Data;
using System.Data.SqlClient;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class SqlRepository : IDatabasePerformanceRepository
    {
        private const string ConnectionString = @"Data Source=(local);Initial Catalog=Test;User ID=sa;Password=password1.;MultipleActiveResultSets=True";

        void IDatabasePerformanceRepository.ExecuteTask(DatabasePerformanceTask task)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                for (var i = 0; i < task.IterationCount; i++)
                {
                    var sqlCommand = new SqlCommand("INSERT INTO [dbo].[PerfTest] ([Data]) VALUES(@data)", sqlConnection) { CommandType = CommandType.Text };
                    sqlCommand.Parameters.AddWithValue("@data", task.Data);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}

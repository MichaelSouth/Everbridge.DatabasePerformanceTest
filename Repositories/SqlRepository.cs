using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    // https://docs.docker.com/samples/aspnet-mssql-compose/
    public class SqlRepository : IDatabasePerformanceRepository
    {
        private const string ConnectionString = @"Server=mssqlserver;Database=master;User=sa;Password=Your_password123;"; //@"Data Source=(local);Initial Catalog=Test;User ID=sa;Password=password1.;MultipleActiveResultSets=True";

        private readonly ILogger<SqlRepository> _logger;

        public SqlRepository(ILogger<SqlRepository> logger)
        {
            _logger = logger;
            var createScriptPath = Path.Combine("Repositories", "CreateDatabase.sql");
            var createDatabaseSQLScript = File.ReadAllText(createScriptPath);

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand(createDatabaseSQLScript, sqlConnection) { CommandType = CommandType.Text };
                sqlCommand.ExecuteNonQuery();
            }

            var createTableScriptPath = Path.Combine("Repositories", "CreatePerfTest.sql");
            var createTableSQLScript = File.ReadAllText(createTableScriptPath);

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                sqlConnection.ChangeDatabase("Test");

                var sqlCommand = new SqlCommand(createTableSQLScript, sqlConnection) { CommandType = CommandType.Text };
                sqlCommand.ExecuteNonQuery();
            }
        }

        void IDatabasePerformanceRepository.ExecuteTask(DatabasePerformanceTask task)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                sqlConnection.ChangeDatabase("Test");

                if (task.Operation == "write")
                {
                    for (var i = 0; i < task.IterationCount; i++)
                    {
                        var sqlCommand = new SqlCommand("INSERT INTO [dbo].[PerfTest] ([Data]) VALUES(@data)", sqlConnection) { CommandType = CommandType.Text };
                        sqlCommand.Parameters.AddWithValue("@data", task.Data);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                else if (task.Operation == "deleteall")
                {
                    task.IterationCount = 1;
                    var sqlCommand = new SqlCommand("DELETE FROM [dbo].[PerfTest]", sqlConnection) { CommandType = CommandType.Text };
                    var rowCount = sqlCommand.ExecuteNonQuery();
                    task.Message = $"Deleted {rowCount} rows";
                }
            }
        }
    }
}

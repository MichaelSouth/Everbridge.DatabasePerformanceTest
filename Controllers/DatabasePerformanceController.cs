using Everbridge.DatabasePerformanceTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Everbridge.DatabasePerformanceTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabasePerformanceController : ControllerBase
    {
        private readonly ILogger<DatabasePerformanceController> _logger;
        private readonly IDatabasePerformanceRepositoryFactory _databasePerformanceRepositoryFactory;
        private readonly static List<DatabasePerformanceTask> _tasks = new List<DatabasePerformanceTask>();

        public DatabasePerformanceController(ILogger<DatabasePerformanceController> logger, IDatabasePerformanceRepositoryFactory databasePerformanceRepositoryFactory)
        {
            _logger = logger;
            _databasePerformanceRepositoryFactory = databasePerformanceRepositoryFactory;
        }

        [HttpGet]
        public IEnumerable<DatabasePerformanceTask> Get()
        {
            return _tasks.ToArray();
        }

        [HttpPost("ClearAllTasks")]
        public IActionResult ClearAllTasks()
        {
            _logger.LogInformation($"ClearAllTasks");
            _tasks.Clear();
            return Ok();
        }

        [HttpPost("StartTask")]
        public IActionResult StartTask(DatabasePerformanceTask task)
        {
            _logger.LogInformation($"Start database performance task: {task.TaskIdentifier}");
           // string userName = HttpContext.User.Identity.Name;

            Task.Run(() =>
            {
                _tasks.Add(task);// TODO remove
                task.Status = "Initialising";

                try
                {
                    var repository = _databasePerformanceRepositoryFactory.Create(ConvertDatabaseProvider(task.DatabaseProvider));
                    task.Status = "Progress";
                    task.StartTime = DateTime.UtcNow;
                    repository.ExecuteTask(task);
                    task.Status = "Completed";
                }
                catch(Exception e)
                {
                    task.Status = "Error";
                    task.Message = e.Message;
                    _logger.LogError($"Database performance exception: {task.TaskIdentifier}:", e);
                }
                finally
                {
                    task.EndTime = DateTime.UtcNow;
                    var executionTime = task.EndTime.Subtract(task.StartTime);
                    task.ExecutionTime = (long)executionTime.TotalMilliseconds;
                    _logger.LogInformation($"End database performance task: {task.TaskIdentifier}: Execution time {executionTime.TotalMilliseconds}ms");
                }
            });

            return Ok();
        }

        private static DatabaseProvider ConvertDatabaseProvider(string databaseProviderString)
        {
            return (DatabaseProvider)Enum.Parse(typeof(DatabaseProvider), databaseProviderString);
        }
    }
}


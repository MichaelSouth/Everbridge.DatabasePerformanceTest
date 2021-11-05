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

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPost]
        public IActionResult StartTask(DatabasePerformanceTask task)
        {
            _logger.LogInformation($"Start database performance task: {task.TaskIdentifier}");
           // string userName = HttpContext.User.Identity.Name;
            var repository = _databasePerformanceRepositoryFactory.Create(ConvertDatabaseProvider(task.DatabaseProvider));

            Task.Run(() =>
            {
                _tasks.Add(task);// TODO remove
                task.StartTime = DateTime.UtcNow;
                repository.ExecuteTask(task);
                task.EndTime = DateTime.UtcNow;
                var executionTime = task.EndTime.Subtract(task.StartTime);
                _logger.LogInformation($"End database performance task: {task.TaskIdentifier}: Execution time {executionTime.TotalMilliseconds}ms");
            });

            return Ok();
        }

        private static DatabaseProvider ConvertDatabaseProvider(string databaseProviderString)
        {
            return (DatabaseProvider)Enum.Parse(typeof(DatabaseProvider), databaseProviderString);
        }
    }
}

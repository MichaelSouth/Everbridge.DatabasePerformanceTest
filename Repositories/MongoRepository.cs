using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class MongoRepository : IDatabasePerformanceRepository
    {
        private readonly IMongoCollection<TaskModel> _collectionPerfTest;
        private readonly ILogger<MongoRepository> _logger;

        public MongoRepository(ILogger<MongoRepository> logger)
        {
            _logger = logger;

            var connectionString = "mongodb://host.docker.internal:12000"; //"mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Test");

            _collectionPerfTest = database.GetCollection<TaskModel>("PerfTest");
        }

        public void ExecuteTask(DatabasePerformanceTask task)
        {
            if (task.Operation == "write")
            {
                for (var i = 0; i < task.IterationCount; i++)
                {
                    var taskModel = new TaskModel { Data = task.Data };
                    _collectionPerfTest.InsertOne(taskModel);
                }
            }
            else if (task.Operation == "deleteall")
            {
                task.IterationCount = 1;
                var result = _collectionPerfTest.DeleteMany("{}");
                task.Message = $"Deleted {result.DeletedCount} rows";
            }
            else if (task.Operation == "read")
            {
               var modelTasks = _collectionPerfTest.Find(x => true).Limit(task.IterationCount).ToList();

               foreach(var modelTask in modelTasks)
               {
                   var tempTask = _collectionPerfTest.Find(_ => _.Id == modelTask.Id).Single();
               }

               task.Message = $"Read {modelTasks.Count} rows";
            }
        }

        public class TaskModel
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            public string Data { get; set; }
        }
    }
}

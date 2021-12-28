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
            for (var i =0; i < task.IterationCount; i++)
            {
                var taskModel = new TaskModel { Data = task.Data };
                _collectionPerfTest.InsertOne(taskModel);
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

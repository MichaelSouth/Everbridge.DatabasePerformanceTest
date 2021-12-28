using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using System;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    public class ElasticsearchRepository : IDatabasePerformanceRepository
    {
        private readonly ILogger<ElasticsearchRepository> _logger;

        public ElasticsearchRepository(ILogger<ElasticsearchRepository> logger)
        {
            _logger = logger;;
        }

        public void ExecuteTask(DatabasePerformanceTask task)
        {
            var settings = new ConnectionConfiguration(new Uri("http://host.docker.internal:9200"))
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var lowlevelClient = new ElasticLowLevelClient(settings);
            
            for (var i = 0; i < task.IterationCount; i++)
            {
                var taskModel = new TaskModel { Data = task.Data };
     
                var indexResponse = lowlevelClient.Index<BytesResponse>("perftest", PostData.Serializable(taskModel));
                byte[] responseBytes = indexResponse.Body;
            }
        }

        public class TaskModel
        {
            public string Id { get; set; }
            public string Data { get; set; }
        }
    }
}

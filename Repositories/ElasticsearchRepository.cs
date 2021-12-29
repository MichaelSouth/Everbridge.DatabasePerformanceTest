using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using System;

namespace Everbridge.DatabasePerformanceTest.Repositories
{
    // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/elasticsearch-net-getting-started.html
    public class ElasticsearchRepository : IDatabasePerformanceRepository
    {
        private readonly ILogger<ElasticsearchRepository> _logger;

        public ElasticsearchRepository(ILogger<ElasticsearchRepository> logger)
        {
            _logger = logger;;
        }

        public void ExecuteTask(DatabasePerformanceTask task)
        {
            var settings = new ConnectionConfiguration(new Uri("http://host.docker.internal:9200")).RequestTimeout(TimeSpan.FromMinutes(2));
            var lowlevelClient = new ElasticLowLevelClient(settings);
                    
            if (task.Operation == "write")
            {
                for (var i = 0; i < task.IterationCount; i++)
                {
                    var taskModel = new TaskModel { Data = task.Data };

                    var indexResponse = lowlevelClient.Index<BytesResponse>("perftest", PostData.Serializable(taskModel));
                    byte[] responseBytes = indexResponse.Body;
                }
            }
            else if (task.Operation == "deleteall")
            {
                task.IterationCount = 1;
                dynamic result = lowlevelClient.DeleteByQuery<DynamicResponse>("perftest", @"{""query"" : { ""match_all"" : { }} }");
                var rowCount = result.Body["total"];
                task.Message = $"Deleted {rowCount} rows";
            }
            else if (task.Operation == "read")
            {
            //    var rows = lowlevelClient.Search<BytesResponse>("perftest", @"""query"": { size=5}");
            //    rows.res
            //    var row = lowlevelClient.Get<BytesResponse>("perftest","3242")
                //task.Message = $"Read {identifiers.Count} rows";
            }
        }

        public class TaskModel
        {
            public string Id { get; set; }
            public string Data { get; set; }
        }
    }
}

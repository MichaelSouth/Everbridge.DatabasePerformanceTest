using System;

namespace Everbridge.DatabasePerformanceTest
{
    public class DatabasePerformanceTask
    {
        public string TaskIdentifier { get; set; }
        public string DatabaseProvider { get; set; }
        public string Data { get; set; }
        public int ThreadCount { get; set; }
        public int IterationCount { get; set; }
        public string Operation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long ExecutionTime { get; set; }
    }
}

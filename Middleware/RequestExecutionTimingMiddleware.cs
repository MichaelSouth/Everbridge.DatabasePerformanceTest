using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Everbridge.DatabasePerformanceTest.Middleware
{
    public class RequestExecutionTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestExecutionTimingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestExecutionTimingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var currentTime = DateTime.UtcNow;

            await _next(context);

            var executionTimeSpan = (DateTime.UtcNow.Subtract(currentTime));
            _logger.LogInformation($"Http request execution time: {context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}?{context.Request.QueryString}: executed in {executionTimeSpan.TotalMilliseconds}ms.");
        }
    }
}

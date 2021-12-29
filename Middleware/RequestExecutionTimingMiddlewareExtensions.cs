using Microsoft.AspNetCore.Builder;

namespace Everbridge.DatabasePerformanceTest.Middleware
{
    public static class RequestExecutionTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestExecutionTiming(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestExecutionTimingMiddleware>();
        }
    }
}

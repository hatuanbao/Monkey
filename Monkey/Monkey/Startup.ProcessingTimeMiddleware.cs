using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public class ProcessingTimeMiddleware
        {
            private readonly RequestDelegate _next;

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseMiddleware<ProcessingTimeMiddleware>();
            }

            public ProcessingTimeMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext context)
            {
                var watch = new Stopwatch();
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    watch.Stop();
                    var elapsedMilliseconds = watch.ElapsedMilliseconds.ToString();
                    httpContext.Response.Headers.Add("X-Processing-Time-Milliseconds", elapsedMilliseconds);
                    return Task.CompletedTask;
                }, context);

                watch.Start();
                return _next(context);
            }
        }
    }
}
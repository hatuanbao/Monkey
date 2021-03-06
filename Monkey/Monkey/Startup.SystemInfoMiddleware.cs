using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public class SystemInfoMiddleware
        {
            private readonly RequestDelegate _next;

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseMiddleware<SystemInfoMiddleware>();
            }

            public SystemInfoMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;

                    // Server
                    if (httpContext.Response.Headers.ContainsKey("Server"))
                    {
                        httpContext.Response.Headers.Remove("Server");
                    }
                    httpContext.Response.Headers.Add("Server", Core.SystemConfigs.Server.Name);

                    // X-Powered-By
                    if (httpContext.Response.Headers.ContainsKey("X-Powered-By"))
                    {
                        httpContext.Response.Headers.Remove("X-Powered-By");
                    }
                    httpContext.Response.Headers.Add("X-Powered-By", Core.SystemConfigs.Server.PoweredBy);

                    // X-Author-Name
                    if (httpContext.Response.Headers.ContainsKey("X-Author-Name"))
                    {
                        httpContext.Response.Headers.Remove("X-Author-Name");
                    }
                    httpContext.Response.Headers.Add("X-Author-Name", Core.SystemConfigs.Server.AuthorName);

                    // X-Author-Website
                    if (httpContext.Response.Headers.ContainsKey("X-Author-Website"))
                    {
                        httpContext.Response.Headers.Remove("X-Author-Website");
                    }
                    httpContext.Response.Headers.Add("X-Author-Website", Core.SystemConfigs.Server.AuthorWebsite);

                    // X-Author-Email
                    if (httpContext.Response.Headers.ContainsKey("X-Author-Email"))
                    {
                        httpContext.Response.Headers.Remove("X-Author-Email");
                    }
                    httpContext.Response.Headers.Add("X-Author-Email", Core.SystemConfigs.Server.AuthorEmail);

                    return Task.CompletedTask;
                }, context);

                return _next(context);
            }
        }
    }
}
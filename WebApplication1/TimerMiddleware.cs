using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1;

namespace WebApplication1
{
    public class TimerMiddleware
    {
        private readonly RequestDelegate _next;

        public TimerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = new Stopwatch();
            timer.Start();

            context.Response.OnStarting(state =>
            {
                timer.Stop();
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("X-Elapsed", new[] { timer.Elapsed.ToString() });

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}

public static class TimerMiddlewareExtensions
{
    public static IApplicationBuilder UseTimer(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TimerMiddleware>();
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly string _cookieName;
        private readonly int _timeout;

       public TimerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _cookieName = configuration["TimerMiddleware:CookieName"];
            //_timeout = int.Parse(configuration["TimerMiddleware:Timeout"]);
            _timeout = configuration.GetValue<int>("TimerMiddleware:Timeout");
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = new Stopwatch();
            timer.Start();

            context.Response.OnStarting(state =>
            {
                timer.Stop();
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add(_cookieName, new[] { timer.Elapsed.ToString() });

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
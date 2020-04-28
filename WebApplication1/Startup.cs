using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ICounterService, CounterService>();
            //services.AddScoped<ICounterService, CounterService>();
            services.AddTransient<ICounterService, CounterService>();

        }  

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //prima  Midlleware Time
            //app.UseTimer();

            //if(env.IsDevelopment()) //se sono in ambiente di sviluppo -> come lo capisco? progetto->proprietà->Debug  In ambiente di produzione cambierò questa variabile
            if (env.IsEnvironment("Development")) //env.IsTest nella classe AspNetEnvironmentExtensions()
                app.UseDeveloperExceptionPage(); //ci mostra una diagnostica dettagliata di dove si è verificata l'eccezione

            //seconda Middleware
            app.Use(async (context, next) =>
            {
                if (!(context.Request.Path.Value == "/favicon.ico"))
                {
                    var counterService = context.RequestServices.GetService<ICounterService>();
                    counterService.Counter++;
                }

                //throw new InvalidOperationException();
                await context.Response.WriteAsync("<div> Hello World from the Middleware 2 </div>");
                await next.Invoke();
            });


            // ultima middleware
            //app.UseStaticFiles(); //gestisci i file statici, se arriva una richiesta http per un file statico (localhost:5000/index.html)

            // se non esiste qualcosa di statico del comando sopra interviene il comando di sotto
            app.Run(async context =>
            {
                await context.Response.WriteAsync("<html><body><h1>Hello World! Middleware Finale!</h1></body></html>");


                if (!(context.Request.Path.Value == "/favicon.ico"))
                {
                    var counterService = context.RequestServices.GetService<ICounterService>();
                    counterService.Counter++;

                    await context.Response.WriteAsync($"Counter: {counterService.Counter}");
                }
                
            });
        }
    }
}

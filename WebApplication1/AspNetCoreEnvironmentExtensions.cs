using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class AspNetCoreEnvironmentExtensions
    {
        const string Test = "Test";

        public static bool IsTest(this IWebHostEnvironment env) => env.IsEnvironment(Test);
    }
}

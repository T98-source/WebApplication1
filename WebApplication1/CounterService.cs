using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public interface ICounterService
    {
        int Counter { get; set; }
    }
    public class CounterService : ICounterService
    {
        public int Counter { get; set; }
    }
}

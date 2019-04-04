using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AsyncWebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPUBoundController : ControllerBase
    {
        private static int requestCount = 0;

        private Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        // GET api/cpubound
        // .\bombardier.exe "http://localhost:5001/api/cpubound?start=1&end=1000000" -n 20 -t 100s
        [HttpGet]
        public async Task<ActionResult<int>> Get([FromQuery] int start, [FromQuery] int end)
        {
            Interlocked.Increment(ref requestCount);
            return await GetPrimesCountAsync(start, end);
        }

        // GET api/cpubound/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/cpubound
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/cpubound/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/cpubound/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

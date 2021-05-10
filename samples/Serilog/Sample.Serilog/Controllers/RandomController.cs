using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.Serilog.Controllers
{
    [Route("api/random")]
    public class RandomController : Controller
    {
        private readonly ILogger<RandomController> _logger;
        
        public RandomController(ILogger<RandomController> logger) => _logger = logger;
        
        [HttpGet]
        public string GetRandomNumber([FromQuery] int low = 0, [FromQuery] int high = 100)
        {
            var random = new Random();
            var number = random.Next(low, high);
            _logger.LogInformation($"Generated number in [{low},{high}]: {number}");
            return $"random number is {number}";
        }
    }
}
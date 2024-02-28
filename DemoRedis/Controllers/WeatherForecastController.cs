using DemoRedis.Attribbutes;
using DemoRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DemoRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IResponseCacheService _responseCacheService;
        private string GetControllerName => ControllerContext.ActionDescriptor.ControllerName;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("GetAll")]
        [Cache(100)]
        public async Task<IActionResult> Get(string keyword = null, int pageIndex = 1, int pageSize = 10)
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            await _responseCacheService.RemoveCacheResponseAsync($"/{GetControllerName}");
            return Ok();
        }
    }
}
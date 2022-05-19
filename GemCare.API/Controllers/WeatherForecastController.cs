using GemCare.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Controllers
{
    public class WeatherForecastController : BaseApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEncryptionDecryptionHelper _encHelper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEncryptionDecryptionHelper encHelper)
        {
            _logger = logger;
            _encHelper = encHelper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("apikey")]
        public IActionResult GetApiKey()
        {
            //GemCareLab.API
            var result = _encHelper.Encrypt("GemCareLab.API");
            return Ok(result);
        }
    }
}

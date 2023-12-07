using Microsoft.AspNetCore.Mvc;

namespace Api_Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaProducerController : ControllerBase
{

    private readonly ILogger<KafkaProducerController> _logger;

    public KafkaProducerController(ILogger<KafkaProducerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
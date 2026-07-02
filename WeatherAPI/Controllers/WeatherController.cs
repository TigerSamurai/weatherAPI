using Microsoft.AspNetCore.Mvc;
using WeatherAPI.ExceptionHandler;
using WeatherAPI.Model;
using WeatherAPI.Service;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;
    
    public WeatherController(WeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<WeatherSummary>> GetCityStatusCheck(string city)
    {
        try
        {
           var weather = await _weatherService.GetWeatherAsync(city);
           if (weather == null)
           {
               return NotFound(new { message = $"Weather data for '{city}' could not be retrieved from the provider." });
           }
           return Ok(weather);
        }
        catch (RedisDataMissException ex)
        {
            _logger.LogError(ex, "Redis infrastructure is down.");
            return StatusCode(500, "Cache infrastructure failure");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            return StatusCode(500, "A generic server error occured");
        }
    }
}
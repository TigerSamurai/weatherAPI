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
    
    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<WeatherSummary>> GetCityStatusCheck(string city)
    {
        try
        {
            var weather = await _weatherService.GetWeatherAsync(city);
            if (weather == null) return NotFound("City not found");
        }
        catch (RedisDataMissException ex)
        {
            _logger.LogError(ex, "Redis infrastructure is down.");
            return StatusCode(500, "Cache infrastructure failure");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "A generic server error occured");
        }

        return Ok(city);
    }
}
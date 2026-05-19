using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WeatherAPI.Properties.ExtensionHandler;

namespace WeatherAPI.Properties;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly ILogger<WeatherService> _logger;
    
    public WeatherService(HttpClient httpClient, IDistributedCache cache,  ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<WeatherSummary?>  CheckRedisForCity(string city)
    {
        string cacheKey = $"city_name:{city.ToLower()}";
        
        string? CheckRedisForCityAsync = await _cache.GetStringAsync(cacheKey);

        if (string.IsNullOrEmpty(CheckRedisForCityAsync))
        {
            Console.WriteLine("Not in cache. Checking database...");
            return null;
        }

        WeatherSummary? foundCity = null;
        
        try
        {
            foundCity = JsonSerializer.Deserialize<WeatherSummary>(CheckRedisForCityAsync);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to show info");
            return null;
        }
        
        Console.WriteLine("Found in cache! Returning data...");
        return foundCity;
    }

    private async Task<RootObject?> getCityWeatherInfo(string city)
    {
        //TODO: write the method
    }
}
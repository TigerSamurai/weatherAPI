using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace WeatherAPI.Properties;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    
    public WeatherService(HttpClient httpClient, IDistributedCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
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

        WeatherSummary? foundCity = JsonSerializer.Deserialize<WeatherSummary>(CheckRedisForCityAsync); // TODO: make a try-catch block
        
        Console.WriteLine("Found in cache! Returning data...");
        return foundCity;
    }
}
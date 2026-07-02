using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WeatherAPI.ExceptionHandler;
using WeatherAPI.Model;

namespace WeatherAPI.Service;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly ILogger<WeatherService> _logger;
    private readonly IConfiguration _configuration;
    
    public WeatherService(HttpClient httpClient, 
        IDistributedCache cache,  
        ILogger<WeatherService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    private async Task<WeatherSummary?>  checkRedisForCity(string city)
    {
        string cacheKey = $"city_name:{city.ToLower()}";
        
        string? CheckRedisForCityAsync = await _cache.GetStringAsync(cacheKey);

        if (string.IsNullOrEmpty(CheckRedisForCityAsync))
        {
            return null;
        }

        WeatherSummary? foundCity;
        
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
        string apiKey = _configuration["WeatherApi:ApiKey"] 
                        ?? throw new InvalidOperationException("API Key is missing from configuration.");
        string baseUrl = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";

        string fullUrl = $"{baseUrl}{city.ToLower()}?key={apiKey}&unitGroup=metric";
        
        _httpClient.Timeout = TimeSpan.FromSeconds(5);

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Fetching data...");
                string rawJsonText = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RootObject>(rawJsonText);
            }
            else
            {
                Console.WriteLine($"Visual Crossing returned an error status: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
        {
            Console.WriteLine($"[Network Error] Visual Crossing API timed out or was unreachable for {city}. Details: {ex.Message}");
            return null;
        }
    }

    public async Task<WeatherSummary?> GetWeatherAsync(string city)
    {
        WeatherSummary? redisInfo = await checkRedisForCity(city);
        if (redisInfo != null)
        {
            return redisInfo;
        }
        else
        {
            Console.WriteLine("Didn't find city's weather info in redis. Fetching info from Visual Crossing...");
            RootObject? rawData = await getCityWeatherInfo(city);
            if (rawData == null)
            {
                return null;
            }
            else
            {
                WeatherSummary cleanSummary = new WeatherSummary(rawData);
                string cacheKey = $"city_name:{city.ToLower()}";
                string cleanJsonText = JsonSerializer.Serialize(cleanSummary);
                await _cache.SetStringAsync(cacheKey, cleanJsonText);
                return cleanSummary;
            }
        }
    }
}
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

    public async Task<WeatherSummary?>  CheckRedisForCountry()
    {
        
    }
}
namespace WeatherAPI.Properties.ExtensionHandler;

public class RedisDataMissException : Exception
{
    public RedisDataMissException(string cityNotInRedis)
        : base("City not in cache")
    {
    }
}
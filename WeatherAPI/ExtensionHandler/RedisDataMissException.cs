namespace WeatherAPI.ExtensionHandler;

public class RedisDataMissException : Exception
{
    public RedisDataMissException()
        : base("City is not found in cache, looking for data in server...")
    {
    }
}
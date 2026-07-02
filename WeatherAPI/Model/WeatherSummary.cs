namespace WeatherAPI.Model;

public class WeatherSummary
{
    public string CityName { get; set; }
    public double? Temperature { get; set; }
    public double? MinTemp  { get; set; }
    public double? MaxTemp { get; set; }

    
    public WeatherSummary() { }
    
    public WeatherSummary(RootObject rootObject)
    {
        CityName = rootObject.address;
        Temperature = rootObject.currentConditions.temp;
        MinTemp = rootObject.days?[0].tempmin;
        MaxTemp = rootObject.days?[0].tempmax;
    }
}
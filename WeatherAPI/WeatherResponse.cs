namespace WeatherAPI.Properties;

public class RootObject
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string resolvedAddress { get; set; }
    public string address { get; set; }
    public string timezone { get; set; }
    public string description { get; set; }
    public Days[] days { get; set; }
    public object[] alerts { get; set; }
    public CurrentConditions currentConditions { get; set; }
}

public class Days
{
    public string datetime { get; set; }
    public double tempmax { get; set; }
    public double tempmin { get; set; }
    public double temp { get; set; }
    public double feelslike { get; set; }
    public double humidity { get; set; }
    public double snow { get; set; }
    public double cloudcover { get; set; }
    public double visibility { get; set; }
    public double uvindex { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
    public Hours[] hours { get; set; }
}

public class CurrentConditions
{
    public string datetime { get; set; }
    public double temp { get; set; }
    public double feelslike { get; set; }
    public double humidity { get; set; }
    public double snow { get; set; }
    public double visibility { get; set; }
    public double cloudcover { get; set; }
    public double uvindex { get; set; }
    public string icon { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
}

public class Hours
{
    public string datetime { get; set; }
    public double temp { get; set; }
    public double feelslike { get; set; }
    public double humidity { get; set; }
    public double snow { get; set; }
    public double visibility { get; set; }
    public double cloudcover { get; set; }
    public double uvindex { get; set; }
    public string icon { get; set; }
    
}


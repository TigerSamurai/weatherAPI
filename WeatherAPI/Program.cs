using StackExchange.Redis;
using WeatherAPI.Service;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddControllers();
builder.Services.AddScoped<WeatherService>();

try {
    var redis = ConnectionMultiplexer.Connect("localhost:6379");
    var db = redis.GetDatabase();
    Console.WriteLine($"Redis Connection State: {redis.IsConnected}");
    db.StringSet("test_key", "It works!");
    Console.WriteLine($"Test Value from Redis: {db.StringGet("test_key")}");
} 
catch (Exception ex) {
    Console.WriteLine($"Redis Connection Failed: {ex.Message}");
}

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "WeatherAPI_";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "API is running!");

app.Run();
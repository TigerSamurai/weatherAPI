using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "API is running!");

app.Run();
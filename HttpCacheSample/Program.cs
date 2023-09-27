using HttpCacheSample;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("weather")
    .AddHttpMessageHandler(provider => new HttpCacheHandler(provider.GetRequiredService<IMemoryCache>()));

builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddScoped<HttpCacheHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("weather/{city}", async (string city, IWeatherService weatherService) =>
{
    var weatherForecast = await weatherService.GetWeatherByCity(city);

    return weatherForecast != null ? Results.Ok(weatherForecast) : Results.NotFound($"Weather data not found for {city}");
});

app.Run();
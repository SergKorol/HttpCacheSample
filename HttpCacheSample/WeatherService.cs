namespace HttpCacheSample;

public interface IWeatherService
{
    Task<WeatherForecast?> GetWeatherByCity(string city);
}

public class WeatherService : IWeatherService
{
    private const string ApiKey = "60bdfb3ef219dcd99a4927f44e8b65bd";
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherForecast?> GetWeatherByCity(string city)
    {
        ArgumentNullException.ThrowIfNull(city);
        var client = _httpClientFactory.CreateClient("weather");
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={ApiKey}";
        
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var weather = await response.Content.ReadFromJsonAsync<OpenWeatherMapResponse>();
        return new WeatherForecast
        {
            TemperatureC = weather!.main.temp,
            Date = DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(weather.dt).LocalDateTime),
            Summary = weather.main.temp < 20
                ? $"The weather is ugly in {weather.name}"
                : $"The weather is fine in {weather.name}"
        };
    }
}
namespace HttpCacheSample;

public record WeatherForecast
{
    public DateOnly Date { get; set; }

    public double TemperatureC { get; set; }

    public double TemperatureF => 32 + TemperatureC / 0.5556;

    public string? Summary { get; set; }
}
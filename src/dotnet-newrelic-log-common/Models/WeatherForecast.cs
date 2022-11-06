namespace dotnet_newrelic_log_common.Models;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public DateTime Hour { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

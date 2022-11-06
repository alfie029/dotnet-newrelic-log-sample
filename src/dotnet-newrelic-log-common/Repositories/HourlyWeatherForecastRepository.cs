using dotnet_newrelic_log_common.Models;

namespace dotnet_newrelic_log_common.Repositories;

public class HourlyWeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [NewRelic.Api.Agent.Trace]
    public IEnumerable<WeatherForecast> Forecast(uint unit)
    {
        if (unit > 48)
        {
            throw new ArgumentOutOfRangeException(nameof(unit), "Can't accumulate forecast more than 48 hours");
        }
        return Enumerable.Range(1, (int)unit)
            .Select(index =>
            {
                // Simulate a long progress
                Thread.Sleep(20 * index);
                return index;
            })
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now,
                Hour = DateTime.Now.AddHours(index),
                TemperatureC = Random.Shared.Next(-10, 25),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}

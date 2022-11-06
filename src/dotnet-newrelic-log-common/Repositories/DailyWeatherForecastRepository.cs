using dotnet_newrelic_log_common.Models;

namespace dotnet_newrelic_log_common.Repositories;

public class DailyWeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [NewRelic.Api.Agent.Trace]
    public IEnumerable<WeatherForecast> Forecast(uint unit)
    {
        if (unit > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(unit), "Can't accumulate forecast more than 10 days");
        }
        return Enumerable.Range(1, (int)unit)
            .Select(index =>
            {
                // Simulate a long progress
                Thread.Sleep(10 * index);
                return index;
            })
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                Hour = DateTime.Now.AddDays(index).Date,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}

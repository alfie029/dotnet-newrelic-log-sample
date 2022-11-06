using System.Diagnostics;
using dotnet_newrelic_log_common.Models;
using dotnet_newrelic_log_common.Repositories;
using Microsoft.Extensions.Logging;

namespace dotnet_newrelic_log_common.Business;

public class WeatherForecastBusiness : IWeatherForecastBusiness
{
    private readonly IEnumerable<IWeatherForecastRepository> _weatherForecastRepositories;
    private readonly ILogger<WeatherForecastBusiness> _logger;

    public WeatherForecastBusiness(
        IEnumerable<IWeatherForecastRepository> weatherForecastRepositories,
        ILogger<WeatherForecastBusiness> logger)
    {
        _weatherForecastRepositories = weatherForecastRepositories;
        _logger = logger;
    }

    [NewRelic.Api.Agent.Trace]
    public IEnumerable<WeatherForecast> DailyForecast(uint days)
    {
        return WrapWithStopwatch(() => _weatherForecastRepositories
            .First(r => r is DailyWeatherForecastRepository)
            .Forecast(days), _logger);
    }

    [NewRelic.Api.Agent.Trace]
    public IEnumerable<WeatherForecast> HourlyForecast(uint hours)
    {
        return WrapWithStopwatch(() => _weatherForecastRepositories
            .First(r => r is HourlyWeatherForecastRepository)!
            .Forecast(hours), _logger);
    }

    [DebuggerStepThrough]
    private static T WrapWithStopwatch<T>(Func<T> inner, ILogger logger)
    {
        var watch = Stopwatch.StartNew();
        try
        {
            return inner();
        }
        finally
        {
            watch.Stop();
            switch (watch.ElapsedMilliseconds)
            {
                case > 5000:
                    logger.LogError("Business run too much time ({Time})", watch.Elapsed);
                    break;
                case > 2000:
                    logger.LogWarning("Business run slow with {Time}", watch.Elapsed);
                    break;
                default:
                    logger.LogInformation("Business run ok with {Time}", watch.Elapsed);
                    break;
            }
        }
    }
}

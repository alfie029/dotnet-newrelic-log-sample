using dotnet_newrelic_log_common.Models;

namespace dotnet_newrelic_log_common.Repositories;

public interface IWeatherForecastRepository
{
    IEnumerable<WeatherForecast> Forecast(uint unit);
}

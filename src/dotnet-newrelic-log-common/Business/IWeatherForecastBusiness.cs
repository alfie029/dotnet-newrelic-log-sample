using dotnet_newrelic_log_common.Models;

namespace dotnet_newrelic_log_common.Business;

public interface IWeatherForecastBusiness
{
    IEnumerable<WeatherForecast> DailyForecast(uint days);

    IEnumerable<WeatherForecast> HourlyForecast(uint hours);
}

using dotnet_newrelic_log_common.Business;
using dotnet_newrelic_log_common.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_newrelic_log_api_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastBusiness _forecastBusiness;

    public WeatherForecastController(IWeatherForecastBusiness forecastBusiness, ILogger<WeatherForecastController> logger)
    {
        _forecastBusiness = forecastBusiness;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [HttpGet("daily", Name = "GetWeatherForecastDaily")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK, "application/json")]
    public IEnumerable<WeatherForecast> GetDaily([FromQuery] uint days = 5)
    {
        return _forecastBusiness.DailyForecast(days);
    }

    [HttpGet("hourly", Name = "GetWeatherForecastHourly")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK, "application/json")]
    public IEnumerable<WeatherForecast> GetHourly([FromQuery] uint hour = 12)
    {
        return _forecastBusiness.HourlyForecast(hour);
    }
}

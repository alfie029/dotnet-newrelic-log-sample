using System.Text.Json;
using dotnet_newrelic_log_common.Business;
using dotnet_newrelic_log_common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace dotnet_newrelic_log_forward.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastBusiness _forecastBusiness;

    public WeatherForecastController(IWeatherForecastBusiness forecastBusiness,
        ILogger<WeatherForecastController> logger)
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
    public IActionResult GetHourly([FromQuery] uint hour = 12)
    {
        try
        {
            return Ok(_forecastBusiness.HourlyForecast(hour));
        }
        catch (ArgumentOutOfRangeException e)
        {
            // simulate to write a very long message
            _logger.LogError(e, "Invalid range for {hour}, {Exception}, {Another}, {Addition}, {Request}",
                hour, e, e, e,
                new { Hour = hour, Controller = nameof(WeatherForecastController), Action = "GetHourly" });
            ModelState.AddModelError("hour", e.Message);
            return BadRequest();
        }
    }
}

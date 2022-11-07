using dotnet_newrelic_log_common.Business;
using dotnet_newrelic_log_common.Formatter;
using dotnet_newrelic_log_common.Repositories;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithNewRelicLogsInContext()
    .WriteTo.Console(formatter: new StructuredJsonFormatter())
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<IWeatherForecastRepository, DailyWeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastRepository, HourlyWeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastBusiness, WeatherForecastBusiness>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

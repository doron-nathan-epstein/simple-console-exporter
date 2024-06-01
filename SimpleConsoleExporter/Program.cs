// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using SimpleConsoleExporter;

await Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logger =>
    {
      //logger.AddSimpleConsole(config =>
      //{
      //  config.SingleLine = true;
      //  config.UseUtcTimestamp = true;
      //  config.TimestampFormat = "dd-MM-yyyy HH:mm:ss";
      //});

      logger.ClearProviders();

      logger.AddOpenTelemetry(config =>
      {
        config.AddConsoleExporter();
      });
    })
    .ConfigureServices(services =>
    {
    })
    .Build()
    .RunAsync();

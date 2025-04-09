using System.Globalization;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using NeuroMedia.Application.Logging;

using Serilog;

namespace NeuroMedia.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static ILogger LoggerConfiguration(IConfiguration configuration, IHostEnvironment env)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.ByTransforming<LogRow>(t => new
                {
                    t.Message,
                    EntityId = t.EntityId == Guid.Empty ? null : t.EntityId.ToString(),
                    t.User,
                    t.CallerMemberName,
                    t.CallerFilePath,
                    t.CallerLineNumber,
                    t.LogData,
                    t.StackTrace
                });

            if (env.IsProduction())
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
                logger.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
            }
            else
            {
                logger.WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);
            }

            return logger.CreateLogger();
        }
    }

}

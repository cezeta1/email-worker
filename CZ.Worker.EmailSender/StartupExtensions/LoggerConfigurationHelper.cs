using CZ.Worker.EmailSender.Domain.Settings;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace CZ.Worker.EmailSender.StartupHelpers;

public static class LoggerConfigurationHelper
{
    public static LoggerConfiguration GetLoggerConfig(IHostApplicationBuilder context, LoggerConfiguration loggerConfig)
    {
        var settings = context.Configuration.GetFromSection<SerilogSettings>();
        var azureConnStr = context.Configuration.GetConnectionString("Storage");
        return loggerConfig
            .Destructure.UsingAttributes()
            // Log level
            .MinimumLevel.Is(
                Enum.Parse<LogEventLevel>(settings?.MinimumLevel ?? "Verbose")
            )
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            // Context variables
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Version", "2.0.0")
            // Sinks
            .WriteTo.Console(
                restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(settings?.Console?.RestrictedToMinimumLevel ?? string.Empty),
                outputTemplate: settings?.OutputTemplate ?? string.Empty,
                theme: AnsiConsoleTheme.Code)
            .WriteTo.AzureBlobStorage(
                connectionString: azureConnStr,
                outputTemplate: settings?.OutputTemplate,
                storageContainerName: settings?.AzureBlob?.StorageContainerName,
                storageFileName: settings?.AzureBlob?.StorageFileName,
                // writeInBatches: settings?.AzureBlob?.WriteInBatches ?? false,
                period: TimeSpan.FromSeconds(settings?.AzureBlob?.PeriodSeconds ?? 0),
                batchPostingLimit: settings?.AzureBlob?.BatchPostingLimit
            );
        //.WriteTo.File(
        //    path: settings?.File?.Path,
        //    restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(settings?.File?.RestrictedToMinimumLevel ?? "Verbose")
        //);
    }
}

using Azure.Messaging.ServiceBus;
using CZ.Worker.EmailSender.Domain.Settings;
using CZ.Worker.EmailSender.Providers;
using CZ.Worker.EmailSender.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CZ.Worker.EmailSender;

public abstract class BaseMessengerWorker : BackgroundService
{
    protected readonly Guid _workerId = Guid.NewGuid();
    protected readonly ILogsService _logsService;
    private readonly int _latencyMiliseconds = 5000;

    private readonly AzureServiceBusProvider _azureServiceBusProvider;

    public BaseMessengerWorker(
        ILogsService logsService,
        AzureServiceBusProviderSettings settings)
    {
        _logsService = logsService;
        _logsService.WorkerId = _workerId;

        _azureServiceBusProvider = new(settings);
    }

    // --- Worker Methods --- //

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logsService.LogInformation(null, "Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(_latencyMiliseconds, stoppingToken);
        }
    }

    // --- Azure Service Bus Methods --- //

    protected ServiceBusProcessor GetMessageProcessor()
        => _azureServiceBusProvider.GetProcessor(MessageHandler, ErrorHandler);

    // --- Default Implementations --- //

    protected abstract Task MessageHandler(ProcessMessageEventArgs args);

    protected abstract Task ErrorHandler(ProcessErrorEventArgs args);
}

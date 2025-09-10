using Azure.Messaging.ServiceBus;
using CZ.Worker.EmailSender.Domain.Settings;

namespace CZ.Worker.EmailSender.Providers;

public class AzureServiceBusProvider(
        AzureServiceBusProviderSettings _settings)
    : IAsyncDisposable
{
    private readonly ServiceBusClient _serviceBusClient = new(
        _settings.AzureConnString,
        new ServiceBusClientOptions {
            TransportType = ServiceBusTransportType.AmqpWebSockets 
        });

    // --- Processor (receiver) --- //

    public ServiceBusProcessor GetProcessor(
        Func<ProcessMessageEventArgs, Task> msgHandler = null,
        Func<ProcessErrorEventArgs, Task> errorHandler = null)
    {
        var processor = _serviceBusClient.CreateProcessor(
            _settings.GetEnvironmentTopicName(),
            _settings.SubscriptionName,
            new ServiceBusProcessorOptions());

        // add handler to process messages
        if (msgHandler != null)
            processor.ProcessMessageAsync += msgHandler;

        // add a handler to process any errors
        if (errorHandler != null)
            processor.ProcessErrorAsync += errorHandler;

        return processor;
    }

    // --- Dispose --- //

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _serviceBusClient.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

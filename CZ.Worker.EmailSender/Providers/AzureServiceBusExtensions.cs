using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace CZ.Worker.EmailSender.Providers;

public static class AzureServiceBusSenderExtensions
{
    public static readonly JsonSerializerOptions _jsonSettings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // --- ServiceBusSender --- //

    public static Task Send<T>(this ServiceBusSender sender, T message)
        => sender.SendBatch([message]);

    public static async Task SendBatch<T>(this ServiceBusSender sender, List<T> msgList)
    {
        if (msgList == null)
            return;

        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        for (int i = 0; i < msgList.Count; i++)
        {
            var strMsg = JsonSerializer.Serialize(msgList[i], _jsonSettings);
            // try adding a message to the batch
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(strMsg)))
            {
                // if it is too large for the batch
                throw new Exception($"The message {i} is too large to fit in the batch.");
            }
        }

        // Use the producer client to send the batch of messages to the Service Bus queue
        await sender.SendMessagesAsync(messageBatch);
    }

    // --- ServiceBusProcessor --- //

    public static bool TryDeserializeBody<Tout>(this ServiceBusReceivedMessage message, out Tout outObj)
    {
        outObj = default;
        try
        {
            outObj = JsonSerializer.Deserialize<Tout>(message.Body.ToString(), _jsonSettings);
            return true;
        }
        catch (Exception) { return false; }
    }
}

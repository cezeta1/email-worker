namespace CZ.Worker.EmailSender.Domain.Settings;

public class AzureServiceBusProviderSettings
{
    public string AzureConnString { get; set; } = string.Empty;
    
    public string TopicName { get; set; } = string.Empty;
    public string SubscriptionName { get; set; } = string.Empty;
    public string DevPrefix { get; set; } = string.Empty;

    // --- Utils --- //

    public string GetEnvironmentTopicName()
        => $"{(string.IsNullOrEmpty(DevPrefix) ? "" : $"{DevPrefix}.")}{TopicName}";
}

using Newtonsoft.Json;

namespace CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

public class QueueEmailMessage<T>
{
    [JsonProperty("traceId")]
    public string TraceId { get; set; } = "";
    [JsonProperty("token")]
    public string Token { get; set; } = "";

    public QueueEmailTemplateInfo TemplateInfo { get; set; }
    [JsonProperty("metadata")]
    public QueueEmailMetadata Metadata { get; set; }
    [JsonProperty("content")]
    public QueueEmailContent<T> Content { get; set; }
}

public class QueueEmailMessage : QueueEmailMessage<dynamic> { }

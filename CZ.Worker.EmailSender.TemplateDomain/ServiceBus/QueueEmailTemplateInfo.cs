namespace CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

public class QueueEmailTemplateInfo
{
    public EmailTemplateTypesEnum Type { get; set; }
    public string Language { get; set; }
    public string Culture { get; set; }
}

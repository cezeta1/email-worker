using SendGrid.Helpers.Mail;

namespace CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

public class QueueEmailMetadata
{
    public string Subject { get; set; }

    public List<string> To { get; set; } = [];
    public List<string> Bcc { get; set; } = [];
    public List<string> Cc { get; set; } = [];

    public List<Attachment> Attachments { get; set; } = [];
}

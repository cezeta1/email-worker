using SendGrid.Helpers.Mail;

namespace CZ.Worker.EmailSender.EmailEngine.Domain;

public class SendEmailPayload
{
    public string Subject { get; set; }
    public string Body { get; set; }

    public List<string> To { get; set; } = [];
    public List<string> Bcc { get; set; } = [];
    public List<string> Cc { get; set; } = [];

    public List<Attachment> Attachments { get; set; } = [];
}

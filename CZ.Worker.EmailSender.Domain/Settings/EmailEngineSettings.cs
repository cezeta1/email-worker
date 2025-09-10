namespace CZ.Worker.EmailSender.Domain.Settings;

public class EmailEngineSettings
{
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string SendGridAPIKey { get; set; }
}

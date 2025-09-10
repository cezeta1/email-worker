using CZ.Worker.EmailSender.Domain.Exceptions;
using CZ.Worker.EmailSender.Domain.Settings;
using CZ.Worker.EmailSender.EmailEngine.Domain;
using CZ.Worker.EmailSender.EmailEngine.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CZ.Worker.EmailSender.EmailEngine;

public class EmailEngine(EmailEngineSettings _settings) : IEmailEngine
{
    public async Task Send(SendEmailPayload p)
    {
        if (string.IsNullOrEmpty(p.Body))
            throw new EmailEngineException("Body can't be empty.");

        if (p.To.Count == 0)
            throw new EmailEngineException("To can't be empty.");

        if (string.IsNullOrEmpty(p.Subject))
            throw new EmailEngineException("Subject can't be empty.");

        var email = BuildMessage(p);

        SendGridClient smtpClient = new(_settings.SendGridAPIKey);
        await smtpClient.SendEmailAsync(email);
    }

    // --- Private Methods --- //

    private SendGridMessage BuildMessage(SendEmailPayload p)
    {
        SendGridMessage e = new();

        // Addresses 
        e.SetFrom(
            _settings.SenderEmail, 
            _settings.SenderName
        );

        p.To.ForEach(x => { if (!string.IsNullOrEmpty(x)) e.AddTo(x); });
        p.Bcc.ForEach(x => { if (!string.IsNullOrEmpty(x)) e.AddBcc(x); });
        p.Cc.ForEach(x => { if (!string.IsNullOrEmpty(x)) e.AddCc(x); });

        // Attachments
        p.Attachments.ForEach(e.AddAttachment);

        // Content

        e.SetGlobalSubject(p.Subject);
        e.AddContent(MimeType.Html, p.Body);

        return e;
    }
}

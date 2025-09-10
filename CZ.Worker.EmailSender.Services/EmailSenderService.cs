using CZ.Worker.EmailSender.EmailEngine.Interfaces;
using CZ.Worker.EmailSender.Services.Interfaces;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;
using CZ.Worker.EmailSender.TemplateResolver.Interfaces;
using Microsoft.Extensions.Logging;

namespace CZ.Worker.EmailSender.Services;

public class EmailSenderService(
        ILogger<EmailSenderService> _logger,
        ITemplateResolver _templateResolver,
        IEmailEngine _emailEngine)
    : IEmailSenderService
{
    public async Task SendEmail(QueueEmailMessage p)
    {
        var parsedEmail = await _templateResolver.GetParsedEmail(p);

        await _emailEngine.Send(new()
        {
            Subject = parsedEmail.Metadata.Subject,
            Body = parsedEmail.Body,
            To = p.Metadata.To
        });
    }
}
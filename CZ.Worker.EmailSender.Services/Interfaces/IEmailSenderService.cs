using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

namespace CZ.Worker.EmailSender.Services.Interfaces;

public interface IEmailSenderService
{
    Task SendEmail(QueueEmailMessage payload);
}


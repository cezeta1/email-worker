using CZ.Worker.EmailSender.EmailEngine.Domain;

namespace CZ.Worker.EmailSender.EmailEngine.Interfaces;

public interface IEmailEngine
{
    Task Send(SendEmailPayload p);
}

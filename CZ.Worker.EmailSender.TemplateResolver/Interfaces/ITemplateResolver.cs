using CZ.Worker.EmailSender.Domain.TemplateResolver;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

namespace CZ.Worker.EmailSender.TemplateResolver.Interfaces;

public interface ITemplateResolver
{
    Task<RenderedTemplate> GetParsedEmail(QueueEmailMessage payload);
}

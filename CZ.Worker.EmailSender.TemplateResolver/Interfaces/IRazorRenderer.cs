using CZ.Worker.EmailSender.Domain.TemplateResolver;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

namespace CZ.Worker.EmailSender.TemplateResolver;

public interface IRazorRenderer
{
    Task<RenderedTemplate> RenderTemplate(string templatePath, QueueEmailMessage model);
}

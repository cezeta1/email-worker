using CZ.Worker.EmailSender.Domain.TemplateResolver;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;
using CZ.Worker.EmailSender.TemplateResolver.Helpers;
using Jering.Javascript.NodeJS;
using Razor.Templating.Core;

namespace CZ.Worker.EmailSender.TemplateResolver;

public class RazorRenderer(INodeJSService nodeService) : IRazorRenderer
{
    public async Task<RenderedTemplate> RenderTemplate(string templateName, QueueEmailMessage model)
        => new()
        {
            Metadata = TemplateCollection.ReadTemplate(templateName),
            Body = await (
                await RazorTemplateEngine
                    .RenderAsync(
                        "~/Templates/Layout/_Layout.cshtml",
                        model,
                        new() { { "Content", $"~/Templates/{templateName}.cshtml" } }))
                .CleanFrontMatter()
                .PostProcess(nodeService)
        };
}

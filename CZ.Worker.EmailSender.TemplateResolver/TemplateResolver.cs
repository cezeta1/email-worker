using CZ.Worker.EmailSender.Domain.TemplateResolver;
using CZ.Worker.EmailSender.TemplateDomain;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;
using CZ.Worker.EmailSender.TemplateResolver.Helpers;
using CZ.Worker.EmailSender.TemplateResolver.Interfaces;

namespace CZ.Worker.EmailSender.TemplateResolver;

public class TemplateResolver(IRazorRenderer _renderer): ITemplateResolver
{
    private static readonly QueueEmailTemplateInfo defaultInfo = new()
    {
        Type = EmailTemplateTypesEnum.Default,
        Language = "en",
        Culture = "au",
    };

    public Task<RenderedTemplate> GetParsedEmail(QueueEmailMessage payload)
        => _renderer.RenderTemplate(GetTemplateName(payload.TemplateInfo), payload);

    // --- Private Methods --- //

    private static string GetTemplateName(QueueEmailTemplateInfo info)
    {
        var templateNames = TemplateCollection.GetAllTemplateNames();

        // --- Choose template --- //

        // Type 

        var filterByType = templateNames
            .Where(tn => tn.Contains(info.Type.ToString()));

        if (!filterByType.Any())
            throw new Exception($"Couldn't find any template for type: {info.Type}.");

        // Language 

        var filterByLang = filterByType
            .Where(tn => tn.Contains(info.Language));

        if (!filterByLang.Any())
        {
            // Couldn't find the desired language. Use default
            filterByLang = filterByType
                .Where(tn => tn.Contains(defaultInfo.Language));

            if (!filterByLang.Any())
                throw new Exception($"Couldn't find any template for type: {info.Type} and lang: {info.Language}.");
        }

        // Culture 

        string selTemplate = filterByLang
            .FirstOrDefault(tn => tn.Contains(info.Culture));

        if (!string.IsNullOrEmpty(selTemplate)) 
            return selTemplate;
        
        // Couldn't find desired culture. Use default
        selTemplate = filterByLang
            .OrderBy(tn => tn.Length)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(selTemplate))
            throw new Exception($"Couldn't find any template for type: {info.Type}, lang: {info.Language} and culture: {info.Culture}.");

        return selTemplate;
    }
}

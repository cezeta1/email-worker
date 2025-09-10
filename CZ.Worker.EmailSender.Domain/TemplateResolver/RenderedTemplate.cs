namespace CZ.Worker.EmailSender.Domain.TemplateResolver;

public class RenderedTemplate
{
    public EmailFrontMatter Metadata { get; set; }
    public string Body { get; set; }
}

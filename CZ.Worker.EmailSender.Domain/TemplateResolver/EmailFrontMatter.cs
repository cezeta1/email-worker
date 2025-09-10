using YamlDotNet.Serialization;

namespace CZ.Worker.EmailSender.Domain.TemplateResolver;

public class EmailFrontMatter
{
    [YamlMember(Alias = "subject")]
    public string Subject { get; set; }
}

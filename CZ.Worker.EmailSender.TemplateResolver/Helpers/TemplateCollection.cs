using System.Reflection;
using CZ.Worker.EmailSender.Domain.TemplateResolver;

namespace CZ.Worker.EmailSender.TemplateResolver.Helpers;

public static class TemplateCollection
{
    public static List<string> GetAllTemplateNames()
        => _FindAllTemplateNames();

    public static EmailFrontMatter ReadTemplate(string tName)
        => _ReadTemplate(tName)
            .GetFrontMatter<EmailFrontMatter>();

    // --- Private Methods --- //

    private static Assembly Assembly
        => Assembly.Load("CZ.Worker.EmailSender.Templates");

    private static List<string> _FindAllTemplateNames()
        => [..
            Assembly
            .GetManifestResourceNames()
            .Select(rn => rn
                .Split('/')
                .LastOrDefault()
                ?.Replace("CZ.Worker.EmailSender.Templates.Templates.","")
                .Replace(".cshtml","")
            )
        ];

    private static string _FindTemplateResource(string name)
        => Assembly.GetManifestResourceNames().FirstOrDefault(rn => rn.Contains($"{name}.cshtml"));

    private static string _ReadTemplate(string templateName)
    {
        var resName = _FindTemplateResource(templateName);
        using Stream stream = Assembly.GetManifestResourceStream(resName);
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
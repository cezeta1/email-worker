using System.Text.Json.Serialization;

namespace CZ.Worker.EmailSender.TemplateDomain.TemplateDataPayloads;

public class ProfileData
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
}

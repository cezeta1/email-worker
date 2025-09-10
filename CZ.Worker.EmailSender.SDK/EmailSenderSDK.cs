using CZ.Worker.EmailSender.TemplateDomain;
using CZ.Worker.EmailSender.TemplateDomain.TemplateDataPayloads;

namespace CZ.Worker.EmailSender.SDK;

public static class EmailSenderSDK
{
    public static readonly Dictionary<EmailTemplateTypesEnum, Type> DataMap = new()
    {
       { EmailTemplateTypesEnum.AccountSignup, typeof(ProfileData) },
       { EmailTemplateTypesEnum.AccountActivated, typeof(ProfileData) },
       { EmailTemplateTypesEnum.AccountResetPassword, typeof(ProfileData) },
    };
}

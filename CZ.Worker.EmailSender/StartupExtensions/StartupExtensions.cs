using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CZ.Worker.EmailSender.StartupHelpers;

public static class StartupExtensions
{
    public static IServiceCollection AddConfiguration<T>(
            this IServiceCollection services, 
            ConfigurationManager config, 
            string sectionName)
        where T : class, new()
        => services.AddSingleton(config.GetFromSection<T>(sectionName) ?? new());

    public static T GetFromSection<T>(
            this IConfiguration configuration, 
            string sectionName = "") 
        where T : new()
    {
        if (string.IsNullOrEmpty(sectionName))
            sectionName = typeof(T).Name;

        return configuration.GetSection(sectionName).Get<T>()
            ?? throw new ApplicationException($"Unable to bind configuration for section {sectionName} to type {typeof(T)}");
    }
}

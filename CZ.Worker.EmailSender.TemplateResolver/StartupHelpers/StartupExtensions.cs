using CZ.Worker.EmailSender.TemplateResolver.Interfaces;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;

namespace CZ.Worker.EmailSender.TemplateResolver.StartupHelpers;

public static class StartupExtensions
{
    public static IServiceCollection AddTemplateResolverServices(this IServiceCollection services)
    {
        services
            .AddTransient<ITemplateResolver, TemplateResolver>()
            .AddTransient<IRazorRenderer, RazorRenderer>()
            .AddNodeJS();

        // ProjectPath set by Dockerfile
        services.Configure<NodeJSProcessOptions>(options => options.ProjectPath = "/node-runtime");
        return services;
    }
}

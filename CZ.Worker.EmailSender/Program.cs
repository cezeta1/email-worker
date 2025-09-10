using CZ.Worker.EmailSender;
using CZ.Worker.EmailSender.Domain.Settings;
using CZ.Worker.EmailSender.EmailEngine;
using CZ.Worker.EmailSender.EmailEngine.Interfaces;
using CZ.Worker.EmailSender.Services;
using CZ.Worker.EmailSender.Services.Auth;
using CZ.Worker.EmailSender.Services.Interfaces;
using CZ.Worker.EmailSender.Services.Logs;
using CZ.Worker.EmailSender.StartupHelpers;
using CZ.Worker.EmailSender.TemplateResolver.StartupHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddConfiguration<EmailEngineSettings>(builder.Configuration, "EmailEngineSettings")
    .AddConfiguration<EmailSenderSettings>(builder.Configuration, "EmailSenderSettings");

builder.Services
    .AddAuthenticationCore(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddAuthorizationCore(opts =>
    {
        opts.AddPolicy(
            AuthPolicies.EmailSenderWorkerPolicy, 
            policy => policy.RequireScope("Worker.EmailSender.Send"));
    })
    .AddSingleton<IAuthService, AuthService>()
    .AddTransient<IEmailEngine, EmailEngine>()
    .AddTransient<IEmailSenderService, EmailSenderService>()
    .AddTemplateResolverServices();

// ================ //

builder.Services
    .AddTransient<ILogsService, LogsService>();

var logger = LoggerConfigurationHelper
    .GetLoggerConfig(builder, new())
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// ================ //

// --- Worker --- //

builder.Services.AddHostedService<EmailSenderWorker>();
var host = builder.Build();
host.Run();
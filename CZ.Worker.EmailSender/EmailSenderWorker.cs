using Azure.Messaging.ServiceBus;
using CZ.Worker.EmailSender.Domain.Settings;
using CZ.Worker.EmailSender.Providers;
using CZ.Worker.EmailSender.Services.Interfaces;
using CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

namespace CZ.Worker.EmailSender;

public class EmailSenderWorker : BaseMessengerWorker
{
    private readonly IAuthService _authService;
    private readonly IEmailSenderService _emailSenderService;

    public EmailSenderWorker(
            ILogsService logsService,
            IAuthService authService,
            IEmailSenderService emailSenderService,
            EmailSenderSettings settings)
        : base(logsService, settings)
    {
        _authService = authService;
        _emailSenderService = emailSenderService;

        var processor = GetMessageProcessor();
        processor.StartProcessingAsync().Wait();
    }

    protected override async Task MessageHandler(ProcessMessageEventArgs args)
    {
        if (!args.Message.TryDeserializeBody(out QueueEmailMessage outObj))
        {
            // Message is not one this EmailService needs to process. 

            // Throw Exception to avoid Azure marking the message as processed
            //      and allow other workers in the same subscription to handle it.

            throw new Exception("Can't process message.");
        }

        // --- Authorization --- //

        if (!await _authService.TryAuthorize(outObj.Token))
            throw new Exception("Invalid token or unauthorized.");

        await _emailSenderService.SendEmail(outObj);
    }

    protected override Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logsService.LogError(
            args.Exception,
            null,
            $"Email sender worker {_workerId} error!\r\n" +
            $"Error msg: {args.Exception.Message}.\r\n" +
            $"Time: {DateTimeOffset.Now}");
        throw args.Exception;
    }
}

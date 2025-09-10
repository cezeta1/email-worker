using CZ.Worker.EmailSender.Services.Interfaces;
using Serilog.Context;
using Serilog.Events;
using SeriLog = Serilog.Log;

namespace CZ.Worker.EmailSender.Services.Logs;

public class LogsService : ILogsService
{
    public Guid WorkerId { get; set; } = Guid.Empty;

    private readonly LogEventLevel _minLogLevel = LogEventLevel.Debug;

    public void LogInformation(Guid? traceId, string msgTemplate, params object[] paramValues)
    {
        SetSerilogMetadata(traceId);
        SeriLog.Information(msgTemplate, paramValues);
    }

    public void LogError(Exception ex, Guid? traceId, string msgTemplate, params object[] paramValues)
    {
        SetSerilogMetadata(traceId);
        SeriLog.Error(ex, msgTemplate, paramValues);
    }

    // --- Private Methods --- //

    private void SetSerilogMetadata(Guid? traceId)
    {
        LogContext.PushProperty("TraceId",
             traceId == null || traceId == Guid.Empty
                ? "No PAPI Trace Id"
                : traceId);

        if (!WorkerId.Equals(Guid.Empty))
            LogContext.PushProperty("WorkerId", WorkerId);
    }
}

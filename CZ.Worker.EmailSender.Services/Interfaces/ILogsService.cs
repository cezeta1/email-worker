namespace CZ.Worker.EmailSender.Services.Interfaces;

public interface ILogsService
{
    Guid WorkerId { get; set; }

    void LogInformation(Guid? traceId, string msg, params object[] paramValues);
    void LogError(Exception ex, Guid? traceId, string msg, params object[] paramValues);
}

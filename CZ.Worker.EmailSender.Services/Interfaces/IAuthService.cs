namespace CZ.Worker.EmailSender.Services.Interfaces;

public interface IAuthService
{
    Task<bool> TryAuthorize(string token);
}

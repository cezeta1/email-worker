using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CZ.Worker.EmailSender.Domain.Settings;
using CZ.Worker.EmailSender.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CZ.Worker.EmailSender.Services.Auth;

public class AuthService(IAuthorizationService _authorization) : IAuthService
{
    private static readonly TokenValidationParameters _validationParameters = new()
    {
        ValidIssuer = "http://localhost:43003",
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        SignatureValidator = (token, _) => new JwtSecurityToken(token)
    };

    public async Task<bool> TryAuthorize(string token)
    {
        try
        {
            var claims = ValidateToken(token);
            var result = await _authorization.AuthorizeAsync(claims, AuthPolicies.EmailSenderWorkerPolicy);
            return (bool)result?.Succeeded;
        }
        catch (Exception) {
            return false; 
        }
    }

    // --- Private Methods --- //

    private static ClaimsPrincipal ValidateToken(string token)
        => new JwtSecurityTokenHandler()
            .ValidateToken(token, _validationParameters, out _);
}
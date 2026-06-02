using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using student_log_api.Common;
using student_log_api.Models;

namespace student_log_api.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserTokenData user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly AppSettings _appSettings;

        public JwtTokenService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateToken(UserTokenData user)
        {
            try
            {
                if (string.IsNullOrEmpty(_appSettings.JwtSecret))
                    throw new InvalidOperationException("JWT Secret is not configured");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.UserName),
                    new Claim("LoginUserID", user.UserID.ToString()),
                    new Claim("AccountID", user.AccountID),
                    new Claim("AccountCode", user.AccountCode),
                    new Claim("SchoolIDs", user.SchoolIDs),
                    new Claim("SchoolNames", user.SchoolNames)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtExpirationMinutes),
                    Issuer = _appSettings.JwtIssuer,
                    Audience = _appSettings.JwtAudience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception($"Error generating JWT token: {ex.Message}", ex);
            }
        }
    }
}

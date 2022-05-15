using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GemCare.API.Interfaces;
using GemCare.API.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GemCare.API.Common
{
    public class TokenGenerator : ITokenGenerator
    {
        private const string JWT_SECTION = "JWTSettings";
        private readonly IConfiguration _configuration;
        private readonly IEncryptionDecryptionHelper _encHelper;
        //private readonly string jwtExpiry;
        public TokenGenerator(IConfiguration configuration, IEncryptionDecryptionHelper encHelper)
        {
            _configuration = configuration;
            _encHelper = encHelper;
        }
        public string GenerateAppUserJWT(int appUserId, string role, bool isRefreshToken = false)
        {
            try
            {
                DateTime dtTokenExpiry = !isRefreshToken ?
                    DateTime.Now.AddDays(int.Parse(GetJwtSettingValue("Expires"))) :
                    DateTime.Now.AddDays(int.Parse(GetJwtSettingValue("RefreshTokenExpiry")));
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, appUserId.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };
                var key = Encoding.UTF8.GetBytes(GetJwtSettingValue("SecretKey"));
                var jwtToken = new JwtSecurityToken(
                    issuer: GetJwtSettingValue("Issuer"),
                    audience: GetJwtSettingValue("Audience"),
                    claims,
                    expires: dtTokenExpiry,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                if (!isRefreshToken)
                {
                    return token;
                }
                else
                {
                    byte[] byteToken = Encoding.UTF8.GetBytes(token);
                    return Convert.ToBase64String(byteToken);
                }
            }
            catch { return string.Empty; }
        }

        public (bool isValid, bool isExpired, string message) ValidateToken(string userToken)
        {
            bool isValidToken = true, isExpiredToken = false;
            string retMessage = "Invalid auth token";
            try
            {
                if (string.IsNullOrEmpty(userToken))
                    return (!isValidToken, isExpiredToken, retMessage);
                // now check jwt
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!(tokenHandler.ReadToken(userToken) is JwtSecurityToken))
                    return (!isValidToken, isExpiredToken, retMessage);
                // validate token
                var validationParameters = GetValidationParameters();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(userToken, validationParameters, out SecurityToken securityToken);
                retMessage = "valid";
            }
            catch (SecurityTokenExpiredException)
            {
                isExpiredToken = true;
                retMessage = "Expired token";
            }
            catch (Exception)
            {
                isValidToken = false;
            }
            return (isValidToken, isExpiredToken, retMessage);
        }

        public string GenerateNewToken(string authToken, string refreshToken)
        {
            try
            {
                var (cPrincipal, sToken) = DecodeJwtToken(authToken);
                if (sToken == null || !sToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                {
                    throw new SecurityTokenException("Invalid token provided");
                }
                //
                int userId = _encHelper.GetUserId(refreshToken);
                var identity = cPrincipal.Identity as ClaimsIdentity;
                if (userId != int.Parse(identity.FindFirst(ClaimTypes.Name)?.Value))
                {
                    throw new SecurityTokenException("Invalid token provided");
                }
                return GenerateAppUserJWT(userId, identity.FindFirst(ClaimTypes.Role)?.Value);
            }
            catch { throw; }
            //return string.Empty;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            var symmetricKey = Encoding.UTF8.GetBytes(GetJwtSettingValue("SecretKey"));
            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidIssuer = GetJwtSettingValue("Issuer"),
                ValidateAudience = true,
                ValidAudience = GetJwtSettingValue("Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                ValidateLifetime = true
            };
            return validationParameters;
        }

        private (ClaimsPrincipal cPrincipal, JwtSecurityToken sToken) DecodeJwtToken(string token, bool isRefreshToken = false)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token provided");
            }
            // refresh token is base64 encoded.
            if (isRefreshToken)
            {
                byte[] byteArray = Convert.FromBase64String(token);
                token = Encoding.UTF8.GetString(byteArray);
            }
            var _secretKey = Encoding.UTF8.GetBytes(GetJwtSettingValue("SecretKey"));
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = GetJwtSettingValue("Issuer"),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                        ValidateAudience = true,
                        ValidAudience = GetJwtSettingValue("Audience"),
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        private string GetJwtSettingValue(string key)
        {
            try
            {
                return _configuration.GetSection(JWT_SECTION).GetChildren().FirstOrDefault(x => x.Key == key).Value;
            }
            catch
            {
                return default;
            }
        }

        public int GetUserId(string authToken)
        {
            try
            {

                //authToken = DecodeBase64String(authToken);
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(authToken) as JwtSecurityToken;
                var validationParameters = GetValidationParameters();
                var principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken securityToken);
                var identity = principal.Identity as ClaimsIdentity;
                return int.Parse(identity.FindFirst(ClaimTypes.Name)?.Value);
            }
            catch
            {
                throw;
            }
            //return customerModel;
        }

        private static string DecodeBase64String(string encodedToken)
        {
            if (string.IsNullOrEmpty(encodedToken) || string.IsNullOrWhiteSpace(encodedToken))
            {
                return string.Empty;
            }

            try
            {
                byte[] byteArray = Convert.FromBase64String(encodedToken);
                return Encoding.UTF8.GetString(byteArray);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

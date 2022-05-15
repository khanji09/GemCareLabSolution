using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateAppUserJWT(int appUserId, string role, bool isRefreshToken = false);
        (bool isValid, bool isExpired, string message) ValidateToken(string userToken);
        string GenerateNewToken(string authToken, string refreshToken);
        int GetUserId(string authToken);
    }
}

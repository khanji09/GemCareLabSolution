using GemCare.Data.DTOs;

namespace GemCare.Data.Interfaces
{
    public interface IAuthenticate
    {
        (int status, string message) CustomerRegistration(CustomerBasicInfo basicInfo);
        (int status, string message, AppUser user) CustomerLogin(string email, string password);
        public (int status, string message) VerifyEmailLoginCode(EmailLoginCodeDTO model);
        (int status, string message) SignOut(int userId, string deviceId);
        (int status, string message, AppUser user) AdminLogin(string email, string password);
    }
}

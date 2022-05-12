using GemCare.Data.DTOs;

namespace GemCare.Data.Interfaces
{
    public interface IAuthenticate
    {
        (int status, string message) CustomerRegistration(CustomerBasicInfo basicInfo);
        (int status, string message, AppUser user) CustomerLogin(string email, string password);
        (int status, string message) SignOut(int userId, string deviceId);
    }
}

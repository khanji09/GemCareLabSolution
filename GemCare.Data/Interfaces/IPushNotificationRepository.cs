using GemCare.Data.DTOs;

namespace GemCare.Data.Interfaces
{
    public interface IPushNotificationRepository
    {
        (int status, string message) SavePushToken(int userId, string pushToken, string deviceId, string devicePlatform);
        (int status, string message, PushDTO deviceInfo) GetAdminDeviceInfoForBookingNotification(int bookingid);
        (int status, string message) UpdateBookingNotificationStatus();
    }
}

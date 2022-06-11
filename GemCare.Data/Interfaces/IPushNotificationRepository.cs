using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IPushNotificationRepository
    {
        (int status, string message) SavePushToken(int userId, string pushToken, string deviceId, string devicePlatform);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Interfaces
{
    public interface IPushService
    {
        (bool notificationSent, string errMessage) SendBookingNotificationToAdmin(string title, string messagebody, string pushToken, string deviceType);
    }
}

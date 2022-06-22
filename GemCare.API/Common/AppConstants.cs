using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemCare.API.Common
{
    public class AppConstants
    {
        public static string APIKEY_ERRMESSAGE = "Invalid apikey request";
        public static string BEARER_ERRMESSAGE = "Invalid bearer request";
        public const string SUPPORT_EMAIL = "support@trots.education";
        public const string SUCCESS_MESSAGE = "SUCCESS";
        public const string PASSWORD_COMPLEXITY_TEXT = "Password must contain one upper case letter, one lower case letter, one digit, one special character, minimum 8 and max 16 characters in length.";
        public const string FCM_LINK = "https://fcm.googleapis.com/fcm/send";
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.API.Services
{
    public class UserCompliance
    {
        private UserCompliance()
        {
        }

        public static string GetUserAgreement()
        {
            string fileName = $"Uploads/Agreements/user_agreement.html";
            var builder = new StringBuilder();
            //
            if (File.Exists(fileName))
            {
                using (var reader = File.OpenText(fileName))
                {
                    builder.Append(reader.ReadToEnd());
                }
                //string content = File.ReadAllText(fileName);
                return builder.ToString();
            }
            return string.Empty;
        }
    }
}

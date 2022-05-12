using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Common
{
    public class DataConstants
    {
        private const int cONNECTION_TIMEOUT = 240;
        public static int CONNECTION_TIMEOUT => cONNECTION_TIMEOUT;
        public static int ERRMESSAGE_LENGTH = 300;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveSubHelper
{
    public static class Base64URLEncodeDecodeHelper
    {
        public static String Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')        // remove padding
                .Replace('+', '-')   // 62nd char
                .Replace('/', '_');  // 63rd char
        }

        public static Byte[] Decode(string base64Url)
        {
            string padded = base64Url
                .Replace('-', '+')
                .Replace('_', '/');

            // add padding if necessary
            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            return Convert.FromBase64String(padded);
        }
    }
}

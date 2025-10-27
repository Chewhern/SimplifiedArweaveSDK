using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveSubHelper
{
    public static class GetProperHexaStringForCSharpHelper
    {
        public static String FormatHexString(string input)
        {
            String FormattedHexString = "";
            int Loop = 0;
            while (Loop < input.Length)
            {
                if (Loop + 4 > input.Length)
                {
                    FormattedHexString += "0x" + input.Substring(Loop, 2);
                }
                else
                {
                    FormattedHexString += "0x" + input.Substring(Loop, 2) + ",";
                }
                Loop += 2;
            }
            return FormattedHexString;
        }
    }
}

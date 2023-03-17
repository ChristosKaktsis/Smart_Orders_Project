using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Constants
{
    public static class SSCC
    {
        public static string GetSSCC(string input,int start,int end) {
            if(end <= 0) return input.Substring(start);
            return input.Substring(start,end);
        }
    }
}

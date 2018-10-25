using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Services
{
    public class CRMUtilityServices
    {
        public static string GetTitleCaseService(string inString)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(inString.ToLower());
        }
    }
}
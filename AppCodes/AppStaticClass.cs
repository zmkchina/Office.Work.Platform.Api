using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.AppCodes
{
    public static class AppStaticClass
    {
        public static string GetIdOfDateTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.GetCultureInfo("zh-CN").DateTimeFormat);
        }
    }
}

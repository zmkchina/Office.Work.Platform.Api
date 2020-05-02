using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.AppCodes
{
    public static class AppStaticClass
    {
        private static int Index = 0;
        public static string GetIdOfDateTime()
        {
            Index = Index > 9 ? 0 : Index++;
            string TimeStr = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.GetCultureInfo("zh-CN").DateTimeFormat);
            return $"{TimeStr}{Index}";
        }
    }
}

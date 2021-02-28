using System;
using System.IO;

namespace Haro.AdminPanel.Utilities.Media
{
    public static class DirectoryHelper
    {
        public static string GenerateTodayMediaDirectory()
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            string str1 = now.Month.ToString("00");
            string str2 = now.Day.ToString("00");
            string str3 = Directory.GetCurrentDirectory() + "/wwwroot";
            string str4 = string.Format("/Media/{0}/{1}/{2}/", year, str1, str2);
            if (Directory.Exists(str3 + str4))
                return str4;
            Directory.CreateDirectory(str3 + str4);
            return str4;
        }
    }
}
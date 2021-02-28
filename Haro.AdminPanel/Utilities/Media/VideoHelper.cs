using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haro.AdminPanel.Utilities.Media;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Utilities.Media
{
    public class VideoHelper
    {
        public static string SaveVideo(IFormFile file, string slug)
        {
            string str1 = file.FileName.Split('.').Last();
            string str2 = DirectoryHelper.GenerateTodayMediaDirectory() +
                          (slug.GenerateSlug() + "-" + UniqueIdentifier.Generate() + "." + str1);
            using (FileStream fileStream = File.Create(Directory.GetCurrentDirectory() + "\\wwwroot" + str2))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            return str2;
        }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Utilities.Media
{
  public class FileHelper
  {
    public static string SaveFile(IFormFile file, string slug = "file")
    {
      string str1 = ((IEnumerable<string>) file.FileName.Split('.', StringSplitOptions.None)).Last<string>();
      string str2 = DirectoryHelper.GenerateTodayMediaDirectory() + (slug.GenerateSlug() + "-" + UniqueIdentifier.Generate() + "." + str1);
      using (FileStream fileStream = File.Create(Directory.GetCurrentDirectory() + "\\wwwroot" + str2))
      {
        file.CopyTo((Stream) fileStream);
        ((Stream) fileStream).Flush();
      }
      return str2;
    }
  }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Utilities.Media
{
    public static class ImageHelper
    {
        public static string SaveImage(IFormFile file, string slug)
        {
            Media media = new Media();
            media.Extension = file.FileName.Split('.').Last<string>();
            media.FileName = slug.GenerateSlug() + "-" + UniqueIdentifier.GenerateStr();
            media.Path = DirectoryHelper.GenerateTodayMediaDirectory();
            using (FileStream fileStream = File.Create(Directory.GetCurrentDirectory() + "/wwwroot" + media.FullPath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            // ImageHelper.Compress(media.FullPath, new int?(), new int?(), "");
            return media.FullPath;
        }

        public static string SaveImage(string base64, string slug, int? x = null, int? y = null)
        {
            Media media = new Media();
            media.Extension = "jpg";
            media.FileName = slug + "-" + UniqueIdentifier.Generate();
            media.Path = DirectoryHelper.GenerateTodayMediaDirectory();

            byte[] imageBytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "/wwwroot" + media.FullPath, imageBytes);

            // ImageHelper.Compress(media.FullPath, x, y);
            return media.FullPath;
        }

        // private static void Compress(string file, int? x, int? y, string preName = "")
        // {
        //     string str = Directory.GetCurrentDirectory() + "/wwwroot";
        //     MagickImage magickImage = new MagickImage(str + file);
        //     try
        //     {
        //         if (x.HasValue && y.HasValue)
        //         {
        //             MagickGeometry geometry = new MagickGeometry(x.Value, y.Value);
        //             magickImage.Resize(geometry);
        //             geometry.IgnoreAspectRatio = false;
        //         }
        //
        //         magickImage.Quality = 70;
        //         magickImage.Write(str + file);
        //     }
        //     finally
        //     {
        //         if ((object) magickImage != null)
        //             magickImage.Dispose();
        //     }
        // }
    }
}
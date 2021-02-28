// Decompiled with JetBrains decompiler
// Type: Haro.Core.Utilities.Media.MediaManager
// Assembly: Haro.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DAF84DE-5A63-4919-BDD1-6AA72BC35296
// Assembly location: C:\Users\harun\Downloads\aracTavsiye\aracTavsiye\Haro.Core.dll

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Haro.AdminPanel.Utilities.Media
{
    public static class MediaManager
    {
        private static string[] _imageExtensions = new string[3]
        {
            "jpg",
            "jpeg",
            "png"
        };

        private static string[] _videoExtensions = new string[2]
        {
            "mp4",
            "avi"
        };

        private static string[] _fileExtensions = new string[4]
        {
            "docx",
            "pdf",
            "rar",
            "zip"
        };

        public static string Media(this string str, IFormFile file, string slug = "image")
        {
            if (file != null)
            {
                string str1 = file.FileName.Split('.').Last().ToLower();
                if (_imageExtensions.Contains(str1)) str = ImageHelper.SaveImage(file, slug);
                else if (_fileExtensions.Contains(str1)) str = FileHelper.SaveFile(file, slug);
                else if (_videoExtensions.Contains(str1)) str = VideoHelper.SaveVideo(file, slug);
                else throw new Exception("Dosya tipi desteklenmiyor.");
            }

            return !string.IsNullOrEmpty(str) ? str.Replace("\\", "/") : str;
        }

        public static string Media(this string str, string base64, string slug = "image")
        {
            if (!string.IsNullOrEmpty(base64)) str = ImageHelper.SaveImage(base64, slug);
            return !string.IsNullOrEmpty(str) ? str.Replace("\\", "/") : str;
        }
    }
}
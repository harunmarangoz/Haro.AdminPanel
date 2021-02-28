using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Haro.AdminPanel.Models.CustomModels
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ExceptionPageModel
    {
        public string Description { get; set; }
        public string Source { get; set; }
    }

    public class GalleryModel
    {
        public string Name { get; set; }
        public Column Column { get; set; }
        public List<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();
        public long ColumnId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public long EntryId { get; set; }
    }

    public class GalleryImage
    {
        public long Id { get; set; }
        public string Image { get; set; }
    }
}
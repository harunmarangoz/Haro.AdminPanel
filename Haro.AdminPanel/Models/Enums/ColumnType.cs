using System.ComponentModel.DataAnnotations;

namespace Haro.AdminPanel.Models.Enums
{
    public enum ColumnType
    {
        [Display(Name = "Yazı")]
        Text = 1,
        [Display(Name = "Yazı Alanı")]
        TextArea = 2,
        [Display(Name = "Editör")]
        Editor = 3,
        [Display(Name = "Seçmeli")]
        SelectList = 4,
        [Display(Name = "Çoktan Seçmeli")]
        MultipleSelectList = 5,
        [Display(Name = "Resim")]
        Image = 6,
        [Display(Name = "Çoklu Resim")]
        MultipleImage = 7,
        [Display(Name = "Sayı")]
        Number = 8,
        [Display(Name = "Evet/Hayır")]
        Bool = 9,
        [Display(Name = "Şifre")]
        Password = 10,
        [Display(Name = "Gizli")]
        Hidden = 11,
        [Display(Name = "Slug")]
        Slug = 12,
    }
}
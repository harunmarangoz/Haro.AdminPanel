using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Haro.AdminPanel.Utilities.Object
{
    public static class Slug
    {
        public static string GenerateSlug(this string phrase)
        {
            string ascii = new IdnMapping().GetAscii(phrase);
            ascii.ClearCharacter(" ", "-");
            ascii.ClearCharacter("--", "-");
            return ascii;
        }

        public static string ClearCharacter(this string phrase, string sourceChr, string targetChr)
        {
            while (phrase.Contains(sourceChr))
                phrase = phrase.Replace(sourceChr, targetChr);
            return phrase;
        }

        public static string RemoveDiacritics(this string text)
        {
            return new string(text.Normalize((NormalizationForm) 2)
                    .Where(c =>
                        CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray())
                .Normalize((NormalizationForm) 1);
        }
    }
}
using System.Globalization;
using System.Linq;
using Haro.AdminPanel.Common;

namespace Haro.AdminPanel.Utilities.Extensions
{
    public static class UrlExtensions
    {
        public static string LocalizeUrl(this string url, string code = null)
        {
            if (string.IsNullOrEmpty(code)) code = App.Common.Language.Code;

            if (code != CultureInfo.DefaultThreadCurrentCulture?.Name)
                url = $"/{code}{url}";

            return url;
        }
    }
}
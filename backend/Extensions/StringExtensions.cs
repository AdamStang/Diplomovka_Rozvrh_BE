using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace backend.Extensions
{
    public static class StringService
    {
        public static string ToSnakeCase(this string str)
        {
            return str.Replace(' ', '_');
        }

        public static string GetUriValue(this string str)
        {
            return str.Split("/").Last();
        }

        public static string RemoveDiacritics(this string str)
        {
            Regex nonSpacingMarkRegex = new Regex(@"\p{Mn}", RegexOptions.Compiled);
            var normalizedText = str.Normalize(NormalizationForm.FormD);
            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }
    }
}

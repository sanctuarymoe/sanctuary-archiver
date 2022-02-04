using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SanctuaryArchiverCore
{
    public class UrlSlugger
    {
        static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);
        static readonly Regex InvalidChars = new Regex(@"[^A-Za-z0-9\-]", RegexOptions.Compiled);
        static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public static string ToUrlSlug(string value)
        {
            value = RemoveDiacritics(value);
            value = WordDelimiters.Replace(value, "-");
            value = InvalidChars.Replace(value, "");
            value = MultipleHyphens.Replace(value, "-");
            return value.Trim('-');
        }

        private static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeExtensions.Services
{
    public class TitleTranslatorService
    {
        private static readonly ImmutableDictionary<char, string> LinguisticComparisons = new Dictionary<char, string>()
    {
        { 'а', "a" },
        { 'б', "b" },
        { 'в', "v" },
        { 'г', "g" },
        { 'д', "d" },
        { 'е', "e" },
        { 'ж', "zh" },
        { 'з', "z" },
        { 'и', "i" },
        { 'й', "y" },
        { 'к', "k" },
        { 'л', "l" },
        { 'м', "m" },
        { 'н', "n" },
        { 'о', "o" },
        { 'п', "p" },
        { 'р', "r" },
        { 'с', "s" },
        { 'т', "t" },
        { 'у', "u" },
        { 'ф', "f" },
        { 'х', "h" },
        { 'ц', "ts" },
        { 'ч', "ch" },
        { 'ш', "sh" },
        { 'щ', "sht" },
        { 'ъ', "a" },
        { 'ы', "y" },
        { 'ь', "y" },
        { 'э', "e" },
        { 'ю', "yu" },
        { 'я', "ya" },
        { ' ', "-" }
    }.ToImmutableDictionary();

        private static bool IsCharLatinOrDigit(char ch)
        {
            if (ch is >= '0' and <= '9') return true;

            var lowerChar = char.ToLower(ch);
            if (lowerChar is >= 'a' and <= 'z') return true;

            return false;
        }

        /// <summary>
        /// Translate
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Translate(string text, int? id = null)
        {
            var result = string.Concat(text.Select(c =>
            {
                var lowerCaseChar = char.ToLower(c);
                return
                    LinguisticComparisons.TryGetValue(lowerCaseChar, out var value)
                        ? value
                        : (IsCharLatinOrDigit(lowerCaseChar)
                            ? lowerCaseChar.ToString()
                            : "-"); // "-" is default
            }));

            // Replace double dashes with a single dash
            result = result.Replace("--", "-");

            // Remove dash at the beginning and end of the string
            result = result.Trim('-');

            if (id is not null)
                result = $"{result}-{id}";

            return result;
        }
    }
}

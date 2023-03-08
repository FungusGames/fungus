// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System.Text;
using System;
using System.Text.RegularExpressions;

namespace Fungus
{
    /// <summary>
    /// Misc string formatting functions.
    /// </summary>
    public static class StringFormatter
    {
        #region Public members

        public static string[] FormatEnumNames(Enum e, string firstLabel)
        {
            string[] enumLabels = Enum.GetNames(e.GetType());
            enumLabels[0] = firstLabel;
            for (int i=0; i<enumLabels.Length; i++)
            {
                enumLabels[i] = SplitCamelCase(enumLabels[i]);
            }
            return enumLabels;
        }

        public static string SplitCamelCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                {
                    newText.Append(' ');
                }
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Shortens string to maxLength and appends suffix. Does nothing if string is within length.
        /// </summary>
        /// <param name="value">string to shorten</param>
        /// <param name="maxLength">maximum length of the string</param>
        /// <param name="suffix">Suffix to apply to the end of the string when shortened.</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value) || maxLength <= 0) { return value; }

            if (value.Length <= maxLength) return value;

            return value.Substring(0, Math.Min(value.Length, maxLength)).Trim() + suffix;
        }


        /// <summary>
        /// Sterilizes a string, removing tokens and returning only text. Variables are returned as the variable name.
        /// </summary>
        /// <param name="value">String to sterilize.</param>
        /// <returns></returns>
        public static string SterilizeString(this string value)
        {
            Regex r = new Regex("{\\$.*?}"); //variable regex from Flowchart.
            string sterilizedString = string.Empty;

            var results = r.Matches(value);
            for (int i = 0; i < results.Count; i++)
            {
                Match match = results[i];
                string key = match.Value.Substring(2, match.Value.Length - 3);
                value = value.Replace("{$" + key + "}", key);
            }

            var tokens = TextTagParser.Tokenize(value, false);

            foreach (TextTagToken token in tokens)
            {
                if (token.type == TokenType.Words)
                {
                    sterilizedString += token.paramList[0].ToString();
                }
            }

            return sterilizedString;
        }

        #endregion
    }    
}
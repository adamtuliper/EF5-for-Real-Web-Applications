using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Gecko.Extensions
{

    /// <summary>
    /// Some Taken from namespace Telerik.Web.Mvc.Examples.StringExtensions as their demo had the IndentHtml code but their released libs didnt have it.
    /// </summary>
    public static class StringExtensions
    {


        /// <summary>
        /// A case-insensitive string replace (if the flag is passed in) as .Net doesn't have one that is case insensitive built in.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="find"></param>
        /// <param name="replaceWith"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string ReplaceInsensitive(this string original, string find, string replaceWith, bool ignoreCase)
        {
            // Get input string length
            int originalLength = original.Length;
            int findLen = find.Length;

            // Check inputs
            if (0 == originalLength || 0 == findLen || findLen > originalLength)
            {
                return original;
            }

            // Use the original method if the case is required
            if (!ignoreCase)
            {
                return original.Replace(find, replaceWith);
            }

            //create the new string to return.
            StringBuilder replaced = new StringBuilder(originalLength);
            int pos = 0;

            while (pos + findLen <= originalLength)
            {
                if (0 == string.Compare(original, pos, find, 0, findLen, ignoreCase))
                {

                    // Add the replaced string
                    replaced.Append(replaceWith);
                    pos += findLen;
                    continue;
                }

                // Advance one character
                replaced.Append(original, pos++, 1);
            }

            // Append remaining characters
            replaced.Append(original, pos, originalLength - pos);

            // Return string
            return replaced.ToString();

        }

        private static string TrimHtmlWhiteSpace(this string html)
        {
            return new Regex("(\\s|\t|\n|\r)+").Replace(html, " ");
        }


        private static int indentSize = 4, indentLevel = 0;

        private static string newLine = Environment.NewLine;
        private static int newLineLength = newLine.Length;

        private static string blockElements = "div|p|img|ul|ol|li|h[1-6]|blockquote";
        private static Regex openingBlockRegex = new Regex("<(" + blockElements + ")[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled),
              closingBlockRegex = new Regex("</(" + blockElements + ")>", RegexOptions.IgnoreCase | RegexOptions.Compiled),
              emptyTagRegex = new Regex("^<.*/>$", RegexOptions.Compiled),
              brTagRegex = new Regex("^<br\\s*/>$", RegexOptions.Compiled);

        private const int WRAP_THRESHOLD = 120;

        public static string IndentTags(this string html)
        {
            bool indenting = true;

            html = new Regex("(<[\\w][\\w\\d]*[^>]*>)|(</[\\w][\\w\\d]*>)", RegexOptions.IgnoreCase).Replace(html, new MatchEvaluator(delegate(Match m)
            {
                var token = m.Groups[0].Value;
                var result = new StringBuilder();
                var currentIndent = newLine.PadRight(newLineLength + indentSize * indentLevel);

                if (openingBlockRegex.IsMatch(token))
                {
                    if (!indenting)
                        result.Append(currentIndent);

                    if (!emptyTagRegex.IsMatch(token))
                        ++indentLevel;

                    while (token.Length > WRAP_THRESHOLD)
                    {
                        int splitPoint = token.Substring(0, WRAP_THRESHOLD).LastIndexOf(' ');

                        result.Append(token.Substring(0, splitPoint))
                              .Append(newLine.PadRight(newLineLength + indentSize * (indentLevel + 1)));

                        token = token.Substring(splitPoint);
                    }

                    result.Append(token)
                          .Append(newLine.PadRight(newLineLength + indentSize * indentLevel));

                    indenting = true;
                }
                else if (closingBlockRegex.IsMatch(token))
                {
                    result.Append(newLine.PadRight(newLineLength + indentSize * (--indentLevel)))
                          .Append(token);
                    indenting = false;
                }
                else if (brTagRegex.IsMatch(token))
                {
                    result.Append(token)
                          .Append(newLine.PadRight(newLineLength + indentSize * indentLevel));
                    indenting = false;
                }
                else
                {
                    result.Append(token);
                    indenting = false;
                }

                return result.ToString();
            }));

            return html;
        }

        public static string WordWrap(this string html)
        {
            // wrap lines
            var lines = html.Split(new string[] { newLine }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length <= WRAP_THRESHOLD)
                    continue;

                var result = new StringBuilder();

                var currentLine = lines[i];

                var currentLineIndentSize = new Regex("^\\s+").Match(currentLine).Length;

                while (currentLine.Length > WRAP_THRESHOLD)
                {
                    int splitPoint = currentLine.Substring(0, WRAP_THRESHOLD - indentSize).LastIndexOf(' ');

                    if (splitPoint < 0)
                        splitPoint = WRAP_THRESHOLD; // cuts though code, though

                    result.Append(currentLine.Substring(0, splitPoint))
                          .Append(newLine.PadRight(newLineLength + currentLineIndentSize + indentSize));

                    currentLine = currentLine.Substring(splitPoint + 1);
                }

                result.Append(currentLine);

                lines[i] = result.ToString();
            }

            return String.Join(newLine, lines);
        }

        public static string IndentHtml(this string html)
        {
            return html.TrimHtmlWhiteSpace()
                       .IndentTags()
                       .WordWrap();
        }
       
    }

}
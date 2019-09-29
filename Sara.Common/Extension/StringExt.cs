using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sara.Common.Extension
{
    public static class StringExt
    {
        /// <summary>
        /// When building a delimited list, you must check if your the first entry.
        /// This method will perform that check for you.
        /// </summary>
        public static void AppendNotFirst(this StringBuilder sb, string toAppend, ref bool first)
        {
            if (first)
                first = false;
            else
                sb.Append(toAppend);
        }
        /// <summary>
        /// Tokenizes a string based on a space delimiter.
        /// Maintaing single and double quotes as whole values.
        /// </summary>
        public static string[] Tokenize(this string input)
        {
            // First do the normal split.
            var lineItems = input.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var items = new List<string>();
            var inQuotes = false;
            var currentItem = "";
            foreach (var item in lineItems)
            {
                if (item.StartsWith("\""))
                {
                    inQuotes = true;
                    currentItem = "";
                }

                if (inQuotes)
                    currentItem += item.Trim('"') + " ";
                else
                    items.Add(item);

                if (item.EndsWith("\""))
                {
                    inQuotes = false;
                    items.Add(currentItem.Substring(0, currentItem.Length - 1));
                }
            }

            return items.ToArray();
        }
        /// <summary>
        /// Returns the string in reverse order.
        /// </summary>
        public static string Reverse(this string input)
        {
            var letters = input.ToCharArray();
            Array.Reverse(letters);
            var sb = new StringBuilder();
            foreach (var letter in letters)
                sb.Append(letter);
            return sb.ToString();
        }
        /// <summary>
        /// Truncates the string to the specified size.
        /// </summary>
        public static string Truncate(this string value, int maxCharacters)
        {
            return value.Length > maxCharacters ? value.Substring(0, maxCharacters) : value;
        }
        public static string ToCsv(this IEnumerable<string> values)
        {
            return string.Join(",", values.ToArray());
        }
        /// <summary>
        /// Returns a string from the end where 'start' is zero based from the end of the string.
        /// </summary>
        public static string SubstringFromEnd(this string input, int start, int length)
        {
            return input.Reverse().Substring(start, length).Reverse();
        }
        public static IEnumerable<string> SplitInChunks(this string str, int chunkSize)
        {
            return Enumerable.Range(0, (str.Length / chunkSize) + (((str.Length % chunkSize) > 0) ? 1 : 0))
                             .Select(i => str.Substring(i * chunkSize, Math.Min(chunkSize, str.Length - i * chunkSize)));
        }
        public static string WordWrap(this string text, int width)
        {
            // Orginal http://www.codeproject.com/Articles/51488/Implementing-Word-Wrap-in-C
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                var eol = text.IndexOf('\n', pos);
                next = (eol == -1) ? eol = text.Length : eol + 1;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        var len = eol - pos;
                        if (len > width)
                            len = LocateBreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append('\n');

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    }
                    while (eol > pos);
                }
                else
                    sb.Append('\n'); // Empty line
            }
            return sb.ToString();
        }
        private static int LocateBreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            var i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
        public static string ToOr(this string[] data)
        {
            var result = string.Empty;
            foreach (var item in data)
            {
                if (result == string.Empty)
                {
                    result = $"'{item}'";
                    continue;
                }

                result = $"{result} or '{item}'";
            }
            return result;
        }
        public static string ToCsv(this List<string> data)
        {
            var result = string.Empty;
            foreach (var item in data)
            {
                if (result == string.Empty)
                {
                    result = item;
                    continue;
                }

                result = $"{result}, {item}";
            }
            return result;
        }
        public static string AddNewLineValue(this string source, string value)
        {
            if (source == string.Empty)
            {
                return value;
            }
            return string.Format(@"{0}
{1}", source, value);
        }
        public static string Indent(this string value)
        {
            // 2 is the Default
            return Indent(value, 2);
        }
        /// <summary>
        /// Indents a string list by the given size
        /// </summary>
        public static string Indent(this string value, int size)
        {
            var space = string.Empty;
            for (var i = 0; i < size; i++)
            {
                space = space + " ";
            }
            return space + value.Replace(Environment.NewLine, Environment.NewLine + space);
        }
        public static string ToFixedColumnRight(this string value, int size)
        {
            if (value == null)
                return new string(' ', size);

            if (value.Length >= size)
                return value;

            while (value.Length != size)
                value = $" {value}";
            return value;
        }
        public static string ToFixedColumnLeft(this string value, int size)
        {
            if (value.Length >= size)
                return value;

            while (value.Length != size)
                value = $"{value} ";
            return value;
        }
        public static string ZeroToBlank(this string value)
        {
            if (string.IsNullOrEmpty(value))
                if (value != null)
                    return new string(' ', value.Length);

            if (!int.TryParse(value, out var test)) return value;
            if (test != 0) return value;
            return value != null ? new string(' ', value.Length) : null;
        }

    }
}

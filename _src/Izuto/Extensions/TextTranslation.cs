using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izuto.Extensions
{
    public class TextTranslation
    {
        public class TranslationEntry
        {
            public string Syllable { get; set; } = "";

            private byte[]? _bytes;


            private byte[] BytesSetter
            {
                get
                {
                    if (_bytes == null)
                        return new byte[1];
                    return _bytes;
                }
                set
                {
                    _bytes = value;
                }
            }

            public byte[] GetBytes()
            {
                return BytesSetter;
            }

            public int[] Bytes
            {
                get
                {
                    if (_bytes == null)
                        return new int[2];
                    // Convert internal bytes to int[] for JSON serialization
                    return _bytes.Select(b => (int)b).ToArray();
                }
                set
                {
                    if (value == null)
                    {
                        _bytes = null;
                    }
                    else
                    {
                        // Ensure each element is cast down to byte
                        _bytes = value.Select(b => (byte)b).ToArray();
                    }
                }
            }
            public string BytesString
            {
                get
                {
                    if (_bytes == null)
                        return "";
                    return System.Text.Encoding.GetEncoding("shift_jis").GetString(_bytes);
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _bytes = new byte[2];
                    }
                    else
                    {
                        // Convert the incoming Shift-JIS string into raw bytes
                        _bytes = Encoding.GetEncoding("shift_jis").GetBytes(value);
                    }
                }
            }
        }

        public static bool ShouldIgnoreText(string text)
        {
            return
                text.StartsWith("tt0")
                || text.StartsWith("tt1")
                || text.StartsWith("tt2")
                || text.StartsWith("tt3")
                || text.StartsWith("tt4")
                || text.StartsWith("tt5")
                || text.StartsWith("tt6")
                || text.StartsWith("tt7")
                || text.StartsWith("tt8")
                || text.StartsWith("tt9")
                || text.StartsWith("2D_")
                || text.StartsWith("ie")
                || text.StartsWith("mr0")
                || text.StartsWith("mr1")
                || text.StartsWith("mr2")
                || text.StartsWith("mr3")
                || text.StartsWith("mr4")
                || text.StartsWith("mr5")
                || text.StartsWith("mr6")
                || text.StartsWith("mr7")
                || text.StartsWith("mr8")
                || text.StartsWith("mr9")
                || text.Contains(".SAD")
                || text.Contains(": %d")
                || text.Contains("＝%d");
        }

        public static string ConvertTextString(List<TranslationEntry> TranslationTable, string Text)
        {
            if (TranslationTable == null)
                return Text;
            if (ShouldIgnoreText(Text))
                return Text;

            // Work with bytes directly
            List<byte> outputBytes = new List<byte>();

            int i = 0;
            while (i < Text.Length)
            {
                bool matched = false;

                // Try each syllable
                foreach (var entry in TranslationTable)
                {
                    string syllable = entry.Syllable;

                    // Check if the text at position i starts with this syllable
                    if (i + syllable.Length <= Text.Length &&
                        Text.Substring(i, syllable.Length) == syllable)
                    {
                        // Add mapped bytes
                        outputBytes.AddRange(entry.GetBytes());

                        // Advance by syllable length
                        i += syllable.Length;
                        matched = true;
                        break; // stop checking other syllables
                    }
                }

                if (!matched)
                {
                    // Fallback: just encode the single char as ASCII
                    outputBytes.Add((byte)Text[i]);
                    i++;
                }
            }

            // Convert collected bytes into a Shift-JIS string
            return Encoding.GetEncoding("shift_jis").GetString(outputBytes.ToArray());
        }

        public static string ConvertBackTextString(List<TranslationEntry> TranslationTable, string text)
        {
            if (TranslationTable == null)
                return text;
            if (ShouldIgnoreText(text))
                return text;

            string newText = text;
            foreach (var t in TranslationTable)
            {
                newText = newText.Replace(t.BytesString, t.Syllable);
            }
            return newText;
        }
    }
}

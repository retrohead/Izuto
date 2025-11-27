using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Izuto.Extensions
{
    public class OptionsFileData
    {
        public string FilePath = "";

        public class ConfigurationFile()
        {
            public List<TranslationEntry> TranslationTable { get; set; } = new List<TranslationEntry>();
            public List<FileReplacementEntry> FileReplacements { get; set; } = new List<FileReplacementEntry>();
        }

        public OptionsFileData()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public class FileReplacementEntry
        {
            public string PathToReplace { get; set; } = "";

            public string RelativePath { get; set; } = "";
        }

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

        public ConfigurationFile Config = new ConfigurationFile();

        public string GetFileActualPath(FileReplacementEntry file)
        {
            string baseDir = Path.GetDirectoryName(FilePath) ?? "";
            return Path.Combine(baseDir ?? "", file.RelativePath);
        }

        public string GetFileRelativePathFromPath(FileReplacementEntry file)
        {
            // Get the base directory of FilePath
            string baseDir = Path.GetDirectoryName(FilePath) ?? "";

            // Compute relative path from baseDir to the given font file path
           return Path.GetRelativePath(baseDir, file.RelativePath);
        }

        public bool IsLoaded()
        {
            return !string.IsNullOrEmpty(FilePath);
        }

        public bool Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            FilePath = "";
            ConfigurationFile? tryconfig = null;
            try
            {
                tryconfig = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigurationFile>(json);
            } catch { }
            if (tryconfig == null)
            {
                Config = new ConfigurationFile();
                return false;
            }
            FilePath = fileName;
            Config = tryconfig;
            return true;
        }

        public void Save()
        {
            string targetPath = FilePath;

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                // Prompt user for location
                var dialog = new SaveFileDialog
                {
                    FileName = "izuto_options.json",
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Save Izuto options configuration"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    targetPath = dialog.FileName;
                    FilePath = targetPath; // remember it
                }
                else
                {
                    // user cancelled
                    return;
                }
            }

            // Serialize this config object to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Config, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(targetPath, json);

            MessageBox.Show($"Configuration saved to:\n\n{targetPath}",
                                           "Save Successful",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);
        }

        public string ConvertTextString(string text)
        {
            if (Config == null)
                return text;
            if (Config.TranslationTable == null)
                return text;
            if (!IsLoaded())
                return text;
            // Work with bytes directly
            List<byte> outputBytes = new List<byte>();

            int i = 0;
            while (i < text.Length)
            {
                bool matched = false;

                // Try each syllable
                foreach (var entry in Config.TranslationTable)
                {
                    string syllable = entry.Syllable;

                    // Check if the text at position i starts with this syllable
                    if (i + syllable.Length <= text.Length &&
                        text.Substring(i, syllable.Length) == syllable)
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
                    outputBytes.Add((byte)text[i]);
                    i++;
                }
            }

            // Convert collected bytes into a Shift-JIS string
            return Encoding.GetEncoding("shift_jis").GetString(outputBytes.ToArray());
        }

        public string ConvertBackTextString(string text)
        {
            if (Config == null)
                return text;
            if (Config.TranslationTable == null)
                return text;
            if (!IsLoaded())
                return text;

            string newText = text;
            foreach (var t in Config.TranslationTable)
            {
                newText = newText.Replace(t.BytesString, t.Syllable);
            }
            return newText;
        }

    }
}

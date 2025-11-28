using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Izuto.Extensions.TextTranslation;
using static System.Net.Mime.MediaTypeNames;

namespace Izuto.Extensions
{
    public class OptionsFileData
    {
        public string FilePath = "";
        public const string OptionsFileFilter = "Izuto Options JSON files (*.json)|*.json|All files (*.*)|*.*";

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

        /// <summary>
        /// Saves the config file to the path it was loaded from unless a path parameter is sent.
        /// If path is blank then the user is prompted for a location to save
        /// </summary>
        /// <param name="path">File filter string (e.g. "CIA files (*.cia)|*.cia").</param>
        /// <returns>True if file was saved or Fales if user cancelled or something went wrong.</returns>
        public bool Save(string path = "Existing Path")
        {
            string targetPath = path == "Existing Path" ? FilePath : path;

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                // Prompt user for location
                var dialog = new SaveFileDialog
                {
                    FileName = "izuto_options.json",
                    Filter = OptionsFileFilter,
                    Title = "Save Izuto options configuration"
                };

                if (dialog.ShowDialog(MainForm.Self) == DialogResult.OK)
                {
                    targetPath = dialog.FileName;
                    if(path == "")
                        FilePath = targetPath; // remember it
                }
                else
                {
                    // user cancelled
                    return false;
                }
            }

            // Serialize this config object to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Config, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(targetPath, json);

            MessageBox.Show($"Configuration saved to:\n\n{targetPath}",
                                           "Save Successful",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);
            return true;
        }

    }
}

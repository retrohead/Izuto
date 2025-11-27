using Ekona;
using Izuto.Extensions;
using plugin_level5.N3DS.Archive;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Izuto
{
    public partial class MainForm : Form
    {
        public static string CurrentWorkingDirectory = "";
        public static OptionsFileData OptionsFile = new OptionsFileData();
        public static string LoadedArchiveFilePath = "";
        private static string ApplicationTempPath = Path.Combine(Path.GetTempPath(), "Izuto");
        public static MainForm? Self;
        public static List<OptionsFileData.FileReplacementEntry> QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
        private ProgressPanel progressPanel = new ProgressPanel();
        public static System.Drawing.Bitmap Logo;
        public enum iconTypes
        {
            Unknown,
            Txt,
            Zip
        }
        public static void DeleteDirWithoutWarning(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch { }

        }

        public static bool IsAnotherInstanceRunning()
        {
            string currentProcessName = Process.GetCurrentProcess().ProcessName;
            int count = Process.GetProcessesByName(currentProcessName).Length;
            return count > 1; // More than one means another instance is running
        }

        public static void DeleteTempDirs()
        {
            // delete this applications temp folder
            DeleteTempDir();
            if (IsAnotherInstanceRunning())
                return;
            // delete all temp folders as this is the only instance of the app running
            var dirs = Directory.GetDirectories(ApplicationTempPath);
            foreach (var dir in dirs)
                DeleteDirWithoutWarning(dir);
        }

        public static void DeleteTempDir()
        {
            DeleteDirWithoutWarning(CurrentWorkingDirectory);
            CurrentWorkingDirectory = "";
        }
        public static void CreateNewTempDirectory()
        {
            DeleteTempDir();
            string newTempDir = Path.Combine(ApplicationTempPath, "temp_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            int appendInt = 2;
            string adjustedTempDir = newTempDir;
            while (Directory.Exists(adjustedTempDir))
            {
                adjustedTempDir = $"{newTempDir} ({appendInt})";
                appendInt++;
            }
            newTempDir = adjustedTempDir;
            Directory.CreateDirectory(newTempDir);
            CurrentWorkingDirectory = newTempDir;
        }

        public MainForm()
        {
            InitializeComponent();
            // Get the version from the assembly
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            // Display it in the form header
            this.Text = $"Izuto - Version {version}";
            Self = this;
            Logo = Properties.Resources.IzutoLogo;
            pictureBoxLogo.Image = Logo;
        }

        /// <summary>
        /// Opens a file browser dialog with a custom filter and title.
        /// </summary>
        /// <param name="filter">File filter string (e.g. "CIA files (*.cia)|*.cia").</param>
        /// <param name="title">Dialog title (e.g. "Select a CIA file").</param>
        /// <returns>Full path of the selected file, or empty string if cancelled.</returns>
        public static string BrowseForFile(string filter = "All files (*.*)|*.*", string title = "Select a file")
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }

            return filePath;
        }
        /// <summary>
        /// Opens a folder browser dialog with a custom description.
        /// </summary>
        /// <param name="description">Text shown in the dialog (e.g. "Select output directory").</param>
        /// <returns>Full path of the selected directory, or empty string if cancelled.</returns>
        public static string BrowseForDirectory(string description = "Select a folder")
        {
            string folderPath = string.Empty;

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = description;
                folderDialog.ShowNewFolderButton = true; // Allow creating new folders

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderDialog.SelectedPath;
                }
            }

            return folderPath;
        }


        private async void btnBrowseArchiveFA_Click(object sender, EventArgs e)
        {
            LoadedArchiveFilePath = BrowseForFile("Level 5 Archive File (*.fa)|*.fa", "Select a FA file");
            textArchiveFaPath.Text = LoadedArchiveFilePath;
            QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
            UpdateProgress("Reading Archive", 0, 1);
            await ListFiles();
            EndProgressUpdates();
        }

        public static List<B123ArchiveFile> ArchiveFiles = new List<B123ArchiveFile>();

        private async Task ListFiles()
        {
            CreateNewTempDirectory();
            ArchiveFiles = await ArchiveFA.ListFiles(LoadedArchiveFilePath);
            listView1.BeginUpdate();
            listView1.Items.Clear();
            List<B123ArchiveFile> pkb_files = ArchiveFiles.Where(f => f.FilePath.FullName.EndsWith(".pkb") && f.FilePath.FullName.Contains("script/") && !f.FilePath.FullName.Contains("pic3d/")).ToList();
            var sortedPkbFiles = pkb_files.OrderBy(p => p.FilePath);
            foreach (var file in sortedPkbFiles)
            {
                listView1.Items.Add(new ListViewItem() { Text = file.FilePath.FullName, Tag = file, ImageIndex = (int)iconTypes.Zip });
            }
            listView1.EndUpdate();
        }

        private async void btnExplorePKB_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (listView1.SelectedItems[0].Tag == null) return;
            if (listView1.SelectedItems[0].Tag?.GetType() != typeof(B123ArchiveFile)) return;
            B123ArchiveFile? file = (B123ArchiveFile?)listView1.SelectedItems[0].Tag;
            if (file == null) return;
            // check whether this is a linked package

            bool doNotSave = false;
            if (file.FilePath.FullName.Contains("t.pkb") && ArchiveFiles.FirstOrDefault(p => p.FilePath.FullName.Equals(file.FilePath.FullName.Replace("t.pkb", ".pkb"))) != null)
            {
                if (MessageBox.Show($"You appear to be loading a linked text script package. To modify the strings in this package you should open\n\n{file.FilePath.FullName.Replace("t.pkb", ".pkb")}\n\nDo you want to view the package anyway without saving changes", "Linked Package Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                doNotSave = true;
            }
            UpdateProgress("Unpacking Archive", 0, 1);
            PKB.FileEntry pkbFileData = await PKB.UnpackPKBFromArchiveFA_Async(textArchiveFaPath.Text, file, CurrentWorkingDirectory);
            EndProgressUpdates();
            PKBForm pkbform = new PKBForm(pkbFileData, file);
            pkbform.StartPosition = FormStartPosition.CenterParent;
            pkbform.ShowDialog(this);
            if (pkbform.DialogResult == DialogResult.Cancel || doNotSave) return;

            int filesToPack = 2 + QueuedImports.Count + (OptionsFile.Config == null ? 0 : OptionsFile.Config.FileReplacements.Count);


            UpdateProgress("Listing Archive Contents", 0, 1);
            B123ArchiveFile? pkhFile = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(file.FilePath.FullName.Replace(".pkb", ".pkh")));

            int filesToReplaceCount = 2;
            UpdateProgress("Queuing Files", 0, filesToPack);
            // add main pkb and pkh 
            await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, file, pkbFileData.FileData.path);
            UpdateProgress("Queuing Files", 1, filesToPack);
            await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, pkhFile, pkbFileData.FileData.path.Replace(".pkb", ".pkh"));

            if (OptionsFile.Config != null)
            {
                foreach (var replaceFile in OptionsFile.Config.FileReplacements)
                {
                    UpdateProgress("Queuing Files", filesToReplaceCount, filesToPack);
                    filesToReplaceCount++;
                    B123ArchiveFile? fileToReplace = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(replaceFile.PathToReplace));
                    if (fileToReplace == null)
                    {
                        if (MessageBox.Show("The file requested to replace was not found:\n\n" + replaceFile.PathToReplace + "\n\nDo you want to continue importing any remaining files?", "Import File Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Cancel)
                            return;
                        continue;
                    }
                    await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, fileToReplace, OptionsFile.GetFileActualPath(replaceFile));
                }
            }
            foreach (OptionsFileData.FileReplacementEntry queuedFile in QueuedImports)
            {
                UpdateProgress("Queuing Files", filesToReplaceCount, filesToPack);
                filesToReplaceCount++;
                B123ArchiveFile? fileToReplace = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(queuedFile.RelativePath));
                if (fileToReplace == null)
                {
                    if (MessageBox.Show("The file requested to replace was not found:\n\n" + queuedFile.RelativePath + "\n\nDo you want to continue importing any remaining files?", "Import File Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        return;
                    continue;
                }
                await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, fileToReplace, queuedFile.PathToReplace);
            }
            await ArchiveFA.ReplaceQueuedFiles(textArchiveFaPath.Text);
            EndProgressUpdates();
            MessageBox.Show("Archive modification completed, rebuild your rom for testing", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void UpdateProgress(string text, int value, int maxValue)
        {
            tableLayoutPanel1.Enabled = false;
            if (!Controls.Contains(progressPanel))
            {
                progressPanel.Dock = DockStyle.Fill;
                Controls.Add(progressPanel);
                progressPanel.BringToFront();
            }
            progressPanel.UpdateProgress(text, value, maxValue);
        }

        public void EndProgressUpdates()
        {
            Controls.Remove(progressPanel);
            tableLayoutPanel1.Enabled = true;
            progressPanel.EndProgressUpdates();
        }


        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.StartPosition = FormStartPosition.CenterParent;
            optionsForm.ShowDialog(this);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.OptionsFilePath))
            {
                if (!File.Exists(Properties.Settings.Default.OptionsFilePath))
                {
                    MessageBox.Show("Failed to load options, the file no longer exists\n\n:" + Properties.Settings.Default.OptionsFilePath, "Options File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Properties.Settings.Default.OptionsFilePath = "";
                    Properties.Settings.Default.Save();
                }
                else
                {
                    if (!OptionsFile.Load(Properties.Settings.Default.OptionsFilePath))
                    {
                        MessageBox.Show("The options file appears to be corrupted\n\n:" + Properties.Settings.Default.OptionsFilePath, "Options File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Properties.Settings.Default.OptionsFilePath = "";
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteTempDirs();
        }
    }
}

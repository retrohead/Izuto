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

        public static string BytesToHexString(byte[]? bytes, string spacing = "", int? bytesPerRow = null)
        {
            if (bytes == null || bytes.Count() == 0)
                return "";
            int len = bytes.Count();

            // Estimate capacity: 2 hex chars per byte + spacing + possible newline
            int estimated = len * (2 + spacing.Length + 1);
            var sb = new StringBuilder(estimated);

            int rowCount = 0;
            for (int i = 0; i < len; i++)
            {
                // Add space between bytes (except at start of line)
                if (rowCount > 0)
                    sb.Append(spacing);

                sb.Append(bytes[i].ToString("X2"));

                if (!string.IsNullOrEmpty(spacing))
                    sb.Append(spacing);

                rowCount++;

                if (bytesPerRow.HasValue && rowCount == bytesPerRow.Value)
                {
                    sb.AppendLine();
                    rowCount = 0;
                }
            }

            return sb.ToString();
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
        public static string CreateNewTempDirectory(bool SetAsWorkingDirectory)
        {
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
            if (SetAsWorkingDirectory)
            {
                DeleteTempDir();
                CurrentWorkingDirectory = newTempDir;
            }
            return newTempDir;
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
            RecentFiles.Init();
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

                if (openFileDialog.ShowDialog(MainForm.Self) == DialogResult.OK)
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

                if (folderDialog.ShowDialog(MainForm.Self) == DialogResult.OK)
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
            await ListFiles();
        }

        public static List<B123ArchiveFile> ArchiveFiles = new List<B123ArchiveFile>();

        private async Task ListFiles()
        {
            RecentFiles.Add(LoadedArchiveFilePath);
            QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
            UpdateProgress("Reading Archive", 0, 1);
            EndProgressUpdates();
            CreateNewTempDirectory(true);
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
            //---------------
            // OPENING FORM
            //---------------
            PKBForm pkbform = new PKBForm(pkbFileData, file);
            pkbform.StartPosition = FormStartPosition.CenterParent;
            pkbform.ShowDialog(this);
            if (pkbform.DialogResult == DialogResult.Cancel || doNotSave) return;

            //---------------
            // FORM CLOSED
            //---------------
            int filesToPack = 2 + QueuedImports.Count + (OptionsFile.Config == null ? 0 : OptionsFile.Config.FileReplacements.Count);
            UpdateProgress("Listing Archive Contents", 0, 1);
            B123ArchiveFile? pkhFile = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(file.FilePath.FullName.Replace(".pkb", ".pkh")));

            int filesToReplaceCount = 2;
            UpdateProgress("Queuing Files", 0, filesToPack);
            // add main pkb and pkh 
            await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, file, pkbFileData.FileData.path);
            UpdateProgress("Queuing Files", 1, filesToPack);
            await ArchiveFA.QueueReplaceFile(textArchiveFaPath.Text, pkhFile, pkbFileData.FileData.path.Replace(".pkb", ".pkh"));
            // queue file replacements from options
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
            // queue the other queued imports (usually coming from linked text pacs)
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
            // actually do the replacements
            await ArchiveFA.ReplaceQueuedFiles(textArchiveFaPath.Text);
            EndProgressUpdates();
            QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public static string ShortenPath(string path, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            // Split into directory segments
            string[] parts = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Always keep first and last segment
            string first = parts[0];
            string last = parts[^1];

            // Build middle segments until we exceed maxLength
            var middle = new List<string>();
            int totalLength = first.Length + last.Length + 5; // 5 for "...\"
            for (int i = 1; i < parts.Length - 1; i++)
            {
                int nextLen = parts[i].Length + 1; // +1 for separator
                if (totalLength + nextLen > maxLength)
                {
                    middle.Add("...");
                    break;
                }
                middle.Add(parts[i]);
                totalLength += nextLen;
            }

            return string.Join(Path.DirectorySeparatorChar.ToString(),
                new[] { first }.Concat(middle).Concat(new[] { last }));
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem fileItem = (ToolStripMenuItem)sender;
            if (RecentFiles.Items.Count > 0)
            {
                // add the recent items
                toolStripMenuItem1.DropDownItems.Clear();
                foreach(var item in RecentFiles.Items)
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem() { Text = ShortenPath(item.FilePath), Tag=item, ToolTipText= item.FilePath };
                    newItem.Click += RecentItem_Click;
                    toolStripMenuItem1.DropDownItems.Add(newItem);
                }
            }
            else
            {
                // make sure the "No Recent Files" item is in place
                toolStripMenuItem1.DropDownItems.Clear();
                toolStripMenuItem1.DropDownItems.Add(noRecentItemsToolStripMenuItem);
            }
        }

        private async void RecentItem_Click(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Tag == null)
                return;
            if(!File.Exists(((RecentFiles.RecentFile)item.Tag).FilePath))
            {
                if(MessageBox.Show("The file no longer exists, do you want to remove it from your recent files list?", "Missing File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    RecentFiles.Items.Remove(((RecentFiles.RecentFile)item.Tag));
                return;
            }
            LoadedArchiveFilePath = ((RecentFiles.RecentFile)item.Tag).FilePath;
            textArchiveFaPath.Text = LoadedArchiveFilePath;
            await ListFiles();
        }
    }
}

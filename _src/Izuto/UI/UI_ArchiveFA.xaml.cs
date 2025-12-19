using Ekona;
using Izuto.Controls;
using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.Inazuma11;
using plugin_level5.N3DS.Archive;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Izuto.UI
{
    /// <summary>
    /// Interaction logic for UI_ArchiveFA.xaml
    /// </summary>
    public partial class UI_ArchiveFA : UserControl
    {
        public static string LoadedArchiveFilePath = "";
        public static List<OptionsFileData.FileReplacementEntry> QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
        public static List<B123ArchiveFile> ArchiveFiles = new List<B123ArchiveFile>();
        private listViewDataType? pkbList;
        public UI_ArchiveFA()
        {
            InitializeComponent();
        }

        private async void btnExplorePKB_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = ((listViewItemDataType?)listView1.SelectedItems[0]);
            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(B123ArchiveFile)) return;
            B123ArchiveFile? file = (B123ArchiveFile?)selectedItem.Tag;
            if (file == null) return;
            await LoadFile(file);
        }


        public async Task ListFiles()
        {
            if (LoadedArchiveFilePath != "")
                RecentFilesManager.AddRecentFile(LoadedArchiveFilePath, System.IO.Path.GetFileName(LoadedArchiveFilePath));
            QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
            UI_MainWindow.Self!.UpdateProgress("Reading Archive", 0, 1);
            UI_MainWindow.CreateNewTempDirectory(true);
            ArchiveFiles = await ArchiveFA.ListFiles(LoadedArchiveFilePath);

            pkbList = new listViewDataType(MainWindow.Self, ref listView1);
            pkbList.Items = new ObservableCollection<listViewItemDataType>();
            List<B123ArchiveFile> pkb_files = ArchiveFiles.Where(f => f.FilePath.FullName.EndsWith(".pkb") && f.FilePath.FullName.Contains("script/") && !f.FilePath.FullName.Contains("pic3d/")).ToList();
            var sortedPkbFiles = pkb_files.OrderBy(p => p.FilePath).ToList();

            for (int i = 0; i < sortedPkbFiles.Count(); i++)
            {
                var file = sortedPkbFiles[i];
                var item = new listViewItemDataType(pkbList, file.FilePath.FullName, i.ToString());
                item.Tag = file;
                item.icon = UI_MainWindow.icon_zip;
                pkbList.AddItem(item);
            }
            listView1.DataContext = pkbList;
            UI_MainWindow.Self!.EndProgressUpdates();
        }

        private async Task LoadFile(B123ArchiveFile file)
        {

            // check whether this is a linked package

            bool doNotSave = false;
            if (file.FilePath.FullName.Contains("t.pkb") && ArchiveFiles.FirstOrDefault(p => p.FilePath.FullName.Equals(file.FilePath.FullName.Replace("t.pkb", ".pkb"))) != null)
            {
                if (MessageBox.Show($"You appear to be loading a linked text script package. To modify the strings in this package you should open\n\n{file.FilePath.FullName.Replace("t.pkb", ".pkb")}\n\nDo you want to view the package anyway without saving changes", "Linked Package Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                doNotSave = true;
            }
            PKB.FileEntry pkbFileData = new PKB.FileEntry();
            string archivepath = textArchiveFaPath.Text;
            await Task.Run(async () =>
            {

                UI_MainWindow.Self.UpdateProgress("Unpacking Archive", 0, 1);
                pkbFileData = await PKB.UnpackPKBFromArchiveFA_Async(archivepath, file, UI_MainWindow.CurrentWorkingDirectory);
                UI_MainWindow.Self!.EndProgressUpdates();
            });
            //---------------
            // OPENING FORM
            //---------------

            CustomWindow win = DockHandler.CreateCustomWindow(MainWindow.Self.Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Resizable });
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            PKBWindow pkbform = new PKBWindow(pkbFileData, file);
            win.ApplyContent(pkbform);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            if (pkbform.DialogResult == false || doNotSave) return;

            await Task.Run(async () =>
            {
                //---------------
                // FORM CLOSED
                //---------------
                int filesToPack = 2 + QueuedImports.Count + (UI_MainWindow.OptionsFile.Config == null ? 0 : UI_MainWindow.OptionsFile.Config.FileReplacements.Count);

                UI_MainWindow.Self!.UpdateProgress("Listing Archive Contents", 0, 1);
                B123ArchiveFile? pkhFile = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(file.FilePath.FullName.Replace(".pkb", ".pkh")));
                int filesToReplaceCount = 2;
                UI_MainWindow.Self!.UpdateProgress("Queuing Files", 0, filesToPack);
                // add main pkb and pkh 
                await ArchiveFA.QueueReplaceFile(archivepath, file, pkbFileData.FileData.path);
                UI_MainWindow.Self!.UpdateProgress("Queuing Files", 1, filesToPack);
                await ArchiveFA.QueueReplaceFile(archivepath, pkhFile, pkbFileData.FileData.path.Replace(".pkb", ".pkh"));
                // queue file replacements from options
                if (UI_MainWindow.OptionsFile.Config != null)
                {
                    foreach (var replaceFile in UI_MainWindow.OptionsFile.Config.FileReplacements)
                    {
                        UI_MainWindow.Self!.UpdateProgress("Queuing Files", filesToReplaceCount, filesToPack);
                        filesToReplaceCount++;
                        B123ArchiveFile? fileToReplace = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(replaceFile.PathToReplace));
                        if (fileToReplace == null)
                        {
                            if (MessageBox.Show("The file requested to replace was not found:\n\n" + replaceFile.PathToReplace + "\n\nDo you want to continue importing any remaining files?", "Import File Error", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                                return;
                            continue;
                        }
                        await ArchiveFA.QueueReplaceFile(archivepath, fileToReplace, UI_MainWindow.OptionsFile.GetFileActualPath(replaceFile));
                    }
                }
                // queue the other queued imports (usually coming from linked text pacs)
                foreach (OptionsFileData.FileReplacementEntry queuedFile in QueuedImports)
                {
                    UI_MainWindow.Self!.UpdateProgress("Queuing Files", filesToReplaceCount, filesToPack);
                    filesToReplaceCount++;
                    B123ArchiveFile? fileToReplace = ArchiveFiles.FirstOrDefault(f => f.FilePath.FullName.Equals(queuedFile.RelativePath));
                    if (fileToReplace == null)
                    {
                        if (MessageBox.Show("The file requested to replace was not found:\n\n" + queuedFile.RelativePath + "\n\nDo you want to continue importing any remaining files?", "Import File Error", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                            return;
                        continue;
                    }
                    await ArchiveFA.QueueReplaceFile(archivepath, fileToReplace, queuedFile.PathToReplace);
                }
                // actually do the replacements
                await ArchiveFA.ReplaceQueuedFiles(archivepath);
                UI_MainWindow.Self!.EndProgressUpdates();
                QueuedImports = new List<OptionsFileData.FileReplacementEntry>();
            });
            MessageBox.Show("Archive modification completed, rebuild your rom for testing", "Completed!", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public async void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            await Open_Click();
        }
        public async Task Open_Click()
        {
            string path = UI_MainWindow.BrowseForFile("All Supported Files (*.fa;*.pkh)|*.fa;*.pkh|Level 5 Archive File (*.fa)|*.fa|PKH File (*.pkh)|*.pkh|All Files (*.)|*.", "Select a FA file");

            if (path.ToLower().EndsWith(".pkh"))
            {
                OpenPKHFile(path);
                return;
            }

            LoadedArchiveFilePath = path;
            textArchiveFaPath.Text = UI_ArchiveFA.LoadedArchiveFilePath;
            await ListFiles();
        }

        public void OpenPKHFile(string path)
        {
            //---------------
            // OPENING FORM
            //---------------

            CustomWindow win = DockHandler.CreateCustomWindow(MainWindow.Self.Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Resizable });
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            sFile pkhFile = new sFile() { path = path, name = Path.GetFileName(path) };
            sFile pkbFile = new sFile() { path = path.Replace(".pkh", ".pkb"), name = Path.GetFileName(path.Replace(".pkh", ".pkb")) };
            INAZUMA11.PKB.PKBContents extractedPKBItems = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);
            var entry = new PKB.FileEntry() { FileData = pkbFile, PKBContents = extractedPKBItems };

            PKBWindow pkbform = new PKBWindow(entry, null);
            win.ApplyContent(pkbform);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            if (pkbform.DialogResult == false) return;
            return;
        }
    }
}

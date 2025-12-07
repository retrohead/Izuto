using Ekona;
using Izuto.Extensions;
using Izuto.UI;
using Microsoft.Win32;
using plugin_level5.N3DS.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Izuto.UI.UI_MainWindow;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for PKBForm.xaml
    /// </summary>
    public partial class PKBWindow : Window
    {
        PKB.FileEntry PKBFileInfo;
        B123ArchiveFile SourceArchiveFile;
        PACWindow? pacform;
        Brush? previousColour;
        listViewDataType pkbContentsList;
        PKB.FileEntry? PACFileInfo;

        public PKBWindow(PKB.FileEntry PKBFileInfo, B123ArchiveFile SourceArchiveFile)
        {
            InitializeComponent();

            Theme.loadTheme(this, "Theme_00.xaml");
            Theme.loadTheme(this, "Theme_Templates.xaml");
            Theme.applyTheme(this);
            this.PKBFileInfo = PKBFileInfo;
            this.SourceArchiveFile = SourceArchiveFile;
            UI_MainWindow.QueuedImports.Clear();
            pkbContentsList = new listViewDataType(MainWindow.Self, ref listView1);
            listView1.DataContext = pkbContentsList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs? e)
        {
            DarkTitleBar.Apply(this);
            textPKBPath.Text = SourceArchiveFile.FilePath.FullName;
            pkbContentsList.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            // disconnect event from the list while updating
            listView1.SelectionChanged -= listView1_SelectionChanged;
            for (int i = 0; i < PKBFileInfo.PKBContents.FolderContents.files.Count; i++)
            {
                var file = PKBFileInfo.PKBContents.FolderContents.files[i];
                var pkbitem = new listViewItemDataType(pkbContentsList, file.name, i.ToString());
                pkbitem.Tag = file;
                pkbitem.icon = UI_MainWindow.icon_zip;
                pkbitem.SubItems.Add(new listViewColumnDataType(pkbitem, file.offset.ToString()));
                pkbitem.SubItems.Add(new listViewColumnDataType(pkbitem, "0x" + file.offset.ToString("X8")));
                pkbitem.SubItems.Add(new listViewColumnDataType(pkbitem, file.size.ToString()));
                pkbitem.SubItems.Add(new listViewColumnDataType(pkbitem, MainWindow.BytesToHexString(PKBFileInfo.PKBContents.Identifiers[i].ID, "")));
                pkbitem.SubItems.Add(new listViewColumnDataType(pkbitem, PKBFileInfo.PKBContents.Identifiers[i].subID.ToString()));
                if (Properties.Settings.Default.LastLoadedPAC == file.name)
                    pkbContentsList.SelectedListItem = pkbitem;

                pkbContentsList.AddItem(pkbitem);
            }
            // Re-attach event
            listView1.SelectionChanged += listView1_SelectionChanged;
            listViewDataType.autoResizeListBoxCols(ref listView1,ref pkbContentsList);
            if (pkbContentsList.SelectedListItem != null)
            {
                listView1.ScrollIntoView(pkbContentsList.SelectedListItem);
            }
        }

        private async Task exploreSelectedPAC()
        {
            if (pkbContentsList.SelectedListItem == null) return;
            if (pkbContentsList.SelectedListItem.Tag == null) return;
            if (pkbContentsList.SelectedListItem.Tag?.GetType() != typeof(sFile)) return;
            sFile file = ((sFile?)pkbContentsList.SelectedListItem.Tag) ?? new sFile();
            if (file.path == "") return;

            // create a folder for the pkb contents
            string pkbContentsDir = PKBFileInfo.FileData.path.Replace(".pkb", "");
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);

            PACFileInfo = await PKB.ExtractPACFileFromPKB_Async(PKBFileInfo, file, pkbContentsDir);
            if (PACFileInfo == null)
            {
                return;
            }
            double left = -1;
            double top = -1;
            if (pacform != null)
            {
                left = pacform.Left;
                top = pacform.Top;
                pacform.Close();
            }
            pacform = new PACWindow(this, PKBFileInfo, PACFileInfo, SourceArchiveFile);
            if (left != -1)
            {
                pacform.WindowStartupLocation = WindowStartupLocation.Manual;
                pacform.Left = left;
                pacform.Top = top;
            }
            else
            {
                pacform.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            pacform.Owner = this;
            Properties.Settings.Default.LastLoadedPAC = file.name;
            Properties.Settings.Default.Save();
            pacform.Show();
            this.Activate();
        }

        public async Task ImportModifiedFile()
        {
            string pkbContentsDir = System.IO.Path.Combine(PKBFileInfo.FileData.path.Replace(".pkb", ""));
            await PKB.ImportDecompressedPACFile_Async(PKBFileInfo, PACFileInfo);
            Directory.Delete(pkbContentsDir, true);
            // delete old files and rename new files
            File.Delete(PKBFileInfo.FileData.path);
            File.Delete(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            File.Move(PKBFileInfo.FileData.path + "_modified", PKBFileInfo.FileData.path);
            File.Move(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh") + "_modified", PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            // reload the new pkb
            sFile pkbFile = new sFile() { path = PKBFileInfo.FileData.path, name = System.IO.Path.GetFileName(PKBFileInfo.FileData.path) };
            sFile pkhFile = new sFile() { path = PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"), name = System.IO.Path.GetFileName(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh")) };

            PKBFileInfo.PKBContents = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);

            Window_Loaded(this, null);
        }

        private async void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await exploreSelectedPAC();
        }

        private void btnImportPKB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private async void btnExplorePAC_Click(object sender, RoutedEventArgs e)
        {
            await exploreSelectedPAC();
        }

        private void btnExportPKB_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save your file";
            sfd.Filter = "Inazuma 11 PKB File (*.pkb)|.pkb";
            sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(PKBFileInfo.FileData.path);  // suggested default name
            sfd.DefaultExt = System.IO.Path.GetExtension(".pkb");
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.Copy(PKBFileInfo.FileData.path, sfd.FileName, true);
                    File.Copy(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"), sfd.FileName.Replace(".pkb", ".pkh"), true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was a problem exporting the files\n\n{ex.Message}", "Export Failed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MessageBox.Show($"The following files were exported succesfully\n\nPKB File:\n\n{sfd.FileName}\n\nPKH File:\n\n{sfd.FileName.Replace(".pkb", ".pkh")}", "Export Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void textSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            previousColour = textSearch.Foreground;
            textSearch.Foreground = (Brush?)MainWindow.Self?.FindResource("ControlTextActive");
            if (textSearch.Text == "Search By ID....")
                textSearch.Text = "";
        }

        private void textSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (pkbContentsList.Items == null || string.IsNullOrEmpty(textSearch.Text) && previousColour != null)
            {
                textSearch.Foreground = previousColour;
                textSearch.Text = "Search By ID....";
            }
            else
            {
                // search for the ID in the list items
                foreach (listViewItemDataType item in pkbContentsList.Items)
                {
                    if (item.SubItems[4].data.ToLower().Contains(textSearch.Text.ToLower()))
                    {
                        listView1.SelectedItem = null;
                        listView1.SelectedItem = item;
                        listView1.Focus();
                        MessageBox.Show($"A package was found with the ID {item.SubItems[4].data}", "Package Found!", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                MessageBox.Show($"A package with a simialar ID to {textSearch.Text} could not be found", "No Package Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void textSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                listView1.Focus();
        }
    }
}

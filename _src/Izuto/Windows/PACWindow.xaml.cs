using Ekona;
using Izuto.Controls;
using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.UI;
using Microsoft.Win32;
using plugin_level5.N3DS.Archive;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using static Izuto.PACStringReplacementOptionsWindow;
using static PAC;
using static System.Net.Mime.MediaTypeNames;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for PKBForm.xaml
    /// </summary>
    public partial class PACWindow : CustomWindowContentBase
    {
        PKB.FileEntry PKBFileInfo;
        PKB.FileEntry PACFileInfo;
        B123ArchiveFile SourceArchiveFile;
        PKBWindow? pkbForm;
        PACWindow? linkedTextForm;
        string LoadedPACID = "";
        PAC? PACData;
        LinkedScriptEntry? LinkedScript;
        public int LinkedScriptSize = 0;
        listViewDataType linkedScriptList;
        listViewDataType textScriptList;
        listViewDataType objectList;
        private const string PACFileFilter = "Inazuma 11 PAC Files (*.pac_)|*.pac_";
        public PACWindow(PKBWindow? pkbForm, PKB.FileEntry PKBFileInfo, PKB.FileEntry PACFileInfo, B123ArchiveFile SourceArchiveFile, LinkedScriptEntry? LinkedScript = null)
        {
            InitializeComponent();
            this.SourceArchiveFile = SourceArchiveFile;
            this.PKBFileInfo = PKBFileInfo;
            this.PACFileInfo = PACFileInfo;
            this.pkbForm = pkbForm;
            this.LinkedScript = LinkedScript;
            linkedScriptList = new listViewDataType(MainWindow.Self, ref listViewLinkedScripts);
            listViewLinkedScripts.DataContext = linkedScriptList;

            textScriptList = new listViewDataType(MainWindow.Self, ref listViewTextScripts);
            listViewTextScripts.DataContext = textScriptList;

            objectList = new listViewDataType(MainWindow.Self, ref listViewObjects);
            listViewObjects.DataContext = objectList;

            var pacItem = PKBFileInfo.PKBContents.FolderContents.files.FirstOrDefault(f => f.name.Equals(PACFileInfo.FileData.name.Replace("_decompressed", "")));
            var pacItemIndex = PKBFileInfo.PKBContents.FolderContents.files.IndexOf(pacItem);
            LoadedPACID = MainWindow.BytesToHexString(PKBFileInfo.PKBContents.Identifiers[pacItemIndex].ID);
        }

        private void Window_Loaded(object sender, RoutedEventArgs? e)
        {
            Title = "Izuto PAC Browser";
            Title += $" ({LoadedPACID})";
            if (LinkedScript != null)
                Title += " [Linked]";

            PACData = new PAC();
            if (!PACData.Load(PACFileInfo.FileData.path))
            {
                Close();
                return;
            }
            txtPACFilePath.Text = SourceArchiveFile.FilePath.FullName + ":" + PACFileInfo.FileData.name.Replace("_decompressed", "");

            objectList.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            for (int i = 0; i < PACData.BinaryEntries.Count; i++)
            {
                if (LinkedScript != null)
                    break; // not looking for binary entries
                var item = PACData.BinaryEntries[i];
                listViewItemDataType newItem = new listViewItemDataType(objectList, $"Item ID#{i}", i.ToString());
                newItem.Tag = item;
                newItem.icon = UI_MainWindow.icon_unknown;
                newItem.SubItems.Add(new listViewColumnDataType(newItem, item.FileSize.ToString()));
                newItem.SubItems.Add(new listViewColumnDataType(newItem, MainWindow.BytesToHexString(item.Data, " ")));
                objectList.AddItem(newItem);
            }

            linkedScriptList.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            textScriptList.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();

            int CurrentTextOffset = 0;
            for (int i = 0; i < PACData.StringEntries.Count; i++)
            {
                var item = PACData.StringEntries[i];
                listViewItemDataType newItem = new listViewItemDataType(linkedScriptList, $"Script ID#{item.ID}", i.ToString());
                newItem.Tag = item;
                newItem.icon = UI_MainWindow.icon_text;


                string ascii = Encoding.GetEncoding(932).GetString(item.TextBytes);
                newItem.SubItems.Add(new listViewColumnDataType(newItem, item.LineNumber.ToString()));
                newItem.SubItems.Add(new listViewColumnDataType(newItem, item.Text));
                newItem.SubItems.Add(new listViewColumnDataType(newItem, TextTranslation.ConvertBackTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, item.Text)));

                byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(item.Text);
                ushort StringSize = (ushort)(text.Length + 4 + (item.Data != null ? item.Data.Count() : 0));

                if (LinkedScript != null)
                {
                    // checking for linked item filter
                    bool showItem = false;
                    if (CurrentTextOffset >= LinkedScript.Offset && CurrentTextOffset < LinkedScript.Offset + LinkedScript.Size)
                    {
                        if (CurrentTextOffset + StringSize > LinkedScript.Offset + LinkedScript.Size)
                        {
                            throw new Exception("string does not match the expected size");
                        }
                        showItem = true;
                    }
                    CurrentTextOffset += StringSize;
                    LinkedScriptSize += StringSize;
                    if (!showItem)
                        continue;
                }
                newItem.SubItems.Add(new listViewColumnDataType(newItem, StringSize.ToString()));
                newItem.Tag = i;

                if (!item.IsLinked)
                {
                    newItem.parent = textScriptList;
                    newItem.SubItems.Add(new listViewColumnDataType(newItem, MainWindow.BytesToHexString(item.TextBytes, " ")));
                    if (item.Data == null)
                        newItem.SubItems.Add(new listViewColumnDataType(newItem, "N/A"));
                    else
                        newItem.SubItems.Add(new listViewColumnDataType(newItem, MainWindow.BytesToHexString(item.Data, " ")));
                    textScriptList.AddItem(newItem);
                }
                else
                {
                    if (LinkedScript != null)
                        continue; // already a linked script, can't be linked more
                    if (item.Data == null)
                        newItem.SubItems[3].data = "N/A";
                    else
                        newItem.SubItems[3].data = MainWindow.BytesToHexString(item.Data, " ");
                    linkedScriptList.AddItem(newItem);
                }
            }

            // Linked scripts tab visibility
            if (listViewLinkedScripts.Items.Count == 0 && tabsMain.Items.Contains(tabPageLinkedTextScripts))
            {
                tabsMain.Items.Remove(tabPageLinkedTextScripts);
            }
            else if (listViewLinkedScripts.Items.Count > 0 && !tabsMain.Items.Contains(tabPageLinkedTextScripts))
            {
                tabsMain.Items.Insert(0, tabPageLinkedTextScripts);
            }
            // Objects tab visibility
            if (tabsMain.Items.Count == 0 && tabsMain.Items.Contains(tabPageObjects))
            {
                tabsMain.Items.Remove(tabPageObjects);
            }
            else if (listViewObjects.Items.Count > 0 && !tabsMain.Items.Contains(tabPageObjects))
            {
                tabsMain.Items.Insert(0, tabPageObjects);
            }
            // Scripts tab visibility
            if (listViewTextScripts.Items.Count == 0 && tabsMain.Items.Contains(tabPageTextScripts))
            {
                tabsMain.Items.Remove(tabPageTextScripts);
            }
            else if (listViewTextScripts.Items.Count > 0 && !tabsMain.Items.Contains(tabPageTextScripts))
            {
                tabsMain.Items.Insert(0, tabPageTextScripts);
            }
        }

        private async void btnAccept_Click(object sender, RoutedEventArgs? e)
        {
            if (LinkedScript != null)
            {
                // calculate the new linked script total size
                LinkedScriptSize = 0;
                foreach (listViewItemDataType lvi in linkedScriptList.Items)
                {
                    LinkedScriptSize += int.Parse(lvi.SubItems[4].data);
                }
            }
            PACData.SaveAs(PACFileInfo.FileData.path + "_modified"); // overwite the original file loaded
            this.Close();
            if (LinkedScript == null)
                await pkbForm.ImportModifiedFile();
        }

        private void btnExportPAC_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save your file";

            sfd.Filter = PACFileFilter;
            sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(txtPACFilePath.Text).Split(":")[1];  // suggested default name
            sfd.DefaultExt = System.IO.Path.GetExtension(".pac_");
            if (sfd.ShowDialog() == true)
            {
                PACData.SaveAs(sfd.FileName);
            }
        }
        private ushort AlignStringTo4Bytes(ref string input)
        {
            string newText = input;
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(newText);
            int len = text.Count();

            ushort StringSize = (ushort)(len + 4);
            int remain = StringSize % 4;
            StringSize += (ushort)remain;
            ushort remained = (ushort)remain;
            while (remain > 0)
            {
                newText = newText + "\0";
                remain--;
            }
            input = newText;
            return remained;
        }

        private async void btnModifyLinkedScript_Click(object sender, RoutedEventArgs e)
        {

            if (listViewLinkedScripts.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewLinkedScripts.SelectedItems[0];
            if (selectedItem == null) return;

            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(int)) return;
            int index = ((int?)selectedItem.Tag) ?? -1;
            if (index >= PACData.StringEntries.Count) return;
            PAC.ScriptEntry entry = PACData.StringEntries[index];
            // get the source PAC file ID
            var pacFile = PKBFileInfo.PKBContents.FolderContents.files.FirstOrDefault(f => f.name.Equals(PACFileInfo.FileData.name.Replace("_decompressed", "")));
            int pacFileIndex = PKBFileInfo.PKBContents.FolderContents.files.IndexOf(pacFile);
            var identifier = PKBFileInfo.PKBContents.Identifiers[pacFileIndex];

            // get the offsets from the string
            LinkedScriptEntry linkedEntry = new LinkedScriptEntry(entry.Text);
            // try to find a corresponding text archive
            string textFileName = SourceArchiveFile.FilePath.FullName.Replace(".pkb", "t.pkb");
            B123ArchiveFile? textFile = UI_MainWindow.ArchiveFiles.Find(f => f.FilePath.FullName.Equals(textFileName));
            if (textFile == null)
            {
                MessageBox.Show($"No corresponding text PKB file was found for this record.\n\n{textFileName}", "Failed to find text archive", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // unpack the text archive
            PKB.FileEntry textPKBFileData = await PKB.UnpackPKBFromArchiveFA_Async(UI_MainWindow.LoadedArchiveFilePath, textFile, UI_MainWindow.CurrentWorkingDirectory);
            // search for the ID in the new archive
            var textPacIdentifiers = textPKBFileData.PKBContents.Identifiers.FindAll(
                tp => BitConverter.ToInt32(tp.ID, 0).ToString("X8").Equals(BitConverter.ToInt32(identifier.ID, 0).ToString("X8"))
                && tp.subID.Equals(identifier.subID)
                );

            if (textPacIdentifiers == null || textPacIdentifiers.Count == 0)
            {
                MessageBox.Show($"No corresponding PAC file was found for this record inside the PKB.\n\nID: {BitConverter.ToInt32(identifier.ID, 0).ToString("X8")}\nSub ID:{identifier.subID}", "Failed to find text archive", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (textPacIdentifiers.Count > 1)
            {
                MessageBox.Show($"Uh oh, multiple corresponding PAC files were found for this record inside the PKB.\n\nID: {BitConverter.ToInt32(identifier.ID, 0).ToString("X8")}\nSub ID:{identifier.subID}", "Failed to identify the text archive", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var textPacIdentifier = textPacIdentifiers[0];
            var textPacIndex = textPKBFileData.PKBContents.Identifiers.IndexOf(textPacIdentifier);
            sFile textPac = textPKBFileData.PKBContents.FolderContents.files[textPacIndex];

            // create a folder for the pkb
            string pkbContentsDir = System.IO.Path.Combine(textPKBFileData.FileData.path.Replace(".pkb", ""));
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);
            // extract the PAC file
            PKB.FileEntry textPACFileInfo = await PKB.ExtractPACFileFromPKB_Async(textPKBFileData, textPac, pkbContentsDir);

            if (textPACFileInfo == null)
                return;
            // launch another copy of this form
            double left = -1;
            double top = -1;
            if (linkedTextForm != null)
            {
                left = linkedTextForm.Window.Left;
                top = linkedTextForm.Window.Top;
                linkedTextForm.Close();
            }
            CustomWindow win = new CustomWindow(Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Resizable });
            linkedTextForm = new PACWindow(null, textPKBFileData, textPACFileInfo, SourceArchiveFile, linkedEntry);
            if (left != -1)
            {
                win.Owner = Window;
                win.Left = left;
                win.Top = top;
            }
            else
            {
                win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            win.ApplyContent(linkedTextForm);
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            win.Activate();

            if (linkedTextForm.DialogResult == false) return;


            await ImportModifiedLinkedScriptFile(textPKBFileData, textPACFileInfo);

            // calculate the size change
            int sizeChange = linkedTextForm.LinkedScriptSize - linkedEntry.Size;

            // update the size in the script of this entry
            entry.Text = $"@{linkedEntry.Offset},{linkedTextForm.LinkedScriptSize}\0";
            AlignStringTo4Bytes(ref entry.Text);


            // update all the other @ that need it
            foreach (var le in PACData.StringEntries.FindAll(se => se.IsLinked))
            {
                LinkedScriptEntry vals = new LinkedScriptEntry(le.Text);
                if (vals.Offset > linkedEntry.Offset)
                    vals.Offset += sizeChange;
                le.Text = $"@{vals.Offset},{vals.Size}";
                AlignStringTo4Bytes(ref le.Text);
            }

            // add the text pkb and pkh to the queue for importing
            UI_MainWindow.QueuedImports.Add(new Extensions.OptionsFileData.FileReplacementEntry() { RelativePath = textFile.FilePath.FullName, PathToReplace = textPKBFileData.FileData.path });
            UI_MainWindow.QueuedImports.Add(new Extensions.OptionsFileData.FileReplacementEntry() { RelativePath = textFile.FilePath.FullName.Replace(".pkb", ".pkh"), PathToReplace = textPKBFileData.FileData.path.Replace(".pkb", ".pkh") });

            // automatically save this form now
            btnAccept_Click(this,null);
        }

        private void btnModifyString_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTextScripts.SelectedItems.Count == 0) return;

            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewTextScripts.SelectedItems[0];
            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(int)) return;
            int index = ((int?)selectedItem.Tag) ?? -1;
            if (index >= PACData.StringEntries.Count) return;
            PAC.ScriptEntry entry = PACData.StringEntries[index];

            CustomWindow win = new CustomWindow(Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Fixed });
            StringWindow stringform = new StringWindow(entry.Text);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ApplyContent(stringform);
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            win.Activate();

            if (stringform.DialogResult == false) return;
            bool changed = entry.Text != stringform.ModifiedString;
            string newText = stringform.ModifiedString;
            AlignStringTo4Bytes(ref newText);
            ushort StringSize = (ushort)(Encoding.GetEncoding("shift_jis").GetBytes(newText).Count() + 4);
            entry.Text = newText;
            selectedItem.SubItems[2].data = entry.Text;
            selectedItem.SubItems[3].data = TextTranslation.ConvertBackTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, entry.Text);
            selectedItem.SubItems[4].data = StringSize.ToString();
            listViewTextScripts.DataContext = null;
            listViewTextScripts.DataContext = textScriptList;
        }


        public async Task ImportModifiedLinkedScriptFile(PKB.FileEntry PKBFileInfo, PKB.FileEntry pacFileInfo)
        {
            string pkbContentsDir = System.IO.Path.Combine(PKBFileInfo.FileData.path.Replace(".pkb", ""));
            await PKB.ImportDecompressedPACFile_Async(PKBFileInfo, pacFileInfo);
            Directory.Delete(pkbContentsDir, true);
            // delete old files and rename new files
            File.Delete(PKBFileInfo.FileData.path);
            File.Delete(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            File.Move(PKBFileInfo.FileData.path + "_modified", PKBFileInfo.FileData.path);
            File.Move(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh") + "_modified", PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

        }

        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            string importfn = UI_MainWindow.BrowseForFile("Inazuma 11 PKH File (*.pkh)|*.pkh", "Select a PKH file linked to a PKB file containing the same PAC ID");
            if (string.IsNullOrEmpty(importfn)) return;

            // open the pkb
            sFile pkhFile = new sFile()
            {
                path = importfn,
                name = System.IO.Path.GetFileName(importfn)
            };
            sFile pkbFile = new sFile()
            {
                path = importfn.Replace(".pkh", ".pkb"),
                name = System.IO.Path.GetFileName(importfn).Replace(".pkh", ".pkb")
            };
            if (!File.Exists(pkbFile.path))
            {
                MessageBox.Show($"A matching .pkb file was not found in the same directory as the .pkh", "Missing PKB File", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            INAZUMA11.PKB.PKBContents extractedPKBItems = new INAZUMA11.PKB.PKBContents();
            await Task.Run(() =>
            {
                extractedPKBItems = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);
            });
            PKB.FileEntry pkbSource = new PKB.FileEntry() { FileData = pkbFile, PKBContents = extractedPKBItems };

            // try to find a package with the same identifier
            var foundPackage = extractedPKBItems.Identifiers.FirstOrDefault((x) => MainWindow.BytesToHexString(x.ID).Equals(LoadedPACID));
            if (foundPackage == null)
            {
                MessageBox.Show($"A pac file with the ID {LoadedPACID} was not found inside the requested PKH / PKB file combination", "Matching PAC not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var sourcePacFile = extractedPKBItems.FolderContents.files[extractedPKBItems.Identifiers.IndexOf(foundPackage)];
            // create a folder for the pkb contents
            string tempDir = UI_MainWindow.CreateNewTempDirectory(false);
            string pkbContentsDir = System.IO.Path.Combine(tempDir, System.IO.Path.GetFileName(pkbFile.path).Replace(".pkb", ""));
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);

            // extract the pac file from the archive
            PKB.FileEntry? SourcePACFileInfo = await PKB.ExtractPACFileFromPKB_Async(pkbSource, sourcePacFile, pkbContentsDir);

            if (SourcePACFileInfo == null)
                return;

            // ask the user how to perform the changes
            PAC SourcePAC = new PAC();
            SourcePAC.Load(SourcePACFileInfo.FileData.path);

            CustomWindow win = new CustomWindow(Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Fixed });
            PACStringReplacementOptionsWindow f = new PACStringReplacementOptionsWindow(SourcePAC, PACData);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ApplyContent(f);
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            win.Activate();
            if (f.DialogResult == false) return;

            ReplacementOptionsType replacementOptions = f.ReplacementOptions;

            if (replacementOptions.ReplacementPriority == ReplacementPriorityType.Source)
            {
                PAC.ImportStringsFromPACSourcePriority(ref PACData, SourcePAC, replacementOptions.SourceTranslationFilePath);
            }
            else
            {
                PAC.ImportStringsFromPACDestinationPriority(ref PACData, SourcePAC, replacementOptions.SourceTranslationFilePath);
            }
            PACData.SaveAs(PACFileInfo.FileData.path);
            Window_Loaded(this, null);
        }
    }
}

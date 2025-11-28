using Ekona;
using INAZUMA11;
using Izuto.Extensions;
using Microsoft.VisualBasic;
using plugin_level5.N3DS.Archive;
using plugin_nintendo.Archives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Izuto.PACStringReplacementOptionsForm;
using static PAC;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Izuto
{
    public partial class PACForm : Form
    {
        PKB.FileEntry PKBFileInfo;
        PKB.FileEntry PACFileInfo;
        B123ArchiveFile SourceArchiveFile;
        PKBForm? pkbForm;
        PACForm? linkedTextForm;
        string LoadedPACID = "";
        PAC? PACData;
        LinkedScriptEntry? LinkedScript;
        public int LinkedScriptSize = 0;
        private const string PACFileFilter = "Inazuma 11 PAC Files (*.pac_)|*.pac_";

        public PACForm(PKBForm? pkbForm, PKB.FileEntry PKBFileInfo, PKB.FileEntry PACFileInfo, B123ArchiveFile SourceArchiveFile, LinkedScriptEntry? LinkedScript = null)
        {
            InitializeComponent();
            this.SourceArchiveFile = SourceArchiveFile;
            this.PKBFileInfo = PKBFileInfo;
            this.PACFileInfo = PACFileInfo;
            this.pkbForm = pkbForm;
            this.LinkedScript = LinkedScript;
            listView1.SmallImageList = MainForm.Self?.imgListFiles;
            listViewTextScripts.SmallImageList = MainForm.Self?.imgListFiles;

            var pacItem = PKBFileInfo.PKBContents.FolderContents.files.FirstOrDefault(f => f.name.Equals(PACFileInfo.FileData.name.Replace("_decompressed", "")));
            var pacItemIndex = PKBFileInfo.PKBContents.FolderContents.files.IndexOf(pacItem);
            LoadedPACID = MainForm.BytesToHexString(PKBFileInfo.PKBContents.Identifiers[pacItemIndex].ID);
            Text += $" ({LoadedPACID})";

            if (LinkedScript != null)
                Text += " [Linked]";
        }

        private void PACForm_Shown(object sender, EventArgs e)
        {
            PACData = new PAC();
            if (!PACData.Load(PACFileInfo.FileData.path))
            {
                Close();
                return;
            }
            txtPACFilePath.Text = SourceArchiveFile.FilePath.FullName + ":" + PACFileInfo.FileData.name.Replace("_decompressed", "");

            listView1.BeginUpdate();
            listView1.Items.Clear();
            for (int i = 0; i < PACData.BinaryEntries.Count; i++)
            {
                if (LinkedScript != null)
                    break; // not looking for binary entries
                var item = PACData.BinaryEntries[i];
                ListViewItem newItem = new ListViewItem() { Text = $"Item ID#{i}", Tag = item, ImageIndex = (int)MainForm.iconTypes.Unknown };
                newItem.SubItems.Add(item.FileSize.ToString());
                newItem.SubItems.Add(MainForm.BytesToHexString(item.Data, " "));
                listView1.Items.Add(newItem);
            }
            listView1.EndUpdate();

            listViewTextScripts.BeginUpdate();
            listViewTextScripts.Items.Clear();
            listViewLinkedScripts.BeginUpdate();
            listViewLinkedScripts.Items.Clear();

            int CurrentTextOffset = 0;
            for (int i = 0; i < PACData.StringEntries.Count; i++)
            {
                var item = PACData.StringEntries[i];
                ListViewItem newItem = new ListViewItem() { Text = $"Script ID#{item.ID}", Tag = item, ImageIndex = (int)MainForm.iconTypes.Txt };


                string ascii = Encoding.GetEncoding(932).GetString(item.TextBytes);
                newItem.SubItems.Add(item.LineNumber.ToString());
                newItem.SubItems.Add(item.Text);
                newItem.SubItems.Add(TextTranslation.ConvertBackTextString(MainForm.OptionsFile.Config.TranslationTable, item.Text));

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
                newItem.SubItems.Add(StringSize.ToString());
                newItem.Tag = i;

                if (!item.IsLinked)
                {
                    newItem.SubItems.Add(MainForm.BytesToHexString(item.TextBytes, " "));
                    if (item.Data == null)
                        newItem.SubItems.Add("N/A");
                    else
                        newItem.SubItems.Add(MainForm.BytesToHexString(item.Data, " "));
                    listViewTextScripts.Items.Add(newItem);
                }
                else
                {
                    if (LinkedScript != null)
                        continue; // already a linked script, can't be linked more
                    if(item.Data == null)
                        newItem.SubItems[3].Text = "N/A";
                    else
                        newItem.SubItems[3].Text = MainForm.BytesToHexString(item.Data, " ");
                    listViewLinkedScripts.Items.Add(newItem);
                }
            }
            listViewTextScripts.EndUpdate();
            listViewLinkedScripts.EndUpdate();
            // Linked scripts tab visibility
            if (listViewLinkedScripts.Items.Count == 0 && tabControl1.TabPages.Contains(tabPageLinkedTextScripts))
            {
                tabControl1.TabPages.Remove(tabPageLinkedTextScripts);
            }
            else if (listViewLinkedScripts.Items.Count > 0 && !tabControl1.TabPages.Contains(tabPageLinkedTextScripts))
            {
                tabControl1.TabPages.Insert(0, tabPageLinkedTextScripts);
            }
            // Objects tab visibility
            if (listView1.Items.Count == 0 && tabControl1.TabPages.Contains(tabPageObjects))
            {
                tabControl1.TabPages.Remove(tabPageObjects);
            }
            else if (listView1.Items.Count > 0 && !tabControl1.TabPages.Contains(tabPageObjects))
            {
                tabControl1.TabPages.Insert(0, tabPageObjects);
            }
            // Scripts tab visibility
            if (listViewTextScripts.Items.Count == 0 && tabControl1.TabPages.Contains(tabPageTextScripts))
            {
                tabControl1.TabPages.Remove(tabPageTextScripts);
            }
            else if (listViewTextScripts.Items.Count > 0 && !tabControl1.TabPages.Contains(tabPageTextScripts))
            {
                tabControl1.TabPages.Insert(0, tabPageTextScripts);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save your file";

                sfd.Filter = PACFileFilter;
                sfd.FileName = Path.GetFileNameWithoutExtension(txtPACFilePath.Text).Split(":")[1];  // suggested default name
                sfd.DefaultExt = Path.GetExtension(".pac_");
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    PACData.SaveAs(sfd.FileName);
                }
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


        private void btnModifyString_Click(object sender, EventArgs e)
        {
            if (listViewTextScripts.SelectedItems.Count == 0) return;
            if (listViewTextScripts.SelectedItems[0].Tag == null) return;
            if (listViewTextScripts.SelectedItems[0].Tag?.GetType() != typeof(int)) return;
            int index = ((int?)listViewTextScripts.SelectedItems[0].Tag) ?? -1;
            if (index >= PACData.StringEntries.Count) return;
            PAC.ScriptEntry entry = PACData.StringEntries[index];
            StringForm stringform = new StringForm(entry.Text);
            stringform.StartPosition = FormStartPosition.CenterParent;
            stringform.ShowDialog(this);
            if (stringform.DialogResult == DialogResult.Cancel) return;
            bool changed = entry.Text != stringform.ModifiedString;
            string newText = stringform.ModifiedString;
            AlignStringTo4Bytes(ref newText);
            ushort StringSize = (ushort)(Encoding.GetEncoding("shift_jis").GetBytes(newText).Count() + 4);
            entry.Text = newText;
            listViewTextScripts.SelectedItems[0].SubItems[2].Text = entry.Text;
            listViewTextScripts.SelectedItems[0].SubItems[3].Text = TextTranslation.ConvertBackTextString(MainForm.OptionsFile.Config.TranslationTable, entry.Text);
            listViewTextScripts.SelectedItems[0].SubItems[4].Text = StringSize.ToString();
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            if (LinkedScript != null)
            {
                // calculate the new linked script total size
                LinkedScriptSize = 0;
                foreach (ListViewItem lvi in listViewTextScripts.Items)
                {
                    LinkedScriptSize += int.Parse(lvi.SubItems[4].Text);
                }
            }
            PACData.SaveAs(PACFileInfo.FileData.path + "_modified"); // overwite the original file loaded
            this.DialogResult = DialogResult.OK;
            this.Close();
            if (LinkedScript == null)
                await pkbForm.ImportModifiedFile();
        }

        private async void btnModifyLinkedScript_Click(object sender, EventArgs e)
        {
            if (listViewLinkedScripts.SelectedItems.Count == 0) return;
            if (listViewLinkedScripts.SelectedItems[0].Tag == null) return;
            if (listViewLinkedScripts.SelectedItems[0].Tag?.GetType() != typeof(int)) return;
            int index = ((int?)listViewLinkedScripts.SelectedItems[0].Tag) ?? -1;
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
            B123ArchiveFile? textFile = MainForm.ArchiveFiles.Find(f => f.FilePath.FullName.Equals(textFileName));
            if (textFile == null)
            {
                MessageBox.Show($"No corresponding text PKB file was found for this record.\n\n{textFileName}", "Failed to find text archive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // unpack the text archive
            PKB.FileEntry textPKBFileData = await PKB.UnpackPKBFromArchiveFA_Async(MainForm.LoadedArchiveFilePath, textFile, MainForm.CurrentWorkingDirectory);
            // search for the ID in the new archive
            var textPacIdentifiers = textPKBFileData.PKBContents.Identifiers.FindAll(
                tp => BitConverter.ToInt32(tp.ID, 0).ToString("X8").Equals(BitConverter.ToInt32(identifier.ID, 0).ToString("X8"))
                && tp.subID.Equals(identifier.subID)
                );

            if (textPacIdentifiers == null || textPacIdentifiers.Count == 0)
            {
                MessageBox.Show($"No corresponding PAC file was found for this record inside the PKB.\n\nID: {BitConverter.ToInt32(identifier.ID, 0).ToString("X8")}\nSub ID:{identifier.subID}", "Failed to find text archive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textPacIdentifiers.Count > 1)
            {
                MessageBox.Show($"Uh oh, multiple corresponding PAC files were found for this record inside the PKB.\n\nID: {BitConverter.ToInt32(identifier.ID, 0).ToString("X8")}\nSub ID:{identifier.subID}", "Failed to identify the text archive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var textPacIdentifier = textPacIdentifiers[0];
            var textPacIndex = textPKBFileData.PKBContents.Identifiers.IndexOf(textPacIdentifier);
            sFile textPac = textPKBFileData.PKBContents.FolderContents.files[textPacIndex];

            // create a folder for the pkb
            string pkbContentsDir = Path.Combine(textPKBFileData.FileData.path.Replace(".pkb", ""));
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);
            // extract the PAC file
            PKB.FileEntry textPACFileInfo = await PKB.ExtractPACFileFromPKB_Async(textPKBFileData, textPac, pkbContentsDir);

            // launch another copy of this form
            int left = -1;
            int top = -1;
            if (linkedTextForm != null)
            {
                left = linkedTextForm.Left;
                top = linkedTextForm.Top;
                linkedTextForm.Close();
            }
            linkedTextForm = new PACForm(null, textPKBFileData, textPACFileInfo, SourceArchiveFile, linkedEntry);
            if (left != -1)
            {
                linkedTextForm.StartPosition = FormStartPosition.Manual;
                linkedTextForm.Location = new Point(left, top);
            }
            else
            {
                linkedTextForm.StartPosition = FormStartPosition.CenterParent;
            }
            linkedTextForm.ShowDialog(this);
            if (linkedTextForm.DialogResult == DialogResult.Cancel) return;


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
            MainForm.QueuedImports.Add(new Extensions.OptionsFileData.FileReplacementEntry() { RelativePath = textFile.FilePath.FullName, PathToReplace = textPKBFileData.FileData.path });
            MainForm.QueuedImports.Add(new Extensions.OptionsFileData.FileReplacementEntry() { RelativePath = textFile.FilePath.FullName.Replace(".pkb", ".pkh"), PathToReplace = textPKBFileData.FileData.path.Replace(".pkb", ".pkh") });

            // automatically save this form now
            btnAccept_Click(this, EventArgs.Empty);
        }


        public async Task ImportModifiedLinkedScriptFile(PKB.FileEntry PKBFileInfo, PKB.FileEntry pacFileInfo)
        {
            string pkbContentsDir = Path.Combine(PKBFileInfo.FileData.path.Replace(".pkb", ""));
            await PKB.ImportDecompressedPACFile_Async(PKBFileInfo, pacFileInfo);
            Directory.Delete(pkbContentsDir, true);
            // delete old files and rename new files
            File.Delete(PKBFileInfo.FileData.path);
            File.Delete(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            File.Move(PKBFileInfo.FileData.path + "_modified", PKBFileInfo.FileData.path);
            File.Move(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh") + "_modified", PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            string importfn = MainForm.BrowseForFile("Inazuma 11 PKH File (*.pkh)|*.pkh", "Select a PKH file linked to a PKB file containing the same PAC ID");
            if (string.IsNullOrEmpty(importfn)) return;
            
            // open the pkb
            sFile pkhFile = new sFile()
            {
                path = importfn,
                name = Path.GetFileName(importfn)
            };
            sFile pkbFile = new sFile()
            {
                path = importfn.Replace(".pkh", ".pkb"),
                name = Path.GetFileName(importfn).Replace(".pkh", ".pkb")
            }; 
            if (!File.Exists(pkbFile.path))
            {
                MessageBox.Show($"A matching .pkb file was not found in the same directory as the .pkh", "Missing PKB File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            INAZUMA11.PKB.PKBContents extractedPKBItems = new INAZUMA11.PKB.PKBContents();
            await Task.Run(() =>
            {
                extractedPKBItems = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);
            });
            PKB.FileEntry pkbSource = new PKB.FileEntry() { FileData = pkbFile, PKBContents = extractedPKBItems };

            // try to find a package with the same identifier
            var foundPackage = extractedPKBItems.Identifiers.FirstOrDefault((x) => MainForm.BytesToHexString(x.ID).Equals(LoadedPACID));
            if(foundPackage == null)
            {
                MessageBox.Show($"A pac file with the ID {LoadedPACID} was not found inside the requested PKH / PKB file combination", "Matching PAC not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var sourcePacFile = extractedPKBItems.FolderContents.files[extractedPKBItems.Identifiers.IndexOf(foundPackage)];
            // create a folder for the pkb contents
            string tempDir = MainForm.CreateNewTempDirectory(false);
            string pkbContentsDir = Path.Combine(tempDir, Path.GetFileName(pkbFile.path).Replace(".pkb", ""));
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);

            // extract the pac file from the archive
            PKB.FileEntry SourcePACFileInfo = await PKB.ExtractPACFileFromPKB_Async(pkbSource, sourcePacFile, pkbContentsDir);

            // ask the user how to perform the changes
            PAC SourcePAC = new PAC();
            SourcePAC.Load(SourcePACFileInfo.FileData.path);
            PACStringReplacementOptionsForm f = new PACStringReplacementOptionsForm(SourcePAC, PACData);
            f.ShowDialog(this);
            if (f.DialogResult == DialogResult.Cancel) return;

            ReplacmentOptionsType replacementOptions = f.ReplacementOptions;

            if(replacementOptions.ReplacementPriority == ReplacementPriorityType.Source)
            {
                PAC.ImportStringsFromPACSourcePriority(ref PACData, SourcePAC, replacementOptions.SourceTranslationFilePath);
            } else
            {
                PAC.ImportStringsFromPACDestinationPriority(ref PACData, SourcePAC, replacementOptions.SourceTranslationFilePath);
            }
            PACData.SaveAs(PACFileInfo.FileData.path);
            PACForm_Shown(this, EventArgs.Empty);
        }
    }
}

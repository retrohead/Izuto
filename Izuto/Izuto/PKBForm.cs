using Ekona;
using Izuto.Extensions;
using plugin_level5.N3DS.Archive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Izuto.MainForm;

namespace Izuto
{
    public partial class PKBForm : Form
    {
        PKB.FileEntry PKBFileInfo;
        B123ArchiveFile SourceArchiveFile;
        PACForm? pacform;

        public PKBForm(PKB.FileEntry PKBFileInfo, B123ArchiveFile SourceArchiveFile)
        {
            InitializeComponent();
            this.PKBFileInfo = PKBFileInfo;
            this.SourceArchiveFile = SourceArchiveFile;
            listView1.SmallImageList = MainForm.Self?.imgListFiles;
            MainForm.QueuedImports.Clear();
        }

        private void PKBForm_Shown(object sender, EventArgs e)
        {
            textBox1.Text = SourceArchiveFile.FilePath.FullName;
            listView1.BeginUpdate();
            listView1.Items.Clear();
            for(int i = 0; i < PKBFileInfo.PKBContents.FolderContents.files.Count; i++)
            {
                var file = PKBFileInfo.PKBContents.FolderContents.files[i];
                var pkbitem = new ListViewItem() { Text = file.name, Tag = file, ImageIndex = (int)iconTypes.Zip };
                pkbitem.SubItems.Add(file.offset.ToString());
                pkbitem.SubItems.Add("0x" + file.offset.ToString("X8"));
                pkbitem.SubItems.Add(file.size.ToString());

                string hex = "";
                for(int j=0;j<4;j++)
                    hex += PKBFileInfo.PKBContents.Identifiers[i].ID[j].ToString("X2");

                pkbitem.SubItems.Add(hex);
                pkbitem.SubItems.Add(PKBFileInfo.PKBContents.Identifiers[i].subID.ToString());
                listView1.Items.Add(pkbitem);
            }

            listView1.EndUpdate();
        }
        PKB.FileEntry? PACFileInfo;
        private async Task exploreSelectedPKB()
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (listView1.SelectedItems[0].Tag == null) return;
            if (listView1.SelectedItems[0].Tag?.GetType() != typeof(sFile)) return;
            sFile file = ((sFile?)listView1.SelectedItems[0].Tag) ?? new sFile();
            if (file.path == "") return;

            // create a folder for the pkb
            string pkbContentsDir = Path.Combine(PKBFileInfo.FileData.path.Replace(".pkb", ""));
            if (!Directory.Exists(pkbContentsDir))
                Directory.CreateDirectory(pkbContentsDir);

            PACFileInfo = await PKB.ExtractPACFileFromPKB_Async(PKBFileInfo, file, pkbContentsDir);

            int left = -1;
            int top = -1;
            if (pacform != null)
            {
                left = pacform.Left;
                top = pacform.Top;
                pacform.Close();
            }
            pacform = new PACForm(this, PKBFileInfo, PACFileInfo, SourceArchiveFile);
            if (left != -1)
            {
                pacform.StartPosition = FormStartPosition.Manual;
                pacform.Location = new Point(left, top);
            }
            else
            {
                pacform.StartPosition = FormStartPosition.CenterParent;
            }
            pacform.Show(this);
            this.Activate();
        }

        public async Task ImportModifiedFile()
        {
            string pkbContentsDir = Path.Combine(PKBFileInfo.FileData.path.Replace(".pkb", ""));
            await PKB.ImportDecompressedPACFile_Async(PKBFileInfo, PACFileInfo);
            Directory.Delete(pkbContentsDir, true);
            // delete old files and rename new files
            File.Delete(PKBFileInfo.FileData.path);
            File.Delete(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            File.Move(PKBFileInfo.FileData.path + "_modified", PKBFileInfo.FileData.path);
            File.Move(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh") + "_modified", PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"));

            // reload the new pkb
            sFile pkbFile = new sFile() { path = PKBFileInfo.FileData.path, name = Path.GetFileName(PKBFileInfo.FileData.path) };
            sFile pkhFile = new sFile() { path = PKBFileInfo.FileData.path.Replace(".pkb", ".pkh"), name = Path.GetFileName(PKBFileInfo.FileData.path.Replace(".pkb", ".pkh")) };

            PKBFileInfo.PKBContents = INAZUMA11.PKB.Unpack(pkbFile, pkhFile);

            PKBForm_Shown(this, EventArgs.Empty);
        }

        private async void btnExplorePAC_Click(object sender, EventArgs e)
        {
            await exploreSelectedPKB();
        }

        private void btnImportPKB_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            exploreSelectedPKB();
        }
    }
}

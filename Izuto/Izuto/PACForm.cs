using Ekona;
using INAZUMA11;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Izuto
{
    public partial class PACForm : Form
    {
        PKB.FileEntry PKBFileInfo;
        PKB.FileEntry PACFileInfo;
        B123ArchiveFile SourceArchiveFile;
        PKBForm pkbForm;

        PAC? PACData;
        public PACForm(MainForm mainForm, PKBForm pkbForm, PKB.FileEntry PKBFileInfo, PKB.FileEntry PACFileInfo, B123ArchiveFile SourceArchiveFile)
        {
            InitializeComponent();
            this.SourceArchiveFile = SourceArchiveFile;
            this.PKBFileInfo = PKBFileInfo;
            this.PACFileInfo = PACFileInfo;
            this.pkbForm = pkbForm;
            listView1.SmallImageList = mainForm.imgListFiles;
            listViewFiles.SmallImageList = mainForm.imgListFiles;
        }

        private void PACForm_Shown(object sender, EventArgs e)
        {
            PACData = new PAC();
            PACData.Load(PACFileInfo.FileData.path);

            txtPACFilePath.Text = SourceArchiveFile.FilePath.FullName + ":" + PACFileInfo.FileData.name.Replace("_decompressed", "");

            listView1.BeginUpdate();
            listView1.Items.Clear();
            for (int i = 0; i < PACData.BinaryEntries.Count; i++)
            {
                var item = PACData.BinaryEntries[i];
                ListViewItem newItem = new ListViewItem() { Text = $"Item ID#{i}", Tag = item, ImageIndex = (int)MainForm.iconTypes.Unknown };
                newItem.SubItems.Add(item.FileSize.ToString());
                listView1.Items.Add(newItem);
            }
            listView1.EndUpdate();

            listViewFiles.BeginUpdate();
            listViewFiles.Items.Clear();

            for (int i = 0; i < PACData.StringEntries.Count; i++)
            {
                var item = PACData.StringEntries[i];
                ListViewItem newItem = new ListViewItem() { Text = $"Script ID#{item.ID}", Tag = item, ImageIndex = (int)MainForm.iconTypes.Txt };
                newItem.SubItems.Add(item.LineNumber.ToString());
                newItem.SubItems.Add(item.Text);
                newItem.SubItems.Add(MainForm.OptionsFile.ConvertBackTextString(item.Text));
                newItem.Tag = i;
                listViewFiles.Items.Add(newItem);
            }
            listViewFiles.EndUpdate();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save your file";

                sfd.Filter = "Inazuma 11 PAC Files (*.pac_)|*.pac_";
                sfd.FileName = Path.GetFileNameWithoutExtension(txtPACFilePath.Text).Split(":")[1];  // suggested default name
                sfd.DefaultExt = Path.GetExtension(".pac_");
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    PACData.SaveAs(sfd.FileName);
                }
            }
        }

        private void btnModifyString_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            if (listViewFiles.SelectedItems[0].Tag == null) return;
            if (listViewFiles.SelectedItems[0].Tag?.GetType() != typeof(int)) return;
            int index = ((int?)listViewFiles.SelectedItems[0].Tag) ?? -1;
            if (index >= PACData.StringEntries.Count) return;
            PAC.ScriptEntry entry = PACData.StringEntries[index];
            StringForm stringform = new StringForm(entry.Text);
            stringform.StartPosition = FormStartPosition.CenterParent;
            stringform.ShowDialog(this);
            if (stringform.DialogResult == DialogResult.Cancel) return;
            bool changed = entry.Text != stringform.ModifiedString;
            entry.Text = stringform.ModifiedString;
            listViewFiles.SelectedItems[0].SubItems[2].Text = entry.Text;
            listViewFiles.SelectedItems[0].SubItems[3].Text = MainForm.OptionsFile.ConvertBackTextString(entry.Text);
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            PACData.SaveAs(PACFileInfo.FileData.path + "_modified"); // overwite the original file loaded
            this.DialogResult = DialogResult.OK;
            this.Close();
            await pkbForm.ImportModifiedFile();
        }

    }
}

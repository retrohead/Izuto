using Izuto.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Izuto
{
    public partial class ReplaceFileForm : Form
    {
        public OptionsFileData.FileReplacementEntry? FileReplacement;
        public ReplaceFileForm(OptionsFileData.FileReplacementEntry? FileReplacement)
        {
            InitializeComponent();
            this.FileReplacement = FileReplacement;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (FileReplacement == null)
                FileReplacement = new OptionsFileData.FileReplacementEntry();
            FileReplacement.PathToReplace = textOriginalFilePath.Text;
            FileReplacement.RelativePath = Path.GetRelativePath(Path.GetDirectoryName(MainForm.OptionsFile.FilePath), textReplacementFile.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowsePKB_Click(object sender, EventArgs e)
        {
            PKBFileSelectForm f = new PKBFileSelectForm();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.Cancel) return;
            textOriginalFilePath.Text = f.SelectedFilePath;
        }

        private void btnBrowseLocal_Click(object sender, EventArgs e)
        {
            string path = MainForm.BrowseForFile();
            if (path != "")
                textReplacementFile.Text = path;

        }
    }
}

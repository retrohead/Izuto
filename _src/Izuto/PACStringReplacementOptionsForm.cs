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
    public partial class PACStringReplacementOptionsForm : Form
    {
        public enum ReplacementPriorityType
        {
            LoadedFile,
            Source
        }
        public class ReplacmentOptionsType
        {
            public ReplacementPriorityType ReplacementPriority = ReplacementPriorityType.Source;
            public string SourceTranslationFilePath = "";
        }

        public ReplacmentOptionsType ReplacementOptions = new ReplacmentOptionsType();

        public PACStringReplacementOptionsForm(PAC SourcePAC, PAC LoadedPAC)
        {
            InitializeComponent();
            textMessage.Text =
                "Everything seems to be going well!" + Environment.NewLine
                + Environment.NewLine
                + "I found a package with the same ID as the one loaded. All that remains is for you to choose your transfer option and an optional Izuto configuration file to use when loading the file." + Environment.NewLine
                + Environment.NewLine
                + $"Source String Count: {SourcePAC.StringEntries.FindAll(s => !s.IsLinked).Count()}" + Environment.NewLine
                + $"Loaded File String Count {LoadedPAC.StringEntries.FindAll(s => !s.IsLinked).Count()}";
            radioSource.Checked = Properties.Settings.Default.ImportPACOption == (int)ReplacementPriorityType.Source;
            textTranslateFile.Text = Properties.Settings.Default.TranslateSourceFilePath;
            checkTextTranslateSource.Checked = !string.IsNullOrEmpty(Properties.Settings.Default.TranslateSourceFilePath);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ReplacementOptions.ReplacementPriority = ReplacementPriorityType.LoadedFile;
            if (radioSource.Checked)
                ReplacementOptions.ReplacementPriority = ReplacementPriorityType.Source;
            ReplacementOptions.SourceTranslationFilePath = "";
            if (checkTextTranslateSource.Checked)
            {
                if (string.IsNullOrEmpty(textTranslateFile.Text))
                {
                    MessageBox.Show("You must select a translation options file when choosing to translate the source", "Source Translation Options File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!File.Exists(textTranslateFile.Text))
                {
                    MessageBox.Show("The selected translation options file does not exist", "Source Translation Options File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ReplacementOptions.SourceTranslationFilePath = textTranslateFile.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkTextTranslateSource_CheckedChanged(object sender, EventArgs e)
        {
            textTranslateFile.Visible = checkTextTranslateSource.Checked;
            btnBrowseOptionsFile.Visible = checkTextTranslateSource.Checked;
            textTranslateFile.Enabled = checkTextTranslateSource.Checked;
            btnBrowseOptionsFile.Enabled = checkTextTranslateSource.Checked;
            if(checkTextTranslateSource.Checked && textTranslateFile.Text == "")
            {
                btnBrowseOptionsFile_Click(this, EventArgs.Empty);
            }
        }

        private void btnBrowseOptionsFile_Click(object sender, EventArgs e)
        {
            string translateOptionsFile = MainForm.BrowseForFile(OptionsFileData.OptionsFileFilter);
            if (translateOptionsFile == "")
            {
                checkTextTranslateSource.Checked = false;
                return;
            }
            textTranslateFile.Text = translateOptionsFile;
            Properties.Settings.Default.TranslateSourceFilePath = translateOptionsFile;
            Properties.Settings.Default.ImportPACOption = (int)(radioSource.Checked ? ReplacementPriorityType.Source : ReplacementPriorityType.LoadedFile);
            Properties.Settings.Default.Save();
        }
    }
}

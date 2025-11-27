using Ekona;
using Izuto.Extensions;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Izuto.Extensions.OptionsFileData;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Izuto
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void btnBrowseFont_Click(object sender, EventArgs e)
        {
            string fontConfigPath = MainForm.BrowseForFile("Font Configuration File (*.json)|*.json", "Select a font configuration file");
            textOptionsFilePath.Text = "";
            if (fontConfigPath == "")
                return;
            if (!MainForm.OptionsFile.Load(fontConfigPath))
            {
                MessageBox.Show("Failed to load the font configuration file", "Invalid font configuration file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return;
            }
            FontForm_Shown(this, EventArgs.Empty);
        }

        int selectedTranslationsIndex = -1;
        int selectedFileReplacementIndex = -1;
        private void FontForm_Shown(object sender, EventArgs e)
        {
            textOptionsFilePath.Text = MainForm.OptionsFile.FilePath;

            // loading text translations
            listViewTextTranslation.BeginUpdate();
            listViewTextTranslation.Items.Clear();
            {
                foreach (var textTranslation in MainForm.OptionsFile.Config.TranslationTable)
                {
                    var newItem = new ListViewItem() { Text = textTranslation.Syllable, Tag = textTranslation };
                    newItem.SubItems.Add(textTranslation.BytesString);
                    newItem.SubItems.Add(BitConverter.ToString(Encoding.UTF8.GetBytes(textTranslation.Syllable)).Replace("-", " "));
                    newItem.SubItems.Add(BitConverter.ToString(textTranslation.GetBytes()).Replace("-", " "));
                    listViewTextTranslation.Items.Add(newItem);
                    if (listViewTextTranslation.Items.Count == selectedTranslationsIndex + 1)
                    {
                        listViewTextTranslation.Items[selectedTranslationsIndex].Selected = true;
                        listViewTextTranslation.Items[selectedTranslationsIndex].EnsureVisible();
                    }
                }
            }
            listViewTextTranslation.EndUpdate();

            // loading file replacements
            if (listViewFileReplacements.SelectedItems.Count > 0)
                selectedFileReplacementIndex = listViewFileReplacements.Items.IndexOf(listViewFileReplacements.SelectedItems[0]);
            listViewFileReplacements.BeginUpdate();
            listViewFileReplacements.Items.Clear();
            {
                foreach (var replacementFile in MainForm.OptionsFile.Config.FileReplacements)
                {
                    var newItem = new ListViewItem() { Text = replacementFile.PathToReplace, Tag = replacementFile };
                    newItem.SubItems.Add(MainForm.OptionsFile.GetFileActualPath(replacementFile));
                    listViewFileReplacements.Items.Add(newItem);
                    if (listViewFileReplacements.Items.Count == selectedFileReplacementIndex + 1)
                    {
                        listViewFileReplacements.Items[selectedFileReplacementIndex].Selected = true;
                        listViewFileReplacements.Items[selectedFileReplacementIndex].EnsureVisible();
                    }
                }
            }
            listViewFileReplacements.EndUpdate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.OptionsFile.Save();
            Properties.Settings.Default.OptionsFilePath = MainForm.OptionsFile.FilePath;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnAddTextTranslation_Click(object sender, EventArgs e)
        {
            TextTranslationForm translationForm = new TextTranslationForm(new TranslationEntry());
            translationForm.ShowDialog();
            if (translationForm.DialogResult == DialogResult.Cancel) return;
            MainForm.OptionsFile.Config.TranslationTable.Add(translationForm.FontTranslation);
            selectedTranslationsIndex = MainForm.OptionsFile.Config.TranslationTable.IndexOf(translationForm.FontTranslation);
            FontForm_Shown(this, EventArgs.Empty);
            listViewTextTranslation.Focus();
        }

        private void btnRemoveTextTranslation_Click(object sender, EventArgs e)
        {
            if (listViewTextTranslation.SelectedItems.Count == 0) return;
            if (listViewTextTranslation.SelectedItems[0].Tag == null) return;
            if (listViewTextTranslation.SelectedItems[0].Tag?.GetType() != typeof(TranslationEntry)) return;
            TranslationEntry entry = ((TranslationEntry?)listViewTextTranslation.SelectedItems[0].Tag) ?? new TranslationEntry();
            if (entry.Syllable == "") return;

            if (MessageBox.Show("Are you sure you want to remove the selected translation?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int thisPos = MainForm.OptionsFile.Config.TranslationTable.IndexOf(entry);
            if (thisPos < MainForm.OptionsFile.Config.TranslationTable.Count - 1)
            {
                listViewTextTranslation.Items[thisPos + 1].Selected = true;
                selectedTranslationsIndex = thisPos;
            }
            else if (thisPos > 0)
            {
                listViewTextTranslation.Items[thisPos - 1].Selected = true;
                selectedTranslationsIndex = thisPos - 1;
            }
            MainForm.OptionsFile.Config.TranslationTable.Remove(entry);
            FontForm_Shown(this, EventArgs.Empty);
            listViewTextTranslation.Focus();
        }

        private void btnModifyTextTranslation_Click(object sender, EventArgs e)
        {
            if (listViewTextTranslation.SelectedItems.Count == 0) return;
            if (listViewTextTranslation.SelectedItems[0].Tag == null) return;
            if (listViewTextTranslation.SelectedItems[0].Tag?.GetType() != typeof(TranslationEntry)) return;
            TranslationEntry entry = ((TranslationEntry?)listViewTextTranslation.SelectedItems[0].Tag) ?? new TranslationEntry();
            if (entry.Syllable == "") return;
            TextTranslationForm translationForm = new TextTranslationForm(entry);
            translationForm.ShowDialog();
            if (translationForm.DialogResult == DialogResult.Cancel) return;
            entry = translationForm.FontTranslation;
            selectedTranslationsIndex = MainForm.OptionsFile.Config.TranslationTable.IndexOf(entry);
            FontForm_Shown(this, EventArgs.Empty);
            listViewTextTranslation.Focus();
        }

        private void btnAddFileReplacement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MainForm.LoadedArchiveFilePath))
            {
                MessageBox.Show("You must load an archive file before you can add file replacements", "No Archive Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MainForm.OptionsFile.Config == null || !File.Exists(MainForm.OptionsFile.FilePath))
            {
                MessageBox.Show("You must save or load an existing options file before you can add file replacements", "No Options File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ReplaceFileForm f = new ReplaceFileForm(new FileReplacementEntry());
            f.ShowDialog();
            if (f.DialogResult == DialogResult.Cancel) return;
            if(f.FileReplacement == null) return;

            MainForm.OptionsFile.Config.FileReplacements.Add(f.FileReplacement);
            selectedFileReplacementIndex = MainForm.OptionsFile.Config.FileReplacements.IndexOf(f.FileReplacement);
            FontForm_Shown(this, EventArgs.Empty);
            listViewTextTranslation.Focus();
        }

        private void btnRemoveFileReplacement_Click(object sender, EventArgs e)
        {
            if (listViewFileReplacements.SelectedItems.Count == 0) return;
            if (listViewFileReplacements.SelectedItems[0].Tag == null) return;
            if (listViewFileReplacements.SelectedItems[0].Tag?.GetType() != typeof(FileReplacementEntry)) return;
            FileReplacementEntry entry = ((FileReplacementEntry?)listViewFileReplacements.SelectedItems[0].Tag) ?? new FileReplacementEntry();
            if (entry.PathToReplace == "") return;
            if (MessageBox.Show("Are you sure you want to remove the selected file replacement?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int thisPos = MainForm.OptionsFile.Config.FileReplacements.IndexOf(entry);
            if (thisPos < MainForm.OptionsFile.Config.FileReplacements.Count - 1)
            {
                listViewFileReplacements.Items[thisPos + 1].Selected = true;
                selectedFileReplacementIndex = thisPos;
            }
            else if (thisPos > 0)
            {
                listViewFileReplacements.Items[thisPos - 1].Selected = true;
                selectedFileReplacementIndex = thisPos - 1;
            }
            MainForm.OptionsFile.Config.FileReplacements.Remove(entry);
            FontForm_Shown(this, EventArgs.Empty);
            listViewFileReplacements.Focus();
        }
    }
}

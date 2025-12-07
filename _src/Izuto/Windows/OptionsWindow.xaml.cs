using Izuto.Extensions;
using Izuto.UI;
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
using Xceed.Wpf.AvalonDock.Themes;
using static Izuto.Extensions.OptionsFileData;
using static Izuto.Extensions.TextTranslation;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        listViewDataType textTranslationListData;
        listViewDataType fileReplacementListData;

        int selectedTranslationsIndex = -1;
        int selectedFileReplacementIndex = -1;

        public OptionsWindow()
        {
            InitializeComponent();
            Theme.loadTheme(this, "Theme_00.xaml");
            Theme.loadTheme(this, "Theme_Templates.xaml");
            Theme.applyTheme(this);

            textTranslationListData = new listViewDataType(MainWindow.Self, ref listViewTextTranslation);
            listViewTextTranslation.DataContext = textTranslationListData;

            fileReplacementListData = new listViewDataType(MainWindow.Self, ref listViewFileReplacements);
            listViewFileReplacements.DataContext = fileReplacementListData;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!UI_MainWindow.OptionsFile.Save())
                return;
            Properties.Settings.Default.OptionsFilePath = UI_MainWindow.OptionsFile.FilePath;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (!UI_MainWindow.OptionsFile.Save(""))
                return;
            Properties.Settings.Default.OptionsFilePath = UI_MainWindow.OptionsFile.FilePath;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnBrowseOptionsFile_Click(object sender, RoutedEventArgs e)
        {
            string fontConfigPath = UI_MainWindow.BrowseForFile("Izuto Configuration File (*.json)|*.json", "Select a font configuration file");
            textOptionsFilePath.Text = "";
            if (fontConfigPath == "")
                return;
            if (!UI_MainWindow.OptionsFile.Load(fontConfigPath))
            {
                MessageBox.Show("Failed to load the Izuto configuration file", "Invalid Izuto configuration file", MessageBoxButton.OK, MessageBoxImage.Exclamation); 
                return;
            }
            Window_Loaded(this, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs? e)
        {
            DarkTitleBar.Apply(this);
            textOptionsFilePath.Text = UI_MainWindow.OptionsFile.FilePath;

            // loading text translations
            textTranslationListData.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            {
                for (var i=0;i< UI_MainWindow.OptionsFile.Config.TranslationTable.Count(); i++)
                {
                    var textTranslation = UI_MainWindow.OptionsFile.Config.TranslationTable[i];
                    var newItem = new listViewItemDataType(textTranslationListData, textTranslation.Syllable, i.ToString());
                    newItem.Tag = textTranslation;

                    newItem.SubItems.Add(new listViewColumnDataType(newItem, textTranslation.BytesString));
                    newItem.SubItems.Add(new listViewColumnDataType(newItem, MainWindow.BytesToHexString(Encoding.UTF8.GetBytes(textTranslation.Syllable), " ")));

                    var bytes = textTranslation.GetBytes();
                    newItem.SubItems.Add(new listViewColumnDataType(newItem, MainWindow.BytesToHexString(bytes, " ")));

                    newItem.SubItems.Add(new listViewColumnDataType(newItem, textTranslation.UnicodeCodePoint));

                    textTranslationListData.Items.Add(newItem);
                    if (textTranslationListData.Items.Count == selectedTranslationsIndex + 1)
                    {
                        textTranslationListData.SelectedListItem = newItem;
                    }
                }
            }
            if(textTranslationListData.SelectedListItem != null)
                listViewTextTranslation.ScrollIntoView(textTranslationListData.SelectedListItem);
            listViewDataType.autoResizeListBoxCols(ref listViewTextTranslation, ref textTranslationListData);

            // loading file replacements
            fileReplacementListData.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            {
                for (var i = 0; i < UI_MainWindow.OptionsFile.Config.FileReplacements.Count(); i++)
                {
                    var fileReplacement = UI_MainWindow.OptionsFile.Config.FileReplacements[i];
                    var newItem = new listViewItemDataType(fileReplacementListData, fileReplacement.PathToReplace, i.ToString());
                    newItem.Tag = fileReplacement;
                    newItem.SubItems.Add(new listViewColumnDataType(newItem, UI_MainWindow.OptionsFile.GetFileActualPath(fileReplacement)));
                    fileReplacementListData.Items.Add(newItem);
                    if (fileReplacementListData.Items.Count == selectedFileReplacementIndex + 1)
                    {
                        listViewFileReplacements.SelectedItem = listViewFileReplacements.Items[selectedFileReplacementIndex];
                    }
                }
            }
            if (fileReplacementListData.SelectedListItem != null)
                listViewFileReplacements.ScrollIntoView(fileReplacementListData.SelectedListItem);
            listViewDataType.autoResizeListBoxCols(ref listViewFileReplacements, ref fileReplacementListData);
        }
        private void btnAddFileReplacement_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UI_MainWindow.LoadedArchiveFilePath))
            {
                MessageBox.Show("You must load an archive file before you can add file replacements", "No Archive Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (UI_MainWindow.OptionsFile.Config == null || !File.Exists(UI_MainWindow.OptionsFile.FilePath))
            {
                MessageBox.Show("You must save or load an existing options file before you can add file replacements", "No Options File Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ReplaceFileWindow f = new ReplaceFileWindow(new FileReplacementEntry());
            f.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            f.Owner = this;
            f.ShowDialog();
            if (f.DialogResult == false) return;
            if (f.FileReplacement == null) return;

            UI_MainWindow.OptionsFile.Config.FileReplacements.Add(f.FileReplacement);
            selectedFileReplacementIndex = UI_MainWindow.OptionsFile.Config.FileReplacements.IndexOf(f.FileReplacement);
            Window_Loaded(this, null);
            listViewTextTranslation.Focus();
        }

        private void btnRemoveFileReplacement_Click(object sender, RoutedEventArgs e)
        {
            if (listViewFileReplacements.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewFileReplacements.SelectedItems[0];

            if (selectedItem == null) return; 
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(FileReplacementEntry)) return;
            FileReplacementEntry entry = (FileReplacementEntry)selectedItem.Tag ?? new FileReplacementEntry();
            if (entry.PathToReplace == "") return;
            if (MessageBox.Show("Are you sure you want to remove the selected file replacement?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            int thisPos = UI_MainWindow.OptionsFile.Config.FileReplacements.IndexOf(entry);
            if (thisPos < UI_MainWindow.OptionsFile.Config.FileReplacements.Count - 1)
            {
                listViewFileReplacements.SelectedItems.Clear();
                listViewFileReplacements.SelectedItems.Add(listViewFileReplacements.Items[thisPos + 1]);
                selectedFileReplacementIndex = thisPos;
            }
            else if (thisPos > 0)
            {
                listViewFileReplacements.SelectedItems.Clear();
                listViewFileReplacements.SelectedItems.Add(listViewFileReplacements.Items[thisPos - 1]);
                selectedFileReplacementIndex = thisPos - 1;
            }
            UI_MainWindow.OptionsFile.Config.FileReplacements.Remove(entry);
            Window_Loaded(this, null);
            listViewFileReplacements.Focus();
        }

        private void btnRemoveAllFileReplacements_Click(object sender, RoutedEventArgs e)
        {
            if (listViewFileReplacements.SelectedItems.Count == 0) return;
            if (MessageBox.Show("Are you sure you want to remove all of the file replacements?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            UI_MainWindow.OptionsFile.Config.FileReplacements.Clear();
            Window_Loaded(this, null);
            listViewFileReplacements.Focus();
        }

        private void btnModifyFileReplacement_Click(object sender, RoutedEventArgs e)
        {
            if (listViewFileReplacements.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewFileReplacements.SelectedItems[0];
            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(FileReplacementEntry)) return;
            FileReplacementEntry entry = ((FileReplacementEntry?)selectedItem.Tag) ?? new FileReplacementEntry();
            if (entry.PathToReplace == "") return;
            ReplaceFileWindow replaceForm = new ReplaceFileWindow(entry);
            replaceForm.Owner = this;
            replaceForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            replaceForm.ShowDialog();
            if (replaceForm.DialogResult == false) return;
            entry = replaceForm.FileReplacement;
            selectedFileReplacementIndex = UI_MainWindow.OptionsFile.Config.FileReplacements.IndexOf(entry);
            Window_Loaded(this, null);
            listViewFileReplacements.Focus();
        }


        private void btnAddTextTranslation_Click(object sender, EventArgs e)
        {
            TextTranslationWindow translationForm = new TextTranslationWindow(new TranslationEntry());
            translationForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            translationForm.Owner = this;
            translationForm.ShowDialog();
            if (translationForm.DialogResult == false) return;
            UI_MainWindow.OptionsFile.Config.TranslationTable.Add(translationForm.FontTranslation);
            selectedTranslationsIndex = UI_MainWindow.OptionsFile.Config.TranslationTable.IndexOf(translationForm.FontTranslation);
            Window_Loaded(this, null);
            listViewTextTranslation.Focus();
        }

        private void btnRemoveTextTranslation_Click(object sender, EventArgs e)
        {
            if (listViewTextTranslation.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewTextTranslation.SelectedItems[0];

            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(TranslationEntry)) return;
            TranslationEntry entry = ((TranslationEntry?)selectedItem.Tag) ?? new TranslationEntry();
            if (entry.Syllable == "") return;

            if (MessageBox.Show("Are you sure you want to remove the selected text translation?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            int thisPos = UI_MainWindow.OptionsFile.Config.TranslationTable.IndexOf(entry);
            if (thisPos < UI_MainWindow.OptionsFile.Config.TranslationTable.Count - 1)
            {
                listViewTextTranslation.SelectedItems.Clear();
                listViewTextTranslation.SelectedItems.Add(listViewTextTranslation.Items[thisPos + 1]);
                selectedTranslationsIndex = thisPos;
            }
            else if (thisPos > 0)
            {
                listViewTextTranslation.SelectedItems.Clear();
                listViewTextTranslation.SelectedItems.Add(listViewTextTranslation.Items[thisPos - 1]);
                selectedTranslationsIndex = thisPos - 1;
            }
            UI_MainWindow.OptionsFile.Config.TranslationTable.Remove(entry);
            Window_Loaded(this, null);
            listViewTextTranslation.Focus();
        }

        private void btnRemoveAllTextTranslations_Click(object sender, EventArgs e)
        {
            if (listViewTextTranslation.Items.Count == 0) return;
            if (MessageBox.Show("Are you sure you want to remove all of the text translations?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            UI_MainWindow.OptionsFile.Config.TranslationTable.Clear();
            Window_Loaded(this, null);
            listViewTextTranslation.Focus();
        }

        private void btnModifyTextTranslation_Click(object sender, EventArgs e)
        {
            if (listViewTextTranslation.SelectedItems.Count == 0) return;
            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewTextTranslation.SelectedItems[0];

            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag?.GetType() != typeof(TranslationEntry)) return;
            TranslationEntry entry = ((TranslationEntry?)selectedItem.Tag) ?? new TranslationEntry();
            if (entry.Syllable == "") return;
            TextTranslationWindow translationForm = new TextTranslationWindow(entry);
            translationForm.Owner = this;
            translationForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            translationForm.ShowDialog();
            if (translationForm.DialogResult == false) return;
            entry = translationForm.FontTranslation;
            selectedTranslationsIndex = UI_MainWindow.OptionsFile.Config.TranslationTable.IndexOf(entry);
            Window_Loaded(this, null);
            listViewTextTranslation.Focus();
        }

    }
}

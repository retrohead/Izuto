using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.Inazuma11;
using Izuto.UI;
using System.Windows;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for PKBForm.xaml
    /// </summary>
    public partial class PACStringReplacementOptionsWindow : CustomWindowContentBase
    {
        public enum ReplacementPriorityType
        {
            LoadedFile,
            Source
        }
        public class ReplacementOptionsType
        {
            public ReplacementPriorityType ReplacementPriority = ReplacementPriorityType.Source;
            public string SourceTranslationFilePath = "";
        }

        public ReplacementOptionsType ReplacementOptions = new ReplacementOptionsType();
        public PACStringReplacementOptionsWindow(PAC SourcePAC, PAC LoadedPAC)
        {
            InitializeComponent();

            textMessage.Text =
            "Everything seems to be going well!" + Environment.NewLine
            + Environment.NewLine
            + "I found a package with the same ID as the one loaded. All that remains is for you to choose your transfer option and an optional Izuto configuration file to use when loading the file." + Environment.NewLine
            + Environment.NewLine
            + $"Source String Count: {SourcePAC.StringEntries.FindAll(s => !s.IsLinked).Count()}" + Environment.NewLine
            + $"Loaded File String Count {LoadedPAC.StringEntries.FindAll(s => !s.IsLinked).Count()}";
            radioSource.IsChecked = SettingsManager.Settings.ImportPACOption == (int)ReplacementPriorityType.Source;
            textTranslateFile.Text = SettingsManager.Settings.TranslateSourceFilePath;
            checkTextTranslateSource.IsChecked = !string.IsNullOrEmpty(SettingsManager.Settings.TranslateSourceFilePath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Izuto PAC String Replacement Options";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            ReplacementOptions.ReplacementPriority = ReplacementPriorityType.LoadedFile;
            if (radioSource.IsChecked == true)
                ReplacementOptions.ReplacementPriority = ReplacementPriorityType.Source;
            ReplacementOptions.SourceTranslationFilePath = "";
            if (checkTextTranslateSource.IsChecked == true)
            {
                if (string.IsNullOrEmpty(textTranslateFile.Text))
                {
                    MessageBox.Show("You must select a translation options file when choosing to translate the source", "Source Translation Options File Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (!System.IO.File.Exists(textTranslateFile.Text))
                {
                    MessageBox.Show("The selected translation options file does not exist", "Source Translation Options File Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                ReplacementOptions.SourceTranslationFilePath = textTranslateFile.Text;
            }
            DialogResult = true;
            Close();
        }

        private void checkTextTranslateSource_Checked(object sender, RoutedEventArgs e)
        {
            if (textTranslateFile == null)
                return;
            textTranslateFile.Visibility = checkTextTranslateSource.IsChecked == true ? Visibility.Visible : Visibility.Hidden;
            btnBrowseOptionsFile.Visibility = checkTextTranslateSource.IsChecked == true ? Visibility.Visible : Visibility.Hidden;
            textTranslateFile.IsEnabled = checkTextTranslateSource.IsChecked ?? false;
            btnBrowseOptionsFile.IsEnabled = checkTextTranslateSource.IsChecked ?? false;
            if (checkTextTranslateSource.IsChecked == true && textTranslateFile.Text == "")
            {
                btnBrowseOptionsFile_Click(this, null);
            }
        }

        private void btnBrowseOptionsFile_Click(object sender, RoutedEventArgs? e)
        {
            string translateOptionsFile = UI_MainWindow.BrowseForFile(OptionsFileData.OptionsFileFilter);
            if (translateOptionsFile == "")
            {
                checkTextTranslateSource.IsChecked = false;
                return;
            }
            textTranslateFile.Text = translateOptionsFile;
            SettingsManager.Settings.TranslateSourceFilePath = translateOptionsFile;
            SettingsManager.Settings.ImportPACOption = (int)(radioSource.IsChecked == true ? ReplacementPriorityType.Source : ReplacementPriorityType.LoadedFile);
            SettingsManager.Save();
        }
    }
}

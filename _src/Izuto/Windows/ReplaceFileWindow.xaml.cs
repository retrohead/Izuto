using Izuto.Extensions;
using Izuto.UI;
using System;
using System.Collections.Generic;
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

namespace Izuto
{
    /// <summary>
    /// Interaction logic for ReplaceFileWindow.xaml
    /// </summary>
    public partial class ReplaceFileWindow : Window
    {
        public OptionsFileData.FileReplacementEntry? FileReplacement;
        public ReplaceFileWindow(OptionsFileData.FileReplacementEntry? FileReplacement)
        {
            InitializeComponent();
            Theme.loadTheme(this, "Theme_00.xaml");
            Theme.loadTheme(this, "Theme_Templates.xaml");
            Theme.applyTheme(this);
            this.FileReplacement = FileReplacement;
        }

        private void btnBrowsePKB_Click(object sender, RoutedEventArgs e)
        {
            PKBFileSelectWindow f = new PKBFileSelectWindow();
            f.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            f.Owner = this;
            f.ShowDialog();
            if (f.DialogResult == false) return;
            textOriginalFilePath.Text = f.SelectedFilePath;
        }

        private void btnBrowseLocal_Click(object sender, RoutedEventArgs e)
        {
            string path = UI_MainWindow.BrowseForFile();
            if (path != "")
                textReplacementFile.Text = path;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DarkTitleBar.Apply(this);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (FileReplacement == null)
                FileReplacement = new OptionsFileData.FileReplacementEntry();
            FileReplacement.PathToReplace = textOriginalFilePath.Text;
            FileReplacement.RelativePath = System.IO.Path.GetRelativePath(System.IO.Path.GetDirectoryName(UI_MainWindow.OptionsFile.FilePath), textReplacementFile.Text);
            DialogResult = true;
            Close();
        }
    }
}

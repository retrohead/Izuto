using Izuto.Controls;
using Izuto.DockManager;
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
    public partial class ReplaceFileWindow : CustomWindowContentBase
    {
        public OptionsFileData.FileReplacementEntry? FileReplacement;
        public ReplaceFileWindow(OptionsFileData.FileReplacementEntry? FileReplacement)
        {
            InitializeComponent();
            this.FileReplacement = FileReplacement;
        }

        private void btnBrowsePKB_Click(object sender, RoutedEventArgs e)
        {
            CustomWindow win = DockHandler.CreateCustomWindow(Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Fixed });
            PKBFileSelectWindow f = new PKBFileSelectWindow();
            win.ApplyContent(f);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();


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
            Title = "Izuto Replace File Configuration";
            if(FileReplacement.RelativePath != "")
            {
                textReplacementFile.Text = UI_MainWindow.OptionsFile.GetFileActualPath(FileReplacement);
                textOriginalFilePath.Text = FileReplacement.PathToReplace;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textOriginalFilePath.Text)) return;
            if (string.IsNullOrWhiteSpace(textReplacementFile.Text)) return;
            if (FileReplacement == null)
                FileReplacement = new OptionsFileData.FileReplacementEntry();
            FileReplacement.PathToReplace = textOriginalFilePath.Text;
            FileReplacement.RelativePath = System.IO.Path.GetRelativePath(System.IO.Path.GetDirectoryName(UI_MainWindow.OptionsFile.FilePath), textReplacementFile.Text);
            DialogResult = true;
            Close();
        }
    }
}

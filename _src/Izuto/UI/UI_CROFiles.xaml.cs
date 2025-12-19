using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Izuto.UI
{
    /// <summary>
    /// Interaction logic for UI_CROFiles.xaml
    /// </summary>
    public partial class UI_CROFiles : UserControl
    {
        public UI_CROFiles()
        {
            InitializeComponent();
        }

        private void btnSelectCROPath_Click(object sender, RoutedEventArgs e)
        {
            string dir = fileHelper.browseForFolder("Select CRO Files Folder");
            if (dir != "")
            {
                loadCROFolder(dir);
            }
        }

        private void loadCROFolder(string path)
        {
            tabsMain.Items.Clear();
            textCROPath.Text = path;
            var croFiles = Directory.GetFiles(path, "*.cro");
            if (croFiles.Count() == 0)
            {
                tabsMain.Visibility = Visibility.Collapsed;
                MessageBox.Show(path + " does not contain any .cro files.", "No CRO Files Found", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            tabsMain.Visibility = Visibility.Visible;
            foreach (var croFile in croFiles)
            {
                TabItem tab = new TabItem();
                tab.Style = (Style)MainWindow.Self.FindResource("TabItemStyle1");
                tab.Header = System.IO.Path.GetFileName(croFile);
                Grid grid = new Grid();
                grid.Background = (Brush)MainWindow.Self.FindResource("WindowBackgroundBrushLight");
                grid.Margin = new Thickness(-5);
                UI_CROFileViewer viewer = new UI_CROFileViewer(croFile);
                grid.Children.Add(viewer);
                tab.Content = grid;
                tabsMain.Items.Add(tab);
            }
        }
    }
}

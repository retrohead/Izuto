using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.UI;
using plugin_level5.N3DS.Archive;
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
    /// Interaction logic for PKBFileSelectForm.xaml
    /// </summary>
    public partial class PKBFileSelectWindow : CustomWindowContentBase
    {
        public string SelectedFilePath = "";
        treeViewDataType treeViewData;
        public PKBFileSelectWindow()
        {
            InitializeComponent();
            treeViewData = new treeViewDataType(treeFiles);
            treeFiles.DataContext = treeViewData;
        }

        private void treeFiles_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            treeFiles_ItemSelected(sender, e);
        }

        private void treeFiles_ItemSelected(object sender, RoutedEventArgs e)
        {
            treeViewItemDataType? item = (treeViewItemDataType?)treeFiles.SelectedItem;
            if (treeFiles.SelectedItem == null || item == null)
            {
                textSelectedFile.Text = "No File Selected";
            }
            else
            {
                B123ArchiveFile? a = (B123ArchiveFile?)item.TagObj;
                textSelectedFile.Text = a.FilePath.FullName;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Izuto Pick PKB File Browser";
            treeViewData.Items = new System.Collections.ObjectModel.ObservableCollection<treeViewItemDataType>();

            var sortedArchives = UI_MainWindow.ArchiveFiles.OrderBy(p => p.FilePath.FullName);
            
            foreach (var item in sortedArchives)
            {
                string[] parts = item.FilePath.FullName.Split('/');

                var currentNodes = treeViewData.Items;
                treeViewItemDataType? currentNode = null;

                foreach (string part in parts)
                {
                    // Try to find existing node
                    treeViewItemDataType? foundNode = currentNodes.Cast<treeViewItemDataType>()
                                                     .FirstOrDefault(n => n.text == part);

                    if (foundNode == null)
                    {
                        // Create new node if not found
                        int id = UI_MainWindow.ArchiveFiles.IndexOf(item);
                        foundNode = new treeViewItemDataType(id);
                        foundNode.text = part;
                        foundNode.TagObj = item;
                        currentNodes.Add(foundNode);
                    }

                    currentNode = foundNode;
                    currentNodes = currentNode.Items;
                }
            }
            treeViewData.Items[0].text = System.IO.Path.GetFileName(UI_MainWindow.LoadedArchiveFilePath);
            treeViewData.Items[0].IsExpanded = true;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (treeFiles.SelectedItem == null)
            {
                MessageBox.Show("You must select a file or close the window to cancel", "File Not Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SelectedFilePath = textSelectedFile.Text;
            DialogResult = true;
            Close();
        }
    }
}

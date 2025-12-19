using Izuto.Controls;
using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.Inazuma11;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Izuto.UI
{
    /// <summary>
    /// Interaction logic for UI_CROFileViewer.xaml
    /// </summary>
    public partial class UI_CROFileViewer : UserControl
    {
        public string LoadedCROPath = "";
        public CRO CROData = new CRO();
        private listViewDataType listViewMenuStringsData;

        public UI_CROFileViewer(string croPath)
        {
            LoadedCROPath = croPath;
            InitializeComponent();
            listViewMenuStringsData = new listViewDataType(MainWindow.Self,ref listViewMenuStrings);
            listViewMenuStrings.DataContext = listViewMenuStringsData;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UI_MainWindow.Self.UpdateProgress("Parsing CRO File", 0, 1);
            await Task.Run(async () =>
            {
                await CROData.Load(LoadedCROPath);
            });

            txtParams.Text = CROData.ReadDataAsString();

            UI_MainWindow.Self.EndProgressUpdates();

            listViewMenuStringsData.Items = new System.Collections.ObjectModel.ObservableCollection<listViewItemDataType>();
            foreach (var str in CROData.MenuStrings)
            {
                listViewItemDataType item = new listViewItemDataType(listViewMenuStringsData, str.Key.ToString(), str.Key.ToString());
                item.SubItems.Add(new listViewColumnDataType(item, "0x" + str.Value.Offset.ToString("X8")));
                item.SubItems.Add(new listViewColumnDataType(item, str.Value.String));
                item.SubItems.Add(new listViewColumnDataType(item, TextTranslation.ConvertBackTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, str.Value.String)));
                item.SubItems.Add(new listViewColumnDataType(item, str.Value.Capacity.ToString() + " bytes"));
                item.Tag = str;
                listViewMenuStringsData.AddItem(item);
            }
            listViewDataType.autoResizeListBoxCols(ref listViewMenuStrings, ref listViewMenuStringsData);
        }

        private void btnModifyString_Click(object sender, RoutedEventArgs e)
        {

            if (listViewMenuStrings.SelectedItems.Count == 0) return;

            listViewItemDataType? selectedItem = (listViewItemDataType?)listViewMenuStrings.SelectedItems[0];
            if (selectedItem == null) return;
            if (selectedItem.Tag == null) return;
            if (selectedItem.Tag is not KeyValuePair<CRO.MenuStringsType, CRO.MenuStringType> stringData)
                return;

            CustomWindow win = new CustomWindow(MainWindow.Self!.Window, new CustomWindowOptions() { WindowType = CustomWindow.WindowTypes.Fixed });
            StringWindow stringform = new StringWindow(stringData.Value.String, stringData.Value.IsAlligned);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ApplyContent(stringform);
            win.Loaded += UI_MainWindow.CustomWindow_Loaded;
            win.ShowDialog();
            win.Activate();

            if (stringform.DialogResult == false) return;
            bool changed = stringData.Value.String != stringform.ModifiedString;
            string newText = stringform.ModifiedString;
            //if (stringSize + 1 > stringData.Value.Capacity)
            //{
            //    MessageBox.Show($"The modified string is too long to fit in the allocated space.\n\nAllocated Size: {stringData.Value.Capacity} bytes\nModified String Size: {stringSize + 1} bytes\n\nPlease shorten the string or increase the allocated size.", "String Too Long", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            stringData.Value.String = newText;
            selectedItem.SubItems[2].data = stringData.Value.String;
            selectedItem.SubItems[3].data = TextTranslation.ConvertBackTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, stringData.Value.String);
            listViewMenuStrings.DataContext = null;
            listViewMenuStrings.DataContext = listViewMenuStringsData;

            if(CROData.WriteMenuStrings())
                UserControl_Loaded(sender, e);
        }

        private void btnVerifyCRR_Click(object sender, RoutedEventArgs e)
        {
            string filename = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(CROData.Header.FilePath))!, ".crr", "static.crr");

            if(!File.Exists(filename))
            {
                MessageBox.Show("The corresponding CRR file was not found.", "CRR Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[]? hashtable;
            var result = CROTools.CROTools.validate_crr(filename, Path.GetDirectoryName(CROData.Header.FilePath)!, out hashtable);
            dynamic pop = new popUpVerifyCRR(MainWindow.Self, result);
            popUps.loadPopUp(MainWindow.Self, "CRR File Verfication Result", "izuto.ico", ref pop);
        }

        private void btnRehashCRR_Click(object sender, RoutedEventArgs e)
        {
            string filename = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(CROData.Header.FilePath))!, ".crr", "static.crr");

            if (!File.Exists(filename))
            {
                MessageBox.Show("The corresponding CRR file was not found.", "CRR Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[]? hashtable;
            var result = CROTools.CROTools.validate_crr(filename, Path.GetDirectoryName(CROData.Header.FilePath)!, out hashtable);
            if(result.Success)
            {
                var failed = CROTools.CROTools.CROResults.FindAll(r => !r.Success);
                if(failed.Count == 0)
                {
                    MessageBox.Show("Hashes are already valid, no patching required", "Already Valid", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            result = CROTools.CROTools.resign_crr(filename, Path.GetDirectoryName(CROData.Header.FilePath)!);
            dynamic pop = new popUpVerifyCRR(MainWindow.Self, result);
            popUps.loadPopUp(MainWindow.Self, "CRR File Rehash Result", "izuto.ico", ref pop);
        }
    }
}

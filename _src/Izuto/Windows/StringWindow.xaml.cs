using Izuto.DockManager;
using Izuto.Extensions;
using Izuto.Inazuma11;
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
    /// Interaction logic for PKBForm.xaml
    /// </summary>
    public partial class StringWindow : CustomWindowContentBase
    {
        public string ModifiedString { get; private set; } = "";
        string OriginalString;
        bool AlignTo4Bytes = false;
        public StringWindow(string StringToMomdify, bool alignTo4Bytes)
        {
            AlignTo4Bytes = alignTo4Bytes;
            InitializeComponent();
            this.OriginalString = StringToMomdify;
            UpdateText();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Izuto String Editor";
        }
        private void UpdateText()
        {
            string replacedNullsAndReturns = OriginalString.Replace("\\n", "\r\n").Replace("\0", "");
            if (UI_MainWindow.OptionsFile.IsLoaded())
            {
                txtString.Text = TextTranslation.ConvertBackTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, replacedNullsAndReturns);
            }
            else
            {
                txtString.Text = replacedNullsAndReturns;
            }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            // Save the modified text before closing
            ModifiedString = txtString.Text;
            if (UI_MainWindow.OptionsFile.IsLoaded())
                ModifiedString = TextTranslation.ConvertTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, ModifiedString);
            ModifiedString = PAC.UpdateString(ModifiedString, AlignTo4Bytes);

            this.DialogResult = true;
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            // Save the modified text before closing
            ModifiedString = txtString.Text;
            if (UI_MainWindow.OptionsFile.IsLoaded())
                ModifiedString = TextTranslation.ConvertTextString(UI_MainWindow.OptionsFile.Config.TranslationTable, ModifiedString);
            ModifiedString = PAC.UpdateString(ModifiedString, AlignTo4Bytes);

            this.DialogResult = true;
            this.Close();
        }
    }
}

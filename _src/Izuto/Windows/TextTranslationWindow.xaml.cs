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
using static Izuto.Extensions.TextTranslation;
using static System.Net.Mime.MediaTypeNames;

namespace Izuto
{
    /// <summary>
    /// Interaction logic for TextTranslationWindow.xaml
    /// </summary>
    public partial class TextTranslationWindow : CustomWindowContentBase
    {
        public TranslationEntry FontTranslation;
        public TextTranslationWindow(TranslationEntry FontTranslation)
        {
            this.FontTranslation = FontTranslation;
            InitializeComponent();
            textOrigHex.Text = MainWindow.BytesToHexString(Encoding.GetEncoding("utf-8").GetBytes(FontTranslation.Syllable));
            textReplacementHex.Text = MainWindow.BytesToHexString(Encoding.GetEncoding("shift_jis").GetBytes(FontTranslation.BytesString));

            textJp.Text = FontTranslation.BytesString;
            textSyllable.Text = FontTranslation.Syllable;
        }
        public static bool TryParseHexBytes(string input, out byte[]? result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(input) || input.Length % 2 != 0)
                return false;

            if (!input.All(c => Uri.IsHexDigit(c)))
                return false;

            try
            {
                int byteCount = input.Length / 2;
                result = new byte[byteCount];

                for (int i = 0; i < byteCount; i++)
                {
                    string hexPair = input.Substring(i * 2, 2);
                    result[i] = Convert.ToByte(hexPair, 16);
                }

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Izuto Text Translation Configuration";
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            byte[]? orig;
            if (!TryParseHexBytes(textOrigHex.Text, out orig))
            {
                MessageBox.Show("Original Hex is not in correct format", "Hex Parse Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            byte[]? replace;
            if (!TryParseHexBytes(textReplacementHex.Text, out replace))
            {
                MessageBox.Show("Replacement Hex is not in correct format", "Hex Parse Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if(replace == null || orig == null)
            {
                MessageBox.Show("Hex parsing resulted in null byte array", "Hex Parse Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Encoding sjis = Encoding.GetEncoding("shift_jis");
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            FontTranslation.Syllable = utf8.GetString(orig);
            FontTranslation.BytesString = sjis.GetString(replace);
            DialogResult = true;
            Close();
        }

        private void textUnicodeEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            string syllable = textUnicodeEntry.Text;
            byte[] origbytes = utf8.GetBytes(syllable);
            textOrigHex_Text.Text = MainWindow.BytesToHexString(origbytes, "");
            if (tabsMain.SelectedItem == tabTextEntry)
                textOrigHex.Text = textOrigHex_Text.Text;
        }

        private void textJp_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            Encoding sjis = Encoding.GetEncoding("shift_jis");
            string replacetext = textJp_Text.Text;
            byte[] replacebytes = sjis.GetBytes(replacetext);
            textReplacementHex_Text.Text = MainWindow.BytesToHexString(replacebytes, "");
            if (tabsMain.SelectedItem == tabTextEntry)
                textReplacementHex.Text = textReplacementHex_Text.Text;
        }

        private void textOrigHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte[]? orig;
            if (!TryParseHexBytes(textOrigHex.Text, out orig))
            {
                textSyllable.Text = "Error";
                textUnicodeEntry.Text = "";
                return;
            }
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            textSyllable.Text = utf8.GetString(orig);
            if (tabsMain.SelectedItem == tabHexEntry || tabsMain.SelectedItem == null)
                textUnicodeEntry.Text = textSyllable.Text;
        }

        private void textReplacementHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            byte[] replace;
            if (!TryParseHexBytes(textReplacementHex.Text, out replace))
            {
                textJp.Text = "Error";
                return;
            }
            Encoding sjis = Encoding.GetEncoding("shift_jis");
            textJp.Text = sjis.GetString(replace);
            if (tabsMain.SelectedItem == tabHexEntry || tabsMain.SelectedItem == null)
                textJp_Text.Text = sjis.GetString(replace);
        }
    }
}

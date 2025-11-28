using plugin_level5.N3DS.Archive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Izuto.Extensions.TextTranslation;

namespace Izuto
{
    public partial class TextTranslationForm : Form
    {
        public TranslationEntry FontTranslation;
        public TextTranslationForm(TranslationEntry FontTranslation)
        {
            this.FontTranslation = FontTranslation;
            InitializeComponent();
            textOrigHex.Text = MainForm.BytesToHexString(Encoding.GetEncoding("utf-8").GetBytes(FontTranslation.Syllable));
            textReplacementHex.Text = MainForm.BytesToHexString(Encoding.GetEncoding("shift_jis").GetBytes(FontTranslation.BytesString));

            textJp.Text = FontTranslation.BytesString;
            textSyllable.Text = FontTranslation.Syllable;
        }
        public static bool TryParseHexBytes(string input, out byte[] result)
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


        private void btnApply_Click(object sender, EventArgs e)
        {
            byte[] orig;
            if (!TryParseHexBytes(textOrigHex.Text, out orig))
            {
                MessageBox.Show("Original Hex is not in correct format", "Hex Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            byte[] replace;
            if (!TryParseHexBytes(textReplacementHex.Text, out replace))
            {
                MessageBox.Show("Replacement Hex is not in correct format", "Hex Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Encoding sjis = Encoding.GetEncoding("shift_jis");
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            FontTranslation.Syllable = utf8.GetString(orig);
            FontTranslation.BytesString = sjis.GetString(replace);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textOrigHex_TextChanged(object sender, EventArgs e)
        {
            byte[] orig;
            if (!TryParseHexBytes(textOrigHex.Text, out orig))
            {
                textSyllable.Text = "Error";
                textUnicodeEntry.Text = "";
                return;
            }
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            textSyllable.Text = utf8.GetString(orig);
            if (tabControl1.SelectedTab == tabPageHexEntry)
                textUnicodeEntry.Text = textSyllable.Text;
        }
        private void textReplacementHex_TextChanged(object sender, EventArgs e)
        {
            byte[] replace;
            if (!TryParseHexBytes(textReplacementHex.Text, out replace))
            {
                textJp.Text = "Error";
                return;
            }
            Encoding sjis = Encoding.GetEncoding("shift_jis");
            textJp.Text = sjis.GetString(replace);
            if (tabControl1.SelectedTab == tabPageHexEntry)
                textJp_Text.Text = sjis.GetString(replace);
        }

        private void textUnicodeEntry_TextChanged(object sender, EventArgs e)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            string syllable = textUnicodeEntry.Text;
            byte[] origbytes = utf8.GetBytes(syllable);
            textOrigHex_Text.Text = MainForm.BytesToHexString(origbytes, "");
            if (tabControl1.SelectedTab == tabPageTextEntry)
                textOrigHex.Text = textOrigHex_Text.Text;
        }

        private void textJp_Text_TextChanged(object sender, EventArgs e)
        {
            Encoding sjis = Encoding.GetEncoding("shift_jis");
            string replacetext = textJp_Text.Text;
            byte[] replacebytes = sjis.GetBytes(replacetext);
            textReplacementHex_Text.Text = MainForm.BytesToHexString(replacebytes, "");
            if (tabControl1.SelectedTab == tabPageTextEntry)
                textReplacementHex.Text = textReplacementHex_Text.Text;
        }
    }
}

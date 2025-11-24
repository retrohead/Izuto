using Izuto.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Izuto
{
    public partial class StringForm : Form
    {
        public string ModifiedString { get; private set; } = "";
        string OriginalString;

        public StringForm(string StringToMomdify, string searchForString = "")
        {
            InitializeComponent();
            this.OriginalString = StringToMomdify;
            UpdateText();
        }

        private void UpdateText()
        {
            string replacedNullsAndReturns = OriginalString.Replace("\\n", "\r\n").Replace("\0", "");
            if (MainForm.OptionsFile.IsLoaded())
            {
                textBox1.Text = MainForm.OptionsFile.ConvertBackTextString(replacedNullsAndReturns);
            }
            else
            {
                textBox1.Text = replacedNullsAndReturns;
            }
        }

        private static string WrapText(string input, int maxBytesPerLine)
        {
            var words = input.Split(new[] { ' ' }, StringSplitOptions.None);
            var sb = new StringBuilder();
            int currentLineBytes = 0;

            var sjis = Encoding.GetEncoding("shift_jis");

            foreach (var word in words)
            {
                // Calculate byte length of this word in Shift-JIS
                int wordBytes = sjis.GetByteCount(word);

                // If adding this word would exceed the byte limit, start a new line
                int spaceBytes = (currentLineBytes > 0) ? sjis.GetByteCount(" ") : 0;
                if (currentLineBytes + wordBytes + spaceBytes > maxBytesPerLine)
                {
                    sb.Append("\\n"); // insert line break marker
                    currentLineBytes = 0;
                    spaceBytes = 0;
                }
                else if (currentLineBytes > 0)
                {
                    sb.Append(" ");
                    currentLineBytes += spaceBytes;
                }

                sb.Append(word);
                currentLineBytes += wordBytes;
            }

            return sb.ToString();
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            // Save the modified text before closing
            ModifiedString = textBox1.Text;
            if (MainForm.OptionsFile.IsLoaded())
                ModifiedString = MainForm.OptionsFile.ConvertTextString(ModifiedString);
            ModifiedString = ModifiedString.Replace("\r\n", "\n");
            //ModifiedString = WrapText(ModifiedString, 44);
            ModifiedString = ModifiedString.Replace("\n", "\\n");
            byte[] text = Encoding.GetEncoding("shift_jis").GetBytes(ModifiedString);
            int remain = ModifiedString.Length % 4;
            while (remain > 0)
            {
                ModifiedString = ModifiedString + "\0";
                remain--;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkConvertText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateText();
        }
    }
}

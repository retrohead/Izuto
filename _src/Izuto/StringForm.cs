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
                textBox1.Text = TextTranslation.ConvertBackTextString(MainForm.OptionsFile.Config.TranslationTable, replacedNullsAndReturns);
            }
            else
            {
                textBox1.Text = replacedNullsAndReturns;
            }
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            // Save the modified text before closing
            ModifiedString = textBox1.Text;
            if (MainForm.OptionsFile.IsLoaded())
                ModifiedString = TextTranslation.ConvertTextString(MainForm.OptionsFile.Config.TranslationTable, ModifiedString);
            ModifiedString = PAC.UpdateString(ModifiedString);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

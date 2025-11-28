
namespace Izuto
{
    partial class TextTranslationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextTranslationForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textOrigHex = new TextBox();
            textSyllable = new TextBox();
            textJp = new TextBox();
            textReplacementHex = new TextBox();
            tabControl1 = new TabControl();
            tabPageHexEntry = new TabPage();
            tabPageTextEntry = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            textUnicodeEntry = new TextBox();
            textOrigHex_Text = new TextBox();
            textReplacementHex_Text = new TextBox();
            textJp_Text = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnApply = new Button();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageHexEntry.SuspendLayout();
            tabPageTextEntry.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label4, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(label3, 2, 0);
            tableLayoutPanel1.Controls.Add(textOrigHex, 1, 0);
            tableLayoutPanel1.Controls.Add(textSyllable, 3, 0);
            tableLayoutPanel1.Controls.Add(textJp, 3, 1);
            tableLayoutPanel1.Controls.Add(textReplacementHex, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(728, 87);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(352, 39);
            label4.Margin = new Padding(3, 7, 3, 0);
            label4.Name = "label4";
            label4.Size = new Size(129, 15);
            label4.TabIndex = 3;
            label4.Text = "Replacement (Shift-JIS)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Original Hex";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 39);
            label2.Margin = new Padding(3, 7, 3, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 15);
            label2.TabIndex = 1;
            label2.Text = "Replacement Hex";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(352, 7);
            label3.Margin = new Padding(3, 7, 3, 0);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(102, 15);
            label3.TabIndex = 2;
            label3.Text = "Syllable (Unicode)";
            // 
            // textOrigHex
            // 
            textOrigHex.Dock = DockStyle.Fill;
            textOrigHex.Location = new Point(123, 3);
            textOrigHex.MaxLength = 6;
            textOrigHex.Name = "textOrigHex";
            textOrigHex.Size = new Size(223, 23);
            textOrigHex.TabIndex = 5;
            textOrigHex.TextChanged += textOrigHex_TextChanged;
            // 
            // textSyllable
            // 
            textSyllable.BackColor = SystemColors.ControlLight;
            textSyllable.Dock = DockStyle.Fill;
            textSyllable.Enabled = false;
            textSyllable.Location = new Point(502, 3);
            textSyllable.Name = "textSyllable";
            textSyllable.Size = new Size(223, 23);
            textSyllable.TabIndex = 6;
            // 
            // textJp
            // 
            textJp.BackColor = SystemColors.ControlLight;
            textJp.Dock = DockStyle.Fill;
            textJp.Enabled = false;
            textJp.Location = new Point(502, 35);
            textJp.Name = "textJp";
            textJp.Size = new Size(223, 23);
            textJp.TabIndex = 7;
            // 
            // textReplacementHex
            // 
            textReplacementHex.Dock = DockStyle.Fill;
            textReplacementHex.Location = new Point(123, 35);
            textReplacementHex.MaxLength = 4;
            textReplacementHex.Name = "textReplacementHex";
            textReplacementHex.Size = new Size(223, 23);
            textReplacementHex.TabIndex = 8;
            textReplacementHex.TextChanged += textReplacementHex_TextChanged;
            // 
            // tabControl1
            // 
            tableLayoutPanel2.SetColumnSpan(tabControl1, 2);
            tabControl1.Controls.Add(tabPageHexEntry);
            tabControl1.Controls.Add(tabPageTextEntry);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(742, 121);
            tabControl1.TabIndex = 1;
            // 
            // tabPageHexEntry
            // 
            tabPageHexEntry.Controls.Add(tableLayoutPanel1);
            tabPageHexEntry.Location = new Point(4, 24);
            tabPageHexEntry.Name = "tabPageHexEntry";
            tabPageHexEntry.Padding = new Padding(3);
            tabPageHexEntry.Size = new Size(734, 93);
            tabPageHexEntry.TabIndex = 0;
            tabPageHexEntry.Text = "Hex Entry";
            tabPageHexEntry.UseVisualStyleBackColor = true;
            // 
            // tabPageTextEntry
            // 
            tabPageTextEntry.Controls.Add(tableLayoutPanel3);
            tabPageTextEntry.Location = new Point(4, 24);
            tabPageTextEntry.Name = "tabPageTextEntry";
            tabPageTextEntry.Padding = new Padding(3);
            tabPageTextEntry.Size = new Size(734, 93);
            tabPageTextEntry.TabIndex = 1;
            tabPageTextEntry.Text = "Text Entry";
            tabPageTextEntry.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label5, 2, 1);
            tableLayoutPanel3.Controls.Add(label6, 0, 0);
            tableLayoutPanel3.Controls.Add(label7, 0, 1);
            tableLayoutPanel3.Controls.Add(label8, 2, 0);
            tableLayoutPanel3.Controls.Add(textUnicodeEntry, 1, 0);
            tableLayoutPanel3.Controls.Add(textOrigHex_Text, 3, 0);
            tableLayoutPanel3.Controls.Add(textReplacementHex_Text, 3, 1);
            tableLayoutPanel3.Controls.Add(textJp_Text, 1, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(728, 87);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(382, 39);
            label5.Margin = new Padding(3, 7, 3, 0);
            label5.Name = "label5";
            label5.Size = new Size(100, 15);
            label5.TabIndex = 3;
            label5.Text = "Replacement Hex";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 7);
            label6.Margin = new Padding(3, 7, 3, 0);
            label6.Name = "label6";
            label6.Size = new Size(102, 15);
            label6.TabIndex = 0;
            label6.Text = "Syllable (Unicode)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 39);
            label7.Margin = new Padding(3, 7, 3, 0);
            label7.Name = "label7";
            label7.Size = new Size(129, 15);
            label7.TabIndex = 1;
            label7.Text = "Replacement (Shift-JIS)";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(382, 7);
            label8.Margin = new Padding(3, 7, 3, 0);
            label8.Name = "label8";
            label8.RightToLeft = RightToLeft.No;
            label8.Size = new Size(73, 15);
            label8.TabIndex = 2;
            label8.Text = "Original Hex";
            // 
            // textUnicodeEntry
            // 
            textUnicodeEntry.Dock = DockStyle.Fill;
            textUnicodeEntry.Location = new Point(153, 3);
            textUnicodeEntry.MaxLength = 6;
            textUnicodeEntry.Name = "textUnicodeEntry";
            textUnicodeEntry.Size = new Size(223, 23);
            textUnicodeEntry.TabIndex = 5;
            textUnicodeEntry.TextChanged += textUnicodeEntry_TextChanged;
            // 
            // textOrigHex_Text
            // 
            textOrigHex_Text.BackColor = SystemColors.ControlLight;
            textOrigHex_Text.Dock = DockStyle.Fill;
            textOrigHex_Text.Enabled = false;
            textOrigHex_Text.Location = new Point(502, 3);
            textOrigHex_Text.Name = "textOrigHex_Text";
            textOrigHex_Text.Size = new Size(223, 23);
            textOrigHex_Text.TabIndex = 6;
            // 
            // textReplacementHex_Text
            // 
            textReplacementHex_Text.BackColor = SystemColors.ControlLight;
            textReplacementHex_Text.Dock = DockStyle.Fill;
            textReplacementHex_Text.Enabled = false;
            textReplacementHex_Text.Location = new Point(502, 35);
            textReplacementHex_Text.Name = "textReplacementHex_Text";
            textReplacementHex_Text.Size = new Size(223, 23);
            textReplacementHex_Text.TabIndex = 7;
            // 
            // textJp_Text
            // 
            textJp_Text.Dock = DockStyle.Fill;
            textJp_Text.Location = new Point(153, 35);
            textJp_Text.MaxLength = 4;
            textJp_Text.Name = "textJp_Text";
            textJp_Text.Size = new Size(223, 23);
            textJp_Text.TabIndex = 8;
            textJp_Text.TextChanged += textJp_Text_TextChanged;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(btnApply, 0, 1);
            tableLayoutPanel2.Controls.Add(tabControl1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(748, 159);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // btnApply
            // 
            btnApply.Dock = DockStyle.Fill;
            btnApply.Location = new Point(3, 130);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(144, 26);
            btnApply.TabIndex = 5;
            btnApply.Text = "Apply Changes";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // TextTranslationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(748, 159);
            Controls.Add(tableLayoutPanel2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(764, 198);
            MinimizeBox = false;
            MinimumSize = new Size(764, 198);
            Name = "TextTranslationForm";
            Text = "Izuto Text Translation Configuration";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageHexEntry.ResumeLayout(false);
            tabPageTextEntry.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label3;
        private TextBox textOrigHex;
        private TextBox textSyllable;
        private TextBox textJp;
        private TextBox textReplacementHex;
        private TabControl tabControl1;
        private TabPage tabPageHexEntry;
        private TabPage tabPageTextEntry;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnApply;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox textUnicodeEntry;
        private TextBox textOrigHex_Text;
        private TextBox textReplacementHex_Text;
        private TextBox textJp_Text;
    }
}

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
            btnApply = new Button();
            textOrigHex = new TextBox();
            textSyllable = new TextBox();
            textJp = new TextBox();
            textReplacementHex = new TextBox();
            tableLayoutPanel1.SuspendLayout();
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
            tableLayoutPanel1.Controls.Add(btnApply, 0, 2);
            tableLayoutPanel1.Controls.Add(textOrigHex, 1, 0);
            tableLayoutPanel1.Controls.Add(textSyllable, 3, 0);
            tableLayoutPanel1.Controls.Add(textJp, 3, 1);
            tableLayoutPanel1.Controls.Add(textReplacementHex, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(613, 98);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(294, 39);
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
            label3.Location = new Point(294, 7);
            label3.Margin = new Padding(3, 7, 3, 0);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(102, 15);
            label3.TabIndex = 2;
            label3.Text = "Syllable (Unicode)";
            // 
            // btnApply
            // 
            btnApply.Dock = DockStyle.Fill;
            btnApply.Location = new Point(3, 67);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(114, 28);
            btnApply.TabIndex = 4;
            btnApply.Text = "Apply Changes";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // textOrigHex
            // 
            textOrigHex.Dock = DockStyle.Fill;
            textOrigHex.Location = new Point(123, 3);
            textOrigHex.MaxLength = 6;
            textOrigHex.Name = "textOrigHex";
            textOrigHex.Size = new Size(165, 23);
            textOrigHex.TabIndex = 5;
            textOrigHex.TextChanged += textOrigHex_TextChanged;
            // 
            // textSyllable
            // 
            textSyllable.BackColor = SystemColors.ControlLight;
            textSyllable.Dock = DockStyle.Fill;
            textSyllable.Enabled = false;
            textSyllable.Location = new Point(444, 3);
            textSyllable.Name = "textSyllable";
            textSyllable.Size = new Size(166, 23);
            textSyllable.TabIndex = 6;
            // 
            // textJp
            // 
            textJp.BackColor = SystemColors.ControlLight;
            textJp.Dock = DockStyle.Fill;
            textJp.Enabled = false;
            textJp.Location = new Point(444, 35);
            textJp.Name = "textJp";
            textJp.Size = new Size(166, 23);
            textJp.TabIndex = 7;
            // 
            // textReplacementHex
            // 
            textReplacementHex.Dock = DockStyle.Fill;
            textReplacementHex.Location = new Point(123, 35);
            textReplacementHex.MaxLength = 4;
            textReplacementHex.Name = "textReplacementHex";
            textReplacementHex.Size = new Size(165, 23);
            textReplacementHex.TabIndex = 8;
            textReplacementHex.TextChanged += textReplacementHex_TextChanged;
            // 
            // TextTranslationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(613, 98);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(629, 137);
            MinimumSize = new Size(629, 137);
            Name = "TextTranslationForm";
            Text = "Izuto Text Translation Configuration";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }


        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label3;
        private Button btnApply;
        private TextBox textOrigHex;
        private TextBox textSyllable;
        private TextBox textJp;
        private TextBox textReplacementHex;
    }
}
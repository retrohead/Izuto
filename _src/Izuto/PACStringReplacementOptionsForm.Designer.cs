namespace Izuto
{
    partial class PACStringReplacementOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PACStringReplacementOptionsForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            radioSource = new RadioButton();
            radioDest = new RadioButton();
            btnContinue = new Button();
            btnCancel = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            textMessage = new TextBox();
            pictureBox1 = new PictureBox();
            checkTextTranslateSource = new CheckBox();
            textTranslateFile = new TextBox();
            btnBrowseOptionsFile = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel1.Controls.Add(radioSource, 0, 3);
            tableLayoutPanel1.Controls.Add(radioDest, 0, 2);
            tableLayoutPanel1.Controls.Add(btnContinue, 1, 5);
            tableLayoutPanel1.Controls.Add(btnCancel, 2, 5);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(checkTextTranslateSource, 0, 4);
            tableLayoutPanel1.Controls.Add(textTranslateFile, 1, 4);
            tableLayoutPanel1.Controls.Add(btnBrowseOptionsFile, 3, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(602, 381);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // radioSource
            // 
            radioSource.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(radioSource, 5);
            radioSource.Dock = DockStyle.Fill;
            radioSource.Location = new Point(30, 289);
            radioSource.Margin = new Padding(30, 7, 3, 3);
            radioSource.Name = "radioSource";
            radioSource.Size = new Size(569, 22);
            radioSource.TabIndex = 0;
            radioSource.Text = "Source Priority (Scans source file and moves matching strings into the loaded file where possible)";
            radioSource.UseVisualStyleBackColor = true;
            // 
            // radioDest
            // 
            radioDest.AutoSize = true;
            radioDest.Checked = true;
            tableLayoutPanel1.SetColumnSpan(radioDest, 5);
            radioDest.Dock = DockStyle.Fill;
            radioDest.Location = new Point(30, 257);
            radioDest.Margin = new Padding(30, 7, 3, 3);
            radioDest.Name = "radioDest";
            radioDest.Size = new Size(569, 22);
            radioDest.TabIndex = 1;
            radioDest.TabStop = true;
            radioDest.Text = "Loaded File Priority (Try to find matching strings based on loaded file and copy where possible)";
            radioDest.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            btnContinue.Dock = DockStyle.Fill;
            btnContinue.Location = new Point(153, 349);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new Size(144, 26);
            btnContinue.TabIndex = 2;
            btnContinue.Text = "Continue";
            btnContinue.UseVisualStyleBackColor = true;
            btnContinue.Click += btnContinue_Click;
            // 
            // btnCancel
            // 
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.Location = new Point(303, 349);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(144, 26);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 5);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.Controls.Add(textMessage, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 103);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(596, 144);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // textMessage
            // 
            textMessage.BackColor = SystemColors.Control;
            textMessage.BorderStyle = BorderStyle.None;
            textMessage.Dock = DockStyle.Fill;
            textMessage.Enabled = false;
            textMessage.Location = new Point(153, 3);
            textMessage.Multiline = true;
            textMessage.Name = "textMessage";
            textMessage.Size = new Size(290, 138);
            textMessage.TabIndex = 4;
            textMessage.Text = "Message";
            textMessage.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            tableLayoutPanel1.SetColumnSpan(pictureBox1, 5);
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.IzutoLogo;
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(596, 94);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // checkTextTranslateSource
            // 
            checkTextTranslateSource.AutoSize = true;
            checkTextTranslateSource.Checked = true;
            checkTextTranslateSource.CheckState = CheckState.Checked;
            checkTextTranslateSource.Location = new Point(30, 321);
            checkTextTranslateSource.Margin = new Padding(30, 7, 3, 3);
            checkTextTranslateSource.Name = "checkTextTranslateSource";
            checkTextTranslateSource.Size = new Size(111, 19);
            checkTextTranslateSource.TabIndex = 7;
            checkTextTranslateSource.Text = "Translate Source";
            checkTextTranslateSource.UseVisualStyleBackColor = true;
            checkTextTranslateSource.CheckedChanged += checkTextTranslateSource_CheckedChanged;
            // 
            // textTranslateFile
            // 
            textTranslateFile.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(textTranslateFile, 2);
            textTranslateFile.Dock = DockStyle.Fill;
            textTranslateFile.Location = new Point(153, 317);
            textTranslateFile.Name = "textTranslateFile";
            textTranslateFile.Size = new Size(294, 23);
            textTranslateFile.TabIndex = 8;
            // 
            // btnBrowseOptionsFile
            // 
            btnBrowseOptionsFile.Dock = DockStyle.Fill;
            btnBrowseOptionsFile.Location = new Point(453, 317);
            btnBrowseOptionsFile.Name = "btnBrowseOptionsFile";
            btnBrowseOptionsFile.Size = new Size(69, 26);
            btnBrowseOptionsFile.TabIndex = 9;
            btnBrowseOptionsFile.Text = "...";
            btnBrowseOptionsFile.UseVisualStyleBackColor = true;
            btnBrowseOptionsFile.Click += btnBrowseOptionsFile_Click;
            // 
            // PACStringReplacementOptionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 381);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(655, 420);
            MinimizeBox = false;
            MinimumSize = new Size(618, 420);
            Name = "PACStringReplacementOptionsForm";
            Text = "Izuto String Replament Options";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private RadioButton radioSource;
        private RadioButton radioDest;
        private Button btnContinue;
        private Button btnCancel;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox textMessage;
        private PictureBox pictureBox1;
        private CheckBox checkTextTranslateSource;
        private TextBox textTranslateFile;
        private Button btnBrowseOptionsFile;
    }
}
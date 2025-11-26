namespace Izuto
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            btnBrowseOptionsFile = new Button();
            textOptionsFilePath = new TextBox();
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPageFontFile = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            listViewFileReplacements = new ListView();
            columnHeader4 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            btnAddFileReplacement = new Button();
            btnRemoveFileReplacement = new Button();
            tabPage1 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            listViewTextTranslation = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            btnAddTextTranslation = new Button();
            btnRemoveTextTranslation = new Button();
            btnModifyTextTranslation = new Button();
            btnSave = new Button();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageFontFile.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(btnBrowseOptionsFile, 3, 0);
            tableLayoutPanel1.Controls.Add(textOptionsFilePath, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(tabControl1, 0, 1);
            tableLayoutPanel1.Controls.Add(btnSave, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(934, 564);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btnBrowseOptionsFile
            // 
            btnBrowseOptionsFile.Dock = DockStyle.Fill;
            btnBrowseOptionsFile.Location = new Point(837, 3);
            btnBrowseOptionsFile.Name = "btnBrowseOptionsFile";
            btnBrowseOptionsFile.Size = new Size(94, 26);
            btnBrowseOptionsFile.TabIndex = 10;
            btnBrowseOptionsFile.Text = "...";
            btnBrowseOptionsFile.UseVisualStyleBackColor = true;
            btnBrowseOptionsFile.Click += btnBrowseFont_Click;
            // 
            // textOptionsFilePath
            // 
            textOptionsFilePath.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(textOptionsFilePath, 2);
            textOptionsFilePath.Dock = DockStyle.Fill;
            textOptionsFilePath.Location = new Point(103, 3);
            textOptionsFilePath.Name = "textOptionsFilePath";
            textOptionsFilePath.ReadOnly = true;
            textOptionsFilePath.Size = new Size(728, 23);
            textOptionsFilePath.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 11;
            label1.Text = "Options File";
            // 
            // tabControl1
            // 
            tableLayoutPanel1.SetColumnSpan(tabControl1, 4);
            tabControl1.Controls.Add(tabPageFontFile);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(928, 494);
            tabControl1.TabIndex = 12;
            // 
            // tabPageFontFile
            // 
            tabPageFontFile.Controls.Add(tableLayoutPanel3);
            tabPageFontFile.Location = new Point(4, 24);
            tabPageFontFile.Name = "tabPageFontFile";
            tabPageFontFile.Padding = new Padding(3);
            tabPageFontFile.Size = new Size(920, 466);
            tabPageFontFile.TabIndex = 1;
            tabPageFontFile.Text = "File Replacements";
            tabPageFontFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(listViewFileReplacements, 0, 0);
            tableLayoutPanel3.Controls.Add(btnAddFileReplacement, 0, 1);
            tableLayoutPanel3.Controls.Add(btnRemoveFileReplacement, 1, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.Size = new Size(914, 460);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // listViewFileReplacements
            // 
            listViewFileReplacements.Columns.AddRange(new ColumnHeader[] { columnHeader4, columnHeader7 });
            tableLayoutPanel3.SetColumnSpan(listViewFileReplacements, 3);
            listViewFileReplacements.Dock = DockStyle.Fill;
            listViewFileReplacements.FullRowSelect = true;
            listViewFileReplacements.Location = new Point(3, 3);
            listViewFileReplacements.Name = "listViewFileReplacements";
            listViewFileReplacements.Size = new Size(908, 422);
            listViewFileReplacements.TabIndex = 1;
            listViewFileReplacements.UseCompatibleStateImageBehavior = false;
            listViewFileReplacements.View = View.Details;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "File To Replace";
            columnHeader4.Width = 400;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Replacement File";
            columnHeader7.Width = 500;
            // 
            // btnAddFileReplacement
            // 
            btnAddFileReplacement.Dock = DockStyle.Fill;
            btnAddFileReplacement.Location = new Point(3, 431);
            btnAddFileReplacement.Name = "btnAddFileReplacement";
            btnAddFileReplacement.Size = new Size(94, 26);
            btnAddFileReplacement.TabIndex = 2;
            btnAddFileReplacement.Text = "Add";
            btnAddFileReplacement.UseVisualStyleBackColor = true;
            btnAddFileReplacement.Click += btnAddFileReplacement_Click;
            // 
            // btnRemoveFileReplacement
            // 
            btnRemoveFileReplacement.Dock = DockStyle.Fill;
            btnRemoveFileReplacement.Location = new Point(103, 431);
            btnRemoveFileReplacement.Name = "btnRemoveFileReplacement";
            btnRemoveFileReplacement.Size = new Size(94, 26);
            btnRemoveFileReplacement.TabIndex = 3;
            btnRemoveFileReplacement.Text = "Remove";
            btnRemoveFileReplacement.UseVisualStyleBackColor = true;
            btnRemoveFileReplacement.Click += btnRemoveFileReplacement_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(920, 466);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Text Translation";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(listViewTextTranslation, 0, 0);
            tableLayoutPanel2.Controls.Add(btnAddTextTranslation, 0, 1);
            tableLayoutPanel2.Controls.Add(btnRemoveTextTranslation, 1, 1);
            tableLayoutPanel2.Controls.Add(btnModifyTextTranslation, 2, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(914, 460);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // listViewTextTranslation
            // 
            listViewTextTranslation.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader5, columnHeader6 });
            tableLayoutPanel2.SetColumnSpan(listViewTextTranslation, 4);
            listViewTextTranslation.Dock = DockStyle.Fill;
            listViewTextTranslation.FullRowSelect = true;
            listViewTextTranslation.Location = new Point(3, 3);
            listViewTextTranslation.Name = "listViewTextTranslation";
            listViewTextTranslation.Size = new Size(908, 422);
            listViewTextTranslation.TabIndex = 1;
            listViewTextTranslation.UseCompatibleStateImageBehavior = false;
            listViewTextTranslation.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Syllable";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Japanese Representation";
            columnHeader2.Width = 150;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Original Hex";
            columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Replacement Hex";
            columnHeader6.Width = 150;
            // 
            // btnAddTextTranslation
            // 
            btnAddTextTranslation.Dock = DockStyle.Fill;
            btnAddTextTranslation.Location = new Point(3, 431);
            btnAddTextTranslation.Name = "btnAddTextTranslation";
            btnAddTextTranslation.Size = new Size(94, 26);
            btnAddTextTranslation.TabIndex = 2;
            btnAddTextTranslation.Text = "Add";
            btnAddTextTranslation.UseVisualStyleBackColor = true;
            btnAddTextTranslation.Click += btnAddTextTranslation_Click;
            // 
            // btnRemoveTextTranslation
            // 
            btnRemoveTextTranslation.Dock = DockStyle.Fill;
            btnRemoveTextTranslation.Location = new Point(103, 431);
            btnRemoveTextTranslation.Name = "btnRemoveTextTranslation";
            btnRemoveTextTranslation.Size = new Size(94, 26);
            btnRemoveTextTranslation.TabIndex = 3;
            btnRemoveTextTranslation.Text = "Remove";
            btnRemoveTextTranslation.UseVisualStyleBackColor = true;
            btnRemoveTextTranslation.Click += btnRemoveTextTranslation_Click;
            // 
            // btnModifyTextTranslation
            // 
            btnModifyTextTranslation.Dock = DockStyle.Fill;
            btnModifyTextTranslation.Location = new Point(203, 431);
            btnModifyTextTranslation.Name = "btnModifyTextTranslation";
            btnModifyTextTranslation.Size = new Size(94, 26);
            btnModifyTextTranslation.TabIndex = 4;
            btnModifyTextTranslation.Text = "Modify";
            btnModifyTextTranslation.UseVisualStyleBackColor = true;
            btnModifyTextTranslation.Click += btnModifyTextTranslation_Click;
            // 
            // btnSave
            // 
            tableLayoutPanel1.SetColumnSpan(btnSave, 2);
            btnSave.Dock = DockStyle.Fill;
            btnSave.Location = new Point(3, 535);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(194, 26);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save Configuration";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 564);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(500, 300);
            Name = "OptionsForm";
            Text = "Izuto Options";
            Shown += FontForm_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageFontFile.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TabControl tabControl1;
        private TabPage tabPageFontFile;
        private TabPage tabPage1;
        private Button btnSave;
        private TableLayoutPanel tableLayoutPanel2;
        private ListView listViewTextTranslation;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnAddTextTranslation;
        private Button btnRemoveTextTranslation;
        private Button btnBrowseOptionsFile;
        private TextBox textOptionsFilePath;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel3;
        private ListView listViewFileReplacements;
        private ColumnHeader columnHeader4;
        private Button btnAddFileReplacement;
        private Button btnRemoveFileReplacement;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private Button btnModifyTextTranslation;
    }
}
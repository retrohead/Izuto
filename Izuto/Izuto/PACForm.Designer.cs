namespace Izuto
{
    partial class PACForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PACForm));
            txtPACFilePath = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnAccept = new Button();
            button2 = new Button();
            tabControl1 = new TabControl();
            tabPageTextScripts = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnModifyString = new Button();
            listViewFiles = new ListView();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            tabPageObjects = new TabPage();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageTextScripts.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tabPageObjects.SuspendLayout();
            SuspendLayout();
            // 
            // txtPACFilePath
            // 
            txtPACFilePath.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(txtPACFilePath, 3);
            txtPACFilePath.Dock = DockStyle.Top;
            txtPACFilePath.Location = new Point(3, 3);
            txtPACFilePath.Name = "txtPACFilePath";
            txtPACFilePath.ReadOnly = true;
            txtPACFilePath.Size = new Size(794, 23);
            txtPACFilePath.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(txtPACFilePath, 0, 0);
            tableLayoutPanel1.Controls.Add(btnAccept, 0, 2);
            tableLayoutPanel1.Controls.Add(button2, 1, 2);
            tableLayoutPanel1.Controls.Add(tabControl1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // btnAccept
            // 
            btnAccept.Dock = DockStyle.Fill;
            btnAccept.Location = new Point(3, 421);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new Size(144, 26);
            btnAccept.TabIndex = 4;
            btnAccept.Text = "Import Changes to PKB";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += btnAccept_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(153, 421);
            button2.Name = "button2";
            button2.Size = new Size(144, 26);
            button2.TabIndex = 6;
            button2.Text = "Export Modifed PAC";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnExport_Click;
            // 
            // tabControl1
            // 
            tableLayoutPanel1.SetColumnSpan(tabControl1, 3);
            tabControl1.Controls.Add(tabPageTextScripts);
            tabControl1.Controls.Add(tabPageObjects);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(794, 380);
            tabControl1.TabIndex = 7;
            // 
            // tabPageTextScripts
            // 
            tabPageTextScripts.Controls.Add(tableLayoutPanel2);
            tabPageTextScripts.Location = new Point(4, 24);
            tabPageTextScripts.Name = "tabPageTextScripts";
            tabPageTextScripts.Padding = new Padding(3);
            tabPageTextScripts.Size = new Size(786, 352);
            tabPageTextScripts.TabIndex = 1;
            tabPageTextScripts.Text = "Text Scripts";
            tabPageTextScripts.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(btnModifyString, 0, 1);
            tableLayoutPanel2.Controls.Add(listViewFiles, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(780, 346);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnModifyString
            // 
            btnModifyString.Dock = DockStyle.Left;
            btnModifyString.Location = new Point(3, 317);
            btnModifyString.Name = "btnModifyString";
            btnModifyString.Size = new Size(144, 26);
            btnModifyString.TabIndex = 6;
            btnModifyString.Text = "Modify Selected String";
            btnModifyString.UseVisualStyleBackColor = true;
            btnModifyString.Click += btnModifyString_Click;
            // 
            // listViewFiles
            // 
            listViewFiles.Columns.AddRange(new ColumnHeader[] { columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
            tableLayoutPanel2.SetColumnSpan(listViewFiles, 2);
            listViewFiles.Dock = DockStyle.Fill;
            listViewFiles.FullRowSelect = true;
            listViewFiles.Location = new Point(3, 3);
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new Size(774, 308);
            listViewFiles.TabIndex = 5;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View = View.Details;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Script Sequence ID";
            columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Script Line #";
            columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Text (Shift-JIS)";
            columnHeader5.Width = 250;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Text (Unicode Conversion)";
            columnHeader6.Width = 250;
            // 
            // tabPageObjects
            // 
            tabPageObjects.Controls.Add(listView1);
            tabPageObjects.Location = new Point(4, 24);
            tabPageObjects.Name = "tabPageObjects";
            tabPageObjects.Padding = new Padding(3);
            tabPageObjects.Size = new Size(786, 352);
            tabPageObjects.TabIndex = 2;
            tabPageObjects.Text = "Objects";
            tabPageObjects.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Dock = DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new Point(3, 3);
            listView1.Name = "listView1";
            listView1.Size = new Size(780, 346);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Object";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Size (Bytes)";
            columnHeader2.Width = 100;
            // 
            // PACForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PACForm";
            Text = "Izuto - PAC Browser";
            Shown += PACForm_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageTextScripts.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tabPageObjects.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtPACFilePath;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnAccept;
        private Button button2;
        private TabControl tabControl1;
        private TabPage tabPageTextScripts;
        private TabPage tabPageObjects;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TableLayoutPanel tableLayoutPanel2;
        private ListView listViewFiles;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private Button btnModifyString;
        private ColumnHeader columnHeader6;
    }
}
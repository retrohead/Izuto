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
            tabPageLinkedTextScripts = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            listViewLinkedScripts = new ListView();
            columnHeader9 = new ColumnHeader();
            columnHeader10 = new ColumnHeader();
            columnHeader11 = new ColumnHeader();
            columnHeader12 = new ColumnHeader();
            columnHeader13 = new ColumnHeader();
            btnModifyLinkedScript = new Button();
            tabPageTextScripts = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnModifyString = new Button();
            listViewTextScripts = new ListView();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            tabPageObjects = new TabPage();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            tableLayoutPanel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageLinkedTextScripts.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
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
            tabControl1.Controls.Add(tabPageLinkedTextScripts);
            tabControl1.Controls.Add(tabPageTextScripts);
            tabControl1.Controls.Add(tabPageObjects);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 35);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(794, 380);
            tabControl1.TabIndex = 7;
            // 
            // tabPageLinkedTextScripts
            // 
            tabPageLinkedTextScripts.Controls.Add(tableLayoutPanel3);
            tabPageLinkedTextScripts.Location = new Point(4, 24);
            tabPageLinkedTextScripts.Name = "tabPageLinkedTextScripts";
            tabPageLinkedTextScripts.Size = new Size(786, 352);
            tabPageLinkedTextScripts.TabIndex = 3;
            tabPageLinkedTextScripts.Text = "Linked Scripts";
            tabPageLinkedTextScripts.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(listViewLinkedScripts, 0, 0);
            tableLayoutPanel3.Controls.Add(btnModifyLinkedScript, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.Size = new Size(786, 352);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // listViewLinkedScripts
            // 
            listViewLinkedScripts.Columns.AddRange(new ColumnHeader[] { columnHeader9, columnHeader10, columnHeader11, columnHeader12, columnHeader13 });
            tableLayoutPanel3.SetColumnSpan(listViewLinkedScripts, 2);
            listViewLinkedScripts.Dock = DockStyle.Fill;
            listViewLinkedScripts.FullRowSelect = true;
            listViewLinkedScripts.Location = new Point(3, 3);
            listViewLinkedScripts.Name = "listViewLinkedScripts";
            listViewLinkedScripts.Size = new Size(780, 314);
            listViewLinkedScripts.TabIndex = 7;
            listViewLinkedScripts.UseCompatibleStateImageBehavior = false;
            listViewLinkedScripts.View = View.Details;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Script ID";
            columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Script Line #";
            columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Text (Shift-JIS)";
            columnHeader11.Width = 250;
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Extra Data (Hex)";
            columnHeader12.Width = 250;
            // 
            // columnHeader13
            // 
            columnHeader13.Text = "Size";
            // 
            // btnModifyLinkedScript
            // 
            btnModifyLinkedScript.Dock = DockStyle.Fill;
            btnModifyLinkedScript.Location = new Point(3, 323);
            btnModifyLinkedScript.Name = "btnModifyLinkedScript";
            btnModifyLinkedScript.Size = new Size(144, 26);
            btnModifyLinkedScript.TabIndex = 8;
            btnModifyLinkedScript.Text = "Modify Linked Script";
            btnModifyLinkedScript.UseVisualStyleBackColor = true;
            btnModifyLinkedScript.Click += btnModifyLinkedScript_Click;
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
            tableLayoutPanel2.Controls.Add(listViewTextScripts, 0, 0);
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
            // listViewTextScripts
            // 
            listViewTextScripts.Columns.AddRange(new ColumnHeader[] { columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8 });
            tableLayoutPanel2.SetColumnSpan(listViewTextScripts, 2);
            listViewTextScripts.Dock = DockStyle.Fill;
            listViewTextScripts.FullRowSelect = true;
            listViewTextScripts.Location = new Point(3, 3);
            listViewTextScripts.Name = "listViewTextScripts";
            listViewTextScripts.Size = new Size(774, 308);
            listViewTextScripts.TabIndex = 5;
            listViewTextScripts.UseCompatibleStateImageBehavior = false;
            listViewTextScripts.View = View.Details;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Script ID";
            columnHeader3.Width = 100;
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
            // columnHeader7
            // 
            columnHeader7.Text = "Size";
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Hex";
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
            tabPageLinkedTextScripts.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
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
        private ListView listViewTextScripts;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private Button btnModifyString;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private TabPage tabPageLinkedTextScripts;
        private TableLayoutPanel tableLayoutPanel3;
        private ListView listViewLinkedScripts;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader13;
        private Button btnModifyLinkedScript;
    }
}
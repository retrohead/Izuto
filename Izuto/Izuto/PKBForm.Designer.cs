namespace Izuto
{
    partial class PKBForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PKBForm));
            textBox1 = new TextBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            btnExplorePAC = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnImportPKB = new Button();
            columnHeader4 = new ColumnHeader();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            tableLayoutPanel1.SetColumnSpan(textBox1, 3);
            textBox1.Dock = DockStyle.Top;
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(794, 23);
            textBox1.TabIndex = 0;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            tableLayoutPanel1.SetColumnSpan(listView1, 3);
            listView1.Dock = DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new Point(3, 35);
            listView1.Name = "listView1";
            listView1.Size = new Size(794, 380);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "File";
            columnHeader1.Width = 600;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Offset";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Size";
            // 
            // btnExplorePAC
            // 
            btnExplorePAC.Dock = DockStyle.Fill;
            btnExplorePAC.Location = new Point(153, 421);
            btnExplorePAC.Name = "btnExplorePAC";
            btnExplorePAC.Size = new Size(144, 26);
            btnExplorePAC.TabIndex = 2;
            btnExplorePAC.Text = "Explore Selected PAC File";
            btnExplorePAC.UseVisualStyleBackColor = true;
            btnExplorePAC.Click += btnExplorePAC_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(listView1, 0, 1);
            tableLayoutPanel1.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(btnImportPKB, 0, 2);
            tableLayoutPanel1.Controls.Add(btnExplorePAC, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // btnImportPKB
            // 
            btnImportPKB.Dock = DockStyle.Fill;
            btnImportPKB.Location = new Point(3, 421);
            btnImportPKB.Name = "btnImportPKB";
            btnImportPKB.Size = new Size(144, 26);
            btnImportPKB.TabIndex = 3;
            btnImportPKB.Text = "Import Changes to FA";
            btnImportPKB.UseVisualStyleBackColor = true;
            btnImportPKB.Click += btnImportPKB_Click;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Type";
            // 
            // PKBForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PKBForm";
            Text = "Izuto - PKB Browser";
            Shown += PKBForm_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private Button btnExplorePAC;
        private TableLayoutPanel tableLayoutPanel1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private Button btnImportPKB;
        private ColumnHeader columnHeader4;
    }
}
namespace Izuto
{
    partial class PKBFileSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PKBFileSelectForm));
            treeView1 = new TreeView();
            tableLayoutPanel1 = new TableLayoutPanel();
            textSelectedFile = new TextBox();
            btnConfirm = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            tableLayoutPanel1.SetColumnSpan(treeView1, 2);
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(3, 3);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(794, 380);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(treeView1, 0, 0);
            tableLayoutPanel1.Controls.Add(textSelectedFile, 0, 1);
            tableLayoutPanel1.Controls.Add(btnConfirm, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // textSelectedFile
            // 
            textSelectedFile.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(textSelectedFile, 2);
            textSelectedFile.Dock = DockStyle.Fill;
            textSelectedFile.Enabled = false;
            textSelectedFile.Location = new Point(3, 389);
            textSelectedFile.Name = "textSelectedFile";
            textSelectedFile.Size = new Size(794, 23);
            textSelectedFile.TabIndex = 1;
            textSelectedFile.Text = "No File Selected";
            // 
            // btnConfirm
            // 
            btnConfirm.Dock = DockStyle.Fill;
            btnConfirm.Location = new Point(3, 421);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(144, 26);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // PKBFileSelectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PKBFileSelectForm";
            Text = "Izuto Pick File Browser";
            Shown += PKBFileSelectForm_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TreeView treeView1;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textSelectedFile;
        private Button btnConfirm;
    }
}
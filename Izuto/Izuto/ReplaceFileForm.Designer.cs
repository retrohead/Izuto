namespace Izuto
{
    partial class ReplaceFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplaceFileForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            btnBrowsePKB = new Button();
            btnBrowseLocal = new Button();
            btnConfirm = new Button();
            label1 = new Label();
            label2 = new Label();
            textOriginalFilePath = new TextBox();
            textReplacementFile = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(btnBrowsePKB, 2, 0);
            tableLayoutPanel1.Controls.Add(btnBrowseLocal, 2, 1);
            tableLayoutPanel1.Controls.Add(btnConfirm, 0, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(textOriginalFilePath, 1, 0);
            tableLayoutPanel1.Controls.Add(textReplacementFile, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(683, 98);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btnBrowsePKB
            // 
            btnBrowsePKB.Dock = DockStyle.Fill;
            btnBrowsePKB.Location = new Point(586, 3);
            btnBrowsePKB.Name = "btnBrowsePKB";
            btnBrowsePKB.Size = new Size(94, 26);
            btnBrowsePKB.TabIndex = 0;
            btnBrowsePKB.Text = "...";
            btnBrowsePKB.UseVisualStyleBackColor = true;
            btnBrowsePKB.Click += btnBrowsePKB_Click;
            // 
            // btnBrowseLocal
            // 
            btnBrowseLocal.Dock = DockStyle.Fill;
            btnBrowseLocal.Location = new Point(586, 35);
            btnBrowseLocal.Name = "btnBrowseLocal";
            btnBrowseLocal.Size = new Size(94, 26);
            btnBrowseLocal.TabIndex = 1;
            btnBrowseLocal.Text = "...";
            btnBrowseLocal.UseVisualStyleBackColor = true;
            btnBrowseLocal.Click += btnBrowseLocal_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.Dock = DockStyle.Fill;
            btnConfirm.Location = new Point(3, 67);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(144, 28);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Confirm Selection";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 7);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(144, 25);
            label1.TabIndex = 3;
            label1.Text = "Path to replace";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 39);
            label2.Margin = new Padding(3, 7, 3, 0);
            label2.Name = "label2";
            label2.Size = new Size(95, 15);
            label2.TabIndex = 4;
            label2.Text = "Replacement file";
            // 
            // textOriginalFilePath
            // 
            textOriginalFilePath.BackColor = SystemColors.ControlLight;
            textOriginalFilePath.Dock = DockStyle.Fill;
            textOriginalFilePath.Enabled = false;
            textOriginalFilePath.Location = new Point(153, 3);
            textOriginalFilePath.Name = "textOriginalFilePath";
            textOriginalFilePath.Size = new Size(427, 23);
            textOriginalFilePath.TabIndex = 5;
            // 
            // textReplacementFile
            // 
            textReplacementFile.BackColor = SystemColors.ControlLight;
            textReplacementFile.Dock = DockStyle.Fill;
            textReplacementFile.Enabled = false;
            textReplacementFile.Location = new Point(153, 35);
            textReplacementFile.Name = "textReplacementFile";
            textReplacementFile.Size = new Size(427, 23);
            textReplacementFile.TabIndex = 6;
            // 
            // ReplaceFileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(683, 98);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(699, 137);
            MinimumSize = new Size(699, 137);
            Name = "ReplaceFileForm";
            Text = "Izuto Replace File Configuration";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btnBrowsePKB;
        private Button btnBrowseLocal;
        private Button btnConfirm;
        private Label label1;
        private Label label2;
        private TextBox textOriginalFilePath;
        private TextBox textReplacementFile;
    }
}
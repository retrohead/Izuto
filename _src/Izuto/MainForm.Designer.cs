namespace Izuto
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            textArchiveFaPath = new TextBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            imgListFiles = new ImageList(components);
            btnExplorePKB = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnBrowseArchiveFA = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            pictureBoxLogo = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            SuspendLayout();
            // 
            // textArchiveFaPath
            // 
            textArchiveFaPath.BackColor = SystemColors.ControlLight;
            tableLayoutPanel1.SetColumnSpan(textArchiveFaPath, 2);
            textArchiveFaPath.Dock = DockStyle.Fill;
            textArchiveFaPath.Location = new Point(103, 35);
            textArchiveFaPath.Name = "textArchiveFaPath";
            textArchiveFaPath.ReadOnly = true;
            textArchiveFaPath.Size = new Size(328, 23);
            textArchiveFaPath.TabIndex = 0;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            tableLayoutPanel1.SetColumnSpan(listView1, 4);
            listView1.Dock = DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new Point(3, 67);
            listView1.Name = "listView1";
            listView1.Size = new Size(528, 431);
            listView1.SmallImageList = imgListFiles;
            listView1.TabIndex = 5;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "File";
            columnHeader1.Width = 700;
            // 
            // imgListFiles
            // 
            imgListFiles.ColorDepth = ColorDepth.Depth32Bit;
            imgListFiles.ImageStream = (ImageListStreamer)resources.GetObject("imgListFiles.ImageStream");
            imgListFiles.TransparentColor = Color.Transparent;
            imgListFiles.Images.SetKeyName(0, "file_unk.png");
            imgListFiles.Images.SetKeyName(1, "file_txt.png");
            imgListFiles.Images.SetKeyName(2, "file_zip.png");
            // 
            // btnExplorePKB
            // 
            tableLayoutPanel1.SetColumnSpan(btnExplorePKB, 2);
            btnExplorePKB.Dock = DockStyle.Fill;
            btnExplorePKB.Location = new Point(3, 504);
            btnExplorePKB.Name = "btnExplorePKB";
            btnExplorePKB.Size = new Size(194, 26);
            btnExplorePKB.TabIndex = 6;
            btnExplorePKB.Text = "Explore Selected PKB File";
            btnExplorePKB.UseVisualStyleBackColor = true;
            btnExplorePKB.Click += btnExplorePKB_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Controls.Add(btnBrowseArchiveFA, 3, 1);
            tableLayoutPanel1.Controls.Add(textArchiveFaPath, 1, 1);
            tableLayoutPanel1.Controls.Add(listView1, 0, 2);
            tableLayoutPanel1.Controls.Add(btnExplorePKB, 0, 3);
            tableLayoutPanel1.Controls.Add(menuStrip1, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxLogo, 3, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(534, 565);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // btnBrowseArchiveFA
            // 
            btnBrowseArchiveFA.Dock = DockStyle.Fill;
            btnBrowseArchiveFA.Location = new Point(437, 35);
            btnBrowseArchiveFA.Name = "btnBrowseArchiveFA";
            btnBrowseArchiveFA.Size = new Size(94, 26);
            btnBrowseArchiveFA.TabIndex = 1;
            btnBrowseArchiveFA.Text = "...";
            btnBrowseArchiveFA.UseVisualStyleBackColor = true;
            btnBrowseArchiveFA.Click += btnBrowseArchiveFA_Click;
            // 
            // menuStrip1
            // 
            tableLayoutPanel1.SetColumnSpan(menuStrip1, 4);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(534, 24);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(146, 22);
            openToolStripMenuItem.Text = "Open Archive";
            openToolStripMenuItem.Click += btnBrowseArchiveFA_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 39);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(94, 25);
            label1.TabIndex = 12;
            label1.Text = "Archive FA File";
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = (Image)resources.GetObject("pictureBoxLogo.Image");
            pictureBoxLogo.Location = new Point(437, 504);
            pictureBoxLogo.Name = "pictureBoxLogo";
            tableLayoutPanel1.SetRowSpan(pictureBoxLogo, 2);
            pictureBoxLogo.Size = new Size(94, 58);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 10;
            pictureBoxLogo.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 565);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(550, 300);
            Name = "MainForm";
            Text = "Form1";
            FormClosed += MainForm_FormClosed;
            Shown += MainForm_Shown;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textArchiveFaPath;
        private ListView listView1;
        private Button btnExplorePKB;
        private ColumnHeader columnHeader1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBoxLogo;
        private Button btnBrowseArchiveFA;
        public ImageList imgListFiles;
        private ImageList imageList1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private Label label1;
    }
}

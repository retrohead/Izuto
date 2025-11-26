using plugin_level5.N3DS.Archive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Izuto
{
    public partial class PKBFileSelectForm : Form
    {
        public string SelectedFilePath = "";
        public PKBFileSelectForm()
        {
            InitializeComponent();
        }

        private async void PKBFileSelectForm_Shown(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            var sortedArchives = MainForm.ArchiveFiles.OrderBy(p => p.FilePath.FullName);

            foreach (var item in sortedArchives)
            {
                string[] parts = item.FilePath.FullName.Split('/');

                TreeNodeCollection currentNodes = treeView1.Nodes;
                TreeNode? currentNode = null;

                foreach (string part in parts)
                {
                    // Try to find existing node
                    TreeNode? foundNode = currentNodes.Cast<TreeNode>()
                                                     .FirstOrDefault(n => n.Text == part);

                    if (foundNode == null)
                    {
                        // Create new node if not found
                        foundNode = new TreeNode(part);
                        foundNode.Tag = item;
                        currentNodes.Add(foundNode);
                    }

                    currentNode = foundNode;
                    currentNodes = currentNode.Nodes;
                }
            }
            treeView1.Nodes[0].Text = Path.GetFileName(MainForm.LoadedArchiveFilePath);
            treeView1.Nodes[0].Expand();
            treeView1.EndUpdate();


            treeView1.EndUpdate();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count > 0)
            {
                textSelectedFile.Text = "No File Selected";
            }
            else
            {
                B123ArchiveFile a = (B123ArchiveFile)treeView1.SelectedNode.Tag;
                textSelectedFile.Text = a.FilePath.FullName;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Nodes.Count > 0)
            {
                MessageBox.Show("You must select a file or close the window to cancel", "File Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SelectedFilePath = textSelectedFile.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

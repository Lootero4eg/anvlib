using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls.Extensions
{
    public static class TreeViewExtensions
    {
        public static void MoveBranchUp(this TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);
                }
            }
            else if (node.TreeView.Nodes.Contains(node)) //root node
            {
                int index = view.Nodes.IndexOf(node);
                if (index > 0)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index - 1, node);
                }
            }
        }

        public static void MoveBranchDown(this TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView view = node.TreeView;
            if (parent != null)
            {
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);
                }
            }
            else if (view != null && view.Nodes.Contains(node)) //root node
            {
                if (view.Nodes.Count == 1 && node == view.Nodes[0])
                    return;

                int index = view.Nodes.IndexOf(node);
                if (index < view.Nodes.Count - 1)
                {
                    view.Nodes.RemoveAt(index);
                    view.Nodes.Insert(index + 1, node);
                }
            }
        }

        public static void MoveUp(this TreeNode node)
        {            
            TreeNode parent = node.Parent;            
            TreeView treeview = node.TreeView;

            int orindex = node.Index;
            
            TreeNode tr = (TreeNode)node.Clone();
            
            if (node.Index == 0)
            {
                if (parent != null)
                    node.Remove();
                else
                    return;
                                
                if (parent.Parent == null)
                    treeview.Nodes.Insert(treeview.Nodes.Count - 1, tr);
                else
                    parent.Parent.Nodes.Insert(parent.Parent.Nodes.Count, tr);
            }
            else
            {
                node.Remove();
                if (parent != null)
                    parent.Nodes.Insert(orindex - 1, tr);
                else
                    treeview.Nodes.Insert(orindex - 1, tr);
            }

            treeview.SelectedNode = tr;            
        }

        #warning Метод еще не доделан
        public static void MoveDown(this TreeNode node)
        {
            TreeNode parent = node.Parent;
            TreeView treeview = node.TreeView;

            int orindex = node.Index;

            TreeNode tr = (TreeNode)node.Clone();
            int nodescount = 0;
            if (parent != null)
                nodescount = parent.Nodes.Count;
            else
                nodescount = treeview.Nodes.Count;

            if (node.Index == nodescount)
            {
                if (parent != null)
                    node.Remove();
                else
                    return;

                if (parent.Parent == null)
                    treeview.Nodes.Insert(treeview.Nodes.Count - 1, tr);
                else
                    parent.Parent.Nodes.Insert(parent.Parent.Nodes.Count, tr);
            }
            else
            {
                node.Remove();
                if (parent != null)
                {
                    parent.Nodes.Insert(orindex - 1, tr);
                }
                else
                {
                    treeview.Nodes[orindex].Nodes.Insert(0, tr);
                    //treeview.Nodes.Insert(orindex - 1, tr);
                }
            }

            treeview.SelectedNode = tr;
        }


    }

}

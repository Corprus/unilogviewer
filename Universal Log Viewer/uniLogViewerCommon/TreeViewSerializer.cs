using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace UniversalLogViewer.Common
{
    public class TreeViewSerializer
    {
        private const string XmlNodeTag = "node";
        private const string XmlNodeTextAtt = "text";
        private const string XmlNodeTagAtt = "tag";
        private const string XmlNodeImageIndexAtt = "imageindex";

        public static void SerializeTreeView(TreeView treeView, string fileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName,
                                          System.Text.Encoding.ASCII);
            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            //textWriter.WriteRaw("\r\n");
            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("TreeView");

            // save the nodes, recursive method
            SaveNodes(treeView.Nodes, textWriter);

            textWriter.WriteEndElement();

            textWriter.Close();
        }
        public static void SerializeTreeNode(TreeNode treeNode, string fileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName,
                                          System.Text.Encoding.ASCII);
            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            //textWriter.WriteRaw("\r\n");
            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("TreeNode");

            // save the nodes, recursive method
            SaveNode(treeNode, textWriter);

            textWriter.WriteEndElement();

            textWriter.Close();
        }
        public static void SerializeTreeNodeList(List<TreeNode> treeNodes, string fileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName,
                                          System.Text.Encoding.ASCII);
            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            //textWriter.WriteRaw("\r\n");
            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("TreeNodes");
            foreach (TreeNode Node in treeNodes)
            {
                textWriter.WriteStartElement("TreeNode" + Node.GetHashCode());

                // save the nodes, recursive method
                SaveNode(Node, textWriter);

                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();
            textWriter.Close();
        }
        
        private static void SaveNodes(TreeNodeCollection nodesCollection, XmlTextWriter textWriter)
        {
            for (int i = 0; i < nodesCollection.Count; i++)
            {
                TreeNode node = nodesCollection[i];
                textWriter.WriteStartElement(XmlNodeTag);
                textWriter.WriteAttributeString(XmlNodeTextAtt,
                                                           node.Text);
                textWriter.WriteAttributeString(
                    XmlNodeImageIndexAtt, node.ImageIndex.ToString());
                if (node.Tag != null)
                    textWriter.WriteAttributeString(XmlNodeTagAtt,
                                                node.Tag.ToString());
                // add other node properties to serialize here  
                if (node.Nodes.Count > 0)
                {
                    SaveNodes(node.Nodes, textWriter);
                }
                textWriter.WriteEndElement();
            }
        }
        private static void SaveNode(TreeNode node, XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(XmlNodeTag);
            textWriter.WriteAttributeString(XmlNodeTextAtt,
                                                       node.Text);
            textWriter.WriteAttributeString(
                XmlNodeImageIndexAtt, node.ImageIndex.ToString());
            if (node.Tag != null)
                textWriter.WriteAttributeString(XmlNodeTagAtt,
                                            node.Tag.ToString());
            // add other node properties to serialize here  
            if (node.Nodes.Count > 0)
            {
                SaveNodes(node.Nodes, textWriter);
            }
            textWriter.WriteEndElement();
        }
    }
}

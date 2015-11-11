using System;
using System.Windows.Forms;
using System.Xml;

namespace MyReg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*
            Dir d = new Dir("/", "main");
            d.sub_dirs.Add("sunmain");
            Val v = new Val("name_value","type_value","value");
            d.values.Add(v);
            MainDir.Add(d);
            Serialize();
            */
        }


        XmlDocument document = new XmlDocument();
        TreeNode tn;

        Random random = new Random();

        public XmlNode findDir(string key)
        {
            XmlNode dirNode = document.DocumentElement;
            string a = string.Format("//Dir[key='{0}']", key);
            return dirNode.SelectSingleNode(a);
        }

        public void addNodes(string path, TreeViewCancelEventArgs e)
        {
            XmlNodeList nodeList = findDir(path).ChildNodes[2].ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                string name = node.ChildNodes[1].InnerText, key = node.ChildNodes[0].InnerText;
                e.Node.Nodes.Add(name, name);
                e.Node.Nodes[name].Text = name;
                e.Node.Nodes[name].Tag = key;
                if (node.ChildNodes[2].ChildNodes.Count > 0)
                {
                    e.Node.Nodes[name].Nodes.Add(string.Empty);
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            document.Load("reg.xml");
            XmlNodeList nodeList = findDir("/").ChildNodes[2].ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                string name = node.ChildNodes[1].InnerText, key = node.ChildNodes[0].InnerText;
                treeView1.Nodes.Add(name,name);
                treeView1.Nodes[name].Text = name;
                treeView1.Nodes[name].Tag = key;
                if(node.ChildNodes[2].ChildNodes.Count>0) treeView1.Nodes[name].Nodes.Add(string.Empty);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == string.Empty)
            {
                e.Node.Nodes.Clear();
                string key = e.Node.Tag.ToString();
                addNodes(key, e);
            }
        }

        public void ShowValues()
        {
            XmlNodeList nodeList = findDir(tn.Tag.ToString()).ChildNodes[3].ChildNodes;
            dataGridView1.Rows.Clear();
            foreach (XmlNode node in nodeList)
            {
                string name = node.ChildNodes[0].InnerText, type = node.ChildNodes[1].InnerText, value = node.ChildNodes[2].InnerText;
                dataGridView1.Rows.Add(name, type, value);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tn = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
                contextMenuStrip1.Show(treeView1, e.Location);
            }
            ShowValues();
        }
        

        private void разделToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode dirNode = findDir(tn.Tag.ToString());
            if (dirNode.ChildNodes[3].ChildNodes.Count == 0)
            {
                tn.Expand();
                dirNode = dirNode.ChildNodes[2];

                XmlNode dir, value;
                string name = "dir_" + random.Next(1000, 10000000).ToString();
                dir = document.CreateElement("Dir");

                value = document.CreateElement("key");
                value.InnerText = tn.Tag.ToString() + name + "/";
                dir.AppendChild(value);

                value = document.CreateElement("name");
                value.InnerText = name;
                dir.AppendChild(value);

                value = document.CreateElement("sub_dirs");
                dir.AppendChild(value);

                value = document.CreateElement("values");
                dir.AppendChild(value);

                dirNode.AppendChild(dir);

                document.Save("reg.xml");

                tn.Nodes.Add(name, name);
                tn.Nodes[name].Text = name;
                tn.Nodes[name].Tag = tn.Tag.ToString() + name + "/";
                tn.Expand();
            }
            else
            {
                MessageBox.Show("Невозможно создать параметр тк есть вложенные параметры");
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode dirNode = findDir(tn.Tag.ToString());
            XmlNode parent = dirNode.ParentNode;
            parent.RemoveChild(dirNode);
            document.Save("reg.xml");
            tn.Remove();
        }

        private void параметрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode dirNode = findDir(tn.Tag.ToString());
            if (dirNode.ChildNodes[2].ChildNodes.Count == 0)
            {
                dirNode = dirNode.ChildNodes[3];

                XmlNode Val, value;
                Val = document.CreateElement("Val");

                value = document.CreateElement("name");
                value.InnerText = "name"+random.Next(0,1000).ToString();
                Val.AppendChild(value);

                value = document.CreateElement("type");
                value.InnerText = "type" + random.Next(0, 1000).ToString();
                Val.AppendChild(value);

                value = document.CreateElement("value");
                value.InnerText = "value" + random.Next(0, 1000).ToString();
                Val.AppendChild(value);

                dirNode.AppendChild(Val);

                document.Save("reg.xml");

                ShowValues();
            }
            else
            {
                MessageBox.Show("Невозможно создать параметр тк есть вложенные папки");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                 e.RowIndex >= 0)
            {               
                if (e.ColumnIndex == 3)
                {
                    XmlNode dirNode = findDir(tn.Tag.ToString());
                    XmlNode valuesNode = dirNode.ChildNodes[3];
                    XmlNode value = valuesNode.ChildNodes[e.RowIndex];
                    value.ChildNodes[0].InnerText = dataGridView1[0, e.RowIndex].Value.ToString();
                    value.ChildNodes[1].InnerText = dataGridView1[1, e.RowIndex].Value.ToString();
                    value.ChildNodes[2].InnerText = dataGridView1[2, e.RowIndex].Value.ToString();

                    document.Save("reg.xml");
                }
                if (e.ColumnIndex == 4)
                {
                    XmlNode dirNode = findDir(tn.Tag.ToString());
                    XmlNode valuesNode = dirNode.ChildNodes[3];
                    valuesNode.RemoveChild(valuesNode.ChildNodes[e.RowIndex]);

                    document.Save("reg.xml");
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
                
            }
        }
    }
}

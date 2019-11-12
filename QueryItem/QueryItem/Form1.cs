using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QueryItem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            items = new List<string>();
            if (File.Exists("items.html"))
                items = File.ReadAllText("items.html").Split(new string[] { "<item>", "</item>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string importtext = textBox1.Text;
            List<string> find = items.FindAll(s => s.Contains(importtext.ToUpper()));
            if (find.Count > 0)
            {
                string htmlpage = "";
                int sum = 0;
                foreach (string s in find)
                {
                    sum++;
                    if (sum > 10) break;
                    htmlpage += "<item>" + s + "</item>\r\n";
                    webBrowser1.DocumentText = htmlpage;
                }
            }
        }

        private List<string> items;
    }
}

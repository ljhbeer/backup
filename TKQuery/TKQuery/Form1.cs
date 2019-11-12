using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace TKQuery
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class Form1 : Form
    {
        public Form1()
        {
           InitializeComponent();
           dc = new DataConfig();
           InitUI();
           itemsdt = dc.ItemsDt;
           dataGridView1.DataSource = itemsdt;
        }
        private void InitUI()
        {
            InitCombox(dc.comboBoxpage,comboBoxpages);
            InitCombox(dc.DtcomboBoxtx, comboBoxtx, "cname", "id");
            InitCombox(dc.DtcomboBoxzsd, comboBoxzsd, "cname", "id");
        }
        private void InitCombox(List<int> list, ComboBox cbx)
        {
            cbx.Items.Clear();
            foreach (int v in list)
            {
                cbx.Items.Add(v);
            }
        }
        private void InitCombox(DataTable dt, ComboBox cbx, string displaymember, string valuemember)
        {
            cbx.DataSource = null;           
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = 0;
                dr["cname"] = "不限";
                dt.Rows.InsertAt(dr, 0);
                cbx.DataSource = dt;
                cbx.DisplayMember = displaymember;
                cbx.ValueMember = valuemember;
            }
        }

        public void AddQuestion(string id)
        {
            if (id != null)
            {
                DataRowCollection drc = dc.QueryItems(id);
                foreach (DataRow dr in drc)
                {
                    itemsdt.ImportRow(dr);
                }                
            }
        }
        public void RemoveQuestion(string id)
        {
            if (id != null)
            {
                DataRow[] check = itemsdt.Select("id = " + id);
                if (check.Count() > 0)
                {
                    foreach (DataRow c in check)
                        itemsdt.Rows.Remove(c);
                }
            }
        }
        public void ShowPaper(string type, string id)
        {
            if (id == "" || id == null) return;
            try
            {
                if (type == "customer")
                    webBrowser1.DocumentText = dc.QueryCustomPaper(id);
                if (type == "paper")
                    webBrowser1.DocumentText = dc.QueryPaper(id);
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                showfiletxt(ole.ToString());
            }
        }
        public void GoPage(int beginrec, int items, int type) // type = 1 //item  2// paper
        {
            this.showfiletxt("beginrec:=" + beginrec + "  " + items + " " + type);
            if (sqlcondition == "" ) return;
            try
            {
                if (type == 1)
                    webBrowser1.DocumentText = dc.QueryPage(items, sqlcondition, sqlorder, beginrec);
                else
                    webBrowser1.DocumentText = dc.QueryPaperList(items, beginrec);
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                showfiletxt(ole.ToString());
            }
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            int items;
            ConstructCondition(out items, out sqlcondition);
            ConstructCondition(out sqlorder);           
            try
            {
                webBrowser1.DocumentText = dc.QueryPage(items, sqlcondition, sqlorder);                
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                showfiletxt(ole.ToString());
            }
        }
        private void buttonQuerPaper_Click(object sender, EventArgs e)
        {
            string sqlkeys = ConstructConditionKeys();
            if (sqlkeys == "")
            {
                this.showfiletxt("请输入正确的关键词重新查询");
                return;
            }
            sqlkeys = sqlkeys.Replace("question.qview", "cname");
            webBrowser1.DocumentText = dc.QueryPaperList(sqlkeys);
        }
        private void buttonShowPaperlist_Click(object sender, EventArgs e)
        {
            int items = ConstructPaperItemCount();
            webBrowser1.DocumentText = dc.QueryPaperList(items); 
        }
        private void buttonShowCustomer_Click(object sender, EventArgs e)
        {
            int items = ConstructPaperItemCount();
            webBrowser1.DocumentText = dc.QueryCustomPaperList(items);
        }
        private void buttonShowItems_Click(object sender, EventArgs e)
        {
            try
            {                
                webBrowser1.DocumentText = dc.QueryItems(itemsdt);
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                showfiletxt(ole.ToString());
            }
        }
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            string ids = ConstructHtmlids();
            DataRowCollection drc = dc.QueryItems(ids);
            foreach (DataRow dr in drc)
            {
                itemsdt.ImportRow(dr);
            }   
        }
        private void buttonOutput_Click(object sender, EventArgs e)
        {
            try
            {
                dc.QueryOutputPaper(itemsdt, checkBoxoutlocalimg.Checked);
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                showfiletxt(ole.ToString());
            }
        }
        private void buttonclearitems_Click(object sender, EventArgs e)
        {
            itemsdt.Rows.Clear();
        }        
        private void buttonlocalimg_Click(object sender, EventArgs e)
        {
            string html = webBrowser1.DocumentText;
            //html = Convertlocalimg(html);
            webBrowser1.DocumentText = html;
        }
        private void buttonsavetoknowledge_Click(object sender, EventArgs e)
        {
            if (itemsdt.Rows.Count > 0)
            {
                string cname = InputBox.Input("输入知识点名称", "zsd", "");
                if (cname == "") return;
                string strlid = "";
                foreach (DataRow drs in itemsdt.Rows)
                {
                    strlid += drs["id"].ToString() + ",";
                }
                strlid = strlid.Remove(strlid.Length - 1);
                dc.SaveItemsToCustom(cname, strlid,itemsdt.Rows.Count);
                this.showfiletxt("保存成功");
            }
            else
            {
                this.showfiletxt("保存失败");
            }
        }
        private void buttonSetDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.textBoxShow.Text = "当前数据库：" + fd.FileName;
                dc.ChangeDatabaseFilename(fd.FileName);
                InitUI();              
            }
        }
        private void buttonPPT_Click(object sender, EventArgs e)
        {
            FormPPT f = new FormPPT(itemsdt, dc);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }       
       
        private void ConstructCondition(out string sqlorder)
        {
            int sufq = 0;
            int stime = 0;
            sqlorder = "";
            if (comboBoxSortUFQ.SelectedIndex != -1)
                sufq = comboBoxSortUFQ.SelectedIndex;
            if (comboBoxSortTime.SelectedIndex != -1)
                stime = comboBoxSortTime.SelectedIndex;
            if (sufq == 0 && stime == 0)
            {
                sqlorder = "";
            }
            else if (sufq == 0 && stime != 0)
            {
                switch (stime)
                {
                    case 1: sqlorder = " order by INDATE DESC"; break;
                    case 2: sqlorder = " order by INDATE ASC"; break;
                    case 3: sqlorder = " order by INDATE DESC"; break;
                }
            }
            else if (sufq != 0 && stime == 0)
            {
                switch (sufq)
                {
                    case 1: sqlorder = " order by UFQ DESC"; break;
                    case 2: sqlorder = " order by UFQ ASC"; break;
                }
            }
            else
            {
                switch (stime)
                {
                    case 1: sqlorder = " order by INDATE DESC"; break;
                    case 2: sqlorder = " order by INDATE ASC"; break;
                    case 3: sqlorder = " order by INDATE DESC"; break;
                }
                switch (sufq)
                {
                    case 1: sqlorder += ", UFQ DESC"; break;
                    case 2: sqlorder = ", UFQ ASC"; break;
                }
            }
        }
        private void ConstructCondition(out int items, out string sqlcondition)
        {
            int tid = 0;
            int kid = 0;
            items = 50;
            if (comboBoxtx.SelectedIndex != -1)
            {
                if (comboBoxtx.SelectedIndex != 0)
                    tid = (int)comboBoxtx.SelectedValue;
            }
            if (comboBoxzsd.SelectedIndex != -1)
            {
                if (comboBoxzsd.SelectedIndex != 0)
                    kid = (int)comboBoxzsd.SelectedIndex;
            }
            if (comboBoxpages.SelectedIndex != -1)
            {
                items = (int)comboBoxpages.SelectedItem;
            }
            sqlcondition = " where question.id = questioninfo.id and questioninfo.pid = tpapers.id  ";
            if (tid != 0)
                sqlcondition += " and tid = " + tid;
            if (kid != 0)
                sqlcondition += " and kid = " + kid;
            sqlcondition += ConstructConditionKeys();
        }
        private string ConstructConditionKeys(  )
        {
            string sqlkeys = "";
            string keys = textBoxKeys.Text.Trim();
            if (keys != "")
            {
                string[] vkeys = keys.Split(new string[] { " ", "\t", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                int ii = 0;
                foreach (string k in vkeys)
                {
                    sqlkeys += "  and question.qview like '%" + k + "%'  ";
                    if (ii++ > 5) break;
                }
            }
            return sqlkeys;
        }
        private string ConstructHtmlids()
        {
            string ids = "";
            Regex r = new Regex("id=\"testitemli-([0-9]+)\"");
            MatchCollection mc = r.Matches(webBrowser1.DocumentText);
            foreach (Match m in mc)
            {
                ids += m.Groups[1] + ",";
            }
            if (ids != "")
                ids = ids.Remove(ids.Length - 1);
            return ids;
        }
        private int ConstructPaperItemCount()
        {
            int items = 200;
            if (comboBoxpages.SelectedIndex != -1)
            {
                items = (int)comboBoxpages.SelectedItem;
            }
            items = items < 200 ? 200 : items;
            return items;
        }
        public static string GetEqualValue(string src, string begin, string end)
        {
            if (!src.Contains(begin))
                return "";
            src = src.Substring(src.IndexOf(begin) + begin.Length) + end;
            src = src.Substring(0, src.IndexOf(end)).Trim();
            return src;
        }
        public void showfiletxt(string text)
        {
            this.textBoxShow.Text = text;
        }
        protected override void OnLoad(EventArgs e)
        {
            webBrowser1.ObjectForScripting = this;
            base.OnLoad(e);
        }

        private string sqlcondition;
        private string sqlorder;
        private DataTable itemsdt;
        private DataConfig dc ;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace TKQuery
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class FormK : Form
    {
        public FormK()
        {
           InitializeComponent();
           activechapterindex = -2;
           dc = new DataConfig();
           itemsdt = dc.ItemsDt;
           listdt = itemsdt.Clone();
           listdt.Rows.Clear();

           t = @"<style type=""text/css""> 
/* 由开发人员工具生成。它可能不是原始源文件的准确表示形式。*/
body{
	font-size:[font-size]px;
}
.spy LI{
	list-style-type: none;
	text-indent: 1em;
}
.spy P{
	margin:0 auto; 
}
</style>
<div class=""spy"">
<--!item-->
</div>";

           InitUI();
        }
        private void InitUI()
        {
            InitCombox(dc.comboBoxpage,comboBoxpages);
            InitCombox(dc.DtcomboBoxtx, comboBoxtx, "cname", "id");
            InitCombox(dc.DtcomboBoxzsd, comboBoxzsd, "cname", "id");
            dgvt.DataSource = itemsdt;
            dgvl.DataSource = itemsdt.Clone();
            dgvz.DataSource = dc.QueryTable("select * from chapter");
            dgvk.DataSource = dc.QueryTable("select * from [section] where 1=2");
            //dgvl.DataSource = 
            ShowDataGridView(dgvz, "cname");
            ShowDataGridView(dgvk, "cname");
            ShowDataGridView(dgvl, "qview|cname");
            ShowDataGridView(dgvt, "qview|cname");
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
                //this.textBoxShow.Text = "当前数据库：" + fd.FileName;
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

        private void buttonAddCustom_Click(object sender, EventArgs e)
        {
            string cname = InputBox.Input("输入章节名称", "章节", "");
            if (cname == "") return;
            dc.InsertNewName("chapter", cname);
            RefreshZj();
        }
        private void buttonAddZsd_Click(object sender, EventArgs e)
        {
            if (dgvz.CurrentRow.Index >= 0 && dgvz.CurrentRow.Index < dgvz.Rows.Count)
            {
                string cname = InputBox.Input("输入知识点名称", "知识点", "");
                cname = cname.Replace("|", "").Replace("-", "");
                if (cname == "") return;
                string zjid = dgvz.CurrentRow.Cells["ID"].Value.ToString(); 
                dc.InsertSection(cname, zjid);                
                RefreshdgvSection();
            }
        }
        private void buttonClearZsdItems_Click(object sender, EventArgs e)
        {
            if (dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count
                && dgvz.CurrentRow.Index >= 0 && dgvz.CurrentRow.Index < dgvz.Rows.Count
                )
            {
                string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                string sql = "delete * from sectionquestion where sectionid =" + sectionid;
                dc.Update(sql);
                listdt.Rows.Clear();
            }
        }
        private void buttonAddItemToZsd_Click(object sender, EventArgs e)
        {
            if (dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count
                && dgvz.CurrentRow.Index >= 0 && dgvz.CurrentRow.Index < dgvz.Rows.Count
                )
            {
                string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                foreach (DataRow dr in itemsdt.Rows)
                {
                    string id = dr["ID"].ToString();
                    if (listdt.Select(" id = " + id).Count() > 0)
                        continue;
                    string sql = "insert into sectionquestion(questionid,sectionid) values(" + id + "," + sectionid + ")";
                    dc.Update(sql);
                    listdt.ImportRow(dr);
                }
                //RefreshdgvList(sectionid);                
            }
        }
        private void buttonhtmlitemtol_Click(object sender, EventArgs e)
        {
            if (!webBrowser1.DocumentText.StartsWith("<style type=\"text/css\"> "))
                return;
            string id = Form1.GetEqualValue(webBrowser1.DocumentText, "testitemli-", "\"");
            if (dgvk.CurrentRow != null && dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count
               && dgvz.CurrentRow.Index >= 0 && dgvz.CurrentRow.Index < dgvz.Rows.Count
               )
            {
                if(listdt.Select(" id = " + id).Count()>0)
                    return;
                string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                string sql = "insert into sectionquestion(questionid,sectionid) values(" + id + "," + sectionid+")";
                dc.Update(sql);                  
                RefreshdgvList(sectionid);
            }

        }
        private void buttonltot_Click(object sender, EventArgs e)
        {
            if (dgvl.Rows.Count > 0 && dgvl.DataSource != null)
            {
                webBrowser1.DocumentText = "";
                foreach (DataRow dr in listdt.Rows)
                {
                    itemsdt.ImportRow(dr);
                }
            }
        }
        private void dgvz_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((dgvz.CurrentRow.Index != -1 && dgvz.CurrentRow.Index != activechapterindex))
            {
                activechapterindex = dgvz.CurrentRow.Index;                
                RefreshdgvSection();
            }
        }
        private void dgvk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count)
            {
                string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();  
                RefreshdgvList(sectionid);
            }
        }
        private void dgvl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvl.CurrentRow.Index >= 0 && dgvl.CurrentRow.Index < dgvl.Rows.Count)
            {
                
                string id = dgvl.CurrentRow.Cells["id"].Value.ToString();
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    foreach (DataRow dr in listdt.Select(" id = " + id))
                    {
                        if (itemsdt.Select(" id = " + id).Count() == 0)
                            itemsdt.ImportRow(dr);
                    }
                }
                else
                {
                    RefreshItemList(id);
                }
            }
        }
        private void dgvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvt.CurrentRow.Index >= 0 && dgvt.CurrentRow.Index < dgvt.Rows.Count)
            {
                string id = dgvt.CurrentRow.Cells["id"].Value.ToString();

                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    bool refresh = false;
                    string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                    foreach (DataRow dr in itemsdt.Select(" id = " + id))
                    {
                        if (listdt.Select(" id = " + id).Count() == 0)
                        {
                            listdt.ImportRow(dr);
                            string sql = "insert into sectionquestion(questionid,sectionid) values(" + id + "," + sectionid + ")";
                            dc.Update(sql);
                            //RefreshdgvList(sectionid);
                            refresh = true;
                        }
                    }
                    if (refresh)
                        //RefreshdgvList(sectionid);
                        ;

                }
                else { RefreshItemListDtsave(id); }
            }
        }
        private void dgvk_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                if (dgvk.CurrentRow != null)
                    if (dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count)
                    {                        
                        string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                        string sql = "delete * from [section] where id = " + sectionid ;
                        dc.Update(sql);
                        RefreshdgvSection();
                        RefreshdgvList("0");
                    }
        }
        private void dgvl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                if (dgvl.CurrentRow != null && dgvk.CurrentRow!= null )
                    if (dgvk.CurrentRow.Index >= 0 && dgvk.CurrentRow.Index < dgvk.Rows.Count)
                    if (dgvl.CurrentRow.Index >= 0 && dgvl.CurrentRow.Index < dgvl.Rows.Count)
                    {
                        string questionid = dgvl.CurrentRow.Cells["ID"].Value.ToString();
                        string sectionid = dgvk.CurrentRow.Cells["ID"].Value.ToString();
                        string sql = "delete * from sectionquestion  where questionid = " + questionid 
                                  + " and  sectionid = " + sectionid;                        
                        dc.Update(sql);
                        DataRow[] drv = listdt.Select("id = " + questionid);
                        if (drv.Length == 1)
                            listdt.Rows.Remove(drv[0]);
                    }
        }
       
        private void RefreshdgvList(string ids)
        {
            listdt = dc.QuerySectionItems(ids);
            dgvl.DataSource = listdt;
        }
        private void RefreshItemList(string id)
        {
            DataRow[] drv = listdt.Select("id in (" + id + ") ");
            if (drv.Length == 1)
            {
                webBrowser1.DocumentText = t.Replace("[font-size]", Fontsize.ToString()).Replace("<--!item-->", dc.QueryItem(drv[0]));
            }
        }
        private void RefreshItemListDtsave(string id)
        {
            DataRow[] drv = itemsdt.Select("id in (" + id + ") ");
            if (drv.Length == 1)
            {
                webBrowser1.DocumentText = t.Replace("[font-size]", Fontsize.ToString()).Replace("<--!item-->", dc.QueryItem(drv[0]));
            }
        }
        private void RefreshZj()
        {
            dgvz.DataSource = dc.QueryTable("select * from chapter");
        }
        private void RefreshdgvSection()
        {
            if (dgvz.CurrentRow.Index >= 0 && dgvz.CurrentRow.Index < dgvz.Rows.Count)
            {
                string chapterid = dgvz.CurrentRow.Cells["ID"].Value.ToString();
                RefreshdgvSection(chapterid);
            }
        }
        private void RefreshdgvSection(string chapterid)
        {
            dgvk.DataSource = dc.QueryTable("select * from [section] where chapterid = " + chapterid );
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
        private static void ShowDataGridView(DataGridView dgv, string shownames) //by | 
        {
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (shownames.Contains(c.Name))
                    c.Visible = true;
                else
                    c.Visible = false;
            }
        }
        public void showfiletxt(string text)
        {
            this.textBox1.Text = text;
        }
        protected override void OnLoad(EventArgs e)
        {
            webBrowser1.ObjectForScripting = this;
            base.OnLoad(e);
        }

        private string sqlcondition;
        private string sqlorder;
        private DataTable itemsdt;
        private DataTable listdt;
        private DataConfig dc ;
        private int activechapterindex;
        private string t;
        private int Fontsize = 20;

        
    }
}

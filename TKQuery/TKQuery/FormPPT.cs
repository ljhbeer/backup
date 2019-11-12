using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TKQuery
{
    public partial class FormPPT : Form
    {
        public FormPPT(DataTable itemsdt, DataConfig dc)
        {
            this.itemsdt = itemsdt;
            this.dc = dc;        
            InitializeComponent();
            this.dgv2.DataSource = this.itemsdt;
            this.activequestionid = -1;
            this.Fontsize = 20;
            this.Zoomsize = 1.8F;
            splitContainer4.Panel2Collapsed = true;
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
            NextShow();
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            if (buttonHideL.Text == "<<")
            {
                buttonHideL.Text = ">>";
                splitContainer2.Panel1Collapsed = true;
            }
            else
            {
                buttonHideL.Text = "<<";
                splitContainer2.Panel1Collapsed = false;
            }
        }
        private void buttonHideR_Click(object sender, EventArgs e)
        {
            if (buttonHideR.Text == "︽")
            {
                buttonHideR.Text = "︾";
                splitContainer4.Panel2Collapsed = true;
            }
            else
            {
                buttonHideR.Text = "︽";
                splitContainer4.Panel2Collapsed = false;
            }
        }
        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            PreviousShow();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextShow();
        }
        private void buttonAj_Click(object sender, EventArgs e)
        {
            Fontsize+=4;
            ShowItem();
        }
        private void buttonAf_Click(object sender, EventArgs e)
        {
            Fontsize-=4;
            ShowItem();
        }
        private void buttonZj_Click(object sender, EventArgs e)
        {
            if( Zoomsize <4)
                Zoomsize += 0.2F;
            ShowZoomItem();
        }
        private void buttonZf_Click(object sender, EventArgs e)
        {
            if (Zoomsize > 0.8)
                Zoomsize -= 0.2F;
            ShowZoomItem();
        }
        private void NextShow()
        {
            this.activequestionid++;
            if (activequestionid < 0 || activequestionid >= itemsdt.Rows.Count)
            {
                this.activequestionid--;
                webBrowser1.DocumentText = "<h1>已经是最后一题没有可以显示的题目了<h1>";
                return;
            }
            ShowItem();
        }
        private void PreviousShow()
        {
            this.activequestionid--;
            if (activequestionid < 0 || activequestionid >= itemsdt.Rows.Count)
            {
                this.activequestionid++;
                webBrowser1.DocumentText = "<h1>这是第一题没有可以显示的题目了</h1>";
                return;
            }
            ShowItem();
        }
        private void ShowItem()
        {
            try
            {
                string html = "<h1>暂时未找到该题</h1>";
                if (itemsdt.Rows.Count > 0 && activequestionid >= 0 && activequestionid < itemsdt.Rows.Count)
                {
                    html = dc.QueryItem(itemsdt.Rows[activequestionid]);
                }
                html = t.Replace("[font-size]", Fontsize.ToString()).Replace("<--!item-->", html);
                webBrowser1.DocumentText = html;
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                webBrowser1.DocumentText = "<h1>找到该题出现故障</h1><p>" + ole.Message + "</p>"; ;
            }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Body.Style = "zoom:" + Zoomsize;
        }
        private void ShowZoomItem()
        {
            this.webBrowser1.Document.Body.Style = "zoom:" + Zoomsize;            
        }

        private int activequestionid;
        private int Fontsize;
        private float Zoomsize;
        private string t;
        private DataTable itemsdt;
        private DataConfig dc;
    }
}

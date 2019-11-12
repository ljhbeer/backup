using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SortDesk
{
    public partial class Form1 : Form
    {
        private bool mdragging;
        private CConfig mcfg;
        private  object draggingdgv;
        private  string mfilename;
        private bool mballowspaceasdel;
        private bool mballowfastinput;
        private bool mballowadjustgap;
        public Form1()
        {
            mcfg = null;
            draggingdgv = null;
            InitializeComponent();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && !mdragging ) // 1 - vbLeftButton 
            {
                Console.WriteLine("Move ");
                DataGridView dgv = (DataGridView)sender;
                Point cp = dgv.PointToClient(new Point(e.X, e.Y));
                DataGridView.HitTestInfo dh = dgv.HitTest(e.X, e.Y);
                int row = dh.RowIndex;
                int col = dh.ColumnIndex;
                Console.WriteLine(row + ":" + col);
                if (row >= 0 && col >= 0)
                {
                    Object temp = dgv[dh.ColumnIndex, dh.RowIndex].Value;
                    if (temp != null && temp.ToString() != "")
                    {
                        Console.Write("Drag.");
                        this.draggingdgv = sender;
                        dgv.AllowDrop = true;
                        dgv.DoDragDrop(dgv[dh.ColumnIndex, dh.RowIndex], DragDropEffects.Move | DragDropEffects.Copy); // OnOLEStartDrag will be called
                    }
                }
            }
        }

        private void dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == -1) return;
            //e.CellStyle.BackColor = m_cfg.GetBackColor(e);
        }

        private void dgv_DragEnter(object sender, DragEventArgs e)
        {
            Console.Write("<");         
        }

        private void dgv_DragLeave(object sender, EventArgs e)
        {
            Console.Write(">");
            mdragging = false;            
        }

        private void dgv_DragOver(object sender, DragEventArgs e)
        {
            Console.Write(".");
            if (e.AllowedEffect == (DragDropEffects.Move | DragDropEffects.Copy))
            {
                DataGridView dgv = (DataGridView)sender;
                e.Effect = DragDropEffects.None;
                Point p = dgv.PointToClient(new Point(e.X, e.Y));
                DataGridView.HitTestInfo droppos = dgv.HitTest(p.X, p.Y);
                if (droppos.RowIndex < 0 || droppos.ColumnIndex < 0) return;
                CStudent dropkv = (CStudent)dgv[droppos.ColumnIndex, droppos.RowIndex].Value;
                DataGridViewTextBoxCell dragcell = (DataGridViewTextBoxCell)e.Data.GetData(typeof(System.Windows.Forms.DataGridViewTextBoxCell));
                CStudent dragkv = (CStudent)dragcell.Value;
                if (dropkv == dragkv)
                    return;
                e.Effect = DragDropEffects.Move;               
            }
        }

        private void dgv_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("|>");
            mdragging = false;
            DataGridView dgvdrop = (DataGridView)sender;
            DataGridView dgvdrag = (DataGridView)this.draggingdgv;
            Point p = dgvdrop.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo droppos = dgvdrop.HitTest(p.X, p.Y);
            if (dgvdrop[droppos.ColumnIndex, droppos.RowIndex] != e.Data.GetData(typeof(System.Windows.Forms.DataGridViewTextBoxCell)))
            {
                DataGridViewTextBoxCell dragcell = (DataGridViewTextBoxCell)e.Data.GetData(typeof(System.Windows.Forms.DataGridViewTextBoxCell));
                object tmp = dgvdrop[droppos.ColumnIndex, droppos.RowIndex].Value;
                dgvdrop[droppos.ColumnIndex, droppos.RowIndex].Value = dragcell.Value;
                dragcell.Value = tmp;    
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitDgvUI(dgv);
            InitDgvUI(dgvstu);
            dgvstu.RowTemplate.Height = 25;
        }
        private void InitDialog()
        {
         
            //int xqsum = m_cfg.Result >> 24;
            //int jcsum = (m_cfg.Result >> 16) % 256;
            InitDgv(mcfg.Layout/256, mcfg.Layout%256, dgv);
            InitDgvStu(mcfg.Layout/256 * mcfg.Layout%256, dgvstu);
        }
        public static void InitDgvUI(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.DefaultCellStyle.Font = new Font(dgv.DefaultCellStyle.Font.FontFamily, 12, FontStyle.Regular);
            dgv.AllowDrop = true;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersWidth = 80;
            dgv.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dgv.RowTemplate.Height = 50;
            dgv.ReadOnly = true;
        }
        private void InitDgvStu(int stucnt, DataGridView dgv)
        {
            dgv.RowCount = stucnt;
            dgv.ColumnCount = 1;
            dgv.RowHeadersWidth = 20;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Width = 80;               
                dgv.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }            
        }
        public void InitDgv(int xqsum, int jcsum, DataGridView dgv) // used by settypeform
        {
            dgv.RowCount = jcsum;
            dgv.ColumnCount = xqsum;
            dgv.RowHeadersWidth = 20;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].Width = 80;               
                dgv.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgv.RowCount; i++)
            {
               // dgv.Rows[i].HeaderCell.Value = "第" + (i + 1).ToString() + "节";
            }
            for (int roomid = 0; roomid < dgv.RowCount; roomid++)
            {
                for (int groupid = 0; groupid < dgv.ColumnCount; groupid++)
                {
                    dgv[groupid, roomid].Value = this.mcfg.Mdesk[groupid, roomid];
                 }
            }
        }

        private void MenuItemNew_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            if (mcfg != null && MessageBox.Show("是否保存了当前文档，本操作将会丢失当前设置，是否继续",
             "警告", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "座位文件|*.desk";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mcfg = new CConfig();
                string filestr = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                string[] vfilestr = filestr.Split(new string[] { "<head>", "</head><gap>", "</gap><front>"
                    , "</front><namelist>","</namelist>" }, StringSplitOptions.RemoveEmptyEntries);
                if (vfilestr.Length != 4)
                {
                    MessageBox.Show("座位表格式不正确");
                    return;
                }
                this.mfilename = openFileDialog.FileName;
                // head  [0] head
                mcfg.Layout = int.Parse(vfilestr[0], System.Globalization.NumberStyles.HexNumber);
                mcfg.SetDeskGap(vfilestr[1]);
                mcfg.SetFront(vfilestr[2]);
                InitNamelistString(vfilestr[3]);
                
                InitDialog();
                stopwatch.Stop();
                Console.WriteLine(1 + ":" + stopwatch.Elapsed);
            }
        }

        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            if (this.mfilename == null || this.mfilename.Length == 0 || !File.Exists(mfilename))
            {
                if (mcfg == null || !SaveAs())
                    return;
            }
            Save();
        }


        private void MenuItemSaveAs_Click(object sender, EventArgs e)
        {
            if (mcfg != null && SaveAs())
            {
                Save();
            }
        }
        private void Save()
        {
            for (int roomid = 0; roomid < mcfg.roomcnt; roomid++)
            {               
                for (int groupid = 0; groupid < mcfg.groupcnt; groupid++)
                {
                    mcfg.Mdesk[groupid, roomid] =(CStudent) dgv[groupid, roomid].Value;
                }
            }
            mcfg.Save(this.mfilename);           
        }
        private bool SaveAs()
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "座位文件|*.desk";
            SaveFileDialog.RestoreDirectory = true;
            SaveFileDialog.FilterIndex = 1;
            SaveFileDialog.Title = "保存座位文件";
            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            this.mfilename = SaveFileDialog.FileName;
            return true;
        }
        private void MenuItemImportNameList_Click(object sender, EventArgs e)
        {
            	//addname
        }

        private void MenuItemExportExcel_Click(object sender, EventArgs e)
        {
           
        }

        private void MenuItemClose_Click(object sender, EventArgs e)
        {
            
        }

        private void MenuItemDesktoList_Click(object sender, EventArgs e)
        {
            List<CStudent> ls = new List<CStudent>();
            CollectData(ls);
            int k=0;
            foreach (CStudent s in ls)
                dgvstu[0, k++].Value = s;
        }

        private void CollectData(List<CStudent> ls)
        {
            for (int i = 0; i < dgvstu.RowCount; i++)
            {
                if (dgvstu[0, i].Value != null)
                {
                    ls.Add((CStudent)dgvstu[0, i].Value);
                    dgvstu[0, i].Value = null;
                }
            }
            for (int roomid = 0; roomid < mcfg.roomcnt; roomid++)
            {
                for (int groupid = 0; groupid < mcfg.groupcnt; groupid++)
                {
                    if (dgv[groupid, roomid].Value != null)
                    {
                        ls.Add((CStudent)dgv[groupid, roomid].Value);
                        dgv[groupid, roomid].Value = null;
                    }
                }
            }
        }

        private void MenuItemAddName_Click(object sender, EventArgs e)
        {
            if (mcfg != null)
            {
                //AppendKbForm f = new AppendKbForm(mcfg);
                //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{

                //}
            }
        }

        private void MenuItemAllowKeyInput_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = MenuItemAllowKeyInput;
            if (m.Checked)
            {
                this.NotAllowFastInput();
            }
            else
            {
                this.AllowFastInput();
            }
            m.Checked = !m.Checked;
        }

       
        private void MenuItemAllowSpaceDelete_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = MenuItemAllowSpaceDelete;
            if (m.Checked)
            {
                this.NotAllowSpaceDelete();
            }
            else
            {
                this.AllowSpaceDelete();
            }
            m.Checked = !m.Checked;
        }
       

        private void MenuItemAllowChangeGap_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem m = MenuItemAllowChangeGap;
            if (m.Checked)
            {
                this.NotAllowChangeGap();
            }
            else
            {
                this.AllowChangeGap();
            }
            m.Checked = !m.Checked;
        }      

        private void MenuItemChangeLayout_Click(object sender, EventArgs e)
        {
            //CMySetDlg dlg;
            //if(dlg.DoModal()!= IDOK){
            //    return;
            //}
            ////
            //OnDesktolist();
            //vector<bool> vgap(dlg.GetCols() +1,false);
            //m_pgrid->SetGrid( dlg.GetRows()+2,dlg.GetCols()+1,vgap  ,false);
        }

        private void OnDesktolist()
        {
            List<CStudent> ls = new List<CStudent>();
            CollectData(ls);
            int k = 0;
            for (int roomid = 0; roomid < mcfg.roomcnt; roomid++)
            {
                for (int groupid = 0; groupid < mcfg.groupcnt; groupid++)
                {
                    dgv[groupid, roomid].Value = ls[k++];
                    if (k == ls.Count) return;
                }
            }
        }

        private void InitNamelistString(string ds)
        {
            string[] vds = ds.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (vds.Length < 1)
            {
                MessageBox.Show("没有座位资料");
                return;
            }
            string[][] vvds = new string[vds.Length][];
            int len = vds[0].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries).Length;
            for (int i = 0; i < vds.Length; i++)
            {
                vvds[i] = vds[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (len != vvds[i].Length || len == 0)
                {
                    MessageBox.Show("文件格式错误！");
                    return;
                }
            }
            mcfg.AddDeskInfo(vvds);
        }

        void AllowSpaceDelete() { mballowspaceasdel = true; }
        void NotAllowSpaceDelete() { mballowspaceasdel = false; }
        void AllowFastInput(){ mballowfastinput = true;}
        void NotAllowFastInput(){ mballowfastinput = false;}
        void AllowChangeGap() { mballowadjustgap = true; }
        void NotAllowChangeGap() { mballowadjustgap = false; }
    }
}

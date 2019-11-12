using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;

namespace JyeoPaper
{
    class XmlConfig
    {
        public XmlConfig()
        {
            xedbroot = null;
            xesrc = null;
            xedst = null;            
        }
        public void InitSql()
        {
            maxid = null;
            Rows = null;
            activeindex = -1;
            ActiveDatarow = null;
            Bid = Eid = 0;
        }
        public XmlElement xedbroot;
        private XmlElement xesrc;
        private XmlElement xedst;
        private string maxid;
        private DataRowCollection Rows;
        private int activeindex;

        public bool InitData(string Mode = "")
        {
            //Src
            //string SrcID = xesrc.GetAttribute("SrcID");
            //string SrcExp = xesrc.GetAttribute("SrcExp");
            //string SrcBeginID = xesrc.GetAttribute("SrcStartID");
            //string SrcEndID = xesrc.GetAttribute("SrcEndID");
            //string ProcessMode = xesrc.GetAttribute("ProcessMode");
            //string SavePath = xesrc.GetAttribute("SavePath");
            //string nextpageBegin = xesrc.GetAttribute("PageListNextBegin");
            //string nextpageEnd = xesrc.GetAttribute("NextPageEnd");
            SrcID = xesrc.GetAttribute("SrcID");
            SrcExp = xesrc.GetAttribute("SrcExp");
            SrcBeginID = xesrc.GetAttribute("SrcStartID");
            SrcEndID = xesrc.GetAttribute("SrcEndID");
            ProcessMode = xesrc.GetAttribute("ProcessMode");
            SavePath = xesrc.GetAttribute("SavePath");
            NextpageBegin = xesrc.GetAttribute("PageListNextBegin");
            NextpageEnd = xesrc.GetAttribute("NextPageEnd");
            SrcDbTableName = xesrc.GetAttribute("SrcDbTableT");
            CustomDown = xesrc.GetAttribute("CustomDown");
            ///Dst
            //string 

            ///DbSet
            //string Itembegin = GetItemBegin(xedbroot);
            //string Itemend = GetItemEnd(xedbroot);
            if (ProcessMode != "download" && Mode == "" )
            {
                Itembegin = GetItemBegin(xedbroot);
                Itemend = GetItemEnd(xedbroot);
            }

            if (Mode == "ZD")
            {

            }
            
            //////DetailSet            
            if (SrcExp.Contains("as"))
            {
                SrcExp = SrcExp.Substring(SrcExp.LastIndexOf("as") + 2).Trim();
            }
            Inserttemple = ConstructInsertSqlTemple(xedbroot);
            Inserttemple = ReplaceInsertSqlTemple(xedst, Inserttemple);

            SrcBeginID = FormatBeginEndID(SrcBeginID);
            SrcEndID = FormatBeginEndID(SrcEndID);

            Sqltemp = "select top 1000 [SrcID],[SrcExp] from [SrcDbTableT] where [SrcID] > [maxid] order by [SrcID]";
            Sqltemp = Sqltemp.Replace("[SrcID]", "[" + xesrc.GetAttribute("SrcID") + "]")
                   .Replace("[SrcExp]", xesrc.GetAttribute("SrcExp"))
                   .Replace("[SrcDbTableT]", "[" + xesrc.GetAttribute("SrcDbTableT") + "]");
            if (SrcEndID != "0")
                Sqltemp = Sqltemp.Replace("[maxid]", "[maxid] and [" + SrcID + "]<=" + SrcEndID + " ");

            //逻辑判断
            if ((NextpageBegin == "" || NextpageEnd == "") && ProcessMode == "text-pagelist")
            {
                return false;
            }
            if (ProcessMode == "text-BeginEndIDNet" || ProcessMode == "BeginEndIDToDb" || Mode == "ZD")
            {
                Bid = Convert.ToInt32(SrcBeginID);
                Eid = Convert.ToInt32(SrcEndID);
                if (Bid < 0 || Bid > Eid || Eid == 0)
                {
                    return false;
                }
            }
            return true;
        }

        
        public void Read(System.Windows.Forms.DataGridView dgvdb, System.Windows.Forms.DataGridView dgvsrc, System.Windows.Forms.DataGridView dgvdst)
        {
            XmlDocument XmlDb = DataGridToXml(dgvdb, "DbSet");
            XmlDocument XmlSrc = DataGridToXml(dgvsrc, "Src");
            XmlDocument XmlDst = DataGridToXml(dgvdst, "Dst");
            xedbroot = XmlDb.DocumentElement;
            xesrc = (XmlElement)XmlSrc.DocumentElement.FirstChild;            
            if (XmlDst != null)
                xedst = (XmlElement)XmlDst.DocumentElement.FirstChild; ;
        }
        public void ImportXml(DataGridView dgvsrc, DataGridView dgvdst, DataGridView dataGridView1)
        {
            XmlDocument XmlDoc = OpenAndLoadXml();
            if (XmlDoc != null)
            {
                XmlToDgv(GetXmlElementByTag(XmlDoc, "Src"), dgvsrc);
                XmlToDgv(GetXmlElementByTag(XmlDoc, "Dst"), dgvdst);
                XmlToDgv(GetXmlElementByTag(XmlDoc, "DbSet"), dataGridView1);
            }
        }
        public void ExportXml(DataGridView dgvsrc, DataGridView dgvdst, DataGridView dataGridView1)
        {
            XmlDocument XmlDb = DataGridToXml(dataGridView1, "DbSet");
            XmlDocument XmlSrc = DataGridToXml(dgvsrc, "Src");
            XmlDocument XmlDst = DataGridToXml(dgvdst, "Dst");

            string rootname = "toolsxml";
            XmlDocument XmlDoc = new XmlDocument();
            XmlElement root = XmlDoc.CreateElement(rootname);
            XmlDoc.AppendChild(root);
            if(XmlSrc!=null)
            root.AppendChild(XmlDoc.ImportNode(XmlSrc.DocumentElement, true));
            if(XmlDst!=null)
            root.AppendChild(XmlDoc.ImportNode(XmlDst.DocumentElement, true));
            if (XmlDb != null)
            root.AppendChild(XmlDoc.ImportNode(XmlDb.DocumentElement, true));

            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "保存xml配置文件";
            fd.Filter = "配置文件(*.xml)|*.xml";
            //fd.DefaultExt = 
            fd.FileName = "NewCfg.xml";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName.EndsWith(".xml"))
                {
                    XmlDoc.Save(fd.FileName);
                }
            }
        }
        private string GetItemEnd(XmlElement xedbroot)
        {
            XmlNode xd = xedbroot.LastChild;
            return xd.Attributes["strend"].Value;
        }
        private string GetItemBegin(XmlElement xedbroot)
        {
            XmlNode xd = xedbroot.FirstChild;
            return xd.Attributes["strbegin"].Value;
        }
        private static string FormatBeginEndID(string SrcBeginEndID)
        {
            string id = "0";
            try
            {
                int startid = Convert.ToInt32(SrcBeginEndID);
                id = startid.ToString();
            }
            catch { }
            return id;
        }
        private static string ConstructInsertSqlTemple(XmlElement xedbroot)
        {
            string inserttemple = "insert into [tablename](";
            string value = "";
            foreach (XmlElement xe in xedbroot.ChildNodes)
            {
                string colname = xe.GetAttribute("colname");
                inserttemple += "[" + colname + "],";
                if (xe.GetAttribute("valuetype") == "int")
                {
                    value += "[-" + colname + "-],";
                }
                else
                {
                    value += "'[-" + colname + "-]',";
                }
            }
            if (inserttemple.EndsWith(","))
                inserttemple = inserttemple.Substring(0, inserttemple.Length - 1);
            if (value.EndsWith(","))
                value = value.Substring(0, value.Length - 1);
            inserttemple = inserttemple + ") values (" + value + " );\r\n";
            return inserttemple;
        }
        private static string ConstructInsertSQL(string inserttemple, string item, XmlElement xedbroot)
        {
            string sql = inserttemple;
            foreach (XmlElement xe in xedbroot.ChildNodes)
            {
                string colname = xe.GetAttribute("colname");
                string strbegin = xe.GetAttribute("strbegin");
                string strend = xe.GetAttribute("strend");
                string multi = xe.GetAttribute("multimatch");
                if (multi == "T")
                    sql = sql.Replace("[-" + colname + "-]", DgvTools.GetEqualValueMulti(item, strbegin, strend));
                else
                    sql = sql.Replace("[-" + colname + "-]", DgvTools.GetEqualValue(item, strbegin, strend));
            }
            return sql;
        }
        private static string ReplaceInsertSqlTemple(XmlElement xedst, string inserttemple)
        {
            if (xedst == null)
                return inserttemple;
            String DstIsSameToSrc = xedst.Attributes["DstIsSameToSrc"].Value;
            String DstIsCreate = xedst.Attributes["DstIsCreate"].Value;
            String dstpath = xedst.Attributes["dstpath"].Value;
            String dsttablename = xedst.Attributes["dsttablename"].Value;
            String DstIsCreateID = xedst.Attributes["DstIsCreateID"].Value;
            String DstsaveSrcIDAs = xedst.Attributes["DstsaveSrcIDAs"].Value;

            string sqltemp = "[;DATABASE=dstpath].[dsttablename]";
            if (DstIsSameToSrc == "True")
                sqltemp = sqltemp.Replace("[;DATABASE=dstpath].", "");
            else
                sqltemp = sqltemp.Replace("dstpath", dstpath);
            if (dsttablename == "") return "";
            sqltemp = sqltemp.Replace("dsttablename", dsttablename);

            string insertinto = "insert into [tablename]([id],";
            if (DstIsCreateID == "False")
                insertinto = "insert into [tablename]([id],";
            else
                insertinto = "insert into [tablename]([" + DstsaveSrcIDAs + "],";

            inserttemple = inserttemple
                .Replace("insert into [tablename](", insertinto)
                .Replace("[tablename]", sqltemp)
                .Replace("values (", "values ([-id-],");
            return inserttemple;
        }
        public static XmlDocument DataGridToXml(DataGridView dgv, string rootname)
        {
            if (dgv == null) return null;
            XmlDocument XmlDoc = new XmlDocument();
            XmlElement root = XmlDoc.CreateElement(rootname);
            XmlDoc.AppendChild(root);
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                if (!dr.IsNewRow)
                {
                    XmlElement xe = XmlDoc.CreateElement("Item");
                    foreach (DataGridViewColumn dc in dgv.Columns)
                    {
                        if (dr.Cells[dc.Index].Value == null)
                        {
                            xe.SetAttribute(dc.Name, "");
                        }
                        else
                        {
                            xe.SetAttribute(dc.Name, dr.Cells[dc.Index].Value.ToString());
                        }
                    }
                    root.AppendChild(xe);
                }
            }
            return XmlDoc;
        }
        private static XmlDocument OpenAndLoadXml()
        {
            XmlDocument XmlDoc = new XmlDocument(); ;
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "打开xml配置文件";
            fd.Filter = "配置文件(*.xml)|*.xml";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName.EndsWith(".xml"))
                {
                    XmlDoc.Load(fd.FileName);
                    return XmlDoc;
                }
            }
            return null;
        }
        private static XmlElement GetXmlElementByTag(XmlDocument XmlDoc, string tagname)
        {
            if (XmlDoc.GetElementsByTagName(tagname).Count == 1)
            {
                return (XmlElement)(XmlDoc.GetElementsByTagName(tagname)[0]);
            }
            return null;
        }
        private static void XmlToDgv(XmlElement root, DataGridView dgv)
        {
                //XmlElement root = XmlDoc.DocumentElement;
                if (root == null || dgv == null) return;
                dgv.Rows.Clear();
                if (dgv.AllowUserToAddRows)
                    dgv.RowCount = root.ChildNodes.Count + 1;
                else
                    dgv.RowCount = root.ChildNodes.Count;
                int rowindex = 0;
                foreach (XmlElement xe in root.ChildNodes)
                {
                    foreach (DataGridViewColumn dc in dgv.Columns)
                    {
                        if (dc.GetType().Name == "DataGridViewCheckBoxColumn")
                        {
                            string value =  xe.GetAttribute(dc.Name);
                            if(value!="True" && value!="TRUE") 
                                value="False";
                            dgv[dc.Index, rowindex].Value = value;
                        }
                        else {
                            dgv[dc.Index, rowindex].Value = xe.GetAttribute(dc.Name);
                        }
                    }
                    rowindex++;
                }
        }

        public string SrcID { get; set; }
        public string SrcExp { get; set; }
        public string SrcBeginID { get; set; }
        public string SrcEndID { get; set; }
        public string ProcessMode { get; set; }
        public string SavePath { get; set; }
        public string NextpageBegin { get; set; }
        public string NextpageEnd { get; set; }
        public string Itembegin { get; set; }
        public string Itemend { get; set; }
        public string Sqltemp { get; set; }
        public DataRow ActiveDatarow { get; set; }
        public int Bid { get; set; }
        public int Eid { get; set; }
        public string Inserttemple { get; set; }
        public string SrcDbTableName { get; set; }

        public string GetNextId(Db.ConnDb db)
        {
            if (ProcessMode == "download"
                || ProcessMode == "text-pagelist"
                || ProcessMode == "text-content"
                || ProcessMode == "text-GetNetfile"
                || ProcessMode == "text-PostNetfile")
            {
                if(maxid == null)
                    maxid = SrcBeginID;
                if (Rows == null || activeindex == Rows.Count)
                {
                    string sql = Sqltemp.Replace("[maxid]", maxid);
                    DataSet ds = db.query(sql);
                    Rows = ds.Tables[0].Rows;
                    if (Rows.Count == 0) return null;
                    maxid = Rows[Rows.Count - 1][SrcID].ToString();
                    activeindex = 0;
                }
                if (activeindex<0)
                    activeindex = 0;
                if (activeindex < Rows.Count)
                {
                    activeindex++;
                    ActiveDatarow = Rows[activeindex -1 ];
                    return ActiveDatarow[SrcID].ToString();
                }
            }
            else if (ProcessMode == "text-BeginEndIDNet" || ProcessMode == "BeginEndIDToDb")
            {
                if (activeindex < Bid)
                    activeindex = Bid;
                if (activeindex > Eid)
                    return null;
                activeindex++;
                return (activeindex-1).ToString();
            }
            return null;
        }
        internal string GetDstCreateID()
        {
            if (xedst != null)
            {
                return xedst.GetAttribute("DstIsCreateID");
            }
            return "";
        }


        public string DstDbTableName
        {
            get
            {
                if(xedst!=null)
                    return   xedst.Attributes["dsttablename"].Value;
                return "";
            }
        }

        public string CustomDown { get; set; }

        internal bool RefPos()
        {
            
            return CustomDown == "T";
        }
    }
}

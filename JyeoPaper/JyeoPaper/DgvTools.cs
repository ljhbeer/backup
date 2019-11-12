using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Net;

namespace JyeoPaper
{
    class DgvTools
    {
        public static void InitDataGridViewColumns(DataGridView dgv, string strcolums)
        {
            if (dgv == null || strcolums == null)
                return;
            strcolums = strcolums.Replace("\r\n", "");
            string[] c = strcolums.Split(new string[] { "{", "}", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in c)
            {
                //string colname = s.Trim(); //GetEqualValue(s, "\"columname\":\"", "\"");
                if (s.Trim() == "")
                    continue;
                string colname = GetEqualValue(s, "\"colname\":[", "]");
                string coltitle = GetEqualValue(s, "\"coltitle\":[", "]");
                dgv.Columns.Add(colname, coltitle);
            }
        }
        public static void InitDataGridViewStyle(DataGridView dgv, string cfg)
        {
            if (dgv == null || cfg == null)
                return;
            string[] c = cfg.Split(new string[] { "{", "}", "(", ")", }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in c)
            {
                string colname = GetEqualValue(s, "\"colname\":[", "]");
                string columset = GetEqualValue(s, "\"columset\":[", "]");
                if (dgv.Columns.Contains(colname))
                {
                    string[] cs = columset.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string r in cs)
                    {
                        string[] vs = r.Split(new string[] { ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (vs.Length != 2) continue;
                        if (vs[0] == "visible")
                        {
                            if (vs[1] == "true")
                                dgv.Columns[colname].Visible = true;
                            else if (vs[1] == "false")
                                dgv.Columns[colname].Visible = false;
                        }
                        else if (vs[0] == "width")
                        {
                            dgv.Columns[colname].Width = Convert.ToInt32(vs[1]);
                        }
                    }
                }
            }

        }
        public static string GetEqualValue(string src, string begin, string end)
        {
            if (!src.Contains(begin))
                return "";
            src = src.Substring(src.IndexOf(begin) + begin.Length) + end;
            src = src.Substring(0, src.IndexOf(end)).Trim();
            return src;
        }
        internal static string GetLastEqualValue(string src, string begin, string end)
        {
            if (!src.Contains(end))
                return "";
            src = src.Substring(0,src.IndexOf(end));
            int pos = src.LastIndexOf(begin);
            if (pos == -1)
            {
                return "";
            }
            src = src.Substring(pos+begin.Length).Trim();
            return src;
        }
        public static string GetEqualValue(string src, string begin, string end, ref int pos)
        {//包含begin
            pos = src.IndexOf(begin, pos);
            if (pos == -1)
            {
                pos = -1;
                return "";
            }
            int pos2 = src.IndexOf(end, pos+begin.Length);
            if (pos2 == -1)
            {
                pos = -1;
                return "";
            }
            src = src.Substring(pos, pos2 - pos);
            pos = pos2+end.Length;
            return src;
        }
        public static string GetEqualValue2(string src, string begin, string end, ref int pos)
        {
            pos = src.IndexOf(begin, pos);
            if (pos == -1)
            {
                pos = -1;
                return "";
            }
            int pos2 = src.IndexOf(end, pos+begin.Length);
            if (pos2 == -1)
            {
                pos = -1;
                return "";
            }
            src = src.Substring(pos+begin.Length, pos2 - pos-begin.Length);
            pos = pos2+end.Length;
            return src;
        }
        public static string GetEqualValueMulti(string item, string strbegin, string strend)
        {
            int pos = 0;
            string src = "";
            while (true)
            {
                string tempsrc = GetEqualValue2(item, strbegin, strend, ref pos);
                if (pos < 0) break;
                src += tempsrc;
            }
            return src;
        }
        internal static string GetEqualValueMulti(string item, string strbegin, string strend, string mmrt)
        {
            int pos = 0;
            string src = "";
            while (true)
            {
                string tempsrc = GetEqualValue2(item, strbegin, strend, ref pos);
                if (pos < 0) break;
                src +=mmrt.Replace("[mm]", tempsrc);
            }
            return src;
        }
        public static List<string> GetEqualValueList(string item, string strbegin, string strend)
        {
            int pos = 0;
                List<string> src = new List<string>();
            try
            {
                while (true)
                {
                    string tempsrc = GetEqualValue2(item, strbegin, strend, ref pos);
                    if (pos < 0) break;
                    src.Add(tempsrc);
                }
            }
            catch
            {
                pos = 0;
                while (true)
                {
                    string tempsrc = GetEqualValue2(item, strbegin, strend, ref pos);
                    if (pos < 0) break;
                    src.Add(tempsrc);
                }
            }
            return src;
        }
        public static int FindIndex(string[] colums, string indexname)
        {
            int roomidindex = -1;
            for (int i = 0; i < colums.Length; i++)
                if (colums[i] == indexname)
                {
                    roomidindex = i;
                    break;
                }
            return roomidindex;
        }
        /// <summary>
        /// 绑定ComBoBox列表到DataGridView控件里
        /// </summary>
        /// <param name="dgv"></param>
        /// 
        private static void InitDataGridViewComBoBox(int rowNumber, DataGridView dgv)
        {
            DataRow drRows;
            //数据通道下来列表的绑定
            DataTable dtPrint_Data = new DataTable();
            dtPrint_Data.Columns.Add("Print_Data");
            drRows = dtPrint_Data.NewRow();
            drRows.ItemArray = new string[] { "Print_data" };
            dtPrint_Data.Rows.Add(drRows);
            DataGridViewComboBoxCell dgvComBoBoxPrint_Data = new DataGridViewComboBoxCell();
            dgvComBoBoxPrint_Data.DisplayMember = "Print_Data";
            dgvComBoBoxPrint_Data.ValueMember = "Print_Data";
            dgvComBoBoxPrint_Data.DataSource = dtPrint_Data;
            //字体的绑定
            DataTable dtFontNames = new DataTable();
            dtFontNames.Columns.Add("FontNames");
            foreach (System.Drawing.FontFamily ff in System.Drawing.FontFamily.Families)
            {
                drRows = dtFontNames.NewRow();
                drRows.ItemArray = new string[] { ff.Name };
                dtFontNames.Rows.Add(drRows);
            }
            DataGridViewComboBoxCell dgvComBoBoxFontNames = new DataGridViewComboBoxCell();
            dgvComBoBoxFontNames.DisplayMember = "FontNames";
            dgvComBoBoxFontNames.ValueMember = "FontNames";
            dgvComBoBoxFontNames.DataSource = dtFontNames;
            //绑定类型
            DataTable dtType = new DataTable();
            dtType.Columns.Add("Type");

            drRows = dtType.NewRow();
            drRows.ItemArray = new string[] { "文本" };
            dtType.Rows.Add(drRows);

            drRows = dtType.NewRow();
            drRows.ItemArray = new string[] { "字段" };
            dtType.Rows.Add(drRows);

            drRows = dtType.NewRow();
            drRows.ItemArray = new string[] { "国标码" };
            dtType.Rows.Add(drRows);

            drRows = dtType.NewRow();
            drRows.ItemArray = new string[] { "内部码" };
            dtType.Rows.Add(drRows);

            drRows = dtType.NewRow();
            drRows.ItemArray = new string[] { "图片" };
            dtType.Rows.Add(drRows);

            DataGridViewComboBoxCell dgvComBoBoxTypes = new DataGridViewComboBoxCell();
            dgvComBoBoxTypes.DisplayMember = "Type";
            dgvComBoBoxTypes.ValueMember = "Type";
            dgvComBoBoxTypes.DataSource = dtType;

            //字段名称绑定
            //DataTable dtDBText = new DataTable();
            //dtDBText.Columns.Add("DBTextX");

            //string strSelectPrint_FieldNames = "select chinesename from Print_FieldNames;";
            //DataSet dtSelectPrint_FieldNames = myCom.DataSelectReader(strSelectPrint_FieldNames, sqlLocalhost);
            //if (dtSelectPrint_FieldNames.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtSelectPrint_FieldNames.Tables[0].Rows.Count; i++)
            //    {
            //        drRows = dtDBText.NewRow();
            //        drRows.ItemArray = new string[] { dtSelectPrint_FieldNames.Tables[0].Rows[i][0].ToString() };
            //        dtDBText.Rows.Add(drRows);
            //    }
            //}
            //DataGridViewComboBoxCell dgvComBoBoxDBTextX = new DataGridViewComboBoxCell();
            //dgvComBoBoxDBTextX.DisplayMember = "DBTextX";
            //dgvComBoBoxDBTextX.ValueMember = "DBTextX";
            //dgvComBoBoxDBTextX.DataSource = dtDBText;

            //将它绑定到表格控件里去
            dgv.Rows[rowNumber].Cells[3] = dgvComBoBoxPrint_Data;
            dgv.Rows[rowNumber].Cells[2] = dgvComBoBoxFontNames;
            dgv.Rows[rowNumber].Cells[1] = dgvComBoBoxTypes;
            //dgv.Rows[rowNumber].Cells[4] = dgvComBoBoxDBTextX;

        }
        public static bool GetWebRequest(string url, string FileName)
        {
            try
            {
                string ext = Path.GetExtension(FileName);
                Uri uri = new Uri(url);
                WebClient wb = new WebClient();
                wb.DownloadFile(uri, FileName);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        public static string CutStr(string str, int len)
        {
            if (str == null || str.Length == 0 || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;

            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }
        
    }
}

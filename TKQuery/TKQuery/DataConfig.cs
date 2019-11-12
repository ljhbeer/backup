using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;

namespace TKQuery
{
    public  class DataConfig
    {
        public DataConfig()
        {
            maxitemcount = 1000;
            sqlitemcol = " '难度:'+trim(DFT) + ' 频率:' + trim(ufq)+ ' ' + trim(indate) as infozt," +
                 " question.id,question.question,question.qview,cname,iif([tid]=5, 1, 2) as tid";
            sqlitemfrom = " from question,questioninfo,tpapers ";
            sqlitemwhere = " where question.id = questioninfo.id and questioninfo.pid = tpapers.id ";
            Init();
        }
        public void ChangeDatabaseFilename(string dbfullname)
        {
            SetNewDatabaseFilename(dbfullname);
            if (!Error)
                Init();
        }

        private void Init()
        {
            Error = false;
            data = null;
            dt = null;
            db = null;
            ReadConfig();
            try
            {
                SetNewDatabaseFilename(dbdatafullname);
                data = new Data();
                InitUIdata();
                db.connClose();
            }
            catch (System.Data.OleDb.OleDbException ole)
            {
                if (db != null)
                    db.connClose();
                Msg.Add(ole.ToString());
                Error = true;
            }
        }
        private void InitUIdata()
        {    
            db.TestConnect();
            comboBoxpage = new List<int>(new int[] { 10, 50, 100, 200, 400, 800 });     
            DtcomboBoxtx = QueryTable("select id,cname from ttx ");
            DtcomboBoxzsd = QueryTable("select id,cname from tknowledges ");            
            
            string sqlcol = " '难度:'+trim(DFT) + ' 频率:' + trim(ufq)+ ' ' + trim(indate) as infozt," +
                " question.id,question.question,question.qview,cname,iif([tid]=5, 1, 2) as tid, 10.1 as sid  ";
            string sql = "select top 1" + sqlcol + " from question,questioninfo,tpapers";
            dt = QueryTable(sql);
            data.Init( QueryTable("select * from thtml "));            
            MsgCount = "本题库共有 " +  QueryTable("select count(*) from question").Rows[0][0] + " 道试题";
            ItemsDt = QueryTable("select top 1 "+sqlitemcol + sqlitemfrom+ sqlitemwhere + " and 1=2");
        }

        
        
        public string QueryPage(int items, string sqlcondition, string sqlorder,int beginrec=0)
        {
            string sql;
            if (beginrec <= 0)
            {
                beginrec = 0;
                string sqlid = "select top " + maxitemcount
                               + " question.id "
                               + sqlitemfrom
                               + sqlcondition + sqlorder;
                dtid = QueryTable(sqlid);
            }
            string sqlids = GetIdConditions(items,beginrec, dtid);
            if (sqlids.Contains("()"))
                return "无法查到试题";
            sql = "select "+ sqlitemcol
                            + sqlitemfrom
                            + sqlcondition
                            + sqlids ;
            string html = data.ConstructItems(QueryTable(sql));
            string pagehtml = data.ConstructPagelist(items, QueryItemsCount(sqlcondition, sqlitemfrom), 500);   //最大限制500条           
            return html.Replace("<!--pagelist-->", pagehtml);
        }
        public string QueryPaperList(string sqlkeys)
        {
            string sql = "select top 500 tpaperscnt.*,tpapers.cname from tpaperscnt,tpapers where tpaperscnt.id = tpapers.id " + sqlkeys + " order by tpapers.id desc";
            string html = data.ConstructPaperItems(QueryTable(sql));
            //int papercount = QueryItemsCount(, "from tpaperscnt");
            //string pagehtml = data.ConstructPagelist(items, papercount, -1);  //-1不限制
            return html;//.Replace("<!--pagelist-->", pagehtml);
        }
        public string QueryPaperList(int items,int beginrec=0) //??
        {
             string sql;
             if (beginrec == 0)
                 sql = "select top " + items + " tpaperscnt.*,tpapers.cname from tpaperscnt,tpapers where tpaperscnt.id = tpapers.id order by tpaperscnt.id";
             else
             {
                 sql = "select top " + items + " tpaperscnt.*,tpapers.cname from tpaperscnt,tpapers where tpaperscnt.id = tpapers.id and tpaperscnt.id > "
                     + "( select max(tpaperscnt.id) from "
                      + "( select top " + beginrec + " * from  tpaperscnt,tpapers where tpaperscnt.id = tpapers.id order by tpaperscnt.id )"
                     + ")  order by tpaperscnt.id";
             }
            string html = data.ConstructPaperItems(QueryTable(sql));
            int papercount = QueryItemsCount("", "from tpaperscnt");
            string pagehtml = data.ConstructPagelist(items, papercount, -1);  //-1不限制
            return html.Replace("<!--pagelist-->", pagehtml);
        }
        public string QueryPaper(string id)
        {
            string sql = "select "
                               + sqlitemcol
                               + sqlitemfrom
                               + sqlitemwhere + " and tpapers.id = " + id;
            string html = data.ConstructItems(QueryTable(sql));
            return html;
        }
        public string QueryCustomPaperList(int items, int beginrec = 0)
        {
            string sql = "select * from Customknowledges";
            string html = data.ConstructCustomPaperItems(QueryTable(sql));
            return html;
        }
        public string QueryCustomPaper(string id)
        {
            string sql = "select Itemids from Customknowledges where id = " + id;
            string ids =(string) QueryTable(sql).Rows[0][0];
            string sqlcondition = "and question.id in (" + ids + ") order by instr('"
                                 + ids + "',question.id)";
            sql = "select " + sqlitemcol 
                            + sqlitemfrom
                            + sqlitemwhere
                            + sqlcondition;
            string html = data.ConstructItems(QueryTable(sql));           
            return html;
        }
        public DataRowCollection QueryItems(string ids)
        {
            string sql;
            sql = "select " + sqlitemcol
                            + sqlitemfrom
                            + sqlitemwhere  +" and question.id in("
                            + ids + ") order by instr('"
                            + ids + "',question.id)";
            return QueryTable(sql).Rows;
        }
        public string QueryItems(DataTable itemsdt)
        {
            return data.ConstructItems(itemsdt);
        }
        internal string QueryItem(DataRow dr)
        {
            return data.ConstructItem(dr);
        }
        public DataTable QuerySectionItems(string sectionid)
        {
            string sql = "select "
                              + sqlitemcol
                              + sqlitemfrom + ",sectionquestion "
                              + sqlitemwhere + " and question.id = sectionquestion.questionid and sectionquestion.sectionid =" + sectionid + "  order by sortid";
           return  QueryTable(sql);
        }
        public void QueryOutputPaper(DataTable itemsdt, bool Localimg)
        {
            string html = data.ConstructOutput(itemsdt);
            string ids = GetItemIds(itemsdt);
            string sql = "select id from question where id in(" + ids + ") and answer is null or id in(" + ids + ") and answer = ''";
            DataSet ds = db.query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {//GET answer from net work
                GetAnswerFromNetwork(ds.Tables[0]);
            }
            sql = "select id,answer from question where id in(" + ids + ") order by instr('" + ids + "',id)";

            string answer = data.ConstructOutputAnswer(QueryTable(sql));
            //输出答案
            html = html.Replace("</body>", answer + "\r\n</body>");
            if (Localimg)
            {
                html = Convertlocalimg(html,itemsdt);
            }
            File.WriteAllText("save.html", html, Encoding.UTF8);            
        }

        private int QueryItemsCount(string sqlcondition, string sqlitemfrom)
        {
            string sql = "select count(*) "
                           + sqlitemfrom
                           + sqlcondition;
            return (int)(QueryTable(sql).Rows[0][0]);
        }
        private string GetIdConditions(int items, int beginrec, DataTable iddt)
        {
            string ids = "";
            if (iddt.Rows.Count < beginrec)
            {
                throw new Exception("起始编号超过最大值");
            }
            int end = iddt.Rows.Count < beginrec + items ? iddt.Rows.Count : beginrec + items;
            for (int i = beginrec; i < end; i++)
            {
                ids += iddt.Rows[i][0].ToString() + ",";
            }
            if(ids!="")
                ids = (ids+")").Replace(",)", " "); 
            string sqlcondition = "and question.id in (" + ids + ") order by instr('"
                                   + ids + "',question.id)";
            return sqlcondition;            
        }
        private bool ReadConfig()
        {
            string dbsqlfullname = "D:\\Backup\\sql.mdb"; //无用
            dbdatafullname = "D:\\back\\swtk.mdb";

            if (File.Exists("cfg.ini"))
            {
                string address = File.ReadAllText("cfg.ini");
                if (!address.Contains("\r\n"))
                    address += "\r\n";

                if (address.Contains("dbsqlfullname="))
                {
                    dbsqlfullname = address.Substring(address.IndexOf("dbsqlfullname=") + "dbsqlfullname=".Length) + "\r\n";
                    dbsqlfullname = dbsqlfullname.Substring(0, dbsqlfullname.IndexOf("\r\n")).Trim();
                }
                if (address.Contains("dbdatafullname="))
                {
                    dbdatafullname = address.Substring(address.IndexOf("dbdatafullname=") + "dbdatafullname=".Length) + "\r\n";
                    dbdatafullname = dbdatafullname.Substring(0, dbdatafullname.IndexOf("\r\n")).Trim();
                }
                return true;
            }
            return false;
        }
        private void SetNewDatabaseFilename(string dbfullname)
        {
            this.dbdatafullname = dbfullname;  
            if (!File.Exists(dbfullname))
            {
                Error = true;
                Msg.Add(dbfullname + "数据库不存在，请重新选择");
                return;
            }
            if (db != null)
            {
                db.connClose();
                db = null;
            }
            db = new Db.ConnDb(dbfullname);
        }
        public DataTable QueryTable(string sql)
        {
            DataSet ds = db.query(sql);
            return ds.Tables[0];
        }
        private string Convertlocalimg(string html, DataTable itemsdt)
        {
            string ids = GetItemIds(itemsdt);
            if (ids == "") 
                return html;
            string sql = "select imgid,path from img where id in(" + ids + ")";
            DataSet ds = db.query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (html.Contains(dr["path"].ToString()))
                {
                    string p = dr["path"].ToString();
                    string localimg = "d:/img/" + dr["imgid"].ToString() + p.Substring(p.LastIndexOf('.'));
                    html = html.Replace(p, localimg);
                }
            }
            return html;
        }
        private void GetAnswerFromNetwork(DataTable dt)
        {
            int i = 0;
            bool banswerdetail = true;
            for (; i < dt.Rows.Count; i++)
            {
                if (banswerdetail)
                {
                    if (!RunIdAnswerdetail(dt.Rows[i]["id"].ToString()))
                    {
                        i--;
                        banswerdetail = false;
                    }
                }
                else
                {
                    if (!RunIdtestdetail(dt.Rows[i]["id"].ToString()))
                        break;
                }
            }

        }

        private bool RunIdAnswerdetail(string id)
        {
            string url = "http://gzsw.cooco.net.cn/answerdetail/" + id;  //test
            string html = "";
            try
            {
                html = GetHttpWebRequest(url);
                if (html == "1、所有试题都有答案，请放心组卷")
                    return false;
                string sql = "update question set answer = '" + html.Replace("'", "''") + "' where id = " + id;
                db.update(sql);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private bool RunIdtestdetail(string id)
        {
            string url = "http://gzsw.cooco.net.cn/testdetail/" + id;  //test
            string html = "";
            try
            {
                html = GetHttpWebRequest(url);
                string answer = html;

                Match m1 = Regex.Match(html, "class =\"daan\" (.*)</table>  </p>(.*)<div class=\"daan\"", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (m1.Success && m1.Groups.Count > 2)
                {
                    html = m1.Groups[2].Value.Trim();
                    if (html.StartsWith("<br/>"))
                        html = html.Substring("<br/>".Length);
                    html = html.Trim();
                    html = html.Replace("\r\n", "\r\r");
                    html = html.Substring(0, html.IndexOf("\n"));
                    html = html.Replace("\r\r", "\r\n");
                    //return false;
                }
                string sql = "update question set answer = '" + html.Replace("'", "''") + "' where id = " + id;
                db.update(sql);
                sql = "insert into answer(id,answer) values(" + id + ",'" + answer.Replace("'", "''") + "' )";
                db.update(sql);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }
        private string GetWebRequest(string url)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        private string GetHttpWebRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        private static string GetItemIds( DataTable itemsdt)
        {
            List<string> ids = new List<string>();
            foreach (DataRow dr in itemsdt.Rows)
            {
                    ids.Add(dr["id"].ToString());
            }
            string idss = "";
            for (int i = 0; i < ids.Count; i++)
                idss += ids[i]+","  ;
            if (idss != "")
                idss = idss.Remove(idss.Length - 1);
            return idss;
        }


        public bool Error { get; set; }
        public DataTable DtcomboBoxtx;
        public DataTable DtcomboBoxzsd;
        public DataTable ItemsDt;
        public List<int> comboBoxpage; 
        private List<string> Msg = new List<string>();
        private string dbdatafullname;
        private Db.ConnDb db;
        private Data data;
        private DataTable dt;
        private DataTable dtid;
        private string sqlitemcol;
        private string sqlitemfrom;
        private int maxitemcount;
        private string sqlitemwhere;
        public string MsgCount { get; set; }



        public void SaveItemsToCustom(string cname, string strlid,int ItemCount)
        {
            DateTime dtime = DateTime.Now;
            string sql = "insert into Customknowledges( itemids, cname,cnt,updatetime,ctype )  values( '" + strlid + "','" + cname + "'," + ItemCount + ",'" + dtime.ToShortDateString().ToString() + "', 1)";
            db.TestConnect();
            db.update(sql);
            db.connClose();
        }
        internal void Update(string sql)
        {
            db.update(sql);
        }
        internal void InsertNewName(string chapter, string cname)
        {
            db.update("insert into " + chapter +"(cname) values('" + cname + "')");
        }
        internal void InsertSection(string cname, string chapterid)
        {
            db.update("insert into [section](chapterid,cname) values("+ chapterid + ",'" + cname + "')");
        }
    }
}

/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016-9-12
 * 时间: 8:18
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StockTest;

namespace ReName
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
    ///
    public delegate bool BoolCondition(string s);
    public delegate string ActCondition(string s);
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}		
		void ButtonBrowseClick(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				this.folderpath = folderBrowserDialog1.SelectedPath;
				this.textBoxPath.Text =  folderpath;
			}
		}
		void ButtonImportClick(object sender, EventArgs e)
		{
			
			string filenames = ImportText();
			string[] filelines = filenames.Split(new string[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
			List<List<string>> fs = new List<List<string>>();
			if(!filenames.Contains("|")){
				foreach(string s in filelines){
					string[] ss = s.Split(new string[]{"\t"," "},StringSplitOptions.RemoveEmptyEntries);
					if(ss.Length==2)
						fs.Add(new List<string>(){ss[0],ss[1]});
				}
			}else{
				foreach(string s in filelines){
					string[] ss = s.Split(new string[]{"|"},StringSplitOptions.RemoveEmptyEntries);
					if(ss.Length==2)
						fs.Add(new List<string>(){ss[0],ss[1]});
				}
			}
			
			
			AddFileNameToTable(fs);
		}
		void ButtonDoneClick(object sender, EventArgs e)
		{
			string path = textBoxPath.Text.Trim();
			string strundone = "";
			if (!System.IO.Directory.Exists(path))
			{
				MessageBox.Show("文件目录不存在");
				return;
			}
			if(!path.EndsWith("\\"))
				path+="\\";
			if(dt!=null)
				foreach(DataRow dr in dt.Rows){
				string srcFileName = path+dr["源文件名"];
				string destFileName = path+dr["新文件名"];
				if (System.IO.File.Exists(srcFileName))
				{
					FileInfo fi = new FileInfo(destFileName);
					if ( !fi.Directory.Exists)
					{
						//					 	System.IO.Directory.CreateDirectory(fi.Directory);
						fi.Directory.Create();
					}
					System.IO.File.Move(srcFileName, destFileName);
				}else{
					if (!System.IO.File.Exists(destFileName))
						strundone += dr["源文件名"]+"|"+dr["新文件名"]+"\r\n";
				}
			}
			if(strundone!="")
				File.WriteAllText("out_undone.txt",strundone);
		}		
		void ButtonDonePathClick(object sender, EventArgs e)
		{
			string path = textBoxPath.Text.Trim();
			string strundone = "";
			if (!System.IO.Directory.Exists(path))
			{
				MessageBox.Show("文件目录不存在");
				return;
			}
			if(!path.EndsWith("\\"))
				path+="\\";
			if(dt!=null)
				foreach(DataRow dr in dt.Rows){
				string srcFileName = path+" "+dr["源文件名"];
				string destFileName = path+dr["新文件名"];
				if (System.IO.Directory.Exists(srcFileName))
				{
//						FileInfo fi = new FileInfo(destFileName);
//						 if ( !fi.Directory.Exists)
//	                     {
//	//					 	System.IO.Directory.CreateDirectory(fi.Directory);
//						 	fi.Directory.Create();
//						 }
					System.IO.Directory.Move(srcFileName, destFileName);
				}else{
					if (!System.IO.File.Exists(destFileName))
						strundone += dr["源文件名"]+"|"+dr["新文件名"]+"\r\n";
				}
			}
			if(strundone!="")
				File.WriteAllText("out_undone.txt",strundone);
		}
		void ButtonUpdateCMDClick(object sender, EventArgs e)
		{
            if (textBoxCMD.Text != "")
            {
                Cmd cmd = new Cmd(textBoxCMD.Text);
                if(cmd.ReplacetoNullTags!=null){
                    foreach(DataRow dr in dt.Rows){
                        string s = dr["新文件名"].ToString();
                        foreach (BETag bt in cmd.ReplacetoNullTags.tags)
                            s = s.Replace(bt.Begin, bt.End);
                        dr["新文件名"] = s;
                    }
                }
                if (cmd.Prefix != "" || cmd.Subfix != "")
                    foreach (DataRow dr in dt.Rows)
                        dr["新文件名"] = cmd.Prefix + dr["新文件名"] + cmd.Subfix;
            }
		}
        private void buttonModifyFileContent_Click(object sender, EventArgs e)
        {
            if (textBoxCMD.Text != "")
            {
                Cmd cmd = new Cmd(textBoxCMD.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    string fn = dr["源文件名"].ToString();
                    string s = File.ReadAllText(folderpath +"\\"+ fn);
                    foreach (BETag bt in cmd.ReplacetoNullTags.tags)
                        s = s.Replace(bt.Begin, bt.End);
                    File.WriteAllText(folderpath +"\\"+fn, s);
                }             
            }
        }
		void ButtonClearBeforeSpaceClick(object sender, EventArgs e)
		{
			string path = textBoxPath.Text.Trim();			
			if (!System.IO.Directory.Exists(path))
			{
				MessageBox.Show("文件目录不存在");
				return;
			}
			if(!path.EndsWith("\\"))
				path+="\\";
			DirectoryInfo  di = new System.IO.DirectoryInfo(path);
			foreach(DirectoryInfo d in di.GetDirectories()){
				if(d.Name.Trim()!=d.Name)
					System.IO.Directory.Move(path+d.Name,path+d.Name.Trim());
			}
		}		
		void ButtonOutDirInfoClick(object sender, EventArgs e)
		{
			string path = textBoxPath.Text.Trim();
			string strout = "文件夹信息如下\r\n\r\n";
			if (!System.IO.Directory.Exists(path))
			{
				MessageBox.Show("文件目录不存在");
				return;
			}
			if(!path.EndsWith("\\"))
				path+="\\";
			DirectoryInfo  di = new System.IO.DirectoryInfo(path);
			foreach(DirectoryInfo d in di.GetDirectories()){
				strout+=d.Name +"\r\n";
			}
			strout+="文件信息如下\r\n\r\n";
			foreach(FileInfo fi in di.GetFiles()){
				strout+=fi.Name+"\r\n";
			}
			FormShow f=new FormShow(strout);
			f.ShowDialog();
		}
		void Button1Click(object sender, EventArgs e)
		{
			List<string> ls = Constructfilters();			
			List<string> filename=GetFileNames(ls);			
			AddFileNameToTable(filename);
		}
		
		void AddFileNameToTable(List< string > fs)
		{
			List<string> titles = new List<string> {
				"源文件名",
				"新文件名"
			};
			dt = DgvTools.CreateDataTable(titles, "SS");
			foreach ( string  s in fs) {
				DataRow dr = dt.NewRow();
				dr["源文件名"] = s;
				dr["新文件名"] = s;
				dt.Rows.Add(dr);
			}
			dgv.Columns.Clear();
			dgv.DataSource = dt;
			foreach (DataGridViewColumn dc in dgv.Columns)
				dc.Width = 250;
		}
		void AddFileNameToTable(List<List<string>> fs)
		{
			List<string> titles = new List<string> {
				"源文件名",
				"新文件名"
			};
			dt = DgvTools.CreateDataTable(titles, "SS");
			foreach (List<string> s in fs) {
				DataRow dr = dt.NewRow();
				dr["源文件名"] = s[0];
				dr["新文件名"] = s[1];
				dt.Rows.Add(dr);
			}
			dgv.Columns.Clear();
			dgv.DataSource = dt;
			foreach (DataGridViewColumn dc in dgv.Columns)
				dc.Width = 250;
		}
		private string ImportText()
		{
			OpenFileDialog fd = new OpenFileDialog();
			if (fd.ShowDialog() == DialogResult.OK) {
				if (fd.FileName.EndsWith(".txt"))
					return File.ReadAllText(fd.FileName);
			}
			return "";
		}
		private List<string>  GetFileNames(List<string> ls )
		{
			List<string> filename = new List<string>();
			string path = textBoxPath.Text.Trim();
			if (!System.IO.Directory.Exists(path)) {
				MessageBox.Show("文件目录不存在");
				return filename;
			}
			if (!path.EndsWith("\\"))
				path += "\\";
			DirectoryInfo di = new System.IO.DirectoryInfo(path);
			foreach (FileInfo fi in di.GetFiles()) {
				if (ls.Count > 0 && !ls.Contains(fi.Extension))
					continue;
				filename.Add(fi.Name);
			}
			return filename;
		}
		private List<string> Constructfilters()
		{
			string filter = textBoxFilterExt.Text;
			string[] fs = filter.Split(new string[] {
			                           	";",
			                           	" ",
			                           	"\t",
			                           	"\r",
			                           	"\n"
			                           }, StringSplitOptions.RemoveEmptyEntries);
			List<string> ls = new List<string>();
			foreach (string s in fs)
				if (s.StartsWith("."))
					ls.Add(s);
			return ls;
		}
		private string folderpath;
		public DataTable dt;

       
	}
	 public class Cmd
    {
        public Cmd(string str)
        {
            string[] items = str.Split(new string[] { ";\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            cmdkeyvalue = new Dictionary<string, string>();
            foreach (string s in items)
            {
                if (s.Contains("="))
                {
                    string name = s.Substring(0, s.IndexOf('=')).Trim();
                    if (ValidTools.ValidName(name) && !cmdkeyvalue.ContainsKey(name))
                        cmdkeyvalue[name] = s.Substring(s.IndexOf('=') + 1);
                }
            }

            if (cmdkeyvalue.ContainsKey("replacetonull"))
            {
                ReplacetoNullTags = new BETags(cmdkeyvalue["replacetonull"]);
            }
            if (cmdkeyvalue.ContainsKey("addprefix"))
                Prefix = cmdkeyvalue["addprefix"];
            if (cmdkeyvalue.ContainsKey("addsubfix"))
                Subfix = cmdkeyvalue["addsubfix"];

            return;

            ///////////////////////////////
            if (cmdkeyvalue.ContainsKey("nexturl"))
                NextUrlTags = new BETags(cmdkeyvalue["nexturl"]);
            if (cmdkeyvalue.ContainsKey("nextexist"))
                NextExist = cmdkeyvalue["nextexist"];
            if (cmdkeyvalue.ContainsKey("replacetemplate"))
            {
                ReplaceTemplate = cmdkeyvalue["replacetemplate"];
                ReplaceTemplate = ReplaceTemplate.Replace("\\r\\n", "\r\n")
                    .Replace("\\t", "\t");
            }
            if (cmdkeyvalue.ContainsKey("multisubitem"))
                MultiSubItemTags = new BETags(cmdkeyvalue["multisubitem"]);
            if (cmdkeyvalue.ContainsKey("reversematch"))
            {
                if (cmdkeyvalue["reversematch"] == "true")
                    ReverseMatch = true;
            }
            if (cmdkeyvalue.ContainsKey("txtasurl"))
            {
                if (cmdkeyvalue["txtasurl"] == "true")
                    TxtAsUrl = true;
            }
            if (cmdkeyvalue.ContainsKey("dbidbeginend"))
            {
               BETags bts = new BETags(cmdkeyvalue["dbidbeginend"]);
               Bedbid = null;
               if (bts.tags.Count == 1 && ValidTools.ValidNumber(bts.tags[0].Begin)
                                       && ValidTools.ValidNumber(bts.tags[0].End))
               {
                   Bedbid = new BEId();
                   Bedbid.B = Convert.ToInt32(bts.tags[0].Begin);
                   Bedbid.E = int.MaxValue;
                   if (ValidTools.ValidNumber(bts.tags[0].End))
                       Bedbid.E = Convert.ToInt32(bts.tags[0].End);                  
                   Bedbid.MoveToTop();
               }//else 未设置BeDbid
            }
            if (cmdkeyvalue.ContainsKey("savepath"))
                SavePath = cmdkeyvalue["savepath"];
            if (cmdkeyvalue.ContainsKey("downloadsuburl"))
            {
                string downloadsuburl = cmdkeyvalue["downloadsuburl"];
                if (downloadsuburl.ToLower().Trim() == "true")
                {
                    DownLoadSuburl = true;
                    DownLoadSuburlType = "auto";
                    if (cmdkeyvalue.ContainsKey("downloadsuburltype"))
                        DownLoadSuburlType = cmdkeyvalue["downloadsuburltype"].Trim();
                }
            }
            if (cmdkeyvalue.ContainsKey("casecmd"))
            {
                BEPos bp = BETag.FormatCmd(cmdkeyvalue["casecmd"], '{', '}');
                if (bp.Valid())
                {
                    string cmdstr = bp.String;
                    caseCmd = new CaseCmd(cmdstr);                    
                }
            }
            if (cmdkeyvalue.ContainsKey("table"))
            {
                BEPos bp = BETag.FormatCmd(cmdkeyvalue["table"], '{', '}');
                if (bp.Valid())
                {
                    string tablecmdstr = bp.String;
                    tableCmd = new TableCmd(tablecmdstr);
                    ListTable.Add(tableCmd); // 以后可以添加多个Table
                }
            }
            if (cmdkeyvalue.ContainsKey("tablevalue"))
            {
                BEPos bp = BETag.FormatCmd(cmdkeyvalue["tablevalue"], '{', '}');
                if (bp.Valid())
                {
                    string tablecmdstr = bp.String;
                    tablevalueCmd = new TableValueCmd(tablecmdstr);
                    
                }
            }
        }
        private Dictionary<string, string> cmdkeyvalue;
        public BETags MultiSubItemTags { get; set; }
        public BETags NextUrlTags { get; set; }
        public BETags ReplacetoNullTags { get; set; }

        public bool TxtAsUrl { get; set; }
        public bool ReverseMatch { get; set; }
        public BEId Bedbid { get; set; }
        public string  ReplaceTemplate { get; set; }
        public string NextExist { get; set; }
        public string SavePath { get; set; }
        public CaseCmd caseCmd { get; set; }
        public TableCmd tableCmd { get; set; }
        public TableValueCmd tablevalueCmd { get; set; }
        public List<TableCmd> ListTable { get; set; }
        public bool DownLoadSuburl { get; set; }
        public string DownLoadSuburlType { get; set; }

        public string Prefix { get; set; }
        public string Subfix { get; set; }
    }
     public class BETag
     {
         public BETag(string s)
         {
             OK = false;
             if (s == null)
                 return;
             if (s.StartsWith("{"))
             {
                 BEPos bp = BETag.FormatCmd(s, '{', '}');
                 if (bp.Valid())
                 {
                     Cmd = new Cmd(bp.String);//////////???????   
                     s = s.Substring(bp.E + 1);
                 }
             }
             string[] items = s.Split(new string[] { "[", "]", "-" }, StringSplitOptions.RemoveEmptyEntries);
             if (items.Length == 2)
             {
                 Begin = items[0].Replace("@@", "-").Replace("##<", "{").Replace(">##", "}");
                 End = items[1].Replace("@@", "-").Replace("##<", "{").Replace(">##", "}");
             }
             else if (items.Length == 1)
             {
                 Begin = items[0].Replace("@@", "-").Replace("##<", "{").Replace(">##", "}");
                 End = "";
             }
             else
                 return;
             OK = true;
         }
         public BEPos BEPos(string s)
         {
             if (OK)
             {
                 int B = s.IndexOf(Begin);
                 if (B != -1)
                 {
                     B = B + Begin.Length;
                     int E = s.IndexOf(End, B);
                     if (E != -1)
                         return new BEPos(B, E, s);
                 }
             }
             return new BEPos(-1, -1, s);
         }
         public BEPos BEPos(BEPos bp)//ReverseMatch
         {
             string s = bp.InnerStr;
             if (!OK || s == null || !bp.Valid())
                 return new BEPos(-1, -1, s);

             int B = s.IndexOf(Begin, bp.B);
             if (Cmd != null && Cmd.ReverseMatch)
                 B = s.LastIndexOf(Begin, bp.B, bp.E - bp.B);
             if (B != -1 && B < bp.E)
             {
                 B = B + Begin.Length;
                 int E = s.IndexOf(End, B, bp.E - B);
                 if (E != -1)
                     return new BEPos(B, E, s);
             }

             B = s.LastIndexOf(Begin, bp.E, bp.E - bp.B);
             if (B != -1)
             {
                 B = B + Begin.Length;
                 int E = s.IndexOf(End, B, bp.E - B);
                 if (E != -1)
                     return new BEPos(B, E, s);
             }
             return new BEPos(-1, -1, s);
         }
         public BEPos NextBEPos(BEPos bp) //同一个才能NextBEPos
         {
             string s = bp.InnerStr;
             if (!OK || !bp.Valid() || s == null)
                 return new BEPos(-1, -1, s);
             int B = s.IndexOf(Begin, bp.E + End.Length);
             if (B == -1)
                 return new BEPos(-1, -1, s);
             B = B + Begin.Length;
             int E = s.IndexOf(End, B);
             if (E == -1)
                 return new BEPos(-1, -1, s);
             return new BEPos(B, E, s);
         }
         public static BEPos FormatCmd(string Rule, char begin, char end, int startIndex = 0)
         {
             int B = 0;
             int Pos = Rule.IndexOfAny(new char[] { begin, end }, startIndex);
             if (Pos != -1 && Rule[Pos] == begin)
             {
                 B = Pos + 1;
                 int stack = 1;
                 while (Pos != -1)
                 {
                     Pos = Rule.IndexOfAny(new char[] { begin, end }, Pos + 1); //??
                     if (Rule[Pos] == begin)
                         stack++;
                     else if (Rule[Pos] == end)
                         stack--;
                     if (stack == 0)
                         return new BEPos(B, Pos, Rule);
                     if (stack < 0)
                         break;
                 }
             }
             return new BEPos(-1, -1, Rule);
         }
         public string Begin { get; set; }
         public string End { get; set; }
         public Cmd Cmd { get; set; }
         public Boolean OK { get; set; }
     }
     public class BETags
     {
         public BETags(List<BETag> rbts)
         {
             this.tags = rbts;
         }
         public BETags(string Rule)
         {
             tags = new List<BETag>();
             BEPos bp;
             if (Rule.StartsWith("{"))
             {
                 bp = BETag.FormatCmd(Rule, '{', '}');
                 if (bp.Valid())
                 {
                     string cmdstr = bp.String;/////////////////////////////////
                     this.Cmd = new Cmd(cmdstr);
                     Rule = Rule.Substring(bp.E + 1);
                 }
             }
             //Compute Rule
             bp = BETag.FormatCmd(Rule, '[', ']', 0);
             while (bp.Valid())
             {
                 BETag bt = new BETag(bp.String);
                 if (bt.OK)
                     tags.Add(bt);
                 bp = BETag.FormatCmd(Rule, '[', ']', bp.E + 1);
             }
             //Compute Cmd
         }
         public void Add(BETag bt)
         {
             tags.Add(bt);
         }
         public BETags SubTags(int b, int length = -1)
         {
             List<BETag> rbts = new List<BETag>();
             if (b >= 0)
                 for (int i = b, len = 0; i < tags.Count && len != length; i++, len++)
                     rbts.Add(tags[i]);
             return new BETags(rbts);
         }
         public List<BETag> tags { get; set; }

         public Cmd Cmd { get; set; }

         public string Match(string txt)
         {
             BEPos bp = null;
             if (tags.Count > 0)
                 bp = tags[0].BEPos(txt);
             for (int i = 1; i < tags.Count; i++)
                 bp = tags[i].BEPos(bp);
             if (bp == null)
                 return "";
             return bp.String;
         }
     }
     public class BEPos
     {
         public BEPos()
         {
             str = null;
             B = -1;
             E = -1;
         }
         public BEPos(int b, int e, string str)
         {
             this.str = str;
             B = b;
             E = e;
         }
         public bool Valid()
         {
             return B > -1 && str != null && (E > -1 && B < E || E == -1);
         }
         public string String
         {
             get
             {
                 if (!Valid()) return "";
                 return str.Substring(B, E == -1 ? -1 : E - B);
             }
         }
         public int B { get; set; }
         public int E { get; set; }
         public string InnerStr { get { return str; } }
         private string str;
     }
     public class BEId
     {
         public BEId()
         {
             B = E = activeindex = -1;
         }
         public void MoveNext()
         {
             activeindex++;
         }
         public void MoveToTop()
         {
             activeindex = 0;
         }
         public bool OK()
         {
             return B > 0 && E > 0 && B < E;
         }
         public bool HasNext
         {
             get
             {
                 return activeindex >= 0 && activeindex < E - B;
             }
         }
         public int B { get; set; }
         public int E { get; set; }
         public int ActiveIndex { get { return activeindex + B; } }
         private int activeindex;
     }
     public class CaseCmd
     {
         public CaseCmd(string str)
         {
             conditions = new List<Condition>();
             string[] items = str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
             foreach (string s in items)
             {
                 if (s.Contains("(") && s.Contains(")=>"))
                     conditions.Add(new Condition(s));
             }
         }
         public List<Condition> conditions;
     }
     public class Condition
     {
         public Condition(string strcdt)
         {
             strcdt = strcdt.Trim();
             int te = strcdt.IndexOf("(");
             int ve = strcdt.IndexOf(")=>");
             ctype = strcdt.Substring(0, te);
             cvalue = strcdt.Substring(te + 1, ve - te - 1);
             string actstr = strcdt.Substring(ve + 3);
             if (ctype == "contains")
                 b = bc_contains;
             else if (ctype == "startswith")
             {
                 b = bc_starswith;
             }
             else if (ctype == "endswith")
             {
                 b = bc_Endswith;
             }
             else if (ctype == "equals")
             {
                 b = bc_Equals;
             }
             else if (ctype == "default")
             {
                 b = bc_default;
             }
             else
             {
                 b = bc_false;
             }

             if (actstr.Contains("N"))
             {
                 actvalue = actstr;
                 a = act_getNcompute;
             }
             else
             {
                 if (actstr == "null" ||ValidTools .ValidDoubleNumber(actstr))
                 {
                     actvalue = actstr;
                     a = act_getactvalue;
                 }
                 else
                 {
                     a = act_getSame;
                 }
             }
         }
         public bool bc_contains(string s)
         {
             return s.Contains(cvalue);
         }
         public bool bc_starswith(string s)
         {
             return s.StartsWith(cvalue);
         }
         public bool bc_Endswith(string s)
         {
             return s.EndsWith(cvalue);
         }
         public bool bc_Equals(string s)
         {
             return s.Equals(cvalue);
         }
         public bool bc_default(string s)
         {
             return true;
         }
         public bool bc_false(string s)
         {
             return false;
         }

         public string act_getnumber(string s)
         {//不能识别复杂数字
             StringBuilder number = new StringBuilder();
             bool bn = false;
             if (".-0123456789".Contains(s[0]))
             {
                 bn = true;
             }
             foreach (char c in s)
             {
                 if (".-0123456789".Contains(c))
                 {
                     if (!bn) number.Clear();
                     else
                         number.Append(c);
                     bn = true;
                 }
                 else
                 {
                     bn = false;
                 }
             }
             return number.ToString();
         }
         public string act_getactvalue(string s)
         {
             return actvalue;
         }
         public string act_getNcompute(string s)
         {
             string N = act_getnumber(s);
             return actvalue.Replace("N", N);
         }
         public string act_getSame(string s)
         {
             return s;
         }
         public string ctype;
         public string cvalue;
         public string actvalue;
         public ActCondition a;
         public BoolCondition b;
     }
     public class TableCmd
     {
         public TableCmd(string str)
         {
             this.str = null;
             cols = null;
             string[] items = str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
             foreach (string s in items)
             {
                 if (s.Contains("name="))
                     Name = s.Substring(s.IndexOf("name=") + 5).Trim();
                 else if (s.Contains("rowgap="))
                     RowGag = s.Substring(s.IndexOf("rowgap=") + 7).Trim().Split('|');
                 else if (s.Contains("colgap="))
                     ColGap = s.Substring(s.IndexOf("colgap=") + 7).Trim().Split('|');
                 else if (s.Contains("location="))
                     Location = new BETags(s.Substring(s.IndexOf("location=") + 9).Trim());
             }
         }
         internal string Table(int RowIndex, int ColIndex)
         {
             if (txt != null)
             {
                 if (str == null || !string.ReferenceEquals(str, txt))
                 {
                     str = txt;
                     string s = Location.Match(txt);
                     rows = s.Split(RowGag, StringSplitOptions.RemoveEmptyEntries);
                     cols = new string[rows.Length][];

                 }
                 if (RowIndex < rows.Length)
                 {
                     if (cols[RowIndex] == null)
                         cols[RowIndex] = rows[RowIndex]
                             .Split(ColGap, StringSplitOptions.RemoveEmptyEntries);
                     if (ColIndex < cols[RowIndex].Length)
                         return cols[RowIndex][ColIndex];
                 }
             }
             return "";
         }
         public string Name { get; set; }
         public string[] RowGag { get; set; }
         public string[] ColGap { get; set; }
         public BETags Location { get; set; }
         private string[] rows;
         private string[][] cols;
         private string str;
         public static string txt;

     }

     public class TableValueCmd
     {
         private int RowIndex;
         private int ColIndex;
         public TableValueCmd(string str)
         {
             Valid = false;
             ColIndex = RowIndex = -1;
             string[] items = str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
             foreach (string s in items)
             {
                 if (s.Contains("name="))
                     Name = s.Substring(s.IndexOf("name=") + 5).Trim();
                 else if (s.Contains("rowindex="))
                     RowIndex = Convert.ToInt32(s.Substring(s.IndexOf("rowindex=") + 9));
                 else if (s.Contains("colindex="))
                     ColIndex = Convert.ToInt32(s.IndexOf("colindex=") + s.Substring(9));
             }

         }

         internal void InitTable(List<TableCmd> listtable)
         {
             if (RowIndex >= 0 && ColIndex >= 0)
                 foreach (TableCmd t in listtable)
                     if (Name == t.Name)
                     {
                         Tablecmd = t;
                         Valid = true;
                         break;
                     }
         }
         public bool Valid { get; set; }
         public string Name { get; set; }
         public TableCmd Tablecmd { get; set; }

         internal string GetValue() //RowIndex,ColIndex必定 >=0
         {
             if (Tablecmd != null)
                 return Tablecmd.Table(RowIndex, ColIndex);
             return "";
         }
     }
}
/*/////////////
if (System.IO.File.Exists(srcFileName))
{
	System.IO.File.Move(srcFileName, destFileName);
}
if (System.IO.Directory.Exists(srcFolderPath))
{
	System.IO.Directory.Move(srcFolderPath, destFolderPath);
}

///////////////*/

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
using System.Globalization;

namespace DataRegex
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
            cfgkeyvalue = new Dictionary<string, string>();
            if (!File.Exists("DataRegexCfg.ini"))
                return;
            ReadConfig();
            if (cfgkeyvalue.ContainsKey("infilename"))
            {
                textBoxInfilename.Text = cfgkeyvalue["infilename"];
            }
            if (cfgkeyvalue.ContainsKey("fileinputmode"))
            {
                string s = cfgkeyvalue["fileinputmode"].ToLower();
                if (s == "true" || s == "T")
                    checkBoxfileInputMode.Checked = true;
                else if (s == "false" || s == "f")
                    checkBoxfileInputMode.Checked = false;
                else
                    ;

            }
            if (cfgkeyvalue.ContainsKey("pattern"))
            {
                textBoxReplacePatten.Text = cfgkeyvalue["pattern"];
            }
            if (cfgkeyvalue.ContainsKey("outdistinct"))
            {
                string s = cfgkeyvalue["outdistinct"];
                if (s == "true" || s == "T")
                    checkBoxDistinct.Checked = true;
                else if (s == "false" || s == "f")
                    checkBoxDistinct.Checked = false;
                else
                    ;

            }
            if (cfgkeyvalue.ContainsKey("outfilename"))
            {
                textBoxOutFileName.Text = cfgkeyvalue["outfilename"];
            }
            if (cfgkeyvalue.ContainsKey("refinematchgroup"))
            {
                string s = cfgkeyvalue["refinematchgroup"];
                if (s == "true" || s == "T")
                    checkBoxGroup.Checked = true;
                else if (s == "false" || s == "f")
                    checkBoxGroup.Checked = false;
                else
                    ;
            }
            if (cfgkeyvalue.ContainsKey("replacepattern"))
            {
                textBoxReplaceOutExp.Text = cfgkeyvalue["replacepattern"];
            }
        }
        private void ReadConfig(string filename = "DataRegexcfg.ini")
        {
            if (File.Exists(filename))
            {
                string content = File.ReadAllText(filename);
                List<string> items = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                cfgkeyvalue = new Dictionary<string, string>();
                foreach (string s in items)
                {
                    if (s.Contains("="))
                    {
                        string name = s.Substring(0, s.IndexOf('=')).Trim();
                        if (ValidateName(name) && !cfgkeyvalue.ContainsKey(name))
                            cfgkeyvalue[name] = s.Substring(s.IndexOf('=') + 1).Replace("<@>", "=");
                    }
                }
            }
        }
        private void buttonInBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName.EndsWith("txt")
                   || fd.FileName.EndsWith("htm")
                   || fd.FileName.EndsWith("html")
                   || fd.FileName.EndsWith("ini"))
                {
                    textBoxInfilename.Text = fd.FileName;
                }
                else
                {
                    MessageBox.Show("暂时只支持txt,Html,htm,ini文件");
                }
            }
        }
        private void buttonReadInfile_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBoxInfilename.Text))
                textBoxIn.Text = File.ReadAllText(textBoxInfilename.Text);
            else
                MessageBox.Show("文件名不存在");
        }
        private void button_MatchCount_Click(object sender, EventArgs e)
        {
            string pattern = textBoxReplacePatten.Text;
            string s = GetInText();
            if (s == "" || pattern == "")
            {
                textBoxOut.Text = "没有文件名，或者文件不存在, 或者正则表达式不存在";
                return;
            }
            textBoxOut.Text = "";
            try
            {
                int cnt = Regex.Matches(s, pattern, RegexOptions.Singleline).Count;
                textBoxOutCnt.Text = "匹配次数：" + cnt;
            }
            catch (Exception ex)
            {
                textBoxOut.Text = "正则表达式出现错误：" + ex.Message;
            }        	
        }
        private void buttonMatchTest_Click(object sender, EventArgs e)
        {
            string pattern = textBoxReplacePatten.Text;
            string s = GetInText();
            if (s == "" || pattern == "")
            {
                textBoxOut.Text = "没有文件名，或者文件不存在, 或者正则表达式不存在";
                return;
            }
            textBoxOut.Text = "";

            try
            {
                MatchCollection mc = Regex.Matches(s, pattern);
                StringBuilder sb = new StringBuilder();
                List<string> ls = new List<string>();
                int num = 1;

                foreach (Match m in mc)
                    ls.Add(m.Value);
                if (checkBoxDistinct.Checked)
                {
                    ls = ls.Distinct().ToList();
                    ls.Sort();
                }

                if (checkBoxOuttheSame.Checked)
                        foreach (string m in ls)
                            sb.Append(m );
                else
                        foreach (string m in ls)
                            sb.AppendLine((num++).ToString() + m);
                textBoxOut.Text = sb.ToString() ;

            }
            catch (Exception ex)
            {
                textBoxOut.Text = "正则表达式出现错误：" + ex.Message;
            } 
        }
        private void buttonOutMatchtoFile_Click(object sender, EventArgs e)
        {
            string pattern = textBoxReplacePatten.Text;
            string s = GetInText();
            if (s == "" || pattern == "")
            {
                textBoxOut.Text = "没有文件名，或者文件不存在, 或者正则表达式不存在";
                return;
            }
            textBoxOut.Text = "";
            
            try
            {
                MatchCollection mc = Regex.Matches(s, pattern, RegexOptions.Singleline);
                StringBuilder sb = new StringBuilder();

                if (!checkBoxDistinct.Checked)
                {
                    if (checkBoxGroup.Checked)
                    {
                        if (checkBoxOuttheSame.Checked)
                            foreach (Match m in mc)
                                sb.Append(m.Groups[1].Value);
                        else
                            foreach (Match m in mc)
                                sb.AppendLine(m.Groups[1].Value);
                    }
                    else
                    {
                        if (checkBoxOuttheSame.Checked)
                            foreach (Match m in mc)
                                sb.Append(m.Value);
                        else
                            foreach (Match m in mc)
                                sb.AppendLine(m.Value);
                    }
                }
                else
                {
                    List<string> ls = new List<string>();
                    int num = 1;
                    foreach (Match m in mc)
                        ls.Add(m.Value);
                    if (checkBoxDistinct.Checked)
                    {
                        ls = ls.Distinct().ToList();
                        ls.Sort();
                    }

                    foreach (string m in ls)
                            sb.AppendLine((num++).ToString() + m);
                    //textBoxOut.Text = sb.ToString();
                }
                string outfilename = GetOutMatchFileName();
                File.WriteAllText(outfilename, sb.ToString());
            }
            catch (Exception ex)
            {
                textBoxOutCnt.Text = "正则表达式出现错误：" + ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = GetInText();
            List<string> ls = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string ws = Regex.Replace(s, "[^\\u4e00-\\u9fa5\r\n]+", "");
            List<string> lws = ws.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> rls = new List<string>();
            int checki=0;
            for (int i = 1; i < lws.Count; i++)
            {
                if (lws[i] == lws[i - 1])
                {
                    checki = ls[i].Length > ls[i - 1].Length ? i : i - 1;
                }
                else
                {
                    rls.Add(ls[checki].Substring( ls[checki].IndexOf(".")));
                    checki = i;
                }
            }
            string content = "<item>"+string.Join("</item>\r\n<item>", rls) + "</item>";
            File.WriteAllText("items.html",content);

        }
        private string GetOutMatchFileName(string mr = "_match") //或者让replace
        {
            string inf = textBoxInfilename.Text;         
            if (!File.Exists(inf))
                return  mr=="_match"? "_match.html":"_replace.html";
            string directoryname =Path.GetDirectoryName( Path.GetFullPath(inf));
            string filenamewithoutext = Path.GetFileNameWithoutExtension(inf);
            string strpaths = string.Join("\r\n",Directory.GetFiles(directoryname, "*.*"));
                       
            string pattern = filenamewithoutext;
            if (Regex.IsMatch(filenamewithoutext, "(_replace|_match)\\d*"))
                pattern = Regex.Replace(pattern, "(_replace|_match)\\d*", "");
            string savefilenamewithoutext = pattern;
            pattern += "(_replace|_match)";
            string mcpattern = "(?<=" + pattern+ ")\\d+(?=\\" + Path.GetExtension(inf)+")";
            MatchCollection mc = Regex.Matches(strpaths,mcpattern );
            int max = 1;
            foreach (Match m in mc)
            {
                int n = Convert.ToInt32(m.Value);
                if (max <= n)
                    max = n+1; 
            }
            return savefilenamewithoutext +mr + max + Path.GetExtension(inf);

        }
        private void buttonRegexReplace_Click(object sender, EventArgs e)
        {
            string pattern = textBoxReplacePatten.Text;
            string replacement = textBoxReplaceOutExp.Text;
            string s = GetInText();
            if (s == "" || pattern == "" || textBoxOutFileName.Text=="")
            {
                textBoxOut.Text = "没有文件名，或者文件不存在, 或者正则表达式不存在";
                return;
            }

            textBoxOut.Text = "";
            try
            {
                string ss = Regex.Replace(s, pattern, replacement);
                string outfilename = GetOutMatchFileName("_replace");
                File.WriteAllText(outfilename, ss);
            }
            catch (Exception ex)
            {
                textBoxOut.Text = "正则表达式出现错误：" + ex.Message;
            }
        }
        private void buttonUnicodeTable_Click(object sender, EventArgs e)
        {
            int unicode = 0;
            for (int f = 0; f < 16; f++)
            {
                File.WriteAllText("unicode_" + f + ".txt", "");
                for (int b = 0; b < 16; b++)
                {
                    unicode = (f << 12) + (b << 8);
                    try
                    {
                        StringBuilder fsb = new StringBuilder();
                        fsb.AppendLine("\r\n\r\n==============" + b + "================\r\n");
                        fsb.AppendLine(UnicodeBlockToTable(unicode));
                        File.AppendAllText("unicode_" + f + ".txt", fsb.ToString());
                    }
                    catch (Exception ee)
                    {
                        File.AppendAllText("unicode_error.log", "\r\nf=" + f + " b=" + b + "  ExeMsg:" + ee.Message);
                    }
                }
            }
        }
        private void buttonCharacterStatic_Click(object sender, EventArgs e)
        {
            string s = GetInText();
            string pattern1 = @"(?![_a-zA-Z0-9\u4e00-\u9fa5\r\n])[\w]";
            string pattern2 = @"[^\d\s\w</>.：，。()；+-=]";
            string pattern3 = @"[\s]";
            try
            {
                // 1、 提取中文中特殊 符号
                StringBuilder sb = new StringBuilder("中文中特殊符号：\r\n");
                MatchCollection mc = Regex.Matches(s, pattern1);
                foreach (Match m in mc)
                    sb.Append(m.Value);
                //2、提取其他特殊符号
                sb.AppendLine("\r\n其他特殊符号(已排除\\d\\s\\w</>.：，。()；+-=):\r\n");
                mc = Regex.Matches(s, pattern2);
                List<string> ls = new List<string>();               
                foreach (Match m in mc)
                    ls.Add(m.Value);
                ls = ls.Distinct().ToList();
                ls.Sort();
                string schar = string.Join("", ls);
                sb.AppendLine(schar);
                int num = 0;
                List<string> lsunicode = new List<string>();
                lsunicode = StringToUniCode.StringToUnicode(schar)
                    .Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                if (ls.Count == lsunicode.Count)
                    for (int i = 0; i < lsunicode.Count; i++)
                        sb.AppendLine((num++).ToString()+"\t"+ls[i]+"\t\\u"+lsunicode[i]);
                
                //3、提取空格字符
                mc = Regex.Matches(s, pattern3);
                ls.Clear();
                foreach (Match m in mc)
                    ls.Add(m.Value);
                ls = ls.Distinct().ToList();
                ls.Sort();
                s = string.Join("",ls);
                sb.AppendLine("\r\n空白字符分析：\r\n"+s+"\r\n"+StringToUniCode.StringToUnicode(s));
                File.WriteAllText("CharacterStatic.txt", sb.ToString());
            }
            catch (Exception ex)
            {
                textBoxOut.Text = "分析错误："+ex.Message;
            }
        }
        private string UnicodeBlockToTable(int unicode)
        {
            string[][] t = new string[17][];
            for (int i = 0; i < 17; i++)
            {
                t[i] = new string[17];
            }
            for (int i = 1; i < 17; i++)
            {
                t[0][i] = (i - 1).ToString();
                t[i][0] = "u" + (unicode + ((i - 1) << 4)).ToString("x");
            }

            int x, y;
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    x = i + 1; y = j + 1;
                    int iu = unicode + (i << 4) + j;
                    t[x][y] = "" + (char)(iu);
                }
            StringBuilder sb = new StringBuilder();
            foreach (string[] ss in t)
                sb.AppendLine(string.Join("\t", ss));
            return sb.ToString();
        }
        private string uncode(string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)//u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }  
        private string GetInText()
        {
            string s = "";
            if(checkBoxfileInputMode.Checked){
                if(File.Exists(textBoxInfilename.Text))
                s = File.ReadAllText(textBoxInfilename.Text);
            }else{
                s = textBoxIn.Text;
            }
            return s;

        }
        private bool ValidateName(string name)
        {
            if (name == "") return false;
            foreach (char c in name.ToLower())
                if (!"0123456789abcdefghikjlmnopqrstuvwxyz".Contains(c))
                    return false;
            if ("0123456789".Contains(name[0]))
                return false;
            return true;
        }
        private Dictionary<string, string> cfgkeyvalue;

       
    }
    public static class StringToUniCode
    {
        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string StringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }
        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }
    }   
}

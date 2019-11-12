using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.IO.Compression;
using Snowball.Common;
using System.Threading;
using System.Text.RegularExpressions;

namespace JyeoPaper
{
    public delegate void ShowDeleGate(string file);
    public partial class FormN : Form
    {
        public FormN()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load("jypepaper.xml");
            XmlElement root = XmlDoc.DocumentElement;
            XmlNodeList xn = root.SelectNodes("*");
            PaperItem.Init(xn); 
        }
        private void buttonOPEN_Click(object sender, EventArgs e)
        {
            try
            {
                //string html = GetHttpWebRequest(textBox1.Text, out  strCookies);
                this.webBrowser1.Navigate(textBox1.Text);
            }
            catch { }
        }
        private void buttonShowHtml_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.webBrowser1.DocumentText != null)
                {
                    FormTxt ft = new FormTxt(this.webBrowser1.DocumentText);
                    ft.Show();
                }
            }
            catch { }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();          
            f.Filter= "html|*.html";
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(f.FileName, webBrowser1.DocumentText);
            }
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                string html = GetHttpWebRequest(textBox1.Text, out  strCookies);               
                OutToPaper(html);
            }
            catch { }
        }

        private void buttonActivePage_Click(object sender, EventArgs e)
        {
            try
            {
                string html = webBrowser1.DocumentText;
                OutToPaper(html);
            }
            catch { }
        }
        private void buttonTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "html|*.htm;*.html;*.txt";
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string html = File.ReadAllText(f.FileName);
                OutToPaper(html);
            }
        }
        private void OutToPaper(string html)
        {
            List<PaperItem> ps = new List<PaperItem>();
            List<string> items = GetItems(html);
            foreach (string s in items)
            {
                ps.Add(new PaperItem(s));
            }
            OutToPaper(ps);
        }
        private void OutToPaper(List<PaperItem> ps)
        {
            string html = "";
            int cnt = 1;
            string answer="" ;
            bool hasselectanswer = false;
            bool hasselect = false;
            foreach (PaperItem p in ps)
            {
                html += p.ToHtml(cnt);
                try
                {
                    answer += p.ToAnswer(cnt);
                    if (p.HasSelectionAnswer())
                        hasselectanswer = true;
                    if (p.IsSelectionItem())
                        hasselect = true;
                }
                catch {
                    ;
                }
                cnt++;
            }
            if (hasselectanswer == false && hasselect)
            {
                 cnt = 1;
                 answer = "";
                foreach (PaperItem p in ps)
                {
                    try
                    {
                        if(p.IsSelectionItem())
                            p.GetAnswerFromNet();
                    }
                    catch {
                        ;
                    }
                    answer += p.ToAnswer(cnt);
                    cnt++;
                }
            }
            html  = PaperItem.tpage.Replace("<!--pagebody-->", html);

      
            //输出答案
            html = html.Replace("</body>", answer + "\r\n</body>")
                       .Replace("<br>", "<p>")
                       .Replace("<br />", "<p>")
                       .Replace("<BR>", "<p>")
                       .Replace("</br>", "")
                       .Replace("</BR>", "")
                       .Replace("<div class='quizPutTag' contenteditable='true'>&nbsp;</div>", "<U>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</U>");

            //Regex regex1 = new Regex(@"<script[\s\s]+</script *>", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"<div class=""sanwser""[\s\S]+</div *>", "");
            webBrowser1.DocumentText = html;
        }
        private string ConstructOutputAnswer(List<PaperItem> ps)
        {
            return "";
        }
        private List<string> GetItems(string text)
        {
            List<string> r = new List<string>();
            string itembegin = "<fieldset";
            string itemend = "</fieldset>";
            int pos = 0;
            while (pos >= 0)
            {
                string item = DgvTools.GetEqualValue(text, itembegin, itemend, ref pos) + itemend;
                if (pos == -1)
                    break;
                r.Add(item);
            }
            return r;
        }
        private void ChangeCookiesJYERN()
        {
            Thread.Sleep(sleeptime);
            string nurl = "http://www.jyeoo.com/bio2/ques/search?f=1&s=0&t=1&q=22";
            string htmlcooke = GetHttpWebRequest(nurl, out  strCookies);
            wl = 1;
        }
        private string GetWebRequest_Charset(ref string charset, string url)
        {
            string txt = GetWebRequest(url, charset);
            if (!txt.Contains(charset))
            {
                if (txt.Contains("gb2312"))
                {
                    charset = "gb2312";
                    txt = GetWebRequest(url, charset);
                }
            }
            return txt;
        }
        private string GetHttpWebRequest(string url, out string strCookies)
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
            strCookies = FenxiCookie(result.Headers["Set-Cookie"]);
            result.Close();
            return strHTML;
        }
        private string GetHttpWebRequest(string url, string strCookies)
        {
            if (wl > 0)
            {
                strCookies += "WL=" + wl + ";";
            }
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            myReq.Headers.Add("Cookie", strCookies);
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            wl = FenxiCookieWl(result.Headers["Set-Cookie"]);
            result.Close();
            return strHTML;
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
        private string GetWebRequest(string url, string charset = "utf-8")
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(charset));
            string strHTML = readerOfStream.ReadToEnd();

            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        private static string PostHtml(string url, ref string strCookies)
        {
            PostSubmitter post = new PostSubmitter();
            System.Net.ServicePointManager.Expect100Continue = false;
            post.Url = url;
            post.Type = PostSubmitter.PostTypeEnum.Post;
            // 加入cookies，必须是这么写，一次性添加是不正确的。
            post.strCookies = strCookies;
            string ret = post.Post();
            strCookies = post.strCookies;
            return ret;
        }
        private int FenxiCookieWl(string cookie)
        {
            if (cookie == null)
                return this.wl;
            int wl = Convert.ToInt32(DgvTools.GetEqualValue(cookie, "WL=", ";"));
            return wl;
        }
        private string FenxiCookie(string cookie)
        {
            string ret = "jyean=" + DgvTools.GetEqualValue(cookie, "jyean=", ";") + ";";
            return ret;
        }
        private int CheckHtml(ref string html, ref string url)
        {
            if (!html.Contains("VIP用户：请直接"))
                return 0;
            int r = 4;
            int i = 1;
            while (i < r)
            {
                Thread.Sleep(20 * i);
                ChangeCookiesJYERN();
                File.AppendAllText("cookie.log", DateTime.Now.ToString() + "\t" + strCookies + "\r\n");
                wl++;
                html = GetHttpWebRequest(url, strCookies);
                if (!html.Contains("VIP用户：请直接"))
                    return 0;
                i++;
            }
            return 1;
        }
        public void showfiletxt(string file)
        {
            this.textBox1.Text = file;
        }
        public static void DownLoadFile(String url, String FileName)
        {
            try
            {
                FileStream outputStream = new FileStream(FileName, FileMode.Create);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 3000;
                WebResponse wr = (HttpWebResponse)request.GetResponse();
                Stream httpStream = wr.GetResponseStream();
                httpStream = DealStreamZip(wr, httpStream);

                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = httpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);

                    readCount = httpStream.Read(buffer, 0, bufferSize);
                }
                httpStream.Close();
                outputStream.Close();
            }
            catch (Exception ex)
            {
                FileStream outputStream = new FileStream(Application.StartupPath + @"\downloaderror.log", FileMode.Append);
                StreamWriter sw = new StreamWriter(outputStream);
                String s = url + "\t" + FileName + "\t 文件下载失败错误为" + ex.Message.ToString() + "\r\n";
                sw.Write(s);
                sw.Close();
                outputStream.Close();
                //return "";
                //MessageBox.Show("文件下载失败错误为" + ex.Message.ToString(), "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private static Stream DealStreamZip(WebResponse wr, Stream httpStream)
        {
            if (wr.Headers["Content-Encoding"] == "gzip")//gzip解压处理
            {
                MemoryStream msTemp = new MemoryStream();
                GZipStream gzs = new GZipStream(httpStream, CompressionMode.Decompress);
                byte[] buf = new byte[1024];
                int len;
                while ((len = gzs.Read(buf, 0, buf.Length)) > 0)
                {
                    msTemp.Write(buf, 0, len);
                }
                msTemp.Position = 0;
                httpStream = msTemp;
            }
            else if (wr.Headers["Content-Encoding"] == "deflate")//gzip解压处理
            {
                MemoryStream msTemp = new MemoryStream();
                DeflateStream gzs = new DeflateStream(httpStream, CompressionMode.Decompress);
                byte[] buf = new byte[1024];
                int len;
                while ((len = gzs.Read(buf, 0, buf.Length)) > 0)
                {
                    msTemp.Write(buf, 0, len);
                }
                msTemp.Position = 0;
                httpStream = msTemp;
            }
            return httpStream;
        }

        private int sleeptime;
        public string strCookies;
        public int wl;


      

       

    }

    public class PaperItem
    {
        public PaperItem(string s)
        {
            this.s = s;
            this.JyeID = "";
            if (s.Contains("<input name=\"QA_") && s.Contains("type=\"radio\" class=\"radio\" />"))
            {
                s = Regex.Replace(s, "<input name=\"QA_[^<>]*type=\"radio\" class=\"radio\" />", "");
            }
            //while (s.Contains("<input name=\"QA_") && s.Contains("type=\"radio\" class=\"radio\" />"))
            //    s = Regex.Replace(s, "<input name[^<>]*/>","",RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (s.Contains("<fieldset id=\"") && s.Contains("\" class=\"quesborder\" s=\"bio2\">"))
            {
                this.JyeID = s.Substring("<fieldset id=\"".Length,36);
            }
            BE1 = DgvTools.GetEqualValue(s, B1, E1);
            BE2 = DgvTools.GetEqualValue(s, B2, E2);
            if (BE2 == "")
            {
                int pos = 0;
                while (pos >= 0)
                {
                    string item = DgvTools.GetEqualValue(s, BA,EA,  ref pos) + EA;
                    if (pos == -1)
                        break;
                    LEA.Add(item);
                    string item2 = DgvTools.GetEqualValue(s, "<div class=\"sanwser\">", "</div>", ref pos) + "</div>";
                    if (pos == -1)
                        break;
                    LEA2.Add(item2);
                }
            }
            else
            {
                //List<string> 
                ABCD = new List<string>();
                string IB = "<label class=\"";
                string IE = "</label>";
                int pos = 0;
                while (pos >= 0)
                {
                    string item = DgvTools.GetEqualValue(s, IB ,IE ,  ref pos);
                    if (pos == -1)
                        break;
                    ABCD.Add(item);
                }
                if (ABCD.Count > 0) //openwidth
                {
                    optionwidth = DgvTools.GetEqualValue(BE2, "<td style=\"width:", "%\"");
                    if (optionwidth.StartsWith("<td style=\"width:"))
                        optionwidth = optionwidth.Substring("<td style=\"width:".Length);
                    if (optionwidth == "")
                        optionwidth = "98";
                }
                //
                string value = "";
                foreach (string str in ABCD)
                {
                    string abcd = "";
                    if (str.StartsWith("<label class=\" s\">"))
                    {
                        abcd = str.Substring("<label class=\" s\">".Length, 1);
                        value = str.Substring("<label class=\" s\">".Length+2);
                        ANSWER += abcd;
                    }
                    else
                    {
                        abcd = str.Substring("<label class=\"\">".Length, 1);
                        value = str.Substring("<label class=\"\">".Length+2);
                    }
                    if (abcd == "A")
                        A = value;
                    else if(abcd == "B")
                        B = value;
                    else if(abcd == "C")
                        C = value;
                    else if(abcd == "D")
                        D = value;                   
                }
            }
        }
        
        private string BE1;
        private string BE2;
        private string ANSWER;
        private string A;
        private string B;
        private string C;
        private string D;
        private List<string> LEA = new List<string>();
        private List<string> LEA2 = new List<string>();
        public static string B1 = "<!--B1-->";
        public static string E1 = "<!--E1-->";
        public static string B2 = "<!--B2-->";
        public static string E2 = "<!--E2-->";
        public static string BA = "<!--BA-->";
        public static string EA = "<!--EA-->";
        private string s;
        private List<string> ABCD;

        private string optionwidth = "";
        private static string titem = "";
        private static string ttitle = "";
        private static string toption = "";
        private static string toption1 = "";
        private static string toption2 = "";
        private static string toption4 = "";
        public static string tpage = "";
        private string Title()
        {
            if (BE1.StartsWith("<span class=\"qseq\">"))
            {
                int pos = BE1.IndexOf("．</span>");
                if (pos != -1)
                    BE1 = BE1.Substring("．</span>".Length + pos);
            }
            return BE1;
        }
        public bool IsSelectionItem()
        {
            return A != null;
        }
        public bool HasSelectionAnswer()
        {
            if (A != null && ANSWER != null && ANSWER != "")
                return true;
            return false;
        }
        public string ToHtml(int i)
        {
            string html = "";

            string option = "";
            if (IsSelectionItem())
            {
                string ttoption = "";
                int tnums = (int)(99 / Convert.ToInt32(optionwidth));
                if (tnums == 4)
                    ttoption = toption4;
                else if (tnums == 2)
                    ttoption = toption2;
                else if (tnums == 1)
                    ttoption = toption1;
                option = ttoption.Replace("[LA]", A)
                             .Replace("[LB]", B)
                             .Replace("[LC]", C)
                             .Replace("[LD]", D);
                option = toption.Replace("<!--content-->", option);
            }
            else
            {
                foreach (string s in LEA)
                {
                    BE1 = BE1.Replace(s, "<U>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</U>");
                }
                foreach (string s in LEA2)
                {
                    BE1 = BE1.Replace(s, "");
                }
            }

            string title = "<P><SPAN>" + i + " .</SPAN>" + Title();
            title = ttitle.Replace("<!--content-->", title);
            html += titem.Replace("<!--title-->", title)
                         .Replace("<!--option-->", option)
                        ;
            
            return html;
        }
       
        internal static void Init(XmlNodeList xn)
        {
            ttitle = GetValueFromId(xn, "ttitle");
            tpage = GetValueFromId(xn, "tpage");
            titem = GetValueFromId(xn, "titem");
            toption = GetValueFromId(xn, "toption");
            toption1 = GetValueFromId(xn, "toption1");
            toption2 = GetValueFromId(xn, "toption2");
            toption4 = GetValueFromId(xn, "toption3");
        }
        private static string GetValueFromId(XmlNodeList xn, string id)
        {
            foreach (XmlNode xe in xn)
            {
                if (id == xe.Attributes["id"].Value)
                    return xe.Attributes["value"].Value;
            }
            return "";
        }

        internal string ToAnswer(int i)
        {

            string html = "<P><SPAN>" + i + " .</SPAN>";
            if (IsSelectionItem())
            {
                if (ANSWER == null)
                    html += "<a href=\"" + "http://www.jyeoo.com/bio2/ques/detail/" + this.JyeID + "\">link</a>";
                else
                    html += ANSWER;
            }
            else
            {
                foreach (string s in LEA)
                {
                    html += s;
                }
            }
            return html;
        }

        internal void GetAnswerFromNet()
        {
            string url="", html;
            //if (this.JyeID == "") return;
            //string url = "http://www.jyeoo.com/bio2/ques/detail/" + this.JyeID;
            //string html = DgvTools.GetWebClient(url);
            //if (html.Contains("查看本题解析需要") && html.Contains("登录菁优网"))
            //    return;
            //if (IsSelectionItem())
            //    GetSelectionAnswerFromHtml(html);
            //else
            //{
            //    string answer = DgvTools.GetEqualValue(html, "<!--B6-->", "<!--E6-->");
            //    if (answer.Contains("<br />故答案是：<br />"))
            //        answer = answer.Substring("<br />故答案是：<br />".Length);
            //}
            //return;
            string t = Title();
            if (!IsSelectionItem() )
                return;
            if(!t.Contains("http://www.jyeoo.com/bio2/report/detail/"))
                return;
            url = url.Substring(0, "http://www.jyeoo.com/bio2/report/detail/a6898529-4677-4422-a62d-1fb2308d29e1".Length);
            html = DgvTools.GetWebClient(url);
            GetSelectionAnswerFromHtml(html);

        }

        private string GetSelectionAnswerFromHtml(string html)
        {
            if (html.Contains(A) && html.Contains(B))
            {
                ANSWER = "";
                html = html.Substring(html.IndexOf(A) - "<label class=\" s\">  ".Length);
                html = html.Substring(0, html.IndexOf("<!--E2-->"));
                if (html.Contains("<label class=\" s\">A"))
                    ANSWER += "A";
                if (html.Contains("<label class=\" s\">B"))
                    ANSWER += "B";
                if (html.Contains("<label class=\" s\">C"))
                    ANSWER += "C";
                if (html.Contains("<label class=\" s\">D"))
                    ANSWER += "D";
            }
            return html;
        }

        public string JyeID { get; set; }
    }
}

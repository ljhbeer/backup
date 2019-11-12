using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SortDesk
{
    class CConfig
    {
        private bool bfront;
        public int roomcnt;
        public int groupcnt;
        private bool[] mgap;
        private CStudent[,] mdesk;

        internal CStudent[,] Mdesk
        {
            get { return mdesk; }
            set { mdesk = value; }
        }
        private List<CStudent> mstudent;
        public CConfig()
        {
            mstudent = new List<CStudent>();
        }
        public int Layout 
        {
            get { return roomcnt*256 + groupcnt; }
            set
            {
                groupcnt = value % 256;
                roomcnt = value / 256;
                mgap = new bool[groupcnt];
                Mdesk = new CStudent[groupcnt, roomcnt];
            }
        }

        internal void SetDeskGap(string str)
        {
            if (str.Length != groupcnt)
                return;
           // if(str.  全为01组成
           for(int i=0; i<str.Length; i++)
           {
               if (str[i] == '0')
                   mgap[i] = false;
               else
                   mgap[i] = true;
           }     
        }

        internal void SetFront(string str)
        {
            if (str == "true" || str == "TRUE")
                this.bfront = true;
            else
                this.bfront = false;
        }

        internal void AddDeskInfo(string[][] vvds)
        {
            if (vvds.Length == this.roomcnt && vvds[0].Length == this.groupcnt + 1)
            {
                for (int roomid = 0; roomid < roomcnt; roomid++)
                {
                    for (int groupid = 0; groupid < groupcnt; groupid++)
                    {
                        Mdesk[roomid,groupid] = GetStudent( vvds[roomid][groupid+1]);
                    }
                }
            }
            
        }

        private CStudent GetStudent(string name)
        {
            if(name == "" || name == "无人")
                return null;
            foreach (CStudent s in mstudent)
                if (s.Name == name)
                    return s;
            CStudent stu= new CStudent(name);
            mstudent.Add(stu);
            return stu;
        }

        internal void Save(string fileName)
        {
            FileStream f = File.Open(fileName, FileMode.Create);
            if (f == null)
            {
                Console.WriteLine("文件无法打开");
                return;
            }
            string headall = "<head>" + this.Layout.ToString("X8") + "</head><gap>";
            WriteFileString(f, headall);
            string str="";
            for (int i = 0; i < mgap.Length; i++)
            {
                if( mgap[i])
                    str += "1";
                else
                    str += "0";
            }
            str += "</gap><front>";
            if (bfront)
                str += "true";
            else
                str += "false";
            str += "</front><namelist>\r\n";
            WriteFileString(f, str);

            for (int roomid = 0; roomid < roomcnt; roomid++)
            {
                str = (roomid + 1).ToString()+"\t";
                for (int groupid = 0; groupid < groupcnt; groupid++)
                {
                    if (mdesk[groupid, roomid] == null)
                        str += "无人\t";
                    else
                        str += mdesk[groupid, roomid] + "\t";
                }
                str += "\r\n";
                WriteFileString(f, str);
            }
            str = "</namelist>";
            WriteFileString(f, str);
            f.Close();
        }

        private static void WriteFileString(FileStream f, string str)
        {
            byte[] bstr = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
            f.Write(bstr, 0, bstr.Length);
        }
    }
}

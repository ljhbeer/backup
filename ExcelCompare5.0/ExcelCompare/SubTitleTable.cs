using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelCompare
{
    class SubTitleTable
    {
        public SubTitleTable(int SBCol, int col, Excel.Range range1)
        {
            this.BCol = SBCol;
            this.ECol = col;
            this.range1 = range1;
        }

        public int BCol { get; set; }
        public int ECol { get; set; }
        private Excel.Range range1;

        internal bool UnSkipTitle()
        {
            return ((Excel.Range)range1[1, BCol]).Interior.ColorIndex != 1;
        }

        public string TableName
        {
            get
            {
                string t = ((Excel.Range)range1[1, BCol]).Text;
                if (t.Contains("-"))
                    return t.Substring(0, t.IndexOf("-"));
                string tablename = ColToName(BCol);
                //((Excel.Range)range1[1, BCol]).Text = tablename + "-" + t;
                return tablename;
            }
        }

        private string ColToName(int BCol)
        {
            char[] names = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
            List<char> lc = new List<char>();
            while (BCol > 0)
            {
                lc.Add(names[BCol % names.Length]);
                BCol /= BCol;
            }
            return lc.ToArray().ToString();
        }
    }
}

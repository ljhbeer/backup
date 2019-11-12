using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel;
using System.Drawing;
using System.Data;

namespace ExcelCompare
{
    public  class ExcelBook
    {
        public ExcelBook(string  FileName)
        {
            this.FileName = FileName;
            object oMissing = System.Reflection.Missing.Value;
            workbook = excel.Workbooks.Open(FileName, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);            
            _sheetnames = new List<string>();
            foreach (Worksheet ws in workbook.Worksheets)
                _sheetnames.Add(ws.Name);
        }

        public ExcelBook()
        {
            this.FileName = null;
            workbook = null;
        }
        public void  Create()
        {
            if (FileName == null && workbook == null)
            {
                object oMissing = System.Reflection.Missing.Value;
                workbook = excel.Workbooks.Add();
                _sheetnames = new List<string>();
            }
        }
        ~ExcelBook()
        {
            workbook = null;
        }
        public Worksheet ExcelSheet(string sheetname)
        {
            if (_sheetnames.Contains(sheetname))
                return workbook.Worksheets[sheetname];
            return null ;
                    
        }
        public Worksheet CopySheet(ExcelSheet S)
        {
            Worksheet ws = (Worksheet)workbook.Sheets[1];
            S.WorkSheet.Copy(ws);
            ws = workbook.Worksheets[1];
            ws.Name = S.Name;
            return ws;
        }
        public Worksheet CopySheetAndDeleteOther(ExcelSheet S)
        {
            Worksheet ws = (Worksheet)workbook.Sheets[1];
            S.WorkSheet.Copy(ws);
            //int trytimes = 0;
            //while (workbook.Sheets.Count >2 && trytimes++<10)
            string sheetname = "";
            foreach (Worksheet w in workbook.Worksheets)
                sheetname += w.Name+ "\t";
            sheetname += "========\r\n";
            for (int i = 1; i < 4; i++)
            {
                Excel.Worksheet w = workbook.Sheets["Sheet" + i];
                sheetname += w.Name + "\t";
                w.Delete();
            }
            ws = workbook.Worksheets[1];
            ws.Name = S.Name;
            return ws;
        }
        public Worksheet CopySheetRows(ExcelSheet S, List<int> Duplication)
        {
            Worksheet ws = (Worksheet)workbook.Worksheets.Add();
            ws.Name = S.Name;
            ExcelSheet WS = new ExcelSheet(ws);
            CopyRow(S, WS, Duplication);
            return ws;
        }
        public Worksheet CopySheetRowsAndDeleteOther(ExcelSheet S, List<int> Duplication)
        {
            Worksheet ws = (Worksheet)workbook.Worksheets.Add();
            ws.Name = S.Name;
            //while (workbook.Sheets.Count > 2)
            //int trytimes = 0;
            //while (workbook.Sheets.Count > 2 && trytimes++ < 10)
            for (int i = 1; i < 4; i++)
                ((Excel.Worksheet)workbook.Sheets["Sheet" + i]).Delete();
            ((Excel.Worksheet)workbook.Sheets[2]).Delete();
            ExcelSheet WS = new ExcelSheet(ws);
            CopyRow(S, WS, Duplication);
            return ws;
        }
        private static void CopyRow(ExcelSheet S, ExcelSheet D, List<int> Duplication)
        {
            int newrow = 2;
            D.CopyRow(1, S, 1);
            foreach (int row in Duplication)
            {
                D.CopyRow(newrow, S, row);
                newrow++;
            }
        }
        public void SaveAs(string Filename)
        {
            workbook.SaveAs(Filename);
        }
       
        public List<string> SheetNames
        {
            get
            {               
                return _sheetnames;
            }
        }
        public static Application excel;
        private List<string> _sheetnames;
        private string FileName;
        private Workbook workbook;

    }

    public class ExcelSheet
    {
        public ExcelSheet(Worksheet worksheet)
        {
            this.worksheet = worksheet;
            range1 = worksheet.Range["A1"];
            RowCount = CountRows(worksheet);
            ColCount = CountCols(worksheet);
            _Names = null;
            _Titles = null;
        }
        
        public void CopyRow(int newrow, ExcelSheet S, int row)
        {
            //range1.Copy(
            for (int col = 1; col < 100; col++)
                if( S.CellValue(row, col)!="")
                ((Range)range1[newrow, col]).Value = S.CellValue(row, col);
        }
        private int CountRows(Worksheet worksheet)
        {
            for (int row = 1; row < worksheet.Rows.Count; row++)
            {
               if (((Range)range1[row, 1]).Text == "")
                {
                    return row;
                }
            }
            return 1;
        }
        private int CountCols(Worksheet worksheet)
        {
            for (int col = 2; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1,col]).Text == "" || ((Range)range1[1,col]).Text=="核对比较")
                {
                    return col;
                }
            }
            return 1;
        }
        public string CellValue(int row, int col)
        {
            return ((Range)range1[row, col]).Text;
        }
        public string Name { get { return worksheet.Name; } }
        public Worksheet WorkSheet { get { return worksheet; } }
        public  List<Cells> Names
        {
            get
            {
                if (_Names == null)
                {
                    _Names = new List<Cells>();
                    for (int row = 2; row < RowCount ; row++)
                    {
                        String Text = ((Range)range1[row, 2]).Text;
                        _Names.Add(new Cells(row,2, Text));  
                    }
                }
                return _Names;
            }
        }
        public  List<Cells> Titles
        {
            get
            {
                if (_Titles == null)
                {
                    _Titles = new List<Cells>();
                    for (int col = 1; col < ColCount; col++)
                    {
                        String Text = ((Range)range1[1, col]).Text;
                        _Titles.Add(new Cells(1, col, Text));
                    }
                }
                return _Titles;
            }
        }
        public int RowCount { get; set; }
        public int ColCount { get; set; }

        public void FormatExcelRange(List<System.Drawing.Point> SP,Color c,string tagname)
        {
            foreach (System.Drawing.Point p in SP)
            {
                FormatExcelRange((Excel.Range)range1[p.X, p.Y], c);
                ((Excel.Range)range1[p.X, ColCount]).Value =tagname;
            }
        }       
        private static void FormatExcelRange(Excel.Range rng,Color c)
        {
            rng.Interior.Color = c;
        }
        internal void ClearBackColor()
        {
            Range r = range1.Resize[RowCount, ColCount];
            //r.Interior.Color = Color.White;
            r.Interior.ColorIndex = -4142;
        }
        private List<Cells> _Names;
        private List<Cells> _Titles;
        private Excel.Range range1;
        private Worksheet worksheet;



        internal bool CanCheckSingleSheet(ExcelSheetTable S, ExcelSheetTable L)
        {
            throw new NotImplementedException();
        }
    }

    public class ExcelSingleSheet
    {
        public ExcelSingleSheet(Worksheet worksheet)
        {
            this.worksheet = worksheet;
            range1 = worksheet.Range["A1"];
            RowCount = CountRows(worksheet);
            ColCount = CountCols(worksheet);
            _Names = null;
            _Titles = null;
            Msg = "";
        }

        private int CountRows(Worksheet worksheet)
        {
            for (int row = 1; row < worksheet.Rows.Count; row++)
            {
                if (((Range)range1[row, 1]).Text == "")
                {
                    return row;
                }
            }
            return 1;
        }
        private int CountCols(Worksheet worksheet)
        {
            for (int col = 2; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "核对比较")
                {
                    return col;
                }
            }
            return 1;
        }
        public string CellValue(int row, int col)
        {
            return ((Range)range1[row, col]).Text;
        }
        public string Name { get { return worksheet.Name; } }
        public Worksheet WorkSheet { get { return worksheet; } }
        public List<Cells> Names
        {
            get
            {
                if (_Names == null)
                {
                    _Names = new List<Cells>();
                    for (int row = 2; row < RowCount; row++)
                    {
                        String Text = ((Range)range1[row, 2]).Text;
                        _Names.Add(new Cells(row, 2, Text));
                    }
                }
                return _Names;
            }
        }
        public List<Cells> Titles
        {
            get
            {
                if (_Titles == null)
                {
                    _Titles = new List<Cells>();
                    for (int col = 1; col < ColCount; col++)
                    {
                        String Text = ((Range)range1[1, col]).Text;
                        _Titles.Add(new Cells(1, col, Text));
                    }
                }
                return _Titles;
            }
        }
        public int RowCount { get; set; }
        public int ColCount { get; set; }

        public void FormatExcelRange(List<System.Drawing.Point> SP, Color c, string tagname,int TagCol = -1)
        {
            foreach (System.Drawing.Point p in SP)
            {
                FormatExcelRange((Excel.Range)range1[p.X, p.Y], c);
                if(TagCol==-1)
                    ((Excel.Range)range1[p.X, ColCount]).Value = tagname;
                else
                    ((Excel.Range)range1[p.X, TagCol ]).Value = tagname;

            }
        }

        internal void SortExcelRange(System.Drawing.Point p, int tagname, int TagCol = -1)
        {
            if (TagCol == -1)
                ((Excel.Range)range1[p.X, ColCount]).Value = tagname;
            else
                ((Excel.Range)range1[p.X, TagCol]).Value = tagname;
        }
        internal void SetExcelRangeTag(int X, string tagname, int TagCol = -1)
        {
            if (TagCol == -1)
                ((Excel.Range)range1[X, ColCount]).Value = tagname;
            else
                ((Excel.Range)range1[X, TagCol]).Value = tagname;
        }
        private static void FormatExcelRange(Excel.Range rng, Color c)
        {
            rng.Interior.Color = c;
        }
        internal void ClearBackColor()
        {
            Range r = range1.Resize[RowCount, ColCount];
            //r.Interior.Color = Color.White;
            r.Interior.ColorIndex = -4142;
        }
        private List<Cells> _Names;
        private List<Cells> _Titles;
        private Excel.Range range1;
        private Worksheet worksheet;
        public string Msg;

        internal bool CanCheckSingleSheet(ref ExcelSheetTable S,ref ExcelSheetTable L)
        {
           //List<string> strTitles = Titles.Select(r => r.Text).ToList();
           //if (!strTitles.Contains("单表比较") ||  strTitles.IndexOf("单表比较")==1) return false;
           //int colP = strTitles.IndexOf("单表比较")+1, colN = strTitles.Count+1;
           //S = new ExcelSheetTable(worksheet, 1, colP);
           //L = new ExcelSheetTable(worksheet, colP + 1, colN);
            int skipCol = 0, SBCol = 0, SECol = 0, LBCol = 0, LECol = 0;
            ComputeLSBECol(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);

            if (SECol > SBCol && SBCol-1 > 0)
            {
                S = new ExcelSheetTable(worksheet, SBCol-1, SECol);
            }
            if (LECol > LBCol && LBCol-1 > SECol)
            {
                L = new ExcelSheetTable(worksheet, LBCol-1, LECol);
            }
           return true;
        }
        internal bool CanCheckSingleSheet(ref System.Data.DataTable S, ref System.Data.DataTable L)
        {
            S = null; L = null;
            int skipCol = 0, SBCol = 0, SECol = 0, LBCol = 0, LECol = 0;
            ComputeLSBECol(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);

            if (SECol > SBCol && SBCol > 0)
            {
                S = CreateDataTable(SBCol, SECol);
                AddDataToTable(S, SBCol, SECol);
            }
            if (LECol > LBCol && LBCol > SECol)
            {
                L = CreateDataTable(LBCol, LECol);
                AddDataToTable(L, LBCol, LECol);
            }
            if (S == null )// || (S!=null && L == null)
                return false;
            return true;
        }

        private void ComputeLSBECol(ref int skipCol, ref int SBCol, ref int SECol, ref int LBCol, ref int LECol)
        {
            ////////// Skip Empty ColTitle
            skipCol = SkipCols(1);
            ////////// Compute SBCol  SECol  //没有显示第一行
            SBCol = 2 + skipCol;
            for (int col = SBCol; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "比较结果")
                {
                    SECol = col;
                    break;
                }
            }

            ////////// Skip 
            LBCol = SECol + SkipCols(SECol, false);
            LBCol += SkipCols(LBCol);

            LBCol += 1;
            for (int col = LBCol; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "比较结果")
                {
                    LECol = col;
                    break;
                }
            }
        }
        private void AddDataToTable(System.Data.DataTable S, int SBCol, int SECol)
        {
            if(S!=null)
            for (int row = 2; row < 7 && row < RowCount; row++)
            {
                DataRow dr = S.NewRow();
                for (int col = SBCol; col < SECol; col++)
                {
                    dr[col - SBCol] = ((Range)range1[row, col]).Text;
                }
                S.Rows.Add(dr);
            }
        }
        private System.Data.DataTable CreateDataTable(int SBCol, int SECol)
        {
            System.Data.DataTable  S = new System.Data.DataTable();
            List<String> Titles = CollectTitles(SBCol, SECol);
            List<String> check = new List<string>();
            foreach (string s in Titles)
            {
                if (!check.Contains(s))
                {
                    check.Add(s);
                    DataColumn dc = new DataColumn(s);
                    dc.DataType = typeof(string);
                    S.Columns.Add(dc);
                }
                else
                {
                    Msg = "表头存在重复";
                    return null;                    
                }
            }
            S.TableName = SBCol + "-" + SECol;
            return S;
        }
        private List<String> CollectTitles(int SBCol, int SECol)
        {
            List<String> _Titles = new List<String>();
            for (int col = SBCol; col < SECol; col++)
            {
                String Text = ((Range)range1[1, col]).Text;
                _Titles.Add(Text);
            }
            return _Titles;
        }
        private int SkipCols(int Bcol,  bool SkipEmpty = true)
        {
            if (SkipEmpty)
            {
                for (int col = Bcol; col < worksheet.Columns.Count; col++)
                    if (((Range)range1[1, col]).Text != "")
                        return  col - Bcol;
            }
            else
            {
                for (int col = Bcol; col < worksheet.Columns.Count; col++)
                    if (((Range)range1[1, col]).Text == "")
                        return  col - Bcol;
            }
            return 0;
        }
    }
    public class ExcelSheetTable
    {
        public ExcelSheetTable(Worksheet worksheet, int BCol, int ECol)
        {
            // TODO: Complete member initialization           
            this.worksheet = worksheet;
            range1 = worksheet.Range["A1"];
            this.BCol = BCol;
            this.ECol = ECol;
            RowCount = CountRows(worksheet);
            ColCount = ECol - BCol;
            _Names = null;
            _Titles = null;
        }
        private int CountRows(Worksheet worksheet)
        {
            for (int row = 1; row < worksheet.Rows.Count; row++)
            {
                if (((Range)range1[row, BCol]).Text == "")
                {
                    return row;
                }
            }
            return 1;
        }
        internal void ClearBackColor()
        {
            Range r = range1.Resize[RowCount, ColCount];
            r = r.get_Offset(BCol-1);
            //r.Interior.Color = Color.White;
            r.Interior.ColorIndex = -4142;
        }
        public string CellValue(int row, int col)
        {
            return ((Range)range1[row, col]).Text;
        }
        public string Name { get { return worksheet.Name; } }
        public Worksheet WorkSheet { get { return worksheet; } }
        public List<Cells> Names
        {
            get
            {
                if (_Names == null)
                {
                    _Names = new List<Cells>();
                    for (int row = 2; row < RowCount; row++)
                    {
                        String Text = ((Range)range1[row, BCol+1]).Text;
                        _Names.Add(new Cells(row, BCol+1, Text));
                    }
                }
                return _Names;
            }
        }
        public List<Cells> Titles
        {
            get
            {
                if (_Titles == null)
                {
                    _Titles = new List<Cells>();
                    for (int col = BCol; col < ECol; col++)
                    {
                        String Text = ((Range)range1[1, col]).Text;
                        _Titles.Add(new Cells(1, col, Text));
                    }
                }
                return _Titles;
            }
        }
        public int RowCount { get; set; }
        public int ColCount { get; set; }

        private List<Cells> _Names;
        private List<Cells> _Titles;
        private Excel.Range range1;
        private Worksheet worksheet;
        private int ECol;
        private int BCol;
        public int TagCol { get { return ECol; } }


        public int NameCol { get { return BCol + 1; } }
    }


    public class ExcelSingleRedSheet
    {
        public ExcelSingleRedSheet(Worksheet worksheet)
        {
            this.worksheet = worksheet;
            range1 = worksheet.Range["A1"];
            Msg = "";
        }
        public bool CanCheckSingleSheet(ref ExcelRedSheetTable S, ref ExcelRedSheetTable L,ref string cmptablename, bool bmultiskip = false)
        {
            int skipCol = 0, SBCol = 0, SECol = 0, LBCol = 0, LECol = 0;
            if (!bmultiskip)
            {
                ComputeLSBECol(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);
            }
            else
            {
                cmptablename =
                ComputeLSBECol_Skip(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);
            }

            if (SECol > SBCol && SBCol > 0)
            {
                List<Cells> Titles = CollectRedTitles(SBCol, SECol);
                S = new ExcelRedSheetTable(worksheet, Titles, SBCol, SECol);
            }
            if (LECol > LBCol && LBCol - 1 > SECol)
            {
                List<Cells> Titles = CollectRedTitles(LBCol, LECol);
                L = new ExcelRedSheetTable(worksheet, Titles, LBCol, LECol);
            }
            if (S == null || L == null) return false;


            return true;
        }
        public bool CanCheckSingleSheet(ref System.Data.DataTable S, ref System.Data.DataTable L, bool bmultiskip = false )
        {
            S = null; L = null;
            int skipCol = 0, SBCol = 0, SECol = 0, LBCol = 0, LECol = 0;
            if (!bmultiskip)
            {
                ComputeLSBECol(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);
            }
            else
            {
                ComputeLSBECol_Skip(ref skipCol, ref SBCol, ref SECol, ref LBCol, ref LECol);
            }



            if (SECol > SBCol && SBCol > 0)
            {
                List<Cells> Titles =  CollectRedTitles(SBCol, SECol);
                if (CheckDuplicateValue(Titles.Select(r => r.Text).ToList()))
                {
                    S = CreateDataTable(Titles);
                    AddDataToTable(S, Titles);
                }
            }
            if (LECol > LBCol && LBCol > SECol)
            {
                List<Cells> Titles = CollectRedTitles(LBCol, LECol);
                L = CreateDataTable(Titles);
                AddDataToTable(L, Titles);
            }
            if (S == null)// || (S!=null && L == null)
                return false;
            return true;
        }

        public void FormatExcelRange(List<System.Drawing.Point> SP, Color c, string tagname, int TagCol)
        {
            foreach (System.Drawing.Point p in SP)
            {
                FormatExcelRange((Excel.Range)range1[p.X, p.Y], c);
                ((Excel.Range)range1[p.X, TagCol]).Value = tagname;
            }
        }
        public void SortExcelRange(System.Drawing.Point p, int tagname, int TagCol)
        {
                ((Excel.Range)range1[p.X, TagCol]).Value = tagname;
        }
        public void SortExcelRange(System.Drawing.Point p, string tagname, int TagCol)
        {
                ((Excel.Range)range1[p.X, TagCol]).Value = tagname;
        }
        public void SetExcelRangeTag(int X, string tagname, int TagCol)
        {
                ((Excel.Range)range1[X, TagCol]).Value = tagname;
        }

        private void ClearBackColor()
        {
            Range r = range1.Resize[2000, 100];
            r.Interior.ColorIndex = -4142;
        }
        private static void FormatExcelRange(Excel.Range rng, Color c)
        {
            rng.Interior.Color = c;
        }
        private void AddDataToTable(System.Data.DataTable S, List<Cells> Titles)
        {
            if (S != null)
                for (int row = 2; row < 7; row++)
                {
                    DataRow dr = S.NewRow();
                    foreach (Cells s in Titles)
                    {
                        if (((Range)range1[row, s.Col]).Text == "")
                            return;
                        dr[s.Text] = ((Range)range1[row, s.Col]).Text;

                    }
                    S.Rows.Add(dr);
                }
        }
        private System.Data.DataTable CreateDataTable(List<Cells> Titles)
        {
            System.Data.DataTable S = new System.Data.DataTable();
            foreach (Cells s in Titles)
            {
                DataColumn dc = new DataColumn(s.Text);
                dc.DataType = typeof(string);
                S.Columns.Add(dc);
            }
            return S;
        }
        private bool CheckDuplicateValue(List<string> list)
        {
            List<String> check = new List<string>();
            foreach (string s in list)
            {
                if (!check.Contains(s))
                {
                    check.Add(s);
                }
                else
                {
                    Msg = "表头存在重复";
                    return false;
                }
            }
            return true;
        }             
        private void ComputeLSBECol(ref int skipCol, ref int SBCol, ref int SECol, ref int LBCol, ref int LECol)
        {
            ////////// Compute SBCol  SECol  // 从第一列开始搜索
            SBCol = 1;
            ////////// Skip Empty ColTitle
            skipCol = SkipCols(SBCol);
            SBCol += skipCol;
            for (int col = SBCol; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "比较结果")
                {
                    SECol = col;
                    break;
                }
            }

            ////////// Skip 
            LBCol = SECol + SkipCols(SECol, false);
            LBCol += SkipCols(LBCol);

           
            for (int col = LBCol; col < worksheet.Columns.Count; col++)
            {
                if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "比较结果")
                {
                    LECol = col;
                    break;
                }
            }
        }

        private string ComputeLSBECol_Skip(ref int skipCol, ref int SBCol, ref int SECol, ref int LBCol, ref int LECol) //return comparetablename
        {
            string cmpname = "";
            ////////// Compute SBCol  SECol  // 从第一列开始搜索
            SBCol = 1;
            ////////// Skip Empty ColTitle
            skipCol = 0;/////////
            int col = 0;
            List<SubTitleTable> sts = new List<SubTitleTable>();
            SubTitleTable S = null, L = null;
            while (true)
            {
                SBCol += SkipCols(SBCol);
                if (SBCol > 100) break;
                for (col = SBCol; col < worksheet.Columns.Count; col++)
                {
                    if (((Range)range1[1, col]).Text == "" || ((Range)range1[1, col]).Text == "比较结果")
                    {
                        sts.Add(  new SubTitleTable(SBCol, col, range1) );
                        break;
                    }
                }
                SBCol = col + SkipCols(col,false)+1;
            }
            foreach (SubTitleTable s in sts)
            {
                if (s.UnSkipTitle())
                {
                    if (S == null)
                        S = s;
                    else
                    {
                        L = s;
                        break;
                    }
                }
            }

            if (S != null)
            {
                SBCol = S.BCol; SECol = S.ECol; cmpname = S.TableName;
            }
            if (L != null)
            {
                LBCol = L.BCol; LECol = L.ECol; cmpname +="-"+ L.TableName;
            }
            return cmpname+"|";

        }
        private List<Cells> CollectRedTitles(int SBCol, int SECol)
        {
            List<Cells> _Titles = new List<Cells>();
            for (int col = SBCol; col < SECol; col++)
            {
                Range r = (Range)range1[1, col];
                if(r.Interior.ColorIndex==3)
                {
                    _Titles.Add(new Cells(1, col, r.Text));
                }               
            }
            return _Titles;
        }      
        private int SkipCols(int Bcol, bool SkipEmpty = true)
        {
            if (SkipEmpty)
            {
                for (int col = Bcol; col < worksheet.Columns.Count; col++)
                    if (((Range)range1[1, col]).Text != "")
                        return col - Bcol;
                return worksheet.Columns.Count - Bcol - 1;
            }
            else
            {
                for (int col = Bcol; col < worksheet.Columns.Count; col++)
                    if (((Range)range1[1, col]).Text == "")
                        return col - Bcol;
                return worksheet.Columns.Count - Bcol - 1;
            }
        }
    
        private Excel.Range range1;
        private Worksheet worksheet;
        public string Msg;
    }
    public class ExcelRedSheetTable
    {
        public ExcelRedSheetTable(Worksheet worksheet, List<Cells> Titles,int BCol, int TagCol)
        {      
            this.worksheet = worksheet;
            range1 = worksheet.Range["A1"];
            _Titles = Titles;
            _TagCol = TagCol;
            _NameCol = Titles[0].Col;
            _BCol = BCol;
            RowCount = CountRows(worksheet);            
            _Names = null;
        }
        private int CountRows(Worksheet worksheet)
        {
            for (int row = 1; row < worksheet.Rows.Count; row++)
            {
                if (((Range)range1[row, _NameCol]).Text == "")
                {
                    return row;
                }
            }
            return 1;
        }
        internal void ClearBackColor()
        {
            Range r = range1[2, _BCol];
            r = r.Resize[RowCount, TagCol - _BCol +1];               
            r.Interior.ColorIndex = -4142;
        }
        public string CellValue(int row, int col)
        {
            return ((Range)range1[row, col]).Text;
        }
        public string Name { get { return worksheet.Name; } }
        public Worksheet WorkSheet { get { return worksheet; } }
        public List<Cells> Names
        {
            get
            {
                if (_Names == null)
                {
                    _Names = new List<Cells>();
                    for (int row = 2; row < RowCount; row++)
                    {
                        String Text = ((Range)range1[row, _NameCol]).Text;
                        _Names.Add(new Cells(row, _NameCol, Text));
                    }
                }
                return _Names;
            }
        }
        public List<Cells> Titles
        {
            get
            {
                return _Titles;
            }
        }
        public int RowCount { get; set; }        

        private List<Cells> _Names;
        private List<Cells> _Titles;
        private Excel.Range range1;
        private Worksheet worksheet;
     
        private int _NameCol;
        private int _TagCol;
        private int _BCol;
        public int TagCol { get { return _TagCol; } }
        public int NameCol { get { return _NameCol ; } }
    }
    public  class Cells
    {
        public Cells(int Row, int Col, string Text)
        {
            this.Row = Row;
            this.Col = Col;
            this.Text = Text;
        }
        public int Row { get; set; }
        public int Col { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return "单元格(" + Col + "," + Row + ")=" + Text;
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            m_cfg = new CConfig();
            ExcelBook.excel = new Excel.Application();
            SP = new List<Point>();
            LP = new List<Point>();
            LDicDuplication = new Dictionary<string, List<int>>();
            SDicDuplication = new Dictionary<string, List<int>>();
            SSpecial = new List<Point>();
            LSpecial = new List<Point>();
            _Same = new List<List<Cells>>();
            checkBoxCopyDuplicationL.Checked = true;
            checkBoxCopyDuplicationS.Checked = true;
            checkBoxDifL.Checked = true;
            checkBoxDifS.Checked = true;
            checkBoxTagL.Checked = true;
            checkBoxTagS.Checked = true;
            checkBoxClearBackColor.Checked = true;
            checkBoxSkipDuplicate.Checked = true;
            InitColor();
            checkBoxCompareSingle.Checked = true;
            checkBoxRedTitle.Checked = true;
            checkBoxMultiBlackSkip.Checked = false;
        }
        ~Form1()
        {
            if (ExcelBook.excel != null)
            {
                ExcelBook.excel.Workbooks.Close();
                ExcelBook.excel = null;
            }
        }
        public void ClearData()
        {
            LP.Clear();
            SP.Clear();
            LDicDuplication.Clear();
            SDicDuplication.Clear();
            LSpecial.Clear();
            SSpecial.Clear();
            _Same.Clear();
        }
        private void InitColor()
        {
            _colorDup = textBoxDuplication.BackColor;
            _colorSpe = textBoxSpecial.BackColor;
            _colorDif = textBoxDif.BackColor;
        }
        private void buttonBrowXlsNameS_Click(object sender, EventArgs e)
        {
            string f = OpenXmlFile( "打开小表文件" );
            if (f != null)
            {
                m_cfg.XlsName = f;
                textBoxExcelFileName.Text = f;
                ClearData();

                if (!m_cfg.XlsName.EndsWith(".xls"))
                {
                    MessageBox.Show("文件输入不对");
                    return;
                }
                ExcelBook E = new ExcelBook(m_cfg.XlsName);
                try
                {
                   
                        comboBoxLSheetName.Items.Clear();
                        comboBoxSSheetName.Items.Clear();
                        comboBoxLSheetName.Items.AddRange(E.SheetNames.ToArray());
                        comboBoxSSheetName.Items.AddRange(E.SheetNames.ToArray());                   
                        comboBoxSSheetName.SelectedIndex = 0;
                        if (comboBoxLSheetName.Items.Count > 1)
                            comboBoxLSheetName.SelectedIndex = 1;
                }
                finally
                {
                    E = null;
                    ExcelBook.excel.Workbooks.Close();
                }
            }
        }        
        private void buttonCompare_Click(object sender, EventArgs e)
        {
            textBoxOut.Text = "";
            if (!checkBoxCompareSingle.Checked  && (comboBoxLSheetName.SelectedIndex==-1 || comboBoxSSheetName.SelectedIndex==-1
                || comboBoxSSheetName.SelectedIndex==comboBoxLSheetName.SelectedIndex )
                || checkBoxCompareSingle.Checked  && comboBoxSSheetName.SelectedIndex==-1 )
            {
                MessageBox.Show("没有打开文件，或者没有选择表名，或者两张表的表名相同");
                return;
            }
            ((Button)sender).Enabled = false;
            if (checkBoxCompareSingle.Checked)
            {
                if(checkBoxRedTitle.Checked)
                CompareRedSingleSheetClick();
                else
                CompareSingleSheetClick();
            }
            else
            {
                CompareMultiSheetClick();
            }
            ((Button)sender).Enabled = true;
        }
        private void buttonOutTxt_Click(object sender, EventArgs e)
        {
            if (textBoxOut.Text != "")
            {//输出
                SaveFileDialog dlg = new SaveFileDialog();   //实例化一个SaveFileDialog保存文件对话框
                dlg.Filter = "Txt files (*.txt)|*.txt";
                dlg.FilterIndex = 0;
                dlg.FileName = m_cfg.ShortName.Replace(".xls", "-文字比对结果.txt");
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(dlg.FileName, textBoxOut.Text);
                }
            }
        }
        
        private void CompareMultiSheetClick()
        {
            ExcelBook E = new ExcelBook(m_cfg.XlsName);
            ExcelSheet S = new ExcelSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
            ExcelSheet L = new ExcelSheet(E.ExcelSheet(comboBoxLSheetName.SelectedItem.ToString()));

            try
            {
                if (checkBoxClearBackColor.Checked)
                {
                    S.ClearBackColor();
                    L.ClearBackColor();
                }
                Compare(S, L);
                if (checkBoxTagL.Checked || checkBoxTagS.Checked)
                    if (LSpecial.Count > 0 || SSpecial.Count > 0)
                    {
                        ExportSpecialExcel(E);
                    }
                if (checkBoxCopyDuplicationL.Checked || checkBoxCopyDuplicationS.Checked)
                {
                    if (LDicDuplication.Count > 0 || SDicDuplication.Count > 0)
                        ExportDuplicationExcel(E);
                }
                if (checkBoxDifL.Checked || checkBoxDifS.Checked)
                    if (LP.Count > 0 || SP.Count > 0)
                        ExportDifferentExcel(E);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序有点小问题\r\n" + ex.Message);
            }
            finally
            {
                S = null;
                L = null;
                ExcelBook.excel.ActiveWorkbook.Save();
                ExcelBook.excel.Workbooks.Close();
            }
        }
        private void Compare(ExcelSheet S, ExcelSheet L)
        {
            if (!CompareListString(S, L))
            {
                textBoxOut.Text = "两张表的表头不一致，无法比较";
                return;
            }
            ClearData();
            string str = "", strL = "", strS = "", strLS = "";
            StringBuilder strb = new StringBuilder();
            StringBuilder strT = new StringBuilder();
            Dictionary<string, Cells> LDic = ListToDictionary(L.Names, ref strL);
            Dictionary<string, Cells> SDic = ListToDictionary(S.Names, ref strS);

            Dictionary<string, Cells> LDicSpecial = new Dictionary<string,Cells>();
            Dictionary<string, Cells> SDicSpecial = new Dictionary<string,Cells>();
            /////////////////////////////////////////////////////  Duplication
            if (checkBoxCopyDuplicationL.Checked)
                LDicDuplication = FindDuplication(L.Names);
            if (checkBoxCopyDuplicationS.Checked)
                SDicDuplication = FindDuplication(S.Names);
            if (strS != "")
                str += "\r\n============小表中以下值存在重复============\r\n" + strS;
            if (strL != "")
                str += "\r\n============大表中以下值存在重复============\r\n" + strL;

            foreach (Cells c in S.Names)
                if (!LDic.ContainsKey(c.Text))
                {
                    strLS += c + "\t";
                }

            ////////////////////////////////////////////////  Special 
            if (checkBoxTagS.Checked || checkBoxSkipDuplicate.Checked )  //排除重复
                foreach (Cells c in S.Names)
                    if (!LDic.ContainsKey(c.Text) && !SDicDuplication.ContainsKey(c.Text) ){
                        SSpecial.Add(new Point(c.Row, c.Col));
                        if(!SDicSpecial.ContainsKey(c.Text))
                            SDicSpecial[c.Text] = c;
                    }
            if (checkBoxTagL.Checked || checkBoxSkipDuplicate.Checked)
                foreach (Cells c in L.Names)
                    if (!SDic.ContainsKey(c.Text) && !LDicDuplication.ContainsKey(c.Text) )
                    {
                        LSpecial.Add(new Point(c.Row, c.Col));
                        if (!LDicSpecial.ContainsKey(c.Text))
                            LDicSpecial[c.Text] = c;
                    }
            if (strLS != "")
                strLS = "\r\n============小表中的以下值在大表中不存在============\r\n" + strLS;
            if (str != "")
            {
                textBoxOut.Text = str+strLS;
                if (!checkBoxSkipDuplicate.Checked)
                {
                    MessageBox.Show("存在重复值，没有进一步比对，可选择忽略之后，继续比对");
                    return;
                }
            }
            ///////////////////////// DIF
            SP = new List<Point>();
            LP = new List<Point>();

            foreach (Cells c1 in S.Names)
            {
                if(str!="" && 
                    ( SDicDuplication.ContainsKey(c1.Text) 
                    || SDicSpecial.ContainsKey(c1.Text)))
                 continue;               
                Cells c2 = LDic[c1.Text];
                for (int col = 3; col < S.ColCount; col++)
                {
                    if (S.CellValue(c1.Row, col) != L.CellValue(c2.Row, col))
                    {
                        SP.Add(new Point(c1.Row, col));
                        LP.Add(new Point(c2.Row, col));
                        strb.AppendLine(c1.Text + " " + PointTostring(new Point(c1.Row, col)) + "=" + S.CellValue(c1.Row, col) + "\t"
                                                      + PointTostring(new Point(c2.Row, col)) + "=" + L.CellValue(c2.Row, col));
                    }
                }
            }
            string str1 = strb.ToString();
            if (str1 == "")
            {
                MessageBox.Show("除忽略项外，待比较项目完全相同");
            }else
                str1 = "\r\n============以下单元格内容两表不一致 （行号，列号）============\r\n" + str1;
            textBoxOut.Text = str+strLS+str1;
           
        }
        private void CompareRedSingleSheetClick()
        {
            DataTable DL = (DataTable)dgvL.DataSource;
            DataTable DS = (DataTable)dgvS.DataSource;
            if (DL == null || DS == null || DL.Columns.Count != DS.Columns.Count) return;
            for (int i = 0; i < DL.Columns.Count; i++)
                if (DL.Columns[i].ColumnName != DS.Columns[i].ColumnName)
                {
                    MessageBox.Show("两边待比较的表头不一致");
                    return;
                }

            ExcelBook E = new ExcelBook(m_cfg.XlsName);
            ExcelSingleRedSheet ES = new ExcelSingleRedSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
            ExcelRedSheetTable S = null, L = null;

            string cmptablename = "";
            try
            {
                if (!ES.CanCheckSingleSheet(ref S, ref L,ref cmptablename, checkBoxMultiBlackSkip.Checked))
                {
                    MessageBox.Show("S=" + (S != null) + " " + "L=" + (L != null) + " SL两表之一可能未完成");
                    return;
                }
                if (checkBoxClearBackColor.Checked)
                {
                    S.ClearBackColor();
                    L.ClearBackColor();
                }
                CompareTable(S, L);
                if (checkBoxTagL.Checked || checkBoxTagS.Checked)
                    if (LSpecial.Count > 0 || SSpecial.Count > 0)
                    {
                        ES.FormatExcelRange(SSpecial, _colorSpe, "特有", S.TagCol);
                        ES.FormatExcelRange(LSpecial, _colorSpe, "特有", L.TagCol);
                    }
                if (checkBoxCopyDuplicationL.Checked || checkBoxCopyDuplicationS.Checked)
                {
                    if (LDicDuplication.Count > 0 || SDicDuplication.Count > 0)
                    {
                        List<int> LDuplication = new List<int>();
                        List<int> SDuplication = new List<int>();
                        foreach (List<int> l in LDicDuplication.Values)
                            LDuplication.AddRange(l);
                        foreach (List<int> l in SDicDuplication.Values)
                            SDuplication.AddRange(l);

                        List<Point> SD = SDuplication.Select(r => new Point(r, S.NameCol)).ToList();
                        List<Point> LD = LDuplication.Select(r => new Point(r, L.NameCol)).ToList();
                        ES.FormatExcelRange(SD, _colorDup, "重复", S.TagCol);
                        ES.FormatExcelRange(LD, _colorDup, "重复", L.TagCol);
                    }
                }
                if (checkBoxDifL.Checked || checkBoxDifS.Checked)
                    if (LP.Count > 0 || SP.Count > 0)
                    {
                        ES.FormatExcelRange(SP, _colorDif, "不同", S.TagCol + 1);
                        ES.FormatExcelRange(LP, _colorDif, "不同", L.TagCol + 1);
                    }

                for (int i = 0; i < _Same.Count; i++)
                {
                    if (_Same[i].Count == 2)
                    {
                        Cells c = _Same[i][0];
                        ES.SortExcelRange(new Point(c.Row, c.Col),cmptablename+ i, S.TagCol);
                        c = _Same[i][1];
                        ES.SortExcelRange(new Point(c.Row, c.Col),cmptablename+ i, L.TagCol);
                    }
                }
                /////
                ES.SetExcelRangeTag(1,cmptablename+  "比较结果", S.TagCol);
                ES.SetExcelRangeTag(1,cmptablename+  "比较结果", L.TagCol);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序有点小问题\r\n" + ex.Message);
            }
            finally
            {
                S = null;
                L = null;
                ExcelBook.excel.ActiveWorkbook.Save();
                ExcelBook.excel.Workbooks.Close();
            }
        }
        private void CompareTable(ExcelRedSheetTable S, ExcelRedSheetTable L)
        {
            if (!CompareListString(S.Titles.Select(r => r.Text).ToList(), L.Titles.Select(r => r.Text).ToList()))
            {
                textBoxOut.Text = "两张表的表头不一致，无法比较";
                return;
            }
            ClearData();
            string str = "", strL = "", strS = "", strLS = "";
            StringBuilder strb = new StringBuilder();
            StringBuilder strT = new StringBuilder();
            Dictionary<string, Cells> LDic = ListToDictionary(L.Names, ref strL);
            Dictionary<string, Cells> SDic = ListToDictionary(S.Names, ref strS);

            Dictionary<string, Cells> LDicSpecial = new Dictionary<string, Cells>();
            Dictionary<string, Cells> SDicSpecial = new Dictionary<string, Cells>();
            /////////////////////////////////////////////////////  Duplication
            if (checkBoxCopyDuplicationL.Checked)
                LDicDuplication = FindDuplication(L.Names);
            if (checkBoxCopyDuplicationS.Checked)
                SDicDuplication = FindDuplication(S.Names);
            if (strS != "")
                str += "\r\n============小表中以下值存在重复============\r\n" + strS;
            if (strL != "")
                str += "\r\n============大表中以下值存在重复============\r\n" + strL;

            foreach (Cells c in S.Names)
                if (!LDic.ContainsKey(c.Text))
                {
                    strLS += c + "\t";
                }

            ////////////////////////////////////////////////  Special 
            if (checkBoxTagS.Checked || checkBoxSkipDuplicate.Checked)  //排除重复
                foreach (Cells c in S.Names)
                    if (!LDic.ContainsKey(c.Text) && !SDicDuplication.ContainsKey(c.Text))
                    {
                        SSpecial.Add(new Point(c.Row, c.Col));
                        if (!SDicSpecial.ContainsKey(c.Text))
                            SDicSpecial[c.Text] = c;
                    }
            if (checkBoxTagL.Checked || checkBoxSkipDuplicate.Checked)
                foreach (Cells c in L.Names)
                    if (!SDic.ContainsKey(c.Text) && !LDicDuplication.ContainsKey(c.Text))
                    {
                        LSpecial.Add(new Point(c.Row, c.Col));
                        if (!LDicSpecial.ContainsKey(c.Text))
                            LDicSpecial[c.Text] = c;
                    }
            if (strLS != "")
                strLS = "\r\n============小表中的以下值在大表中不存在============\r\n" + strLS;
            if (str != "")
            {
                textBoxOut.Text = str + strLS;
                if (!checkBoxSkipDuplicate.Checked)
                {
                    MessageBox.Show("存在重复值，没有进一步比对，可选择忽略之后，继续比对");
                    return;
                }
            }

            if (SDicDuplication.Count > 0 || SDicSpecial.Count > 0)
                if (str == "")
                    str = " ";
            ///////////////////////// DIF
            SP = new List<Point>();
            LP = new List<Point>();
            _Same = new List<List<Cells>>();
         
            foreach (Cells c1 in S.Names)
            {
                if (str != "" &&
                    (SDicDuplication.ContainsKey(c1.Text)
                    || SDicSpecial.ContainsKey(c1.Text)))
                    continue;
                Cells c2 = LDic[c1.Text];
                for (int i = 1; i < S.Titles.Count ; i++)
                {
                    if (S.CellValue(c1.Row, S.Titles[i].Col) != L.CellValue(c2.Row, L.Titles[i].Col))
                    {
                        SP.Add(new Point(c1.Row, S.Titles[i].Col));
                        LP.Add(new Point(c2.Row, L.Titles[i].Col));
                        strb.AppendLine(c1.Text + " " + PointTostring(new Point(c1.Row, S.Titles[i].Col)) + "=" + S.CellValue(c1.Row, S.Titles[i].Col) + "\t"
                                                      + PointTostring(new Point(c2.Row, L.Titles[i].Col)) + "=" + L.CellValue(c2.Row, L.Titles[i].Col));
                    }
                }
                _Same.Add(new List<Cells>() { c1, c2 });
            }
            string str1 = strb.ToString();
            if (str1 == "")
            {
                MessageBox.Show("除忽略项外，待比较项目完全相同");
            }
            else
                str1 = "\r\n============以下单元格内容两表不一致 （行号，列号）============\r\n" + str1;
            textBoxOut.Text = str + strLS + str1;

        }
        private void CompareSingleSheetClick()
        {
            DataTable DL = (DataTable)dgvL.DataSource;
            DataTable DS = (DataTable)dgvS.DataSource;            
            if(DL==null || DS == null || DL.Columns.Count != DS.Columns.Count )return ;
            for (int i = 0; i < DL.Columns.Count; i++)
                if (DL.Columns[i].ColumnName != DS.Columns[i].ColumnName)
                {
                    MessageBox.Show("两边待比较的表头不一致");
                    return;
                }
            
            ExcelBook E = new ExcelBook(m_cfg.XlsName);
            ExcelSingleSheet ES = new ExcelSingleSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
            ExcelSheetTable S = null, L = null;


            try
            {
                if (!ES.CanCheckSingleSheet(ref S, ref L)) return;

                if (checkBoxClearBackColor.Checked)
                {
                    S.ClearBackColor();
                    L.ClearBackColor();
                }
                CompareTable(S, L);
                if (checkBoxTagL.Checked || checkBoxTagS.Checked)
                    if (LSpecial.Count > 0 || SSpecial.Count > 0)
                    {
                        ES.FormatExcelRange(SSpecial, _colorSpe, "特有", S.TagCol);
                        ES.FormatExcelRange(LSpecial, _colorSpe, "特有",L.TagCol);
                    }
                if (checkBoxCopyDuplicationL.Checked || checkBoxCopyDuplicationS.Checked)
                {
                    if (LDicDuplication.Count > 0 || SDicDuplication.Count > 0)
                    {
                        List<int> LDuplication = new List<int>();
                        List<int> SDuplication = new List<int>();
                        foreach (List<int> l in LDicDuplication.Values)
                            LDuplication.AddRange(l);
                        foreach (List<int> l in SDicDuplication.Values)
                            SDuplication.AddRange(l);

                        List<Point> SD = SDuplication.Select(r => new Point(r, S.NameCol)).ToList();
                        List<Point> LD = LDuplication.Select(r => new Point(r, L.NameCol)).ToList();
                        ES.FormatExcelRange(SD, _colorDup, "重复", S.TagCol);
                        ES.FormatExcelRange(LD, _colorDup, "重复",L.TagCol);
                    }
                }
                if (checkBoxDifL.Checked || checkBoxDifS.Checked)
                    if (LP.Count > 0 || SP.Count > 0)
                    {
                        ES.FormatExcelRange(SP, _colorDif, "不同", S.TagCol+1);
                        ES.FormatExcelRange(LP, _colorDif, "不同",L.TagCol+1);
                    }

                for (int i = 0; i < _Same.Count; i++)
                {
                    if (_Same[i].Count == 2)
                    {
                        Cells c = _Same[i][0];
                        ES.SortExcelRange(new Point(c.Row, c.Col), i, S.TagCol);
                        c = _Same[i][1];
                        ES.SortExcelRange(new Point(c.Row, c.Col), i, L.TagCol);
                    }
                }
                /////
                ES.SetExcelRangeTag(1,"比较结果",S.TagCol);
                ES.SetExcelRangeTag(1,"比较结果",L.TagCol);
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序有点小问题\r\n" + ex.Message);
            }
            finally
            {
                S = null;
                L = null;
                ExcelBook.excel.ActiveWorkbook.Save();
                ExcelBook.excel.Workbooks.Close();
            }
        }
        private void CompareTable(ExcelSheetTable S, ExcelSheetTable L)
        {
            if (!CompareListString(S.Titles.Select(r => r.Text).ToList(), L.Titles.Select(r => r.Text).ToList()))
            {
                textBoxOut.Text = "两张表的表头不一致，无法比较";
                return;
            }
            ClearData();
            string str = "", strL = "", strS = "", strLS = "";
            StringBuilder strb = new StringBuilder();
            StringBuilder strT = new StringBuilder();
            Dictionary<string, Cells> LDic = ListToDictionary(L.Names, ref strL);
            Dictionary<string, Cells> SDic = ListToDictionary(S.Names, ref strS);

            Dictionary<string, Cells> LDicSpecial = new Dictionary<string, Cells>();
            Dictionary<string, Cells> SDicSpecial = new Dictionary<string, Cells>();
            /////////////////////////////////////////////////////  Duplication
            if (checkBoxCopyDuplicationL.Checked)
                LDicDuplication = FindDuplication(L.Names);
            if (checkBoxCopyDuplicationS.Checked)
                SDicDuplication = FindDuplication(S.Names);
            if (strS != "")
                str += "\r\n============小表中以下值存在重复============\r\n" + strS;
            if (strL != "")
                str += "\r\n============大表中以下值存在重复============\r\n" + strL;

            foreach (Cells c in S.Names)
                if (!LDic.ContainsKey(c.Text))
                {
                    strLS += c + "\t";
                }

            ////////////////////////////////////////////////  Special 
            if (checkBoxTagS.Checked || checkBoxSkipDuplicate.Checked)  //排除重复
                foreach (Cells c in S.Names)
                    if (!LDic.ContainsKey(c.Text) && !SDicDuplication.ContainsKey(c.Text))
                    {
                        SSpecial.Add(new Point(c.Row, c.Col));
                        if (!SDicSpecial.ContainsKey(c.Text))
                            SDicSpecial[c.Text] = c;
                    }
            if (checkBoxTagL.Checked || checkBoxSkipDuplicate.Checked)
                foreach (Cells c in L.Names)
                    if (!SDic.ContainsKey(c.Text) && !LDicDuplication.ContainsKey(c.Text))
                    {
                        LSpecial.Add(new Point(c.Row, c.Col));
                        if (!LDicSpecial.ContainsKey(c.Text))
                            LDicSpecial[c.Text] = c;
                    }
            if (strLS != "")
                strLS = "\r\n============小表中的以下值在大表中不存在============\r\n" + strLS;
            if (str != "")
            {
                textBoxOut.Text = str + strLS;
                if (!checkBoxSkipDuplicate.Checked)
                {
                    MessageBox.Show("存在重复值，没有进一步比对，可选择忽略之后，继续比对");
                    return;
                }
            }
            ///////////////////////// DIF
            SP = new List<Point>();
            LP = new List<Point>();
             _Same = new List<List<Cells>>();
            foreach (Cells c1 in S.Names)
            {
                if (str != "" &&
                    (SDicDuplication.ContainsKey(c1.Text)
                    || SDicSpecial.ContainsKey(c1.Text)))
                    continue;
                Cells c2 = LDic[c1.Text];
                for (int col = 1; col < S.ColCount-1; col++)
                {
                    if (S.CellValue(c1.Row, S.NameCol + col) != L.CellValue(c2.Row, L.NameCol + col))
                    {
                        SP.Add(new Point(c1.Row, S.NameCol + col));
                        LP.Add(new Point(c2.Row, L.NameCol +col));
                        strb.AppendLine(c1.Text + " " + PointTostring(new Point(c1.Row, col)) + "=" + S.CellValue(c1.Row,S.NameCol + col) + "\t"
                                                      + PointTostring(new Point(c2.Row, col)) + "=" + L.CellValue(c2.Row,L.NameCol + col));
                    }
                }
                _Same.Add(new List<Cells>() { c1, c2 });
            }
            string str1 = strb.ToString();
            if (str1 == "")
            {
                MessageBox.Show("除忽略项外，待比较项目完全相同");
            }
            else
                str1 = "\r\n============以下单元格内容两表不一致 （行号，列号）============\r\n" + str1;
            textBoxOut.Text = str + strLS + str1;

        }
       
        private void ExportDifferentExcel(ExcelBook E,string FileName = "")
        {
            ExcelBook N = new ExcelBook();
            try
            {
                ExcelSheet S = new ExcelSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
                ExcelSheet L = new ExcelSheet(E.ExcelSheet(comboBoxLSheetName.SelectedItem.ToString()));
                if (FileName != "")
                {
                    N.Create();
                    ExcelSheet NL = new ExcelSheet(N.CopySheetAndDeleteOther(L));
                    ExcelSheet NS = new ExcelSheet(N.CopySheet(S));
                    NS.FormatExcelRange(SP, _colorDif,"不同");
                    NL.FormatExcelRange(LP, _colorDif,"不同");
                    N.SaveAs(FileName);
                }
                else
                {
                    S.FormatExcelRange(SP, _colorDif,"不同");
                    L.FormatExcelRange(LP, _colorDif,"不同");                   
                }
            }
            finally
            {
                N = null;
            }
        }
        private void ExportSpecialExcel(ExcelBook E, string FileName = "")
        {
            ExcelBook N= new ExcelBook();
            try
            {
                    ExcelSheet S = new ExcelSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
                    ExcelSheet L = new ExcelSheet(E.ExcelSheet(comboBoxLSheetName.SelectedItem.ToString()));
                    if (FileName != "")
                    {
                        N.Create();
                        if (LSpecial.Count > 0)
                            new ExcelSheet(N.CopySheetAndDeleteOther(L)).FormatExcelRange(LSpecial, Color.Pink,"特有");
                        if (SSpecial.Count > 0)
                        {
                            if (LSpecial.Count > 0)
                                new ExcelSheet(N.CopySheet(S)).FormatExcelRange(SSpecial, Color.Pink, "特有");
                            else
                                new ExcelSheet(N.CopySheetAndDeleteOther(S)).FormatExcelRange(SSpecial, Color.Pink, "特有");
                        }
                        N.SaveAs(FileName);
                    }
                    else
                    {
                        S.FormatExcelRange(SSpecial, _colorSpe, "特有");
                        L.FormatExcelRange(LSpecial, _colorSpe, "特有");    
                    }
            }
            finally
            {
                N = null;
            }
        }
        private void ExportDuplicationExcel(ExcelBook E, string FileName = "")
        {
            ExcelBook N= new ExcelBook();
            try
            {
                    ExcelSheet S = new ExcelSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
                    ExcelSheet L = new ExcelSheet(E.ExcelSheet(comboBoxLSheetName.SelectedItem.ToString()));

                    List<int> LDuplication = new List<int>();
                    List<int> SDuplication = new List<int>();
                    foreach (List<int> l in LDicDuplication.Values)
                        LDuplication.AddRange(l);
                    foreach (List<int> l in SDicDuplication.Values)
                        SDuplication.AddRange(l);

                    if (FileName != "")
                    {
                        N.Create();

                        if (LDuplication.Count > 0)
                            new ExcelSheet(N.CopySheetRowsAndDeleteOther(L, LDuplication));

                        if (SDuplication.Count > 0)
                        {
                            if (LDuplication.Count > 0)
                                new ExcelSheet(N.CopySheetRows(S, SDuplication));
                            else
                                new ExcelSheet(N.CopySheetRowsAndDeleteOther(S, SDuplication));
                        }
                        N.SaveAs(FileName);
                    }
                    else
                    {
                        List<Point> SD = SDuplication.Select(r => new Point(r, 2)).ToList();
                        List<Point> LD = SDuplication.Select(r => new Point(r, 2)).ToList();
                        S.FormatExcelRange(SD, _colorDup, "重复");
                        L.FormatExcelRange(LD, _colorDup, "重复");
                    }
            }
            finally
            {
                N = null;
            }
        }

        private static string OpenXmlFile(string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = "Excel文件|*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return null;
        }
        private static string PointTostring(Point p)
        {
            return "("+p.X+","+p.Y+")";
        }
        private static Dictionary<string, Cells> ListToDictionary(List<Cells> Names,ref string str)
        {
            Dictionary<string, Cells> Dic = new Dictionary<string, Cells>();
            foreach (Cells c in Names)
                if (!Dic.ContainsKey(c.Text))
                    Dic[c.Text] = c;
                else
                {
                    str += c + "\t" + Dic[c.Text]+"\r\n";
                }           
            return Dic;
        }
        private static Dictionary<string, List<int>> FindDuplication(List<Cells> Names)
        {
            Dictionary<string, List<int>> DL = new Dictionary<string, List<int>>();
            Dictionary<string, Cells> Dic = new Dictionary<string, Cells>();
            foreach (Cells c in Names)
                if (!Dic.ContainsKey(c.Text))
                    Dic[c.Text] = c;
                else
                {
                    if (!DL.ContainsKey(c.Text))
                    {
                        List<int> v = new List<int>();
                        v.Add( c.Row);
                        v.Add(Dic[c.Text].Row);
                        DL[c.Text] = v;
                    }
                    else
                    {
                        DL[c.Text].Add(c.Row);
                    }
                }           
            return DL;
        }
        private static Boolean CompareListString(ExcelSheet S, ExcelSheet L)
        {
            if (S.Titles.Count == L.Titles.Count)
            {
                for (int i = 0; i < S.Titles.Count; i++)
                    if (S.Titles[i].Text  != L.Titles[i].Text)
                        return false;
                return true;
            }
            return false;
        }
        private static Boolean CompareListString(List<string> STitles, List<string> LTitles)
        {
            if (STitles.Count == LTitles.Count)
            {
                for (int i = 0; i < STitles.Count; i++)
                    if (STitles[i] != LTitles[i])
                        return false;
                return true;
            }
            return false;
        }

        private CConfig m_cfg { get; set; }
        private List<Point> SP;
        private List<Point> LP;
        private List<Point> SSpecial;
        private List<Point> LSpecial;
        private Color _colorDup;
        private Color _colorSpe;
        private Color _colorDif;
        private List<List<Cells>> _Same;
        public Dictionary<string, List<int>> LDicDuplication { get; set; }
        public Dictionary<string, List<int>> SDicDuplication { get; set; }

        private void checkBoxCompareSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCompareSingle.Checked)
            {
                label3.Visible = false;
                comboBoxLSheetName.Visible = false;
                label2.Text = "单表表名";
                dgvL.Visible = true;
                dgvS.Visible = true;
                textBoxOut.Top = dgvS.Bottom;
                textBoxOut.Height = textBox1.Top  - dgvS.Bottom;
                comboBoxSSheetName.SelectedIndex = comboBoxSSheetName.SelectedIndex;
            }
            else
            {
                label3.Visible = true;
                comboBoxLSheetName.Visible = true;
                label2.Text = "小表表名";
                dgvS.DataSource = dgvL.DataSource = null;
                dgvL.Visible = false;
                dgvS.Visible = false;
                textBoxOut.Top = dgvS.Top;
                textBoxOut.Height = textBox1.Top - dgvS.Top; 
            }
        }
        private void comboBoxSSheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxCompareSingle.Checked)
            {
                ExcelBook E = new ExcelBook(m_cfg.XlsName);
                DataTable S = null, L = null;
                try
                {
                    if (!checkBoxRedTitle.Checked)
                    {
                        ExcelSingleSheet ES = new ExcelSingleSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
                        if (!ES.CanCheckSingleSheet(ref S, ref L))
                        {
                            MessageBox.Show(ES.Msg);
                            return;
                        }
                    }
                    else
                    {
                        ExcelSingleRedSheet ES = new ExcelSingleRedSheet(E.ExcelSheet(comboBoxSSheetName.SelectedItem.ToString()));
                        if (!ES.CanCheckSingleSheet(ref S, ref L, checkBoxMultiBlackSkip.Checked))
                        {
                            MessageBox.Show(ES.Msg);
                            return;
                        }
                    }
                    dgvL.DataSource = L;
                    dgvS.DataSource = S;
                }
                finally
                {
                    E = null;
                    ExcelBook.excel.Workbooks.Close();
                }
            }
        }

        private void checkBoxMultiBlackSkip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMultiBlackSkip.Checked)
            {
                checkBoxCopyDuplicationL.Checked = false;
                checkBoxCopyDuplicationS.Checked = false;
                checkBoxDifL.Checked = true;
                checkBoxDifS.Checked = true;
                checkBoxTagL.Checked = false;
                checkBoxTagS.Checked = false;
                checkBoxClearBackColor.Checked = false;
                checkBoxSkipDuplicate.Checked = true;
               
                checkBoxCompareSingle.Checked = true;
                checkBoxRedTitle.Checked = true;  
            }
            else
            {
                checkBoxCopyDuplicationL.Checked = true;
                checkBoxCopyDuplicationS.Checked = true;
                checkBoxDifL.Checked = true;
                checkBoxDifS.Checked = true;
                checkBoxTagL.Checked = true;
                checkBoxTagS.Checked = true;
                checkBoxClearBackColor.Checked = true;
                checkBoxSkipDuplicate.Checked = true;
               
                checkBoxCompareSingle.Checked = true;
                checkBoxRedTitle.Checked = true;               
            }
        }
    }
    /////////////////////
    //private void buttonOutExcel_Click(object sender, EventArgs e)
    //{
    //    if (textBoxOut.Text != "")
    //    {//输出
    //        SaveFileDialog dlg = new SaveFileDialog();
    //        dlg.Filter = "Excel files (*.xls)|*.xls";
    //        dlg.FilterIndex = 0;
    //        dlg.RestoreDirectory = true;
    //        dlg.FileName = m_cfg.ShortName.Replace(".xls", "-比对.不同之处红色标出.xls");
    //        if (dlg.ShowDialog() == DialogResult.OK)
    //        {
    //            ExportDifferentExcel(dlg.FileName);
    //        }
    //    }
    //}

    //private void ExportSpecialExcelDlg()
    //{
    //    if (SSpecial.Count == 0 && LSpecial.Count == 0)
    //    {
    //        //MessageBox.Show("没有数据可以导出的数据。 可能核对数据相同或 还没有核对");
    //        return;
    //    }
    //    SaveFileDialog dlg = new SaveFileDialog();   //实例化一个SaveFileDialog保存文件对话框
    //    dlg.Filter = "Excel files (*.xls)|*.xls";
    //    dlg.FileName = m_cfg.ShortName.Replace(".xls", "-特有.特有之处粉红色标出.xls");
    //    dlg.FilterIndex = 0;
    //    dlg.RestoreDirectory = true;

    //    if (dlg.ShowDialog() == DialogResult.OK)
    //    {
    //        ExportSpecialExcel(dlg.FileName);
    //    }
    //}
    //private void ExportDuplicationExcelDlg()
    //{
    //    if (LDicDuplication.Count == 0 && SDicDuplication.Count == 0)
    //    {
    //        MessageBox.Show("没有数据可以导出的数据。 可能核对数据相同或 还没有核对");
    //        return;
    //    }
    //    SaveFileDialog dlg = new SaveFileDialog();   //实例化一个SaveFileDialog保存文件对话框
    //    dlg.Filter = "Excel files (*.xls)|*.xls";
    //    dlg.FileName = m_cfg.ShortName.Replace(".xls", "-重复.重复之处复制出来.xls");
    //    dlg.FilterIndex = 0;
    //    dlg.RestoreDirectory = true;

    //    if (dlg.ShowDialog() == DialogResult.OK)
    //    {
    //        ExportDuplicationExcel(dlg.FileName);
    //    }
    //}
    ////////////////////
}

namespace ExcelCompare
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowXlsNameS = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCompare = new System.Windows.Forms.Button();
            this.textBoxOut = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOutTxt = new System.Windows.Forms.Button();
            this.checkBoxTagS = new System.Windows.Forms.CheckBox();
            this.checkBoxTagL = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyDuplicationS = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyDuplicationL = new System.Windows.Forms.CheckBox();
            this.comboBoxSSheetName = new System.Windows.Forms.ComboBox();
            this.comboBoxLSheetName = new System.Windows.Forms.ComboBox();
            this.textBoxExcelFileName = new System.Windows.Forms.TextBox();
            this.checkBoxClearBackColor = new System.Windows.Forms.CheckBox();
            this.checkBoxDifS = new System.Windows.Forms.CheckBox();
            this.textBoxSpecial = new System.Windows.Forms.TextBox();
            this.textBoxDuplication = new System.Windows.Forms.TextBox();
            this.checkBoxDifL = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipDuplicate = new System.Windows.Forms.CheckBox();
            this.textBoxDif = new System.Windows.Forms.TextBox();
            this.checkBoxCompareSingle = new System.Windows.Forms.CheckBox();
            this.dgvS = new System.Windows.Forms.DataGridView();
            this.dgvL = new System.Windows.Forms.DataGridView();
            this.checkBoxRedTitle = new System.Windows.Forms.CheckBox();
            this.checkBoxMultiBlackSkip = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvL)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 399);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(631, 63);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "文件第一行为表头，第二列为核对人名，并且值唯一 第一列为流水号\r\n第一行表头为“比较结果” 的前一列默认为核对比较列\r\n第一行和第一列不能出现空行，否则视为无数据" +
                "\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 425);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "说明";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "小表表名";
            // 
            // buttonBrowXlsNameS
            // 
            this.buttonBrowXlsNameS.Location = new System.Drawing.Point(290, 53);
            this.buttonBrowXlsNameS.Name = "buttonBrowXlsNameS";
            this.buttonBrowXlsNameS.Size = new System.Drawing.Size(201, 38);
            this.buttonBrowXlsNameS.TabIndex = 4;
            this.buttonBrowXlsNameS.Text = "打开Excel文件";
            this.buttonBrowXlsNameS.UseVisualStyleBackColor = true;
            this.buttonBrowXlsNameS.Click += new System.EventHandler(this.buttonBrowXlsNameS_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "大表表名";
            // 
            // buttonCompare
            // 
            this.buttonCompare.Location = new System.Drawing.Point(57, 107);
            this.buttonCompare.Name = "buttonCompare";
            this.buttonCompare.Size = new System.Drawing.Size(444, 38);
            this.buttonCompare.TabIndex = 8;
            this.buttonCompare.Text = "比较";
            this.buttonCompare.UseVisualStyleBackColor = true;
            this.buttonCompare.Click += new System.EventHandler(this.buttonCompare_Click);
            // 
            // textBoxOut
            // 
            this.textBoxOut.Location = new System.Drawing.Point(40, 257);
            this.textBoxOut.Multiline = true;
            this.textBoxOut.Name = "textBoxOut";
            this.textBoxOut.ReadOnly = true;
            this.textBoxOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOut.Size = new System.Drawing.Size(631, 141);
            this.textBoxOut.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "输出";
            // 
            // buttonOutTxt
            // 
            this.buttonOutTxt.Location = new System.Drawing.Point(614, 126);
            this.buttonOutTxt.Name = "buttonOutTxt";
            this.buttonOutTxt.Size = new System.Drawing.Size(64, 19);
            this.buttonOutTxt.TabIndex = 9;
            this.buttonOutTxt.Text = "导出文本";
            this.buttonOutTxt.UseVisualStyleBackColor = true;
            this.buttonOutTxt.Click += new System.EventHandler(this.buttonOutTxt_Click);
            // 
            // checkBoxTagS
            // 
            this.checkBoxTagS.AutoSize = true;
            this.checkBoxTagS.Location = new System.Drawing.Point(507, 6);
            this.checkBoxTagS.Name = "checkBoxTagS";
            this.checkBoxTagS.Size = new System.Drawing.Size(132, 16);
            this.checkBoxTagS.TabIndex = 10;
            this.checkBoxTagS.Text = "标记小表中的特有项";
            this.checkBoxTagS.UseVisualStyleBackColor = true;
            // 
            // checkBoxTagL
            // 
            this.checkBoxTagL.AutoSize = true;
            this.checkBoxTagL.Location = new System.Drawing.Point(507, 23);
            this.checkBoxTagL.Name = "checkBoxTagL";
            this.checkBoxTagL.Size = new System.Drawing.Size(132, 16);
            this.checkBoxTagL.TabIndex = 11;
            this.checkBoxTagL.Text = "标记大表中的特有项";
            this.checkBoxTagL.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopyDuplicationS
            // 
            this.checkBoxCopyDuplicationS.AutoSize = true;
            this.checkBoxCopyDuplicationS.Location = new System.Drawing.Point(507, 40);
            this.checkBoxCopyDuplicationS.Name = "checkBoxCopyDuplicationS";
            this.checkBoxCopyDuplicationS.Size = new System.Drawing.Size(132, 16);
            this.checkBoxCopyDuplicationS.TabIndex = 10;
            this.checkBoxCopyDuplicationS.Text = "标记小表中的重复项";
            this.checkBoxCopyDuplicationS.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopyDuplicationL
            // 
            this.checkBoxCopyDuplicationL.AutoSize = true;
            this.checkBoxCopyDuplicationL.Location = new System.Drawing.Point(507, 57);
            this.checkBoxCopyDuplicationL.Name = "checkBoxCopyDuplicationL";
            this.checkBoxCopyDuplicationL.Size = new System.Drawing.Size(132, 16);
            this.checkBoxCopyDuplicationL.TabIndex = 11;
            this.checkBoxCopyDuplicationL.Text = "标记大表中的重复项";
            this.checkBoxCopyDuplicationL.UseVisualStyleBackColor = true;
            // 
            // comboBoxSSheetName
            // 
            this.comboBoxSSheetName.FormattingEnabled = true;
            this.comboBoxSSheetName.Location = new System.Drawing.Point(104, 53);
            this.comboBoxSSheetName.Name = "comboBoxSSheetName";
            this.comboBoxSSheetName.Size = new System.Drawing.Size(155, 20);
            this.comboBoxSSheetName.TabIndex = 12;
            this.comboBoxSSheetName.SelectedIndexChanged += new System.EventHandler(this.comboBoxSSheetName_SelectedIndexChanged);
            // 
            // comboBoxLSheetName
            // 
            this.comboBoxLSheetName.FormattingEnabled = true;
            this.comboBoxLSheetName.Location = new System.Drawing.Point(104, 75);
            this.comboBoxLSheetName.Name = "comboBoxLSheetName";
            this.comboBoxLSheetName.Size = new System.Drawing.Size(155, 20);
            this.comboBoxLSheetName.TabIndex = 13;
            // 
            // textBoxExcelFileName
            // 
            this.textBoxExcelFileName.Location = new System.Drawing.Point(47, 26);
            this.textBoxExcelFileName.Name = "textBoxExcelFileName";
            this.textBoxExcelFileName.ReadOnly = true;
            this.textBoxExcelFileName.Size = new System.Drawing.Size(444, 21);
            this.textBoxExcelFileName.TabIndex = 14;
            // 
            // checkBoxClearBackColor
            // 
            this.checkBoxClearBackColor.AutoSize = true;
            this.checkBoxClearBackColor.Location = new System.Drawing.Point(507, 129);
            this.checkBoxClearBackColor.Name = "checkBoxClearBackColor";
            this.checkBoxClearBackColor.Size = new System.Drawing.Size(96, 16);
            this.checkBoxClearBackColor.TabIndex = 11;
            this.checkBoxClearBackColor.Text = "预先清除背景";
            this.checkBoxClearBackColor.UseVisualStyleBackColor = true;
            // 
            // checkBoxDifS
            // 
            this.checkBoxDifS.AutoSize = true;
            this.checkBoxDifS.Location = new System.Drawing.Point(507, 77);
            this.checkBoxDifS.Name = "checkBoxDifS";
            this.checkBoxDifS.Size = new System.Drawing.Size(132, 16);
            this.checkBoxDifS.TabIndex = 15;
            this.checkBoxDifS.Text = "标记小表的不同之处";
            this.checkBoxDifS.UseVisualStyleBackColor = true;
            // 
            // textBoxSpecial
            // 
            this.textBoxSpecial.BackColor = System.Drawing.Color.Pink;
            this.textBoxSpecial.Location = new System.Drawing.Point(638, 11);
            this.textBoxSpecial.Name = "textBoxSpecial";
            this.textBoxSpecial.ReadOnly = true;
            this.textBoxSpecial.Size = new System.Drawing.Size(40, 21);
            this.textBoxSpecial.TabIndex = 16;
            // 
            // textBoxDuplication
            // 
            this.textBoxDuplication.BackColor = System.Drawing.Color.RoyalBlue;
            this.textBoxDuplication.Location = new System.Drawing.Point(638, 44);
            this.textBoxDuplication.Name = "textBoxDuplication";
            this.textBoxDuplication.ReadOnly = true;
            this.textBoxDuplication.Size = new System.Drawing.Size(40, 21);
            this.textBoxDuplication.TabIndex = 17;
            // 
            // checkBoxDifL
            // 
            this.checkBoxDifL.AutoSize = true;
            this.checkBoxDifL.Location = new System.Drawing.Point(507, 94);
            this.checkBoxDifL.Name = "checkBoxDifL";
            this.checkBoxDifL.Size = new System.Drawing.Size(132, 16);
            this.checkBoxDifL.TabIndex = 18;
            this.checkBoxDifL.Text = "标记大表的不同之处";
            this.checkBoxDifL.UseVisualStyleBackColor = true;
            // 
            // checkBoxSkipDuplicate
            // 
            this.checkBoxSkipDuplicate.AutoSize = true;
            this.checkBoxSkipDuplicate.Location = new System.Drawing.Point(507, 111);
            this.checkBoxSkipDuplicate.Name = "checkBoxSkipDuplicate";
            this.checkBoxSkipDuplicate.Size = new System.Drawing.Size(84, 16);
            this.checkBoxSkipDuplicate.TabIndex = 19;
            this.checkBoxSkipDuplicate.Text = "忽略重复项";
            this.checkBoxSkipDuplicate.UseVisualStyleBackColor = true;
            // 
            // textBoxDif
            // 
            this.textBoxDif.BackColor = System.Drawing.Color.DarkMagenta;
            this.textBoxDif.Location = new System.Drawing.Point(638, 82);
            this.textBoxDif.Name = "textBoxDif";
            this.textBoxDif.ReadOnly = true;
            this.textBoxDif.Size = new System.Drawing.Size(40, 21);
            this.textBoxDif.TabIndex = 20;
            // 
            // checkBoxCompareSingle
            // 
            this.checkBoxCompareSingle.AutoSize = true;
            this.checkBoxCompareSingle.Location = new System.Drawing.Point(47, 4);
            this.checkBoxCompareSingle.Name = "checkBoxCompareSingle";
            this.checkBoxCompareSingle.Size = new System.Drawing.Size(72, 16);
            this.checkBoxCompareSingle.TabIndex = 21;
            this.checkBoxCompareSingle.Text = "比较单表";
            this.checkBoxCompareSingle.UseVisualStyleBackColor = true;
            this.checkBoxCompareSingle.CheckedChanged += new System.EventHandler(this.checkBoxCompareSingle_CheckedChanged);
            // 
            // dgvS
            // 
            this.dgvS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvS.Location = new System.Drawing.Point(41, 148);
            this.dgvS.Name = "dgvS";
            this.dgvS.RowHeadersVisible = false;
            this.dgvS.RowTemplate.Height = 23;
            this.dgvS.Size = new System.Drawing.Size(316, 108);
            this.dgvS.TabIndex = 22;
            // 
            // dgvL
            // 
            this.dgvL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvL.Location = new System.Drawing.Point(361, 148);
            this.dgvL.Name = "dgvL";
            this.dgvL.RowHeadersVisible = false;
            this.dgvL.RowTemplate.Height = 23;
            this.dgvL.Size = new System.Drawing.Size(308, 108);
            this.dgvL.TabIndex = 23;
            // 
            // checkBoxRedTitle
            // 
            this.checkBoxRedTitle.AutoSize = true;
            this.checkBoxRedTitle.Location = new System.Drawing.Point(137, 4);
            this.checkBoxRedTitle.Name = "checkBoxRedTitle";
            this.checkBoxRedTitle.Size = new System.Drawing.Size(72, 16);
            this.checkBoxRedTitle.TabIndex = 24;
            this.checkBoxRedTitle.Text = "红色表头";
            this.checkBoxRedTitle.UseVisualStyleBackColor = true;
            // 
            // checkBoxMultiBlackSkip
            // 
            this.checkBoxMultiBlackSkip.AutoSize = true;
            this.checkBoxMultiBlackSkip.Location = new System.Drawing.Point(239, 4);
            this.checkBoxMultiBlackSkip.Name = "checkBoxMultiBlackSkip";
            this.checkBoxMultiBlackSkip.Size = new System.Drawing.Size(120, 16);
            this.checkBoxMultiBlackSkip.TabIndex = 25;
            this.checkBoxMultiBlackSkip.Text = "多表首位黑色跳过";
            this.checkBoxMultiBlackSkip.UseVisualStyleBackColor = true;
            this.checkBoxMultiBlackSkip.CheckedChanged += new System.EventHandler(this.checkBoxMultiBlackSkip_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 464);
            this.Controls.Add(this.checkBoxMultiBlackSkip);
            this.Controls.Add(this.checkBoxRedTitle);
            this.Controls.Add(this.dgvL);
            this.Controls.Add(this.dgvS);
            this.Controls.Add(this.checkBoxCompareSingle);
            this.Controls.Add(this.textBoxDif);
            this.Controls.Add(this.checkBoxSkipDuplicate);
            this.Controls.Add(this.checkBoxDifL);
            this.Controls.Add(this.textBoxDuplication);
            this.Controls.Add(this.textBoxSpecial);
            this.Controls.Add(this.checkBoxDifS);
            this.Controls.Add(this.textBoxExcelFileName);
            this.Controls.Add(this.comboBoxLSheetName);
            this.Controls.Add(this.comboBoxSSheetName);
            this.Controls.Add(this.checkBoxClearBackColor);
            this.Controls.Add(this.checkBoxCopyDuplicationL);
            this.Controls.Add(this.checkBoxCopyDuplicationS);
            this.Controls.Add(this.checkBoxTagL);
            this.Controls.Add(this.checkBoxTagS);
            this.Controls.Add(this.buttonOutTxt);
            this.Controls.Add(this.buttonCompare);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonBrowXlsNameS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOut);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Excel表核对4.2";
            ((System.ComponentModel.ISupportInitialize)(this.dgvS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBrowXlsNameS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCompare;
        private System.Windows.Forms.TextBox textBoxOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOutTxt;
        private System.Windows.Forms.CheckBox checkBoxTagS;
        private System.Windows.Forms.CheckBox checkBoxTagL;
        private System.Windows.Forms.CheckBox checkBoxCopyDuplicationS;
        private System.Windows.Forms.CheckBox checkBoxCopyDuplicationL;
        private System.Windows.Forms.ComboBox comboBoxSSheetName;
        private System.Windows.Forms.ComboBox comboBoxLSheetName;
        private System.Windows.Forms.TextBox textBoxExcelFileName;
        private System.Windows.Forms.CheckBox checkBoxClearBackColor;
        private System.Windows.Forms.CheckBox checkBoxDifS;
        private System.Windows.Forms.TextBox textBoxSpecial;
        private System.Windows.Forms.TextBox textBoxDuplication;
        private System.Windows.Forms.CheckBox checkBoxDifL;
        private System.Windows.Forms.CheckBox checkBoxSkipDuplicate;
        private System.Windows.Forms.TextBox textBoxDif;
        private System.Windows.Forms.CheckBox checkBoxCompareSingle;
        private System.Windows.Forms.DataGridView dgvS;
        private System.Windows.Forms.DataGridView dgvL;
        private System.Windows.Forms.CheckBox checkBoxRedTitle;
        private System.Windows.Forms.CheckBox checkBoxMultiBlackSkip;
    }
}


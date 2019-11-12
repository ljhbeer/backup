namespace DataRegex
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
            this.buttonUnicodeTable = new System.Windows.Forms.Button();
            this.checkBoxDistinct = new System.Windows.Forms.CheckBox();
            this.buttonMatchTest = new System.Windows.Forms.Button();
            this.textBoxOut = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxReplaceOutExp = new System.Windows.Forms.TextBox();
            this.buttonRegexReplace = new System.Windows.Forms.Button();
            this.checkBoxGroup = new System.Windows.Forms.CheckBox();
            this.buttonOutMatchtoFile = new System.Windows.Forms.Button();
            this.textBoxInfilename = new System.Windows.Forms.TextBox();
            this.button_MatchCount = new System.Windows.Forms.Button();
            this.textBoxOutCnt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxReplacePatten = new System.Windows.Forms.TextBox();
            this.textBoxIn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonInBrowse = new System.Windows.Forms.Button();
            this.buttonReadInfile = new System.Windows.Forms.Button();
            this.checkBoxfileInputMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxOutFileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxOuttheSame = new System.Windows.Forms.CheckBox();
            this.buttonCharacterStatic = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUnicodeTable
            // 
            this.buttonUnicodeTable.Location = new System.Drawing.Point(59, 430);
            this.buttonUnicodeTable.Name = "buttonUnicodeTable";
            this.buttonUnicodeTable.Size = new System.Drawing.Size(63, 35);
            this.buttonUnicodeTable.TabIndex = 33;
            this.buttonUnicodeTable.Text = "Unicode码表";
            this.buttonUnicodeTable.UseVisualStyleBackColor = true;
            this.buttonUnicodeTable.Visible = false;
            this.buttonUnicodeTable.Click += new System.EventHandler(this.buttonUnicodeTable_Click);
            // 
            // checkBoxDistinct
            // 
            this.checkBoxDistinct.Location = new System.Drawing.Point(167, 222);
            this.checkBoxDistinct.Name = "checkBoxDistinct";
            this.checkBoxDistinct.Size = new System.Drawing.Size(76, 23);
            this.checkBoxDistinct.TabIndex = 32;
            this.checkBoxDistinct.Text = "Distinct";
            this.checkBoxDistinct.UseVisualStyleBackColor = true;
            // 
            // buttonMatchTest
            // 
            this.buttonMatchTest.Location = new System.Drawing.Point(59, 222);
            this.buttonMatchTest.Name = "buttonMatchTest";
            this.buttonMatchTest.Size = new System.Drawing.Size(102, 21);
            this.buttonMatchTest.TabIndex = 31;
            this.buttonMatchTest.Text = "匹配测试到下面";
            this.buttonMatchTest.UseVisualStyleBackColor = true;
            this.buttonMatchTest.Click += new System.EventHandler(this.buttonMatchTest_Click);
            // 
            // textBoxOut
            // 
            this.textBoxOut.Location = new System.Drawing.Point(60, 251);
            this.textBoxOut.Multiline = true;
            this.textBoxOut.Name = "textBoxOut";
            this.textBoxOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOut.Size = new System.Drawing.Size(541, 87);
            this.textBoxOut.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 18);
            this.label3.TabIndex = 29;
            this.label3.Text = "替换表达式";
            // 
            // textBoxReplaceOutExp
            // 
            this.textBoxReplaceOutExp.Location = new System.Drawing.Point(95, 20);
            this.textBoxReplaceOutExp.Name = "textBoxReplaceOutExp";
            this.textBoxReplaceOutExp.Size = new System.Drawing.Size(367, 21);
            this.textBoxReplaceOutExp.TabIndex = 28;
            // 
            // buttonRegexReplace
            // 
            this.buttonRegexReplace.Location = new System.Drawing.Point(468, 20);
            this.buttonRegexReplace.Name = "buttonRegexReplace";
            this.buttonRegexReplace.Size = new System.Drawing.Size(74, 22);
            this.buttonRegexReplace.TabIndex = 27;
            this.buttonRegexReplace.Text = "替换";
            this.buttonRegexReplace.UseVisualStyleBackColor = true;
            this.buttonRegexReplace.Click += new System.EventHandler(this.buttonRegexReplace_Click);
            // 
            // checkBoxGroup
            // 
            this.checkBoxGroup.Location = new System.Drawing.Point(369, 346);
            this.checkBoxGroup.Name = "checkBoxGroup";
            this.checkBoxGroup.Size = new System.Drawing.Size(80, 26);
            this.checkBoxGroup.TabIndex = 26;
            this.checkBoxGroup.Text = "提取分组1";
            this.checkBoxGroup.UseVisualStyleBackColor = true;
            // 
            // buttonOutMatchtoFile
            // 
            this.buttonOutMatchtoFile.Location = new System.Drawing.Point(230, 346);
            this.buttonOutMatchtoFile.Name = "buttonOutMatchtoFile";
            this.buttonOutMatchtoFile.Size = new System.Drawing.Size(125, 24);
            this.buttonOutMatchtoFile.TabIndex = 25;
            this.buttonOutMatchtoFile.Text = "输出到文件_match_n";
            this.buttonOutMatchtoFile.UseVisualStyleBackColor = true;
            this.buttonOutMatchtoFile.Click += new System.EventHandler(this.buttonOutMatchtoFile_Click);
            // 
            // textBoxInfilename
            // 
            this.textBoxInfilename.Location = new System.Drawing.Point(136, 12);
            this.textBoxInfilename.Name = "textBoxInfilename";
            this.textBoxInfilename.Size = new System.Drawing.Size(170, 21);
            this.textBoxInfilename.TabIndex = 21;
            this.textBoxInfilename.Text = "out.html";
            // 
            // button_MatchCount
            // 
            this.button_MatchCount.Location = new System.Drawing.Point(369, 221);
            this.button_MatchCount.Name = "button_MatchCount";
            this.button_MatchCount.Size = new System.Drawing.Size(96, 21);
            this.button_MatchCount.TabIndex = 24;
            this.button_MatchCount.Text = "测试匹配次数";
            this.button_MatchCount.UseVisualStyleBackColor = true;
            this.button_MatchCount.Click += new System.EventHandler(this.button_MatchCount_Click);
            // 
            // textBoxOutCnt
            // 
            this.textBoxOutCnt.Location = new System.Drawing.Point(471, 222);
            this.textBoxOutCnt.Name = "textBoxOutCnt";
            this.textBoxOutCnt.Size = new System.Drawing.Size(130, 21);
            this.textBoxOutCnt.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "Pattern";
            // 
            // textBoxReplacePatten
            // 
            this.textBoxReplacePatten.Location = new System.Drawing.Point(60, 194);
            this.textBoxReplacePatten.Name = "textBoxReplacePatten";
            this.textBoxReplacePatten.Size = new System.Drawing.Size(541, 21);
            this.textBoxReplacePatten.TabIndex = 17;
            // 
            // textBoxIn
            // 
            this.textBoxIn.Location = new System.Drawing.Point(60, 35);
            this.textBoxIn.Multiline = true;
            this.textBoxIn.Name = "textBoxIn";
            this.textBoxIn.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxIn.Size = new System.Drawing.Size(541, 153);
            this.textBoxIn.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "输入内容";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 36;
            this.label4.Text = "输入文件名:";
            // 
            // buttonInBrowse
            // 
            this.buttonInBrowse.Location = new System.Drawing.Point(312, 12);
            this.buttonInBrowse.Name = "buttonInBrowse";
            this.buttonInBrowse.Size = new System.Drawing.Size(83, 21);
            this.buttonInBrowse.TabIndex = 37;
            this.buttonInBrowse.Text = "浏览...";
            this.buttonInBrowse.UseVisualStyleBackColor = true;
            this.buttonInBrowse.Click += new System.EventHandler(this.buttonInBrowse_Click);
            // 
            // buttonReadInfile
            // 
            this.buttonReadInfile.Location = new System.Drawing.Point(401, 13);
            this.buttonReadInfile.Name = "buttonReadInfile";
            this.buttonReadInfile.Size = new System.Drawing.Size(83, 21);
            this.buttonReadInfile.TabIndex = 38;
            this.buttonReadInfile.Text = "读入文件";
            this.buttonReadInfile.UseVisualStyleBackColor = true;
            this.buttonReadInfile.Click += new System.EventHandler(this.buttonReadInfile_Click);
            // 
            // checkBoxfileInputMode
            // 
            this.checkBoxfileInputMode.Location = new System.Drawing.Point(504, 10);
            this.checkBoxfileInputMode.Name = "checkBoxfileInputMode";
            this.checkBoxfileInputMode.Size = new System.Drawing.Size(94, 25);
            this.checkBoxfileInputMode.TabIndex = 39;
            this.checkBoxfileInputMode.Text = "文件为输入";
            this.checkBoxfileInputMode.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 40;
            this.label5.Text = "输出结果";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 42;
            this.label6.Text = "输出文件名:";
            // 
            // textBoxOutFileName
            // 
            this.textBoxOutFileName.Location = new System.Drawing.Point(129, 347);
            this.textBoxOutFileName.Name = "textBoxOutFileName";
            this.textBoxOutFileName.Size = new System.Drawing.Size(95, 21);
            this.textBoxOutFileName.TabIndex = 41;
            this.textBoxOutFileName.Text = "out.html";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxReplaceOutExp);
            this.groupBox1.Controls.Add(this.buttonRegexReplace);
            this.groupBox1.Location = new System.Drawing.Point(59, 377);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 47);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "替换模式";
            // 
            // checkBoxOuttheSame
            // 
            this.checkBoxOuttheSame.Location = new System.Drawing.Point(446, 346);
            this.checkBoxOuttheSame.Name = "checkBoxOuttheSame";
            this.checkBoxOuttheSame.Size = new System.Drawing.Size(80, 26);
            this.checkBoxOuttheSame.TabIndex = 44;
            this.checkBoxOuttheSame.Text = "原样输出";
            this.checkBoxOuttheSame.UseVisualStyleBackColor = true;
            // 
            // buttonCharacterStatic
            // 
            this.buttonCharacterStatic.Location = new System.Drawing.Point(538, 430);
            this.buttonCharacterStatic.Name = "buttonCharacterStatic";
            this.buttonCharacterStatic.Size = new System.Drawing.Size(63, 30);
            this.buttonCharacterStatic.TabIndex = 45;
            this.buttonCharacterStatic.Text = "字符分析";
            this.buttonCharacterStatic.UseVisualStyleBackColor = true;
            this.buttonCharacterStatic.Click += new System.EventHandler(this.buttonCharacterStatic_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(395, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 46;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 463);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCharacterStatic);
            this.Controls.Add(this.checkBoxOuttheSame);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxOutFileName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBoxfileInputMode);
            this.Controls.Add(this.buttonReadInfile);
            this.Controls.Add(this.buttonInBrowse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIn);
            this.Controls.Add(this.buttonUnicodeTable);
            this.Controls.Add(this.checkBoxDistinct);
            this.Controls.Add(this.buttonMatchTest);
            this.Controls.Add(this.textBoxOut);
            this.Controls.Add(this.checkBoxGroup);
            this.Controls.Add(this.buttonOutMatchtoFile);
            this.Controls.Add(this.textBoxInfilename);
            this.Controls.Add(this.button_MatchCount);
            this.Controls.Add(this.textBoxOutCnt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxReplacePatten);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUnicodeTable;
        private System.Windows.Forms.CheckBox checkBoxDistinct;
        private System.Windows.Forms.Button buttonMatchTest;
        private System.Windows.Forms.TextBox textBoxOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxReplaceOutExp;
        private System.Windows.Forms.Button buttonRegexReplace;
        private System.Windows.Forms.CheckBox checkBoxGroup;
        private System.Windows.Forms.Button buttonOutMatchtoFile;
        private System.Windows.Forms.TextBox textBoxInfilename;
        private System.Windows.Forms.Button button_MatchCount;
        private System.Windows.Forms.TextBox textBoxOutCnt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxReplacePatten;
        private System.Windows.Forms.TextBox textBoxIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonInBrowse;
        private System.Windows.Forms.Button buttonReadInfile;
        private System.Windows.Forms.CheckBox checkBoxfileInputMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxOutFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxOuttheSame;
        private System.Windows.Forms.Button buttonCharacterStatic;
        private System.Windows.Forms.Button button1;
    }
}


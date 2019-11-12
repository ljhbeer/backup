/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016-9-12
 * 时间: 8:18
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace ReName
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.dgv = new System.Windows.Forms.DataGridView();
            this.SrcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DSTName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonUpdateCMD = new System.Windows.Forms.Button();
            this.textBoxCMD = new System.Windows.Forms.TextBox();
            this.textBoxReadMe = new System.Windows.Forms.TextBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonDonePath = new System.Windows.Forms.Button();
            this.buttonClearBeforeSpace = new System.Windows.Forms.Button();
            this.buttonOutDirInfo = new System.Windows.Forms.Button();
            this.buttonReadNameToTable = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFilterExt = new System.Windows.Forms.TextBox();
            this.buttonModifyFileContent = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SrcName,
            this.DSTName});
            this.dgv.Location = new System.Drawing.Point(3, 12);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(528, 340);
            this.dgv.TabIndex = 0;
            // 
            // SrcName
            // 
            this.SrcName.HeaderText = "源文件名";
            this.SrcName.Name = "SrcName";
            this.SrcName.Width = 250;
            // 
            // DSTName
            // 
            this.DSTName.HeaderText = "新文件名";
            this.DSTName.Name = "DSTName";
            this.DSTName.Width = 250;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(87, 355);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(120, 21);
            this.textBoxPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "设置文件路径";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(213, 356);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(40, 21);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "浏览";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.ButtonBrowseClick);
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(537, 12);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(114, 27);
            this.buttonImport.TabIndex = 4;
            this.buttonImport.Text = "导入";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.ButtonImportClick);
            // 
            // buttonUpdateCMD
            // 
            this.buttonUpdateCMD.Location = new System.Drawing.Point(538, 208);
            this.buttonUpdateCMD.Name = "buttonUpdateCMD";
            this.buttonUpdateCMD.Size = new System.Drawing.Size(124, 31);
            this.buttonUpdateCMD.TabIndex = 5;
            this.buttonUpdateCMD.Text = "修改新文件名";
            this.buttonUpdateCMD.UseVisualStyleBackColor = true;
            this.buttonUpdateCMD.Click += new System.EventHandler(this.ButtonUpdateCMDClick);
            // 
            // textBoxCMD
            // 
            this.textBoxCMD.Location = new System.Drawing.Point(538, 114);
            this.textBoxCMD.Multiline = true;
            this.textBoxCMD.Name = "textBoxCMD";
            this.textBoxCMD.Size = new System.Drawing.Size(124, 98);
            this.textBoxCMD.TabIndex = 6;
            // 
            // textBoxReadMe
            // 
            this.textBoxReadMe.Location = new System.Drawing.Point(538, 282);
            this.textBoxReadMe.Multiline = true;
            this.textBoxReadMe.Name = "textBoxReadMe";
            this.textBoxReadMe.ReadOnly = true;
            this.textBoxReadMe.Size = new System.Drawing.Size(124, 110);
            this.textBoxReadMe.TabIndex = 7;
            this.textBoxReadMe.Text = "导入时如果文件中有| 则使用|分割新旧文件\r\n\r\n修改新文件的命令有( 使用“;\\r\\n”分割命令行)：\r\nreplacetonull=[-][-];\r\naddp" +
                "refix=;\r\naddsubfix=;\r\n";
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(538, 38);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(114, 27);
            this.buttonDone.TabIndex = 8;
            this.buttonDone.Text = "修改";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.ButtonDoneClick);
            // 
            // buttonDonePath
            // 
            this.buttonDonePath.Location = new System.Drawing.Point(538, 65);
            this.buttonDonePath.Name = "buttonDonePath";
            this.buttonDonePath.Size = new System.Drawing.Size(114, 27);
            this.buttonDonePath.TabIndex = 9;
            this.buttonDonePath.Text = "修改目录";
            this.buttonDonePath.UseVisualStyleBackColor = true;
            this.buttonDonePath.Click += new System.EventHandler(this.ButtonDonePathClick);
            // 
            // buttonClearBeforeSpace
            // 
            this.buttonClearBeforeSpace.Location = new System.Drawing.Point(401, 358);
            this.buttonClearBeforeSpace.Name = "buttonClearBeforeSpace";
            this.buttonClearBeforeSpace.Size = new System.Drawing.Size(85, 34);
            this.buttonClearBeforeSpace.TabIndex = 10;
            this.buttonClearBeforeSpace.Text = "除去目录名前的前导空格";
            this.buttonClearBeforeSpace.UseVisualStyleBackColor = true;
            this.buttonClearBeforeSpace.Click += new System.EventHandler(this.ButtonClearBeforeSpaceClick);
            // 
            // buttonOutDirInfo
            // 
            this.buttonOutDirInfo.Location = new System.Drawing.Point(492, 359);
            this.buttonOutDirInfo.Name = "buttonOutDirInfo";
            this.buttonOutDirInfo.Size = new System.Drawing.Size(40, 33);
            this.buttonOutDirInfo.TabIndex = 11;
            this.buttonOutDirInfo.Text = "输出文件信息";
            this.buttonOutDirInfo.UseVisualStyleBackColor = true;
            this.buttonOutDirInfo.Click += new System.EventHandler(this.ButtonOutDirInfoClick);
            // 
            // buttonReadNameToTable
            // 
            this.buttonReadNameToTable.Location = new System.Drawing.Point(213, 377);
            this.buttonReadNameToTable.Name = "buttonReadNameToTable";
            this.buttonReadNameToTable.Size = new System.Drawing.Size(74, 20);
            this.buttonReadNameToTable.TabIndex = 12;
            this.buttonReadNameToTable.Text = "读入文件名";
            this.buttonReadNameToTable.UseVisualStyleBackColor = true;
            this.buttonReadNameToTable.Click += new System.EventHandler(this.Button1Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "过滤文件后缀";
            // 
            // textBoxFilterExt
            // 
            this.textBoxFilterExt.Location = new System.Drawing.Point(87, 376);
            this.textBoxFilterExt.Name = "textBoxFilterExt";
            this.textBoxFilterExt.Size = new System.Drawing.Size(120, 21);
            this.textBoxFilterExt.TabIndex = 13;
            // 
            // buttonModifyFileContent
            // 
            this.buttonModifyFileContent.Location = new System.Drawing.Point(538, 245);
            this.buttonModifyFileContent.Name = "buttonModifyFileContent";
            this.buttonModifyFileContent.Size = new System.Drawing.Size(124, 31);
            this.buttonModifyFileContent.TabIndex = 15;
            this.buttonModifyFileContent.Text = "修改文件内容";
            this.buttonModifyFileContent.UseVisualStyleBackColor = true;
            this.buttonModifyFileContent.Click += new System.EventHandler(this.buttonModifyFileContent_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 396);
            this.Controls.Add(this.buttonModifyFileContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxFilterExt);
            this.Controls.Add(this.buttonReadNameToTable);
            this.Controls.Add(this.buttonOutDirInfo);
            this.Controls.Add(this.buttonClearBeforeSpace);
            this.Controls.Add(this.buttonDonePath);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.textBoxReadMe);
            this.Controls.Add(this.textBoxCMD);
            this.Controls.Add(this.buttonUpdateCMD);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.dgv);
            this.Name = "MainForm";
            this.Text = "ReName";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.TextBox textBoxFilterExt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonReadNameToTable;
		private System.Windows.Forms.Button buttonOutDirInfo;
		private System.Windows.Forms.Button buttonClearBeforeSpace;
		private System.Windows.Forms.Button buttonDonePath;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.TextBox textBoxReadMe;
		private System.Windows.Forms.TextBox textBoxCMD;
		private System.Windows.Forms.Button buttonUpdateCMD;
		private System.Windows.Forms.Button buttonImport;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxPath;
		private System.Windows.Forms.DataGridViewTextBoxColumn DSTName;
		private System.Windows.Forms.DataGridViewTextBoxColumn SrcName;
		private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button buttonModifyFileContent;
	}
}

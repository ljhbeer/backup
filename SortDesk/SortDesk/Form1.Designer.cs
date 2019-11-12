namespace SortDesk
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemImportNameList = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAction = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDesktoList = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAddName = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAllowKeyInput = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAllowSpaceDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAllowChangeGap = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemChangeLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvstu = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstu)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.Location = new System.Drawing.Point(111, 27);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(772, 539);
            this.dgv.TabIndex = 4;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_CellPainting);
            this.dgv.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_DragDrop);
            this.dgv.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_DragEnter);
            this.dgv.DragOver += new System.Windows.Forms.DragEventHandler(this.dgv_DragOver);
            this.dgv.DragLeave += new System.EventHandler(this.dgv_DragLeave);
            this.dgv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFile,
            this.MenuItemAction});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNew,
            this.MenuItemOpen,
            this.MenuItemSave,
            this.MenuItemSaveAs,
            this.MenuItemImport,
            this.MenuItemExportExcel,
            this.MenuItemClose});
            this.MenuItemFile.Name = "MenuItemFile";
            this.MenuItemFile.Size = new System.Drawing.Size(41, 20);
            this.MenuItemFile.Text = "文件";
            // 
            // MenuItemNew
            // 
            this.MenuItemNew.Name = "MenuItemNew";
            this.MenuItemNew.Size = new System.Drawing.Size(124, 22);
            this.MenuItemNew.Text = "新建...";
            this.MenuItemNew.Click += new System.EventHandler(this.MenuItemNew_Click);
            // 
            // MenuItemOpen
            // 
            this.MenuItemOpen.Name = "MenuItemOpen";
            this.MenuItemOpen.Size = new System.Drawing.Size(124, 22);
            this.MenuItemOpen.Text = "打开...";
            this.MenuItemOpen.Click += new System.EventHandler(this.MenuItemOpen_Click);
            // 
            // MenuItemSave
            // 
            this.MenuItemSave.Name = "MenuItemSave";
            this.MenuItemSave.Size = new System.Drawing.Size(124, 22);
            this.MenuItemSave.Text = "保存...";
            this.MenuItemSave.Click += new System.EventHandler(this.MenuItemSave_Click);
            // 
            // MenuItemSaveAs
            // 
            this.MenuItemSaveAs.Name = "MenuItemSaveAs";
            this.MenuItemSaveAs.Size = new System.Drawing.Size(124, 22);
            this.MenuItemSaveAs.Text = "另存为...";
            this.MenuItemSaveAs.Click += new System.EventHandler(this.MenuItemSaveAs_Click);
            // 
            // MenuItemImport
            // 
            this.MenuItemImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemImportNameList});
            this.MenuItemImport.Name = "MenuItemImport";
            this.MenuItemImport.Size = new System.Drawing.Size(124, 22);
            this.MenuItemImport.Text = "导入";
            // 
            // MenuItemImportNameList
            // 
            this.MenuItemImportNameList.Name = "MenuItemImportNameList";
            this.MenuItemImportNameList.Size = new System.Drawing.Size(118, 22);
            this.MenuItemImportNameList.Text = "班级名单";
            this.MenuItemImportNameList.Click += new System.EventHandler(this.MenuItemImportNameList_Click);
            // 
            // MenuItemExportExcel
            // 
            this.MenuItemExportExcel.Name = "MenuItemExportExcel";
            this.MenuItemExportExcel.Size = new System.Drawing.Size(124, 22);
            this.MenuItemExportExcel.Text = "导出Excel";
            this.MenuItemExportExcel.Click += new System.EventHandler(this.MenuItemExportExcel_Click);
            // 
            // MenuItemClose
            // 
            this.MenuItemClose.Name = "MenuItemClose";
            this.MenuItemClose.Size = new System.Drawing.Size(124, 22);
            this.MenuItemClose.Text = "关闭";
            this.MenuItemClose.Click += new System.EventHandler(this.MenuItemClose_Click);
            // 
            // MenuItemAction
            // 
            this.MenuItemAction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemDesktoList,
            this.MenuItemAddName,
            this.MenuItemAllowKeyInput,
            this.MenuItemAllowSpaceDelete,
            this.MenuItemAllowChangeGap,
            this.MenuItemChangeLayout});
            this.MenuItemAction.Name = "MenuItemAction";
            this.MenuItemAction.Size = new System.Drawing.Size(41, 20);
            this.MenuItemAction.Text = "操作";
            // 
            // MenuItemDesktoList
            // 
            this.MenuItemDesktoList.Name = "MenuItemDesktoList";
            this.MenuItemDesktoList.Size = new System.Drawing.Size(142, 22);
            this.MenuItemDesktoList.Text = "座位到名单";
            this.MenuItemDesktoList.Click += new System.EventHandler(this.MenuItemDesktoList_Click);
            // 
            // MenuItemAddName
            // 
            this.MenuItemAddName.Name = "MenuItemAddName";
            this.MenuItemAddName.Size = new System.Drawing.Size(142, 22);
            this.MenuItemAddName.Text = "允许添加名单";
            this.MenuItemAddName.Click += new System.EventHandler(this.MenuItemAddName_Click);
            // 
            // MenuItemAllowKeyInput
            // 
            this.MenuItemAllowKeyInput.Name = "MenuItemAllowKeyInput";
            this.MenuItemAllowKeyInput.Size = new System.Drawing.Size(142, 22);
            this.MenuItemAllowKeyInput.Text = "允许快速输入";
            this.MenuItemAllowKeyInput.Click += new System.EventHandler(this.MenuItemAllowKeyInput_Click);
            // 
            // MenuItemAllowSpaceDelete
            // 
            this.MenuItemAllowSpaceDelete.Name = "MenuItemAllowSpaceDelete";
            this.MenuItemAllowSpaceDelete.Size = new System.Drawing.Size(142, 22);
            this.MenuItemAllowSpaceDelete.Text = "允许空格删除";
            this.MenuItemAllowSpaceDelete.Click += new System.EventHandler(this.MenuItemAllowSpaceDelete_Click);
            // 
            // MenuItemAllowChangeGap
            // 
            this.MenuItemAllowChangeGap.Name = "MenuItemAllowChangeGap";
            this.MenuItemAllowChangeGap.Size = new System.Drawing.Size(142, 22);
            this.MenuItemAllowChangeGap.Text = "允许调整过道";
            this.MenuItemAllowChangeGap.Click += new System.EventHandler(this.MenuItemAllowChangeGap_Click);
            // 
            // MenuItemChangeLayout
            // 
            this.MenuItemChangeLayout.Name = "MenuItemChangeLayout";
            this.MenuItemChangeLayout.Size = new System.Drawing.Size(142, 22);
            this.MenuItemChangeLayout.Text = "调整布局";
            this.MenuItemChangeLayout.Click += new System.EventHandler(this.MenuItemChangeLayout_Click);
            // 
            // dgvstu
            // 
            this.dgvstu.Location = new System.Drawing.Point(0, 27);
            this.dgvstu.Name = "dgvstu";
            this.dgvstu.RowTemplate.Height = 23;
            this.dgvstu.Size = new System.Drawing.Size(113, 539);
            this.dgvstu.TabIndex = 6;
            this.dgvstu.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_DragDrop);
            this.dgvstu.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_DragEnter);
            this.dgvstu.DragOver += new System.Windows.Forms.DragEventHandler(this.dgv_DragOver);
            this.dgvstu.DragLeave += new System.EventHandler(this.dgv_DragLeave);
            this.dgvstu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 566);
            this.Controls.Add(this.dgvstu);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dgv);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvstu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem MenuItemNew;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveAs;
        private System.Windows.Forms.ToolStripMenuItem MenuItemImport;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExportExcel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemClose;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAction;
        private System.Windows.Forms.DataGridView dgvstu;
        private System.Windows.Forms.ToolStripMenuItem MenuItemImportNameList;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDesktoList;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddName;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAllowKeyInput;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAllowSpaceDelete;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAllowChangeGap;
        private System.Windows.Forms.ToolStripMenuItem MenuItemChangeLayout;
    }
}


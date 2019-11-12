/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016-9-13
 * 时间: 15:03
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ReName
{
	/// <summary>
	/// Description of FormShow.
	/// </summary>
	public partial class FormShow : Form
	{
		public FormShow(string str)
		{
			InitializeComponent();			
			textBox1.Text = str;
		}
		
		void ButtonSaveClick(object sender, EventArgs e)
		{
			SaveFile(textBox1.Text);
		}
		private void SaveFile(string str){
			SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "txt文件";
            fd.Filter = "文本文件(*.txt)|*.txt";
            fd.FileName = "文本.txt";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName.EndsWith(".txt"))
                {
                	File.WriteAllText(fd.FileName,str);
                }
            }
		}
	}
}

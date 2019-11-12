using System;
using System.Windows.Forms;
using System.Drawing;

    class InputBox : Form
    {
        private Label labelText=new Label();
        private TextBox textboxValue=new TextBox();
        private Button buttonOK=new Button();
        private bool onlyNumeric;
        public InputBox()
        {
            Init();
        }

        private void Init()
        {
            this.Width = 400;
            this.Height = 150;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            labelText.AutoSize = true;
            labelText.Location = new Point(10, 20);
            textboxValue.Location = new Point(10, (this.ClientSize.Height - textboxValue.Height) / 2);
            textboxValue.Width = this.ClientSize.Width - 20;
            buttonOK.Text = "确定(&O)";
            buttonOK.Location = new Point((this.ClientSize.Width-buttonOK.Width)/2, this.ClientSize.Height - buttonOK.Height - 10);
            this.Controls.Add(labelText);
            this.Controls.Add (textboxValue);
            this.Controls.Add(buttonOK);
            this.AcceptButton=buttonOK;
            buttonOK.Click+=new EventHandler(buttonOK_Click);
            textboxValue.KeyPress += new KeyPressEventHandler(textboxValue_KeyPress);
        }

        void textboxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(onlyNumeric)
                if ((e.KeyChar < (char)Keys.D0 || e.KeyChar > (char)Keys.D9) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
        }

        /// <summary>
        /// InputBox的静态函数，返回输入的字符串
        /// </summary>
        /// <param name="Title">窗口标题</param>
        /// <param name="Text">提示文本</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns>返回字符串</returns>
        public static string Input(string Title, string Text, string DefaultValue)
        {
            InputBox inputBox = new InputBox();
            inputBox.Text = Title;
            inputBox.labelText.Text = Text;
            DialogResult result = inputBox.ShowDialog();
            if (result == DialogResult.OK)
                return inputBox.textboxValue.Text;
            else
                return DefaultValue;
        }

        /// <summary>
        /// InputBox的静态函数，返回输入的字符串
        /// </summary>
        /// <param name="Title">窗口标题</param>
        /// <param name="Text">提示文本</param>
        /// <param name="DefaultValue">默认值</param>
        /// <param name="OnlyNumeric">是否只允许输入数字</param>
        /// <returns>返回字符串</returns>
        public static string Input(string Title, string Text, string DefaultValue,bool OnlyNumeric)
        {
            InputBox inputBox = new InputBox();
            inputBox.Text = Title;
            inputBox.labelText.Text = Text;
            inputBox.onlyNumeric = OnlyNumeric;
            DialogResult result = inputBox.ShowDialog();
            if (result == DialogResult.OK)
                return inputBox.textboxValue.Text;
            else
                return DefaultValue;
        }

        private void buttonOK_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
//调用示例：
//string value=InputBox.Input(窗口标题,提示文本,默认返回值);
//只允许输入数字的InputBox调用：
//string value=InputBox.Input(窗口标题,提示文本,默认返回值,true);

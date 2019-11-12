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
            buttonOK.Text = "ȷ��(&O)";
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
        /// InputBox�ľ�̬����������������ַ���
        /// </summary>
        /// <param name="Title">���ڱ���</param>
        /// <param name="Text">��ʾ�ı�</param>
        /// <param name="DefaultValue">Ĭ��ֵ</param>
        /// <returns>�����ַ���</returns>
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
        /// InputBox�ľ�̬����������������ַ���
        /// </summary>
        /// <param name="Title">���ڱ���</param>
        /// <param name="Text">��ʾ�ı�</param>
        /// <param name="DefaultValue">Ĭ��ֵ</param>
        /// <param name="OnlyNumeric">�Ƿ�ֻ������������</param>
        /// <returns>�����ַ���</returns>
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
//����ʾ����
//string value=InputBox.Input(���ڱ���,��ʾ�ı�,Ĭ�Ϸ���ֵ);
//ֻ�����������ֵ�InputBox���ã�
//string value=InputBox.Input(���ڱ���,��ʾ�ı�,Ĭ�Ϸ���ֵ,true);

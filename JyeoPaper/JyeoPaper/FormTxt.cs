using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JyeoPaper
{
    public partial class FormTxt : Form
    {
        public FormTxt(string txt)
        {
            InitializeComponent();
            this.textBox1.Text = txt;
        }
    }
}

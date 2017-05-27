using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recepty
{
    public partial class Form4 : Form
    {
        public Form4(string menu)
        {
            InitializeComponent();
            this.menu = menu;
        }
        private string menu;

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = menu;
        }
    }
}

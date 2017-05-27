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
    public partial class Form3 : Form
    {
        public Form3(string Lst)
        {
            InitializeComponent();
            this.Lst = Lst;
        }

        private string Lst;
        
        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = Lst;
            
    
  

        }
    }
}

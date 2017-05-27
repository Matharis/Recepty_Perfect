using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Recepty
{
    public partial class Form2 : Form
    {
        public Form2(string DBpath,  List<Methods.Elements> Base)
        {
            InitializeComponent();
            this.DBPath = DBpath;
            this.Base = Base;
        }
        private string DBPath;
        private List<Methods.Elements> Base;
        //private Methods.Recp Recepts;
        ComboBox[] arr = new ComboBox[4];
        NumericUpDown[] arr2 = new NumericUpDown[4];
        private void Add_Click(object sender, EventArgs e)
        {

            string[] inp1 = new string[4];
            int[] inp2 = new int[4];

            inp1[0] = comboBox1.Text;
            inp1[1] = comboBox2.Text;
            inp1[2] = comboBox3.Text;
            inp1[3] = comboBox4.Text;

            inp2[0] = Convert.ToInt32(numericUpDown1.Value);
            inp2[1] = Convert.ToInt32(numericUpDown2.Value);
            inp2[2] = Convert.ToInt32(numericUpDown3.Value);
            inp2[3] = Convert.ToInt32(numericUpDown4.Value);


            string s = Methods.Combine(inp1, inp2, Base);
            Methods.AddRecp(DBPath, textBox1.Text, s, richTextBox1);


            //возвращаем все к исходному виду
            textBox1.Text = "";
            textBox2.Text = "";
            richTextBox1.Text = "";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Base;

            arr[0] = comboBox1;
            arr[1] = comboBox2;
            arr[2] = comboBox3;
            arr[3] = comboBox4;

            arr2[0] = numericUpDown1;
            arr2[1] = numericUpDown2;
            arr2[2] = numericUpDown3;
            arr2[3] = numericUpDown4;


            string[] srs1 = new string[Base.Count];
            string[] srs2 = new string[Base.Count];
            string[] srs3 = new string[Base.Count];
            string[] srs4 = new string[Base.Count];
            for (int i = 0; i < Base.Count; i++)
            {
                srs1[i] = Base[i].Name;
                srs2[i] = Base[i].Name;
                srs3[i] = Base[i].Name;
                srs4[i] = Base[i].Name;
            }
            comboBox1.DataSource = srs1;
            comboBox2.DataSource = srs2;
            comboBox3.DataSource = srs3;
            comboBox4.DataSource = srs4;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Text = Base[comboBox1.SelectedIndex].Name;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Text = Base[comboBox2.SelectedIndex].Name;





        }

    }
}

    


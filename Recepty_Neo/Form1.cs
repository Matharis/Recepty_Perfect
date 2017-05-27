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
    public partial class Number : Form
    {

        public Number()
        {
            InitializeComponent();
            Recepts = new List<Methods.Recp>();
            Day = new List<Methods.Recp>();
            CB_Rec.DisplayMember = "Name";
        }
        public Methods.Elements[] Base = new Methods.Elements[13];
        public List<Methods.Elements> Warehouse = new List<Methods.Elements>();
        public Label[] Arr = new Label[4];
        public Random randNum = new Random();
        public List<Methods.Recp> Recepts;
        public List<Methods.Recp> Day;
        public string DBPath;


        private void button1_Click(object sender, EventArgs e)
        {
            
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                //
                //Определяем путь к базе, Получаем лист по этому пути, Задаем этот лист как источник инфорации для комбоБокса(CB_Rec)
                //
                CB_Rec.DataSource = null;
                DBName.Text = openFileDialog1.FileName;
                DBPath = DBName.Text;
                string basepath = Path.GetDirectoryName(openFileDialog1.FileName);

                Recepts = Methods.Open(DBPath, Warehouse);
                
                openFileDialog1.FileName = String.Empty;
                CB_Rec.DataSource = Recepts;

                Day = Methods.Approve(Recepts, Warehouse);
                
                if (Day.Count > 0) label2.Text = "Сегодняшний рецепт дня:  " + Day[randNum.Next(0, Day.Count)].Name;
            }
        }

        private void CB_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            Arr[0] = label1;
            Arr[1] = label3;
            Arr[2] = label4;
            Arr[3] = label5;



            if (checkBox1.Checked)
            {
                checkBox1.Text = "Все" + "\r\n" + " Рецепты";
                //CB_Rec.DataSource = Day;
                Methods.Setindex(Arr, Day, CB_Rec, richTextBox1);
            }
            else
            {
                checkBox1.Text = "Доступные" + "\r\n" + " Рецепты";
                //CB_Rec.DataSource = Recepts;
                Methods.Setindex(Arr, Recepts, CB_Rec, richTextBox1);
            }
        }

        private void AddL_Click(object sender, EventArgs e)
        {
            Form2 m = new Form2(DBName.Text, Warehouse);
            m.Show();
            Recepts = Methods.Open(DBPath, Warehouse);
            CB_Rec.DataSource = Recepts;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ind = Methods.Search(Recepts, SRC.Text);
            if (ind >= 0) CB_Rec.SelectedIndex = ind;



        }

        private void button3_Click(object sender, EventArgs e)
        {
            Methods.DeleteRecept(Recepts, CB_Rec.SelectedIndex, DBName.Text);
            Recepts = Methods.Open(DBPath, Warehouse);
            CB_Rec.DataSource = Recepts;
        }

        private void button4_Click(object sender, EventArgs e)
        {
           //переоткрываем базу рецептов и чистим остатки от первого открытия
            CB_Rec.DataSource = null;
            DBName.Text = DBPath;
            Recepts.Clear();


            Recepts = Methods.Open(DBPath, Warehouse);
            CB_Rec.DataSource = Recepts;

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string ncl = "";
            ncl = Methods.Invoice(Day, Warehouse);

            Form3 n = new Form3(ncl);
            n.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string menu = "";
            int n = Convert.ToInt32(numer.Value);
            menu = Methods.Menu(Day, Warehouse, n);
            Form4 v = new Form4(menu);
            v.Show();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                Warehouse = Methods.ReadBaseList("Warehouse.txt");
                //
                //Определяем путь к базе, Получаем лист по этому пути, Задаем этот лист как источник инфорации для комбоБокса(CB_Rec)
                //
                CB_Rec.DataSource = null;
                DBName.Text = openFileDialog1.FileName;
                DBPath = DBName.Text;
                string basepath = Path.GetDirectoryName(openFileDialog1.FileName);

                Recepts = Methods.Open(DBPath, Warehouse);

                openFileDialog1.FileName = String.Empty;
                CB_Rec.DataSource = Recepts;

                Day = Methods.Approve(Recepts, Warehouse);

                if (Day.Count > 0) label2.Text = "Сегодняшний рецепт дня:  " + Day[randNum.Next(0, Day.Count)].Name;
            }

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //переоткрываем базу рецептов и чистим остатки от первого открытия
            CB_Rec.DataSource = null;
            DBName.Text = DBPath;
            Recepts.Clear();
            Recepts = Methods.Open(DBPath, Warehouse);
            CB_Rec.DataSource = Recepts;

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 m = new Form2(DBName.Text, Warehouse);
            m.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Methods.DeleteRecept(Recepts, CB_Rec.SelectedIndex, DBName.Text);
            CB_Rec.DataSource = Recepts;

        }

        private void Number_Load(object sender, EventArgs e)
        {
            Warehouse = Methods.ReadBaseList("Warehouse.txt");
        }

        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ncl = "";
            ncl = Methods.Invoice(Day, Warehouse);

            Form3 n = new Form3(ncl);
            n.Show();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Recepts = Methods.Open(DBPath, Warehouse);
            if (checkBox1.Checked)
            {
                checkBox1.Text = "Все" + "\r\n" + " Рецепты";
                CB_Rec.DataSource = Day;
            }
            else
            {
                checkBox1.Text = "Доступные" + "\r\n" + " Рецепты";
                CB_Rec.DataSource = Recepts;
            }
        }




    }
}
    

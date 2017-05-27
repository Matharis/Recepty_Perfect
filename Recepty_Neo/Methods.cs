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
    public static class Methods
    {
        

        public struct Elements
        {
            public string Name;
            public int Price;
            public int Number;
            public string Term;
        }
        
        public struct Recp
        {
            public string Name;
            public string RPth;
            public string IPth;
            public Elements[] cur;
            public override string ToString()
            {
                return Name;
            }
        }


        //Cчитываем Количество продуктов на складе по фиксированному пути
        public static List<Elements> ReadBaseList(string path)
        {
            StreamReader infile = new StreamReader(path);
            Elements r;
            List<Elements> Base = new List<Elements>();
            Base.Clear();
            while (!infile.EndOfStream)
            {
                //
                //
                //
                r.Name = infile.ReadLine();
                if (infile.EndOfStream)
                    break;


                string p = infile.ReadLine();
                r.Number = Convert.ToInt32(p);
                if (String.IsNullOrWhiteSpace(p))
                    continue;


                string p2 = infile.ReadLine();
                r.Price = Convert.ToInt32(p);
                if (String.IsNullOrWhiteSpace(p2))
                    continue;

                string p3 = infile.ReadLine();
                r.Term = p3;
                if (String.IsNullOrWhiteSpace(p3))
                    continue;

                Base.Add(r);
            }

            infile.Close();
            return Base;

        }

        //открываем базу данных рецептов
        public static List<Recp> Open(string path, List<Elements> Base)
        {

            List<Recp> Recepts = new List<Recp>();
            Recp r;

            string basepath = Path.GetDirectoryName(path);
            StreamReader infile = new StreamReader(path);
        
            Recepts.Clear();

            while (!infile.EndOfStream)
            {
                //
                // Считываем сначала первые три поля для элемента структуры Recp
                //
                r.Name = infile.ReadLine();
                if (infile.EndOfStream)
                    break;

                //заодно проверяем на отсутствие пустых строк

                string p = infile.ReadLine();
                if (String.IsNullOrWhiteSpace(p))
                    continue;

                r.RPth = Path.Combine(basepath, p);
                if (!File.Exists(r.RPth))
                    continue;

                string p2 = infile.ReadLine();
                if (String.IsNullOrWhiteSpace(p2))
                    continue;

                r.IPth = Path.Combine(basepath, p2);
                if (!File.Exists(r.RPth))
                    continue;

                StreamReader infile2 = new StreamReader(r.IPth);
                r.cur = new Elements[4];
                //
                //Тут считываем четвертый (он является масивом структур Elements)
                //
                for (int i = 0; i <= 3; i++)
                {
                    r.cur[i].Name = infile2.ReadLine();
                    r.cur[i].Number = Convert.ToInt32(infile2.ReadLine());
                    r.cur[i].Price = Base[i].Price;
                    r.cur[i].Term = Base[i].Term;
                }
                Recepts.Add(r);
                infile2.Close();
            }

            infile.Close();
            //
            //Возвращаем Лист, готовый к открытию в Form1
            //
            return Recepts;
        }       

   
        //проверяем элементы на возможность приготовления
        public static List<Recp> Approve(List<Recp> Recepts, List<Elements> Base) 
        {
            //
            //Возвращаем Лист, каждый рецепт которого мы можем приготовить хотя-бы один раз
            //
            List<Recp> Day = new List<Recp>();

                for (int i = 0; i < Recepts.Count; i++)
                {
                    int c = 0;
                    for(int i2 = 0; i2 < 4  ; i2 ++)
                    {

                        for (int i3 = 0; i3 < Base.Count; i3++)
                        {
                            if (c == 4) break;

                            if (Recepts[i].cur[i2].Number <= Base[i3].Number && Recepts[i].cur[i2].Name == Base[i3].Name)
                            {
                                c++;
                                break;
                            }
                                                           
                          
                        }
                    }
                  if(c == 4) Day.Add(Recepts[i]);
                }
            return Day;
        }


        //Перезаписываем базу данных рецептов без удаляемого объекта
        public static void DeleteRecept(List<Recp> Recepts, int ind , string path) 
        {
            //
            //Удаляем два файла и перезаписываем ссылки в файле базы (удаляет все элементы с соответствующим именем)
            //
            string[] arr = new string[(Recepts.Count) * 2];
            File.Delete(path);
            StreamWriter outfile = new StreamWriter(path, true);
            for (int i = 0; i < Recepts.Count; i++)
            {
                if (i != ind)
                {

                    outfile.WriteLine(Recepts[i].Name);
                    outfile.WriteLine(Recepts[i].Name + ".rtf");
                    outfile.WriteLine("ингридиенты" + Recepts[i].Name + ".txt");
                }
                if (i == ind)
                {
                    File.Delete(Recepts[i].RPth);
                    File.Delete(Recepts[i].IPth);
                }
            }
            outfile.Close();
        }

 
        //дописываем в базу данных еще один рецепт
        public static void AddRecp(string DBPath, String text1, String text2, RichTextBox RTB)
        {
            if (!File.Exists(DBPath))
            {
                MessageBox.Show("Не найден файл базы", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // добавление в базу
            StreamWriter outfile = new StreamWriter(DBPath, true);
            outfile.WriteLine(text1);
            string filename = text1;
            string filename2 = text1;
            //  замена плохих символов
            foreach (char c in @"""/\")
            filename = filename.Replace(c, '_');
            filename2 = "ингридиенты" + filename + ".txt";
            filename = filename + ".rtf";
            outfile.WriteLine(filename);
            outfile.WriteLine(filename2);

            outfile.Close();


            // сохранение файла рецепта
            string basepath = Path.GetDirectoryName(DBPath);
            filename = Path.Combine(basepath, filename);
            filename2 = Path.Combine(basepath, filename2);

            string[] s = text2.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            StreamWriter outfile2 = new StreamWriter(filename2, true);
            for (int i = 0; i < 8; i++)
            {
                outfile2.WriteLine(s[i]);
            }
            outfile2.Close();
            RTB.SaveFile(filename, RichTextBoxStreamType.RichText);
            return;
        }

 
        //позволяет увидеть всю досткпную информацию , касающуюся продуктов на базе
        public static string Invoice(List<Recp> Day, List<Elements> Base) 
        {
            //
            //создаем файл накладной, превращаем его в строку и передаем в Form3 
            //
            string ncl = "";
            for(int i = 0; i < Base.Count; i++)
            {
                ncl += (Base[i].Name + "\r\n");
                ncl += ("Number: " + Base[i].Number + "\r\n");
                ncl += ("Price: " + Base[i].Price + "\r\n");
                ncl += ("Term: " + Base[i].Term + "\r\n" + "\r\n");

            }
            return ncl;
        }

 
        //формирует список достуных блюд на заданное количество персон
        public static string Menu(List<Recp> Day, List<Elements> Base, int numer) 
        {
            //
            //создаем файл меню, превращаем его в строку и передаем в Form4 
            //
            string menu = "";
            int sum = 0;
            for (int i = 0; i < Day.Count; i++)
            {
                int n = numer;
                bool chk = false;
                for (int i2 = 0; i2 < 4; i2++)
                {
                    for (int i3 = 0; i3 < Base.Count; i3++)
                    {

                        if (Day[i].cur[i2].Number * n <= Base[i3].Number)
                        {
                            chk = true;
                            // n = i3;
                        }

                        else
                        {
                            chk = false;
                            break;
                        }
                    }
                }
                if (chk)
                {
                    for (int s = 0; s < 4; s++)
                    {
                        sum += Day[i].cur[s].Price;
                    }
                    menu += Day[i].Name + " Для " + n + " Персон: " + (sum * n) + "\r\n";
                }
                else menu = "Извините, мы не можем выполнить этот заказ ";

            }
            return menu;
        }

 
        //Cовмещает текст из КомбоБоксов и NumericUpDown Form2 в одну строку
        public static string Combine(string[] arr, int[] arr2, List<Elements> Base)
        {
            //
            //Пихаем все в одну строку
            //
            string res = "";
            string[] r = new string[8];
                for (int i = 0; i <= 3; i++)
                {
                    r[2 * i] = arr[i];
                    r[(2 * i) + 1] = Convert.ToString(arr2[i]);
                }
            for (int i = 0; i < r.Length; i++) 
            {
                res += (r[i] + "\r\n");
            }
            return res;

        }

  
        //для Корректного отображения выбираемого рецепта
        public static void Setindex(Label[] arr, List<Recp> Recepts, ComboBox CB_Rec,  RichTextBox richTextBox1) 
        {
            //
            // Ништяк для смены индекса в Чекбоксе. Также выводит количество ингридиентов выбранного блюда.
            //

            int index = CB_Rec.SelectedIndex;
            if (Recepts.Count > 0)
            {

                for (int i = 0; i < 4; i++) 
                {
                    arr[i].Text = "";
                }

                    if (index >= 0)
                    {
                        richTextBox1.Clear();
                        richTextBox1.LoadFile(Recepts[index].RPth, RichTextBoxStreamType.RichText);
                        for (int i = 0; i < 4; i++)
                        {
                            if (Recepts[index].cur[i].Number != 0) arr[i].Text = Recepts[index].cur[i].Name + ": " + Recepts[index].cur[i].Number;
                        }
                    }
            }
        }

  
        //поиск по имени
        public static int Search(List<Recp> Recepts, string text) 
        {
            //
            //Поиск по имени рецепта
            //
            int ind = -1;
            for (int i = 0; i < Recepts.Count; i++)
            {
                if (Recepts[i].Name == text)
                {
                    ind = i;
                    break;
                }
            }
            return ind;
        }
    }
}

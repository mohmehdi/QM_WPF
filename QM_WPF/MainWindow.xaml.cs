using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QM_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        int bits=4;
        int num = 0;
        //list of binery minterms
        List<string> inputs = new List<string>();
        //list of PIs
        List<string> PIlist = new List<string>();

        List<string> copinput = new List<string>();
        int[,] out_table;

        private  bool Check(string a, string b)
        {
            //برای چک کردن تعداد اختلاف ها
            int c = 0;
            for (int j = 0; j < a.Length; j++)
            {
                if ((a[j] == '_' && b[j] != '_') || (b[j] == '_' && a[j] != '_'))
                {
                    return false;
                }
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i] && !(a[i] == '_' || b[i] == '_'))
                    c++;
            }
            if (c == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private  string Show(List<string> list, int[,] table)
        {
            //استرینگ استفاده شده برای خروجی
            string final = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //استرینگ خروجی
            string simple = "";

            for (int i = 0; i < table.GetLength(1); i++)
            {
                //پیدا کردن پی آی اصلی
                if (table[table.GetLength(0) - 1, i] == 1)
                {
                    for (int j = 0; j < table.GetLength(0) - 1; j++)
                    {
                        //صفر کردن مینترم هایی که پی آی اصلی شامل میشه
                        if (table[j, i] == 1)
                        {
                            for (int p = 0; p < table.GetLength(1); p++)
                            {
                                if (table[j, p] == 1)
                                {
                                    for (int g = 0; g < table.GetLength(0); g++)
                                    {
                                        table[g, p] = 0;
                                    }
                                }
                            }
                            //تبدیل پی آی به جمله ساده شده
                            for (int k = 0; k < list[j].Length; k++)
                            {
                                if (list[j][k] == '0')
                                {
                                    simple +=  final[k]+ "'" ;

                                }
                                else if (list[j][k] == '1')
                                {
                                    simple += final[k];
                                }
                            }
                            simple += " + ";
                        }

                    }
                }

            }
            /*  //چاپ جدول
              Console.WriteLine();
              for (int f = 0; f < table.GetLength(0) ; f++)
              {
                  for (int n = 0; n < table.GetLength(1); n++)
                  {
                      Console.Write(table[f, n]);
                      Console.Write("   ");
                  }
                `  Console.WriteLine();
              }
              */

            //چک کردن باقی ماندن مینترم ها
            bool flag = false;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (table[table.GetLength(0) - 1, i] > 1)
                {
                    flag = true;
                }
            }
            while (flag)
            {
                //پیدا کردن دسته بزرگتر و اضافه به 
                //simple 

                //شمردن تعداد مینترم های هر پی آی
                int[] larger = new int[list.Count];
                for (int i = 0; i < table.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        if (table[i, j] == 1)
                        {
                            larger[i]++;
                        }
                    }
                }

                //پیدا کردن بزرگترین دسته و اندیس ان
                int max = larger[0];
                int andis = 0;
                for (int i = 1; i < larger.Length; i++)
                {
                    if (larger[i] > max)
                    {
                        max = larger[i];
                        andis = i;
                    }
                }
                //اضافه به جواب نهایی
                for (int k = 0; k < list[andis].Length; k++)
                {
                    if (list[andis][k] == '0')
                    {
                        simple +=  final[k]+"'";

                    }
                    else if (list[andis][k] == '1')
                    {
                        simple += final[k];
                    }
                }
                simple += " + ";


                //صفر کردن مینترم هایی که پی آی انتخاب شده شامل میشه
                for (int p = 0; p < table.GetLength(1); p++)
                {
                    if (table[andis, p] == 1)
                    {
                        for (int g = 0; g < table.GetLength(0); g++)
                        {
                            table[g, p] = 0;
                        }
                    }
                }


                //چک کردن باقی ماندن مینترم ها
                flag = false;
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[table.GetLength(0) - 1, i] > 1)
                    {
                        flag = true;
                    }
                }
            }

            return simple;

        }





        public MainWindow()
        {
            InitializeComponent();
            bit.Text = "";
            minterms.Text = "";
            output1.Text = "";
            minterms.IsEnabled = false;
            bit.IsEnabled = true;
            add.IsEnabled = false;
            finish.IsEnabled = false;
            pis.IsEnabled = false;
            Table.IsEnabled = false;
            
            output1.IsReadOnly = true;
        }

        private void Top_properties_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (minterms.Text == "")
            {


                minterms.IsEnabled = false;

                finish.IsEnabled = false;
                add.IsEnabled = false;
                pis.IsEnabled = true;
                Table.IsEnabled = true;



                int tedad = num;





                copinput = inputs;



                //.............................  دسته بندی  ............................//
                for (int i = 0; i < inputs.Count; i++)
                {
                    bool Is_combined = false;
                    for (int j = 0; j < inputs.Count; j++)
                    {
                        string str = "";
                        if (Check(inputs[i], inputs[j]))
                        {
                            Is_combined = true;
                            for (int k = 0; k < bits; k++)
                            {
                                if (inputs[i][k] == inputs[j][k])
                                {
                                    str += inputs[i][k];
                                }
                                else
                                {
                                    str += "_";
                                }
                            }
                            //اضافه کردن به لیست پی آی ها
                            inputs.Add(str);
                            str = "";
                            //  Console.WriteLine(inputs[i]+"  dasteh  " +inputs[j]);
                        }
                    }
                    //.......add PIs to list...........//
                    if (!Is_combined)
                    {
                        PIlist.Add(inputs[i]);
                    }
                }
                //.............   حذف تکراری   ..............//
                for (int i = 0; i < PIlist.Count; i++)
                {
                    for (int j = i + 1; j < PIlist.Count; j++)
                    {
                        if (PIlist[i] == PIlist[j])
                        {
                            PIlist.RemoveAt(j);
                            j--;
                        }
                    }
                }



                //.................check for including minterms..................//

                //  bool[,] table = new bool[PIlist.Count, tedad];
                //جدول مینترم بر حسب پی آی
                int[,] table = new int[PIlist.Count + 1, tedad];

                for (int j = 0; j < PIlist.Count; j++)
                {
                    for (int i = 0; i < tedad; i++)
                    {
                        bool flag = true;
                        for (int k = 0; k < bits; k++)
                        {
                            if (!(PIlist[j][k] == '_'))
                            {
                                if (PIlist[j][k] == copinput[i][k])
                                {
                                    continue;
                                }
                                else
                                {
                                    flag = false;
                                }
                            }
                        }
                        if (flag)
                        {
                            table[j, i] = 1;
                        }
                        else
                        {
                            table[j, i] = 0;
                        }
                    }
                }
                //counting number of 1 in table for each minterm
                for (int j = 0; j < tedad; j++)
                {
                    int p = 0;
                    int count = 0;
                    for (; p < PIlist.Count; p++)
                    {
                        if (table[p, j] == 1)
                        {
                            count++;
                        }
                    }
                    table[PIlist.Count, j] = count;
                }
                 out_table = new int[PIlist.Count + 1, tedad];
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        out_table[i,j] = table[i,j];
                    }
                }
            
                //رسم پی آی ها
                /*
                for (int i = 0; i < PIlist.Count + 1; i++)
                    {
                        for (int j = 0; j < tedad; j++)
                        {
                            Console.Write(table[i, j] + "   ");
                        }
                        Console.WriteLine();
                    }
                    */

                string final_final = Show(PIlist, table);

                for (int i = 0; i < final_final.Length - 2; i++)
                {

                    output1.Text += final_final[i];


                }
            }
            else
            {
                MessageBox.Show("add or clear minterm input");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (minterms.Text != "")
            {
                int bin = Convert.ToInt32(minterms.Text);
                if (bin > Math.Pow(2, bits))
                {
                    MessageBox.Show("enter valid numbers");
                    minterms.Text = "";
                }
                else
                {
                    num++;
                    inputs.Add(Convert.ToString(bin, 2).PadLeft(bits, '0'));
                    finish.IsEnabled = true;
                    minterms.Text = "";
                }
            }
            else
            {
                MessageBox.Show("enter something");
            }

        }

        private void Add_KeyDown(object sender, KeyEventArgs e)
        {
          if (e.Key==Key.Enter)
           {
                if (minterms.Text != "")
                {
                    int bin = Convert.ToInt32(minterms.Text);
                    if (bin > Math.Pow(2, bits))
                    {
                        MessageBox.Show("enter valid numbers");
                        minterms.Text = "";
                    }
                    else
                    {                
                    num++;
                    inputs.Add(Convert.ToString(bin, 2).PadLeft(bits, '0'));
                    finish.IsEnabled = true;
                    minterms.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("enter something");
                }
            }
        }

        private void Bit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {

                if (bit.Text!="")
                {
                bits = Convert.ToInt32(bit.Text);
                bit.IsEnabled = false;
                minterms.IsEnabled = true;
                    add.IsEnabled = true;
                
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.F1)
            {
                MessageBox.Show("by mohmehdi");
            }
        }

        private void Table_Click(object sender, RoutedEventArgs e)
        {
            output1.Text += "\r\n";
            Table.IsEnabled = false;
            for (int i = 0; i < out_table.GetLength(0); i++)
            {
                for (int j = 0; j < out_table.GetLength(1); j++)
                {
                    output1.Text += out_table[i, j] + "  ";
                }
                output1.Text += "\r\n";
            }
        }

        private void Pis_Click(object sender, RoutedEventArgs e)
        {
            pis.IsEnabled = false;
            output1.Text += "\r\n";
            for (int i = 0; i < PIlist.Count; i++)
            {
                output1.Text += PIlist[i]+"\r\n";
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            bit.Text = "";
            minterms.Text = "";
            output1.Text = "";
            minterms.IsEnabled = false;
            bit.IsEnabled = true;
            add.IsEnabled = false;
            finish.IsEnabled = false;
            pis.IsEnabled = false;
            Table.IsEnabled = false;
            
            output1.IsReadOnly = true;
            num = 0;
            bits = 0;

            //list of binery minterms
             inputs = new List<string>();
            PIlist = new List<string>();


             copinput = new List<string>();
             out_table=null;
        }
    }
}

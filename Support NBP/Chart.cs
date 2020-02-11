using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SupportNBP
{
    public partial class Chart : Form
    {
        Random rand = new Random();

        public Chart()
        {
            InitializeComponent();
            dateTimePicker1.MaxDate = dateTimePicker2.MaxDate = DateTime.Today;
            dateTimePicker1.MinDate = dateTimePicker2.MinDate = new DateTime(2015, 1, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = listBox1.GetItemText(listBox1.SelectedItem);
            if(text != "")
            {
                double min = 100000, max = 0;
                double diff = (dateTimePicker2.Value - dateTimePicker1.Value).TotalDays;

                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }

                if ((int)diff > 0)
                {
                    for (int i = 0; i <= (int)diff; i++)
                    {
                        double yolo = retCourse(text, i);
                        if (yolo != 0)
                        {
                            chart1.Series["CurrencyChart"].Points.AddXY(dateTimePicker1.Value.AddDays(i), yolo);
                            if (yolo < min)
                            {
                                min = yolo;
                            }
                            if (max < yolo)
                            {
                                max = yolo;
                            }
                        }

                    }

                    chart1.ChartAreas[0].AxisY.Maximum = max + 0.005;
                    chart1.ChartAreas[0].AxisY.Minimum = min - 0.005;
                }
                else
                {
                    MessageBox.Show("Cos sie cos sie stanelo i nie bylo mnie slychac");
                }
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            this.Hide();
            cl.Show();
        }

        private double retCourse(String firstCur, int sub)
        {
            XmlDocument waluty = new XmlDocument();
            double first = 0;

            bool ifGoodDate = false;

            while (!ifGoodDate)
            {
                try
                {
                    waluty.Load(@"Kursy\" + ChangeData(sub) + ".xml");
                    XmlNodeList spis_walut = waluty.GetElementsByTagName("pozycja");

                    foreach (XmlNode data in spis_walut)
                    {
                        if (data.LastChild.PreviousSibling.InnerText.Equals(firstCur))
                        {
                            first = Convert.ToDouble(data.LastChild.InnerText);
                            return first;
                        }
                    }

                    ifGoodDate = true;
                }
                catch
                {
                    return 0;
                }
            }

            return 0;
        }

        private string ChangeData(int i)
        {
            string date = dateTimePicker1.Value.AddDays(i).ToString();
            date = date.Substring(2).Replace("-", "").Substring(0,6).Trim();
            return date;
        }
    }
}

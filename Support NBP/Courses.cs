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
using System.IO;
using System.Net;

namespace SupportNBP
{
    public partial class Courses : Form
    {
        int count = 0;
        public Courses()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string date = dateTimePicker1.Value.ToString("yyMMdd");
            DataInGrid(date);
        }

        private void DataInGrid(string date)
        {
            XmlDocument currency = new XmlDocument();
            string currOfClient = textBox1.Text.ToString().ToUpper();
            try
            {
                currency.Load(@"Kursy\" + date + ".xml");
                XmlNodeList types_of_curr = currency.GetElementsByTagName("pozycja");
                foreach (XmlNode data in types_of_curr)
                {
                    if (textBox1.Text.ToString().Equals(""))
                    {
                        dataGridView1.Rows.Add(data.FirstChild.InnerText, data.FirstChild.NextSibling.InnerText, data.LastChild.PreviousSibling.InnerText, data.LastChild.InnerText);
                    }
                    else if (data.LastChild.PreviousSibling.InnerText.Contains(currOfClient))
                    {
                        dataGridView1.Rows.Add(data.FirstChild.InnerText, data.FirstChild.NextSibling.InnerText, data.LastChild.PreviousSibling.InnerText, data.LastChild.InnerText);
                    }

                }
                
            }
            catch
            {
                if(count == 0)
                {
                    MessageBox.Show("There's no data for today or data are crashed");
                    count++;
                }
                textBox1.Text = "";
                count = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            this.Hide();
            cl.Show();
        }
    }
}
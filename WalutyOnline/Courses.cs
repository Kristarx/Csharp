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

namespace WalutyOnline
{
    public partial class Courses : Form
    {
        public Courses()
        {
            InitializeComponent();
            dataGridView1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            dataGridView1.Visible = true;
            textBox1.Visible = true;
            label2.Visible = true;
            label1.Visible = false;
            pictureBox1.Width = 457;
            pictureBox1.Height = 477;
            pictureBox1.Location = new Point(496, 12);
            label3.Visible = true;
            dateTimePicker1.Visible = true;
            dateTimePicker1.MaxDate = DateTime.Now;
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
                foreach(XmlNode data in types_of_curr)
                {
                    if(textBox1.Text.ToString().Equals(""))
                    {
                        dataGridView1.Rows.Add(data.FirstChild.InnerText, data.FirstChild.NextSibling.InnerText, data.LastChild.PreviousSibling.InnerText, data.LastChild.InnerText);
                    }
                    else if(data.LastChild.PreviousSibling.InnerText.Contains(currOfClient))
                    {
                        dataGridView1.Rows.Add(data.FirstChild.InnerText, data.FirstChild.NextSibling.InnerText, data.LastChild.PreviousSibling.InnerText, data.LastChild.InnerText);
                    }
                    
                }
            }
            catch
            {
                MessageBox.Show("There's no data for today or data are crashed");
            }
        }
    }
}
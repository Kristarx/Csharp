using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SupportNBP
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            SetData();
        }

        private void SetData()
        {
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            String Question = "SELECT * FROM `klient` WHERE `id_kli` = @ID";

            conn.OpenConnection();
            msc.CommandText = Question;
            msc.Connection = conn.GetConnection();
            msc.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.gimme());
            msda.SelectCommand = msc;
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow userData = dtReg.Rows[0];
                label5.Text += " " + userData["imie"].ToString();
                label6.Text += " " + userData["nazwisko"].ToString();
                label7.Text += " " + userData["data_ur"].ToString();
                label8.Text += " " + userData["PESEL"].ToString();
                label9.Text += " " + userData["Miasto"].ToString();
                label10.Text += " " + userData["Adres"].ToString();
                String accNum = userData["NrKonta"].ToString();
                label2.Text += " " + accNum[0] + accNum[1];
                for(int i = 2; i < accNum.Length; i++)
                {
                    if((i-2)%4 == 0)
                    {
                        label2.Text += " " + accNum[i];
                    }
                    else
                    {
                        label2.Text += accNum[i];
                    }
                }
            }

            conn.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Courses cs = new Courses();
            cs.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Chart ch = new Chart();
            this.Hide();
            ch.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Zlecenie zl = new Zlecenie();
            this.Hide();
            zl.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1.setId();
            Form1 f = new Form1();
            f.Show();
            MessageBox.Show("Wylogowano", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

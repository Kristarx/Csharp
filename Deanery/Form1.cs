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

namespace Dziekanat
{
    public partial class Form1 : Form
    {
        //SELECT TIMESTAMPDIFF(month, dataStartu, '2020-12-03') FROM `student` WHERE imie = 'Krzysztof'
        public static String Id_kli = "";

        public Form1()
        {
            InitializeComponent();
           // textBox1.Text = textBox2.Text = "";
        }

        public static void setId()
        {
            Id_kli = "";
        }

        public static String retId()
        {
            return Id_kli;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();
            String QuesLog = "SELECT * FROM `uzytkownicy` WHERE `login` = @usn AND `haslo` = @pass";

            command.CommandText = QuesLog;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = textBox2.Text;

            adapter.SelectCommand = command;
            try
            {
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Id_kli = row["id_klienta"].ToString();
                    String status = row["typ_uzyt"].ToString();
                    if(status.Equals("s"))
                    {
                        student st = new student();
                        this.Hide();
                        st.Show();
                    }
                    else if (status.Equals("p"))
                    {
                        prowadzacy pr = new prowadzacy();
                        this.Hide();
                        pr.Show();
                    }
                    else if (status.Equals("a"))
                    {
                        admin ad = new admin();
                        this.Hide();
                        ad.Show();
                    }
                }

                else
                {
                    if (textBox1.Text.Trim().Equals("") && !textBox2.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your login!", "Empty login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }

                    else if (textBox2.Text.Trim().Equals("") && !textBox1.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your password!", "Empty password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }

                    else if (textBox1.Text.Trim().Equals("") && textBox2.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("Give your password and login!", "Empty login and password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = textBox1.Text = "";
                    }
                }

            }
            catch
            {
                MessageBox.Show("Unable to connect with database!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

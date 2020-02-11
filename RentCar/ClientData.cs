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

namespace RentCar
{
    public partial class ClientData : Form
    {
        public String haslo = "";

        public void setHaslo()
        {
            haslo = "";
        }

        public void getHaslo(String pass)
        {
            haslo = pass;
        }

        public ClientData()
        {
            InitializeComponent();
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client cl = new Client();
            this.Hide();
            cl.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(button3.Text.Equals("Kliknij by zmienić dane"))
            {
                button3.Text = "Zatwierdź zmiany";
                textBox1.Enabled = true; textBox1.ReadOnly = false;
                textBox2.Enabled = true; textBox2.ReadOnly = false;
                textBox3.Enabled = true; textBox3.ReadOnly = false;
                textBox4.Enabled = true; textBox4.ReadOnly = false;
                textBox5.Enabled = true; textBox5.ReadOnly = false;
                textBox7.Enabled = true; textBox7.ReadOnly = false;
                textBox8.Enabled = true; textBox8.ReadOnly = false;
                textBox9.Enabled = true; textBox9.ReadOnly = false;
            }

            else if(button3.Text.Equals("Zatwierdź zmiany"))
            { 
                String updateChanges = "UPDATE `klienci` SET `IMIE`= @imie,`NAZWISKO`= @nazw,`NR_PRAWA_JAZDY`= @nr,`KATEGORIA_PJ`= @kat,`MIEJSCOWOSC`= @miejsc,`ULICA`= @ul WHERE ID_KLI = @ID";
                String updatePass = "UPDATE `uzytkownicy` SET `HASLO` = @haslo WHERE ID_UZYT = @ID";
                Connection conn = new Connection();
                DataTable dtReg = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();
                MySqlCommand command2 = new MySqlCommand();

                conn.OpenConnection();

                command.CommandText = updateChanges;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@imie", MySqlDbType.VarChar).Value = textBox1.Text;
                command.Parameters.Add("@nazw", MySqlDbType.VarChar).Value = textBox2.Text;
                command.Parameters.Add("@nr", MySqlDbType.VarChar).Value = textBox4.Text;
                command.Parameters.Add("@kat", MySqlDbType.VarChar).Value = textBox3.Text;
                command.Parameters.Add("@miejsc", MySqlDbType.VarChar).Value = textBox8.Text;
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = textBox7.Text;
                command.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.gimme());
                command.ExecuteNonQuery();

                if(textBox9.Text != "")
                {
                    if(textBox9.Text.Equals(textBox5.Text))
                    {
                        command2.CommandText = updatePass;
                        command2.Connection = conn.GetConnection();
                        command2.Parameters.Add("@haslo", MySqlDbType.VarChar).Value = textBox9.Text;
                        command2.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.gimme());
                        command2.ExecuteNonQuery();
                        MessageBox.Show("Password changed correctly");
                    }
                    else
                    {
                        MessageBox.Show("Passwords are not equal!");
                    }
                }

                conn.CloseConnection();

                Init();
                button3.Text = "Kliknij by zmienić dane";
            }
        }

        private void Init()
        {
            textBox1.Enabled = false;   textBox1.ReadOnly = true;
            textBox2.Enabled = false;   textBox2.ReadOnly = true;
            textBox3.Enabled = false;   textBox3.ReadOnly = true;
            textBox4.Enabled = false;   textBox4.ReadOnly = true;
            textBox5.Enabled = false;   textBox5.ReadOnly = true;
            textBox6.Enabled = false;   textBox6.ReadOnly = true;
            textBox7.Enabled = false;   textBox7.ReadOnly = true;
            textBox8.Enabled = false;   textBox8.ReadOnly = true;
            textBox9.Enabled = false;   textBox9.ReadOnly = true;

            getUserData();
        }

        private void getUserData()
        {
            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            String Question = "SELECT * FROM klienci K JOIN uzytkownicy u ON u.ID_UZYT = k.ID_KLI WHERE k.ID_KLI = @ID";

            conn.OpenConnection();

            command.CommandText = Question;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.gimme());
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                DataRow row = dtReg.Rows[0];
                textBox1.Text = row["IMIE"].ToString();
                textBox2.Text = row["NAZWISKO"].ToString();
                textBox4.Text = row["NR_PRAWA_JAZDY"].ToString();
                textBox3.Text = row["KATEGORIA_PJ"].ToString();
                textBox8.Text = row["MIEJSCOWOSC"].ToString();
                textBox7.Text = row["ULICA"].ToString();
                textBox6.Text = row["LOGIN"].ToString();
                textBox5.Text = "";
                textBox9.Text = "";
                getHaslo(row["HASLO"].ToString());
            }
            conn.CloseConnection();
        }
    }
}

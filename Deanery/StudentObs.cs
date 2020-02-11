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
    public partial class StudentObs : Form
    {
        public StudentObs()
        {
            InitializeComponent();
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            admin ad = new admin();
            this.Hide();
            ad.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chooseSpecStu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showAllStudents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Visible == true)
            {
                if(listBox2.GetItemText(listBox2.SelectedItem) != "")
                {
                    String updateSem = "UPDATE `student` SET`semestr`= @semestr WHERE INDEKS = @INDEKS";

                    Connection conn = new Connection();
                    MySqlDataAdapter msda = new MySqlDataAdapter();
                    MySqlCommand msc = new MySqlCommand();

                    conn.OpenConnection();

                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string indeks = Convert.ToString(selectedRow.Cells["INDEKS"].Value);
                    string semester = listBox2.GetItemText(listBox2.SelectedItem);


                    msc.CommandText = updateSem;
                    msc.Connection = conn.GetConnection();
                    msc.Parameters.Add("@semestr", MySqlDbType.Int16).Value = Convert.ToInt16(semester);
                    msc.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = indeks;

                    if (msc.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Update correct");
                    }
                    else
                    {
                        MessageBox.Show("Something goes wrong");
                    }
                    conn.CloseConnection();
                    showAllStudents();
                }
            }

            if(dataGridView2.Visible == true)
            {
                String updateSem = "UPDATE `student_przedmiot_ocena` SET `id_oceny`= @id_oceny WHERE INDEKS = @INDEKS AND id_przedmiotu = @id_przedmiotu";
                
                Connection conn = new Connection();
                MySqlDataAdapter msda = new MySqlDataAdapter();
                MySqlCommand msc = new MySqlCommand();

                conn.OpenConnection();

                msc.CommandText = updateSem;
                msc.Connection = conn.GetConnection();
                msc.Parameters.Add("@id_oceny", MySqlDbType.Int16).Value = Convert.ToInt16(getRatID());
                msc.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = textBox1.Text;
                msc.Parameters.Add("@id_przedmiotu", MySqlDbType.VarChar).Value = getSubID();

                if (msc.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Update correct");
                }
                else
                {
                    MessageBox.Show("Something goes wrong");
                }
                conn.CloseConnection();

                chooseSpecStu();
            }
        }

        private void showAllStudents()
        {
            textBox1.Text = "";
            String student = "SELECT * FROM student WHERE semestr >= 1 AND semestr <=7 ORDER BY semestr ASC";
            String ID = "", PESEL = "", imie = "", nazwisko = "", INDEKS = "", typ_uzy = "", dataStartu = "", semestr = "";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = student;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dt);

            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (DataRow row in dt.Rows)
            {
                ID = row["ID"].ToString();
                PESEL = row["PESEL"].ToString();
                imie = row["imie"].ToString();
                nazwisko = row["nazwisko"].ToString();
                INDEKS = row["INDEKS"].ToString();
                typ_uzy = row["typ_uzyt"].ToString();
                dataStartu = row["dataStartu"].ToString();
                semestr = row["semestr"].ToString();

                dataGridView1.Rows.Add(ID, PESEL, imie, nazwisko, INDEKS, typ_uzy, dataStartu.Substring(0, 10), semestr);
            }

            conn.CloseConnection();
        }

        private String getSubID()
        {
            String idPrzedmiotu = "SELECT id_przedmiotu FROM przedmiot WHERE nazwa = @nazwa";
            String ID = "";

            int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];
            string Sub = Convert.ToString(selectedRow.Cells["Przedmiot"].Value);

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = idPrzedmiotu;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msc.Parameters.Add("@nazwa", MySqlDbType.VarChar).Value = Sub.Trim();
            msda.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                DataRow row = dt.Rows[0];
                ID = row["id_przedmiotu"].ToString();
            }

            conn.CloseConnection();
            return ID;
        }

        private String getRatID()
        {
            String idOceny = "SELECT id_oceny FROM ocena WHERE wartosc = @wartosc";
            String ID = "";
            String RatID = listBox1.GetItemText(listBox1.SelectedItem);

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = idOceny;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msc.Parameters.Add("@wartosc", MySqlDbType.VarChar).Value = RatID.Trim();
            msda.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                DataRow row = dt.Rows[0];
                ID = row["id_oceny"].ToString();
            }
            conn.CloseConnection();
            return ID;
        }

        private void chooseSpecStu()
        {
            if (textBox1.Text != "")
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;

                String specStudent = "SELECT p.nazwa, o.wartosc, p.nr_sem FROM student_przedmiot_ocena spo JOIN przedmiot p ON spo.id_przedmiotu = p.id_przedmiotu JOIN ocena o ON spo.id_oceny = o.id_oceny WHERE spo.indeks = @INDEKS ORDER BY p.nr_sem ASC";
                String nazwa = "", wartosc = "", semestr = "";
                Connection conn = new Connection();
                DataTable dt = new DataTable();
                MySqlDataAdapter msda = new MySqlDataAdapter();
                MySqlCommand msc = new MySqlCommand();

                conn.OpenConnection();

                msc.CommandText = specStudent;
                msc.Connection = conn.GetConnection();
                msda.SelectCommand = msc;
                msc.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = textBox1.Text;
                msda.Fill(dt);

                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();

                foreach (DataRow row in dt.Rows)
                {
                    nazwa = row["nazwa"].ToString();
                    wartosc = row["wartosc"].ToString();
                    semestr = row["nr_sem"].ToString();
                    dataGridView2.Rows.Add(nazwa, semestr, wartosc);
                }

                conn.CloseConnection();

            }
            else
            {
                MessageBox.Show("Write Index Number!");
            }
        }
    }
}

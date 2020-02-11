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
    public partial class prowadzacy : Form
    {
        public prowadzacy()
        {
            InitializeComponent();
            getSubjects();
        }
        
        private void getSubjects()
        {
            String getSub = "SELECT p.nazwa, p.nr_sem FROM przedmiot p JOIN prowadzacy pr ON p.id_prow = pr.id WHERE p.id_prow = @ID";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();


            command.CommandText = getSub;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.retId());
            adapter.SelectCommand = command;
            adapter.Fill(dt);

            listBox1.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                List<string> MyList = new List<string>();
                DateTime moment = DateTime.Today;
                int year = moment.Year;
                int parzysty = 0;

                DateTime date1 = new DateTime(year, 4, 1);
                DateTime date2 = new DateTime(year, 9, 30);
                
                if (DateTime.Compare(moment, date1) < 0 || DateTime.Compare(moment, date2) > 0)
                {
                    parzysty = 0;
                }
                else
                {
                    parzysty = 1;
                }

                foreach (DataRow row in dt.Rows)
                {
                    int sem = Convert.ToInt16(row["nr_sem"]);
                    if(parzysty == 0)
                    {
                        if(sem%2!=0)
                        {
                            MyList.Add(row["nazwa"].ToString());
                        }
                    }
                    else if(parzysty == 1)
                    {
                        if (sem % 2 == 0)
                        {
                            MyList.Add(row["nazwa"].ToString());
                        }
                    }
                    
                }
                listBox1.DataSource = MyList;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showDataOnGrid();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            Connection conn = new Connection();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();
            DataTable dt = new DataTable();
            DataTable d2 = new DataTable();
            String getSubSem = "SELECT id_przedmiotu, nr_sem FROM przedmiot WHERE nazwa = @nazwa";
            String wstawOcene = "UPDATE `student_przedmiot_ocena` SET `id_oceny`= @id_oceny WHERE INDEKS = @INDEKS AND id_przedmiotu = @id_przedmiotu";
            String getID = "SELECT id_oceny FROM ocena WHERE wartosc = @wartosc";
            String id_przed = "", id_oceny = "";

            conn.OpenConnection();

            command.CommandText = getSubSem;
            command.Connection = conn.GetConnection();
            command.Parameters.Add("@nazwa", MySqlDbType.VarChar).Value = listBox1.GetItemText(listBox1.SelectedItem);
            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                id_przed = row["id_przedmiotu"].ToString();
            }

            string text = listBox2.GetItemText(listBox2.SelectedItem);
            if(text != "")
            {
                command.CommandText = getID;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@wartosc", MySqlDbType.VarChar).Value = listBox2.GetItemText(listBox2.SelectedItem);
                adapter.SelectCommand = command;
                adapter.Fill(d2);

                if (d2.Rows.Count > 0)
                {
                    DataRow row = d2.Rows[0];
                    id_oceny = row["id_oceny"].ToString();
                }

                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string indeks = Convert.ToString(selectedRow.Cells["Indeks"].Value);

                    command.CommandText = wstawOcene;
                    command.Connection = conn.GetConnection();
                    command.Parameters.Add("@id_oceny", MySqlDbType.Int16).Value = Convert.ToInt16(id_oceny);
                    command.Parameters.Add("@INDEKS", MySqlDbType.VarChar).Value = indeks;
                    command.Parameters.Add("@id_przedmiotu", MySqlDbType.Int16).Value = Convert.ToInt16(id_przed);

                    if ((command.ExecuteNonQuery() == 1))
                    {
                        MessageBox.Show("Your data has been updated");
                    }
                }

            }

            conn.CloseConnection();
            showDataOnGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1.setId();
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void showDataOnGrid()
        {
            String getSubSem = "SELECT id_przedmiotu, nr_sem FROM przedmiot WHERE nazwa = @nazwa";
            String getOcena = "SELECT s.imie, s.nazwisko, s.INDEKS, o.wartosc FROM student s JOIN student_przedmiot_ocena spo ON s.INDEKS = spo.INDEKS JOIN ocena o ON spo.id_oceny = o.id_oceny JOIN przedmiot p ON spo.id_przedmiotu = p.id_przedmiotu WHERE s.semestr = @semestr AND p.id_przedmiotu = @ID";
            String name = "", surname = "", indeks = "", whichSem = "", ocena = "", id_przed = "";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();


            if (listBox1.GetItemText(listBox1.SelectedItem) != "")
            {
                command.CommandText = getSubSem;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@nazwa", MySqlDbType.VarChar).Value = listBox1.GetItemText(listBox1.SelectedItem);
                adapter.SelectCommand = command;
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    whichSem = row["nr_sem"].ToString();
                    id_przed = row["id_przedmiotu"].ToString();
                }

                command.CommandText = getOcena;
                command.Connection = conn.GetConnection();
                command.Parameters.Add("@semestr", MySqlDbType.Int16).Value = whichSem;
                command.Parameters.Add("@ID", MySqlDbType.Int16).Value = id_przed;
                adapter.SelectCommand = command;
                adapter.Fill(dt2);

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow row in dt2.Rows)
                    {
                        name = row["imie"].ToString();
                        surname = row["nazwisko"].ToString();
                        indeks = row["INDEKS"].ToString();
                        ocena = row["wartosc"].ToString();
                        dataGridView1.Rows.Add(name, surname, indeks, listBox1.GetItemText(listBox1.SelectedItem), whichSem, ocena);
                    }

                }
            }
        }

    }
}

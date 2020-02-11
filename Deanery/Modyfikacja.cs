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
    public partial class Modyfikacja : Form
    {
        public Modyfikacja()
        {
            InitializeComponent();
            pobierzProwadzacych();
            pobierzPrzedmioty();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin ad = new admin();
            ad.Show();
            this.Hide();
        }

        private void pobierzProwadzacych()
        {
            String MaxValue = "SELECT * FROM prowadzacy";
            String ID = "", name = "", surname = "", PESEL = "";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dt);

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ID = row["id"].ToString();
                    name = row["imie"].ToString();
                    surname = row["nazwisko"].ToString();
                    PESEL = row["PESEL"].ToString();
                    dataGridView1.Rows.Add(ID, name, surname, PESEL);
                }
            }

            conn.CloseConnection();
        }

        private void pobierzPrzedmioty()
        {
            String MaxValue = "SELECT * FROM przedmiot";
            String ID = "", nazwa = "";

            Connection conn = new Connection();
            DataTable dt = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();

            conn.OpenConnection();

            msc.CommandText = MaxValue;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msda.Fill(dt);

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ID = row["id_przedmiotu"].ToString();
                    nazwa = row["nazwa"].ToString();
                    dataGridView2.Rows.Add(ID, nazwa);
                }
            }

            conn.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0 && dataGridView2.SelectedCells.Count > 0)
            {
                String updateSub = "UPDATE `przedmiot` SET`id_prow`= @IDprow WHERE id_przedmiotu = @IDprzed";

                Connection conn = new Connection();
                MySqlDataAdapter msda = new MySqlDataAdapter();
                MySqlCommand msc = new MySqlCommand();

                conn.OpenConnection();

                int selectedrowindex1 = dataGridView1.SelectedCells[0].RowIndex;
                int selectedrowindex2 = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow1 = dataGridView1.Rows[selectedrowindex1];
                DataGridViewRow selectedRow2 = dataGridView2.Rows[selectedrowindex2];
                string indeks1 = Convert.ToString(selectedRow1.Cells["ID"].Value);
                string indeks2 = Convert.ToString(selectedRow2.Cells["IDprow"].Value);

                msc.CommandText = updateSub;
                msc.Connection = conn.GetConnection();
                msc.Parameters.Add("@IDprow", MySqlDbType.Int16).Value = Convert.ToInt16(indeks1);
                msc.Parameters.Add("@IDprzed", MySqlDbType.Int16).Value = Convert.ToInt16(indeks2);

                if(msc.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Update correct");
                }
                else
                {
                    MessageBox.Show("Something goes wrong");
                }
                conn.CloseConnection();
            }
            else
            {
                MessageBox.Show("Choose data from both data Grid!");
            }
        }
    }
}

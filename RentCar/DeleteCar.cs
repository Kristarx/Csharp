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
using System.IO;

namespace RentCar
{
    public partial class DeleteCar : Form
    {
        String idSam = "", marka = "", model = "", rok = "", nrRej = "";

        public DeleteCar()
        {
            InitializeComponent();
            initDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin ad = new admin();
            ad.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                String ques = "SELECT ID_SAM, MARKA, MODEL, ROK_PROD, NR_REJ FROM samochody WHERE NR_REJ LIKE '" + textBox1.Text.Trim() + "%'";
                string IDSam = "", marka = "", model = "", rok = "", nrRej = "";

                Connection conn = new Connection();
                DataTable dtReg = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();

                conn.OpenConnection();

                command.CommandText = ques;
                command.Connection = conn.GetConnection();
                adapter.SelectCommand = command;
                adapter.Fill(dtReg);

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                foreach (DataRow rows in dtReg.Rows)
                {
                    IDSam = rows["ID_SAM"].ToString();
                    marka = rows["MARKA"].ToString();
                    model = rows["MODEL"].ToString();
                    rok = rows["ROK_PROD"].ToString();
                    nrRej = rows["NR_REJ"].ToString();
                    dataGridView1.Rows.Add(IDSam, marka, model, rok, nrRej);
                }

                conn.CloseConnection();
            }
            else
            {
                initDataGrid();
            }
            
        }

        private void deleteCarsFromWypoz(int id)
        {
            String ques = "DELETE FROM `wypozyczenia` WHERE DATA_WYP >= CURRENT_DATE AND ID_SAM = @IDSam";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            command.Parameters.Add("@IDSam", MySqlDbType.Int16).Value = id;
            command.ExecuteNonQuery();
        }

        private void deleteSpecCar(int id)
        {
            String ques = "DELETE FROM `samochody` WHERE ID_SAM = @IDSam";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            command.Parameters.Add("@IDSam", MySqlDbType.Int16).Value = id;
            command.ExecuteNonQuery();
        }

        private void initDataGrid()
        {
            String ques = "SELECT ID_SAM, MARKA, MODEL, ROK_PROD, NR_REJ FROM samochody";
            string IDSam = "", marka = "", model = "", rok = "", nrRej = "";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            adapter.Fill(dtReg);

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            foreach (DataRow rows in dtReg.Rows)
            {
                IDSam = rows["ID_SAM"].ToString();
                marka = rows["MARKA"].ToString();
                model = rows["MODEL"].ToString();
                rok = rows["ROK_PROD"].ToString();
                nrRej = rows["NR_REJ"].ToString();
                dataGridView1.Rows.Add(IDSam, marka, model, rok, nrRej);
            }

            conn.CloseConnection();
        }

        private void checkIfDeleteFolder(String marka, String model, String rok)
        {
            String ques = "SELECT COUNT(ID_SAM) FROM samochody WHERE marka = @marka AND model = @model AND ROK_PROD = @rok";

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            conn.OpenConnection();

            command.CommandText = ques;
            command.Connection = conn.GetConnection();
            adapter.SelectCommand = command;
            command.Parameters.Add("@marka", MySqlDbType.VarChar).Value = marka;
            command.Parameters.Add("@model", MySqlDbType.VarChar).Value = model;
            command.Parameters.Add("@rok", MySqlDbType.VarChar).Value = rok;
            adapter.Fill(dtReg);

            DataRow row = dtReg.Rows[0];

            if(Convert.ToInt16(row["COUNT(ID_SAM)"].ToString()) == 0)
            {
                Directory.Delete(@"C:/Users/g580/Documents/Programming/C++, C#/C#/Applications/RentCar/RentCar/RentCar/bin/Debug/Cars/" + marka + " " + model + " " + rok, true);
            }

            conn.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            deleteCarsFromWypoz(Convert.ToInt16(idSam));
            deleteSpecCar(Convert.ToInt16(idSam));
            checkIfDeleteFolder(marka, model, rok);
            MessageBox.Show("Car deleted");
            admin ad = new admin();
            ad.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {        
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow1 = dataGridView1.Rows[selectedrowindex];
            idSam = Convert.ToString(selectedRow1.Cells[0].Value);
            marka = Convert.ToString(selectedRow1.Cells[1].Value);
            model = Convert.ToString(selectedRow1.Cells[2].Value);
            rok = Convert.ToString(selectedRow1.Cells[3].Value);
            nrRej = Convert.ToString(selectedRow1.Cells[4].Value);
        }
    }

    //wyslac projekt kod
    //rysunek z serwerami - architektura, aplikacja klienta (serwer aplikacji, serwer baz danych)
    //podpisac metody co robia
    //dodac kilka zapytan
    //dodac indeks
    //dodac indeks dla marki, silnika itd w ALTER TABLE
    //pokazac SELECT i powiedziec ze poprawiamy zapytanie bo wprowadzamy indeks
}

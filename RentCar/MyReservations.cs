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
    public partial class MyReservations : Form
    {
        public MyReservations()
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

        private void Init()
        {
            dataGridView1.Rows.Clear();

            Connection conn = new Connection();
            DataTable dtReg = new DataTable();
            MySqlDataAdapter msda = new MySqlDataAdapter();
            MySqlCommand msc = new MySqlCommand();
   

            String Question = "SELECT k.imie, k.nazwisko, s.*, w.* FROM klienci k JOIN uzytkownicy u ON k.ID_KLI = u.ID_UZYT JOIN wypozyczenia w ON w.ID_KLI = k.ID_KLI JOIN samochody s ON w.ID_SAM = s.ID_SAM WHERE w.ID_KLI = @ID"; //AND w.DATA_ZWR <= '2019-12-31' AND W.DATA_WYP >'2019-12-27'
            String marka = "", model = "", nrRej = "", rokProd = "", krajProd = "", pojSil = "", dataWyp = "", dataZwr = "", calkKoszt = "", status = "";

            conn.OpenConnection();

            msc.CommandText = Question;
            msc.Connection = conn.GetConnection();
            msda.SelectCommand = msc;
            msc.Parameters.Add("@ID", MySqlDbType.Int16).Value = Convert.ToInt16(Form1.gimme());
            msda.Fill(dtReg);

            if (dtReg.Rows.Count != 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                foreach (DataRow row in dtReg.Rows)
                {
                    marka = row["MARKA"].ToString();
                    model = row["MODEL"].ToString();
                    nrRej = row["NR_REJ"].ToString();
                    rokProd = row["ROK_PROD"].ToString();
                    krajProd = row["KRAJ_PROD"].ToString();
                    pojSil = row["POJ_SIL"].ToString();
                    dataWyp = row["DATA_WYP"].ToString().Substring(0,10);
                    dataZwr = row["DATA_ZWR"].ToString().Substring(0,10);
                    calkKoszt = row["KOSZT"].ToString();
                    DateTime date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                    int firstDate = DateTime.Compare(DateTime.Parse(dataZwr), date);
                    int secondDate = DateTime.Compare(DateTime.Parse(dataWyp), date);
                    if (firstDate > 0 && secondDate > 0)
                    {
                        status = "Zaplanowane";
                    }
                    else if(firstDate < 0 && secondDate < 0)
                    {
                        status = "Zakonczone";
                    }
                    else
                    {
                        status = "W trakcie";
                    }
                    dataGridView1.Rows.Add(marka, model, nrRej, rokProd, krajProd, pojSil, dataWyp, dataZwr, calkKoszt, status);
                }
            }
        }
    }
}

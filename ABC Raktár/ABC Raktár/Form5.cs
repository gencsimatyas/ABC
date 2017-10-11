using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ABC_Raktár
{
    public partial class Form5 : Form
    {
        public string username, rang;
        public string connect, dbutvonal;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader datar; // ebbe lesz kiolvasva a cucc

        public Form5()
        {
            InitializeComponent();
            adatbazis_utvonal();
            dropdown_upload();
        }


        private void user()
        {
            TextReader user = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\user.txt");
            username = user.ReadLine();
            rang = user.ReadLine();
            user.Close();

            switch (rang)
            {
                case "ruser":
                    {
                        rang = "Raktáros";
                    } break;

                case "euser":
                    {
                        rang = "Pénztáros";
                    } break;

                case "admin":
                    {
                        rang = "Adminisztrátor";
                    } break;
            }
        }
        private class Item
        {
            public string Name;
            public int Value;
            public Item(string name, int value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            dbutvonal = utvonal.ReadLine();

            connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

            utvonal.Close();
        }

        private void dropdown_upload()
        {
            try
            {

                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand(); //letrehozza a command obejktumot

                command.CommandText = "Select * from Kategória";
                datar = command.ExecuteReader();//nem valtoztat az adatbazison "select"

                while (datar.Read())
                {
                    comboBox1.Items.Add(new Item(datar["Név"].ToString(), Convert.ToInt32(datar["Azonosító"])));
                }

                datar.Close();



            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        public void Torles_LOG(string nev, string id)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Törölte a következő kategóriát: " + id + " | " + nev + "')";
            command.ExecuteNonQuery();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
                {
            Item itm = (Item)comboBox1.SelectedItem;
            string katnev, katid;

            katnev = itm.Name;
            katid = itm.Value.ToString() ;

            DialogResult result = MessageBox.Show("Biztos vagy benne, hogy törölni akarod a következő kategóriát: " + katnev + ", és a kategóriába tartozó összes árut?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                
                    command.CommandText = "DELETE FROM Kategória WHERE Azonosító =" + katid;
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM Áru WHERE Kategória_Id =" + katid;
                    command.ExecuteNonQuery();

                    user();
                    Torles_LOG(katnev, katid);
                    MessageBox.Show("Kategória sikeresen törölve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("A Kategória nem lett törölve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
                }
            catch (Exception h)
            {
                MessageBox.Show(h.Message);
            }
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "") button1.Enabled = true;
            else button1.Enabled = false;
        }
    }
}

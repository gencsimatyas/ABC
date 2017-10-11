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
    public partial class Form4 : Form
    {
        OleDbConnection kon, connection; //kapcsolat
        OleDbCommand cimd, command;// sql parancs kuldi
        OleDbDataReader db;
        public string connect, dbutvonal;
        public string username, rang;
        Boolean ellenor = true;

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            dbutvonal = utvonal.ReadLine();

            connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

            utvonal.Close();
        }

        public void ellenorzes(string ujnev)
        {
            connection = new OleDbConnection(connect); //letrehozza az objektumot
            connection.Open(); //kapcsolodik
            command = connection.CreateCommand();

            command.CommandText = "SELECT Név FROM Kategória";
            db = command.ExecuteReader();

            while (db.Read())
            {
                if (db["Név"].ToString() == ujnev) ellenor = false;
            }

            db.Close();
        }

        public Form4()
        {
            InitializeComponent();
            adatbazis_utvonal();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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

        public void LOG(string kategoria)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;
            
            cimd.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum +"', '" + username + "', '" + rang + "', 'Létrehozott egy új kategóriát. Kategória neve: "+ kategoria +"')";
            cimd.ExecuteNonQuery();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kat = textBox1.Text;

            DialogResult result = MessageBox.Show("Biztos hozzá akarod adni a következő kategóriát: " + kat + "?","Kérdés",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            ellenorzes(kat);

            if (result == DialogResult.Yes)
            {
                if (ellenor == true)
                {
                    try
                    {
                        kon = new OleDbConnection(connect); //letrehozza az objektumot
                        kon.Open(); //kapcsolodik
                        cimd = kon.CreateCommand(); //letrehozza a command obejktumot

                        cimd.CommandText = "INSERT INTO Kategória (Név) VALUES ('" + kat + "') ";
                        cimd.ExecuteNonQuery();

                        //LOGOLAS KEZDETE

                        user();
                        LOG(kat);

                        //LOGOLAS VEGE


                        MessageBox.Show("Kategória Sikeresen Hozzáadva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    catch (Exception i)
                    {
                        MessageBox.Show(i.Message);
                    }
                }
                else
                {
                    ellenor = true;
                    MessageBox.Show("Nem lehet hozzáadni, mivel ilyen kategória név már létezik, kérjük adjon neki más nevet!", "Figyelemztetés", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    textBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Kategória nem lett hozzáadva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = "";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "") button2.Enabled = true;
            else button2.Enabled = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i != 34)&&(i != 39))
            {
            }
            else
            {
                MessageBox.Show("Nem használhatod ezt: " + Convert.ToChar(i) + " a karaktert!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }
    }
}

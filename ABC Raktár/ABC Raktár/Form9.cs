using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace ABC_Raktár
{
    public partial class Form9 : Form
    {
        public string username, rang;
        public string connect, dbutvonal;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader db;
        Boolean ellenor = true;

        public Form9()
        {
            InitializeComponent();
            dropdown_upload();
            adatbazis_utvonal();
            user();
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

        public void dropdown_upload()
        {
            comboBox1.Items.Add("admin");
            comboBox1.Items.Add("ruser");
            comboBox1.Items.Add("euser");
        }

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            dbutvonal = utvonal.ReadLine();

            connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

            utvonal.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button()
        {
            if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (comboBox1.Text != "")) button2.Enabled = true;
            else button2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i != 34) && (i != 39))
            {
            }
            else
            {
                MessageBox.Show("Nem használhatod ezt: " + Convert.ToChar(i) + " a karaktert!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i != 34) && (i != 39))
            {
            }
            else
            {
                MessageBox.Show("Nem használhatod ezt: " + Convert.ToChar(i) + " a karaktert!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i != 34) && (i != 39))
            {
            }
            else
            {
                MessageBox.Show("Nem használhatod ezt: " + Convert.ToChar(i) + " a karaktert!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void user_ellenor(string username)
        {
            connection = new OleDbConnection(connect); //letrehozza az objektumot
            connection.Open(); //kapcsolodik
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Felhasználók";
            db = command.ExecuteReader();

            while (db.Read())
            {
                if (db["Felhasználónév"].ToString() == username) ellenor = false;
            }

            db.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string felhasznalonev, jelszo, jelszo2, rang;

            felhasznalonev = textBox1.Text;
            jelszo = textBox2.Text;
            jelszo2 = textBox3.Text;
            rang = comboBox1.Text;

            if (jelszo == jelszo2)
            {
                user_ellenor(felhasznalonev);
                if (ellenor == true)
                {
                    connection = new OleDbConnection(connect); //letrehozza az objektumot
                    connection.Open(); //kapcsolodik
                    command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO Felhasználók (Felhasználónév, Jelszó, Rang) VALUES ('"+ felhasznalonev +"', '"+ jelszo +"', '"+ rang +"')";
                    command.ExecuteNonQuery();

                    MessageBox.Show("Felhasználó sikeresen regisztrálva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    ellenor = true;
                    MessageBox.Show("Ez a felhasználónév már foglalt, kérem válasszon másikat!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    textBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("A két jelszó nem egyezik meg!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox2.Text = "";
                textBox3.Text = "";
            }
        }
    }
}

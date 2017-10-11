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
    public partial class Form3 : Form
    {
        public string username, rang;
        public string connect, dbutvonal;
        public string név, mértékegység, kiszerelés, hely, szavatosság;
        public int kategória_id, mennyiség;
        public float ár;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader datar; // ebbe lesz kiolvasva a cucc
        Boolean ellenor = true;
        int szamlalo = 0;

        public Form3()
        {
            InitializeComponent();
            adatbazis_utvonal();
            dropdown_upload();
        }

        public void ellenorzes(string ujnev, string kategoria, string kszereles, string hely, string ar, string megyseg, string mennyiseg)
        {
            try
            {
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand();

                command.CommandText = "SELECT Mennyiség, Név, Kategória_Id, Kiszerelés, Hely, Ár, Szavatosság, Mértékegység FROM Áru";
                datar = command.ExecuteReader();

                while (datar.Read())
                {
                    if ((datar["Név"].ToString() == ujnev) && (datar["Kategória_Id"].ToString() == kategoria) && (datar["Mennyiség"].ToString() == mennyiseg) && (datar["Kiszerelés"].ToString() == kszereles) && (datar["Hely"].ToString() == hely) && (datar["Ár"].ToString() == ar) && (datar["Mértékegység"].ToString() == megyseg)) ellenor = false;
                }

                datar.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void hozzaadas()
        {
            try
            {
                if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != "") && (textBox6.Text != "") && (dateTimePicker1.Text != "")) button1.Enabled = true;
                else button1.Enabled = false;
            }
            catch (Exception q)
            {
                MessageBox.Show(q.Message);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            dateTimePicker1.Enabled = true;
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            adatbazis_utvonal();
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

        public void LOG(string kategoria, string nev, string mertekegyseg, string kiszereles, string hely, string ar, string mennyiseg, string szavatossag)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Hozzáadott egy új árut. Áru adatai: " + kategoria + " | " + nev + " | " + mertekegyseg + " | " + kiszereles + " | " + hely + " | " + ar + " | " + mennyiseg + " | " + szavatossag + "')";
            command.ExecuteNonQuery();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Item itm = (Item)comboBox1.SelectedItem;

                string kategorianev = itm.Name;

                kategória_id = itm.Value;
                név = textBox1.Text;
                mértékegység = textBox2.Text;
                kiszerelés = textBox3.Text;
                hely = textBox4.Text;
                ár = Convert.ToSingle(textBox5.Text);
                mennyiség = Convert.ToInt32(textBox6.Text);
                szavatosság = dateTimePicker1.Value.Day.ToString()+ "." + dateTimePicker1.Value.Month.ToString() + "." + dateTimePicker1.Value.Year.ToString();

                ellenorzes(név, kategória_id.ToString(), kiszerelés, hely, ár.ToString(), mértékegység, mennyiség.ToString());

                if (ellenor == true)
                {

                    connection = new OleDbConnection(connect); //letrehozza az objektumot
                    connection.Open(); //kapcsolodik
                    command = connection.CreateCommand(); //letrehozza a command obejktumot

                    command.CommandText = "INSERT INTO Áru (Kategória_Id, Név, Mértékegység, Kiszerelés, Hely, Ár, Mennyiség, Szavatosság) VALUES ('" + kategória_id + "','" + név + "','" + mértékegység + "','" + kiszerelés + "','" + hely + "','" + ár + "','" + mennyiség + "','" + szavatosság + "')";
                    command.ExecuteNonQuery();

                    user();
                    LOG(kategorianev, név, mértékegység, kiszerelés, hely, ár.ToString(), mennyiség.ToString(), szavatosság);

                    MessageBox.Show("Áru sikeresen hozzáadva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    ellenor = true;
                    MessageBox.Show("Nem lehet hozzáadni, mivel ez az áru már raktáron van!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    textBox1.Text = "";
                    textBox3.Text = "";
                }

            }
            catch (Exception q)
            {
                MessageBox.Show(q.Message);
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            hozzaadas();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            string stomb = textBox5.Text;
            bool igaz = true;

            try
            {

                if ((i == 44) && (szamlalo < 1))
                {
                    szamlalo++;
                    igaz = true;
                }
                else
                {
                    igaz = false;
                }

                if ((i == 8) && (textBox5.Text != ""))
                {
                    if (Convert.ToByte(stomb[stomb.Length - 1]) == 44)
                    {
                        szamlalo--;
                    }
                }

                if ((i >= 48 && i <= 57) || (i == 8) || (i == 44))
                {
                    if ((textBox5.Text.Length == 0) && (i == 44))
                    {
                        MessageBox.Show("Nem kezdődhet az ár tizedes vesszővel!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                        igaz = true;
                        szamlalo = 0;
                    }
                    else
                        if ((igaz == false) && (i == 44))
                        {
                            MessageBox.Show("Csak egy tizedes vesszőt lehet használni!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            e.Handled = true;
                        }

                }
                else
                {
                    MessageBox.Show("Ide csak számot lehet beírni!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Handled = true;
                }
            }
            catch
            {
                MessageBox.Show("Hiba történt, kérjük próbálja meg ismét!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i >= 49 && i <= 57) || (i == 8))
            {
            }
            else
            {
                MessageBox.Show("Ide csak nullánál nagyobb természetes számot lehet beírni!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}

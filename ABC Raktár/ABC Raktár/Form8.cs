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
    public partial class Form8 : Form
    {
        public string stabiluser, stabilrang;
        public string username, rang;
        public string connect, dbutvonal;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader db; // ebbe lesz kiolvasva a cucc
        Boolean userellenor = true;
        Boolean ellenor = true;

        public Form8()
        {
            InitializeComponent();
            dropdown_upload();
            adatbazis_utvonal();
            user();
            frissites();
        }

        private void user_ellenorzes(string id, string username)
        {
            connection = new OleDbConnection(connect); //letrehozza az objektumot
            connection.Open(); //kapcsolodik
            command = connection.CreateCommand();

            command.CommandText = "SELECT Azonosító, Felhasználónév FROM Felhasználók";
            db = command.ExecuteReader();

            while (db.Read())
            {
                if ((db["Azonosító"].ToString() == id) && (db["Felhasználónév"].ToString() == username)) userellenor = false;
            }

            db.Close();
            connection.Close();
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

        public void frissites()
        {
            try
            {
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand();

                listView1.Items.Clear();

                command.CommandText = "SELECT * FROM Felhasználók";
                db = command.ExecuteReader();

                while (db.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = db["Azonosító"].ToString();
                    sor.SubItems.Add(db["Felhasználónév"].ToString());
                    sor.SubItems.Add(db["Jelszó"].ToString());
                    sor.SubItems.Add(db["Rang"].ToString());

                    listView1.Items.Add(sor);
                }
                db.Close();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                comboBox1.SelectedIndex = -1;
                connection.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                textBox1.Text = listView1.Items[e.ItemIndex].SubItems[0].Text;
                textBox2.Text = listView1.Items[e.ItemIndex].SubItems[1].Text;
                textBox3.Text = listView1.Items[e.ItemIndex].SubItems[2].Text;
                comboBox1.Text = listView1.Items[e.ItemIndex].SubItems[3].Text;

            }
        }

        private void button()
        {
            if ((textBox1.Text != "")&&(textBox2.Text != "")&&(textBox3.Text != ""))
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frissites();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id;

            id = textBox1.Text;

            user_ellenorzes(id, username);

            if (userellenor == true)
            {
                DialogResult result = MessageBox.Show("Biztos hogy törölni szeretnéd az '" + stabilrang + "'-rangú, " + stabiluser + " felhasználót?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        connection = new OleDbConnection(connect); //letrehozza az objektumot
                        connection.Open(); //kapcsolodik
                        command = connection.CreateCommand();

                        command.CommandText = "DELETE FROM Felhasználók WHERE Azonosító =" + id;
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    catch (Exception i)
                    {
                        MessageBox.Show(i.Message);
                    }

                    MessageBox.Show("Az '" + stabilrang + "'-rangú, " + stabiluser + " felhasználó sikeresen törölve lett!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Threading.Thread.Sleep(1000);
                    frissites();
                }
                else
                {
                    MessageBox.Show("Az '" + stabilrang + "'-rangú, " + stabiluser + " felhasználó nem lett törölve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.SelectedIndex = -1;
                }

            }
            else
            {
                userellenor = true;
                MessageBox.Show("Nem törölheted saját magad!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                comboBox1.SelectedIndex = -1;
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            stabiluser = textBox2.Text;
            stabilrang = comboBox1.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.ShowDialog();
            frissites();
        }

        public void MODOSIT_LOG(string user)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();
            string loguser;

            if (user == username) loguser = "Módosította a saját jelszavát";
            else loguser = "Módosította | " + user + " | felhasználó felhasználónevét/jelszavát/rangját";

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            connection = new OleDbConnection(connect); //letrehozza az objektumot
            connection.Open(); //kapcsolodik
            command = connection.CreateCommand();

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', '"+ loguser + "')";
            command.ExecuteNonQuery();

            connection.Close();

        }

  

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBox1.Text;
                string felhasznalonev, jelszo, rang;

                felhasznalonev = textBox2.Text;
                jelszo = textBox3.Text;
                rang = comboBox1.Text;


                if (username != stabiluser) //ha NEM ugyanaz a bejelentkezett felhasznalo mint a kivalasztott
                {
                    if (ellenor == true)
                    {
                        connection = new OleDbConnection(connect); //letrehozza az objektumot
                        connection.Open(); //kapcsolodik
                        command = connection.CreateCommand();

                        command.CommandText = "UPDATE Felhasználók SET Felhasználónév ='" + felhasznalonev + "', Jelszó ='" + jelszo + "', Rang ='" + rang + "' WHERE Azonosító =" + id;
                        command.ExecuteNonQuery();

                        MODOSIT_LOG(stabiluser);

                        connection.Close();

                        MessageBox.Show("Sikeres módosítás!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        System.Threading.Thread.Sleep(1000);
                        frissites();
                    }
                    else
                    {
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        comboBox1.SelectedIndex = -1;
                        MessageBox.Show("Ez a felhasználónév már foglalt, kérem válasszon másikat!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                else //ha ugyanaz a bejelentkezett felhasznalo minta kivalasztott
                {
                    if ((rang == stabilrang) && (felhasznalonev == stabiluser))
                    {
                        connection = new OleDbConnection(connect); //letrehozza az objektumot
                        connection.Open(); //kapcsolodik
                        command = connection.CreateCommand();

                        command.CommandText = "UPDATE Felhasználók SET Jelszó ='" + jelszo + "' WHERE Azonosító =" + id;
                        command.ExecuteNonQuery();

                        MODOSIT_LOG(stabiluser);

                        connection.Close();

                        MessageBox.Show("Sikeres módosítás!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        System.Threading.Thread.Sleep(1000);
                        frissites();
                    }
                    else
                    {
                        MessageBox.Show("Nem változtathatod meg a saját felhasználónevedet vagy rangodat!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        comboBox1.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception j)
            {
                MessageBox.Show(j.Message);
            }
        }

    }
}

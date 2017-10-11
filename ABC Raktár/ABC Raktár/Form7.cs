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
    public partial class Form7 : Form
    {

        public string username, rang;
         public string connect, dbutvonal;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader db; // ebbe lesz kiolvasva a cucc
        public string katnev;
        Boolean ellenor = true;


        public Form7()
        {
            InitializeComponent();
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

        private void frissites()
        {
            try
            {
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand();

                listView1.Items.Clear();

                command.CommandText = "SELECT * FROM Kategória";
                db = command.ExecuteReader();

                while (db.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = db["Azonosító"].ToString();
                    sor.SubItems.Add(db["Név"].ToString());

                    listView1.Items.Add(sor);
                }

                db.Close();
            }
            catch (Exception h)
            {
                MessageBox.Show(h.Message);
            }
        }

        private void user()
        {
            try
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
            catch (Exception o)
            {
                MessageBox.Show(o.Message);
            }

        }

        public void adatbazis_utvonal()
        {
            try
            {
                TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

                dbutvonal = utvonal.ReadLine();

                connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

                utvonal.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void Form7_Load_1(object sender, EventArgs e)
        {
            adatbazis_utvonal();
            user();
            frissites(); 
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void listView1_ItemSelectionChanged_1(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.IsSelected)
                {
                    textBox1.Text = listView1.Items[e.ItemIndex].SubItems[1].Text;
                    textBox2.Text = listView1.Items[e.ItemIndex].SubItems[0].Text;
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "") button1.Enabled = true;
            else button1.Enabled = false;
        }

        public void LOG(string reginev, string ujnev)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Megváltoztatta ezt a kategória nevet: " + reginev + ", erre: " + ujnev + "')";
            command.ExecuteNonQuery();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") && (textBox2.Text != ""))
            {
                DialogResult result = MessageBox.Show("Biztos, hogy módosítani akarja ezt a kategórianevet: " + katnev + ", erre: " + textBox1.Text + "?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                ellenorzes(textBox1.Text);

                    if (result == DialogResult.Yes)
                    {
                        if (ellenor == true)
                        {
                            try
                            {
                                command.CommandText = "UPDATE Kategória SET Név ='" + textBox1.Text + "' WHERE Azonosító =" + textBox2.Text + "";
                                command.ExecuteNonQuery();

                                LOG(katnev, textBox1.Text);

                                MessageBox.Show("A kategória neve sikeresen módisítva lett", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Close();
                            }
                            catch (Exception a)
                            {
                                MessageBox.Show(a.Message);
                            }
                        }
                        else
                        {
                            ellenor = true;
                            MessageBox.Show("Nem lehet módosítani, mivel ilyen kategória név már létezik, kérjük adjon neki más nevet!", "Figyelemztetés", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            textBox1.Text = "";
                            textBox2.Text = "";
                        }
                    }
            
                else
                {
                    MessageBox.Show("Az adott kategória neve nem lett módisítva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    button1.Enabled = false;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            katnev = textBox1.Text;
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


    }
}

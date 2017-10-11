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
    public partial class Form2 : Form
    {
        public string arunev;
        public string username, rang;
        public int user_rang; // 1 - admin  , 0 - raktáros, 2 - pénztáros
        public string connect, dbutvonal;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command; // sql parancs kuldo
        OleDbDataReader db; // ebbe lesz kiolvasva a cucc
        Boolean ellenor = true;
        int szamlalo = 0;

        public Form2()
        {
            InitializeComponent();
            user();
            admin();
            euser();
            ruser();
        }

        public void ellenorzes(string ujnev, string kategoria, string kszereles, string hely, string ar, string megyseg, string mennyiseg)
        {
            try
            {
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand();

                command.CommandText = "SELECT Mennyiség, Név, Kategória_Id, Kiszerelés, Hely, Ár, Szavatosság, Mértékegység FROM Áru";
                db = command.ExecuteReader();

                while (db.Read())
                {
                    if ((db["Név"].ToString() == ujnev) && (db["Kategória_Id"].ToString() == kategoria) && (db["Mennyiség"].ToString() == mennyiseg) && (db["Kiszerelés"].ToString() == kszereles) && (db["Hely"].ToString() == hely) && (db["Ár"].ToString() == ar) && (db["Mértékegység"].ToString() == megyseg)) ellenor = false;
                }

                db.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        public void admin()
        {
            if (user_rang == 1)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
        }

        public void euser() //penztaros/elado felhasznalo
        {
            if (user_rang == 2)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
            }
        }

        public void ruser() //raktaros felhasznalo
        {
            if (user_rang == 0)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                button7.Enabled = false;
                button10.Enabled = false;
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

        private void user()
        {
            TextReader user = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\user.txt");
            username = user.ReadLine();
            rang = user.ReadLine();
            user.Close();

            label11.Text = "Helló " + username + ", jó munkát!";
            label11.ForeColor = System.Drawing.Color.Green;

            switch (rang)
            {
                case "ruser":
                    {
                        rang = "Raktáros";
                        user_rang = 0;
                    } break;

                case "euser":
                    {
                        rang = "Pénztáros";
                        user_rang = 2;
                    } break;

                case "admin":
                    {
                        rang = "Adminisztrátor";
                        user_rang = 1;
                    } break;
            }

            label10.Text = "Rang: " + rang;
            label10.ForeColor = System.Drawing.Color.Red;

        }

        private void dropdown_upload()
        {
            try
            {

                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand(); //letrehozza a command obejktumot

                command.CommandText = "Select * from Kategória";
                db = command.ExecuteReader();//nem valtoztat az adatbazison "select"

                while (db.Read())
                {
                    comboBox1.Items.Add(new Item(db["Név"].ToString(), Convert.ToInt32(db["Azonosító"])));
                }

                db.Close();



            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void frissites()
        {
            try
            {
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand(); //letrehozza a command obejktumot

                listView1.Items.Clear();

                if (textBox1.Text != "")
                {
                    if (radioButton1.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Kategória.Név LIKE '%" + textBox1.Text + "%'"; //rb1 kategoria
                        textBox1.Enabled = true;
                    }
                    if (radioButton2.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Név LIKE '%" + textBox1.Text + "%'"; // rb2 nev
                        textBox1.Enabled = true;
                    }
                    if (radioButton3.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Azonosító LIKE '%" + textBox1.Text + "%'"; // rb3 azonosito
                        textBox1.Enabled = true;
                    }
                    if (radioButton4.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mértékegység LIKE '%" + textBox1.Text + "%'"; // rb4 mertekegyseg
                        textBox1.Enabled = true;
                    }
                    if (radioButton5.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Kiszerelés LIKE '%" + textBox1.Text + "%'"; // rb4 kiszereles
                        textBox1.Enabled = true;
                    }
                    if (radioButton6.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Hely LIKE '%" + textBox1.Text + "%'"; // rb4 hely
                        textBox1.Enabled = true;
                    }
                    if (radioButton7.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Ár LIKE '%" + textBox1.Text + "%'"; // rb4 ar
                        textBox1.Enabled = true;
                    }
                    if (radioButton8.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mennyiség LIKE '%" + textBox1.Text + "%'"; // rb4 mennyiseg
                        textBox1.Enabled = true;
                    }
                    if (radioButton9.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Szavatosság LIKE '%" + textBox1.Text + "%'"; // rb4 szavatossag
                        textBox1.Enabled = true;
                    }
                    
                }
                if (textBox1.Text == "") command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id";

                db = command.ExecuteReader();//nem valtoztat az adatbazison "select"


                while (db.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = db["Kategória"].ToString();
                    sor.SubItems.Add(db["Név"].ToString());
                    sor.SubItems.Add(db["Mértékegység"].ToString());
                    sor.SubItems.Add(db["Kiszerelés"].ToString());
                    sor.SubItems.Add(db["Hely"].ToString());
                    sor.SubItems.Add(db["Ár"].ToString());
                    sor.SubItems.Add(db["Mennyiség"].ToString());
                    sor.SubItems.Add(db["Szavatosság"].ToString());
                    sor.SubItems.Add(db["Azonosító"].ToString());

                    listView1.Items.Add(sor);
                }
                db.Close();

                comboBox1.SelectedIndex = -1;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                maskedTextBox1.Text = "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            dbutvonal = utvonal.ReadLine();

            connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

            utvonal.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            adatbazis_utvonal();
            //MessageBox.Show(connect); 
            frissites();
            //listView1.CheckBoxes = true; 
            dropdown_upload();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);
                Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private DialogResult PreClosingConfirmation()
        {
            DialogResult res = System.Windows.Forms.MessageBox.Show("Biztos ki akar jelentkezni?          ", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            frissites();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frissites();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                comboBox1.Text = listView1.Items[e.ItemIndex].SubItems[0].Text;
                textBox2.Text = listView1.Items[e.ItemIndex].SubItems[1].Text;
                textBox3.Text = listView1.Items[e.ItemIndex].SubItems[2].Text;
                textBox4.Text = listView1.Items[e.ItemIndex].SubItems[3].Text;
                textBox5.Text = listView1.Items[e.ItemIndex].SubItems[4].Text;
                textBox6.Text = listView1.Items[e.ItemIndex].SubItems[5].Text;
                textBox7.Text = listView1.Items[e.ItemIndex].SubItems[6].Text;
                dateTimePicker1.Text = listView1.Items[e.ItemIndex].SubItems[7].Text;
                maskedTextBox1.Text = listView1.Items[e.ItemIndex].SubItems[8].Text;

            }
        }

        public void Modositas_LOG(string azonosito, string kategoria, string nev, string mertekegyseg, string kiszereles, string hely, string ar, string mennyiseg, string szavatossag)
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Módosította a következő árut: "+ azonosito +" | "+ kategoria +" | "+ nev +" | "+ mertekegyseg +" | "+ kiszereles +" | "+ hely +" | "+ ar +" | "+ mennyiseg +" | "+ szavatossag +"')";
            command.ExecuteNonQuery();

        }

        private void button3_Click(object sender, EventArgs e) //Áru modositasa
        {
            if ((comboBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != "") && (textBox6.Text != "") && (textBox7.Text != ""))
            {
                DialogResult result = MessageBox.Show("Biztos akarod módosítani a következő árut: " + arunev + "?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {

                    string nev, mertekegyseg, kiszereles, hely, ar, mennyiseg, szavatossag, azonosito, kategorianev;
                    int kategoria;

                    Item itm = (Item)comboBox1.SelectedItem;
                    kategoria = itm.Value;
                    kategorianev = itm.Name;
                    nev = textBox2.Text;
                    mertekegyseg = textBox3.Text;
                    kiszereles = textBox4.Text;
                    hely = textBox5.Text;
                    ar = textBox6.Text;
                    mennyiseg = textBox7.Text;
                    szavatossag = dateTimePicker1.Text;
                    azonosito = maskedTextBox1.Text;

                    ellenorzes(nev, kategoria.ToString(), kiszereles, hely, ar, mertekegyseg, mennyiseg);

                    if (ellenor == true)
                    {
                        try
                        {
                            command.CommandText = "Update Áru set Kategória_Id ='" + kategoria + "', Név ='" + nev + "', Mértékegység ='" + mertekegyseg + "', Kiszerelés ='" + kiszereles + "', Hely='" + hely + "', Ár='" + ar + "', Mennyiség ='" + mennyiseg + "', Szavatosság ='" + szavatossag + "' WHERE Azonosító =" + azonosito;
                            //MessageBox.Show(command.CommandText);
                            command.ExecuteNonQuery();

                            Modositas_LOG(azonosito, kategorianev, nev, mertekegyseg, kiszereles, hely, ar, mennyiseg, szavatossag);

                            MessageBox.Show("Az áru sikeresen módosítva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            System.Threading.Thread.Sleep(1000);

                            frissites();
                        }
                        catch (Exception y)
                        {
                            MessageBox.Show(y.Message);
                        }
                    }
                    else
                    {
                        ellenor = true;
                        MessageBox.Show("Nem lehet végrehalytani ezt a módosítást, mivel ilyen áru már szerepel az adatbázisban!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        frissites();
                    }
                }
                else
                {
                    MessageBox.Show("Az áru nem lett módosítva!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frissites();
                }


            }
            else
            {
                MessageBox.Show("Nem választottál ki egy árut sem!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            string stomb = textBox6.Text;
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

                if ((i == 8) && (textBox6.Text != ""))
                {
                    if (Convert.ToByte(stomb[stomb.Length - 1]) == 44)
                    {
                        szamlalo--;
                    }
                }

                if ((i >= 48 && i <= 57) || (i == 8) || (i == 44))
                {
                    if ((textBox6.Text.Length == 0) && (i == 44))
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

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i >= 48 && i <= 57) || (i == 8))
            {
            }
            else
            {
                MessageBox.Show("Ide csak számot lehet beírni!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
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

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Törölte a következő árut: "+ id +" | "+ nev + "')";
            command.ExecuteNonQuery();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((comboBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != "") && (textBox6.Text != "") && (textBox7.Text != ""))
            {
                DialogResult result = MessageBox.Show("Biztos akarod törölni a következő árut: " + arunev + "?","Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    string azonosito = maskedTextBox1.Text;

                    command.CommandText = "Delete from Áru where Azonosító =" + azonosito;
                    //MessageBox.Show(command.CommandText);
                    command.ExecuteNonQuery();

                    Torles_LOG(textBox2.Text, azonosito);

                    MessageBox.Show("Áru sikeresen törölve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Threading.Thread.Sleep(1000);

                    frissites();
                }
                else
                {
                    MessageBox.Show("Az áru nem lett törölve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frissites();
                }
                        
                 
            }
            else
            {
                MessageBox.Show("Nem választottál ki egy árut sem!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ShowDialog();
            frissites();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            dropdown_upload();
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
            frissites();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            dropdown_upload();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            frissites();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissites();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true; 
            frissites();
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

        private void button7_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.ShowDialog();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            arunev = textBox2.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.ShowDialog();
            frissites();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            dropdown_upload();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();

            f10.ShowDialog();
        }

 
    }
}

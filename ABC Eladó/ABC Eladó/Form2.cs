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
using System.Diagnostics;

namespace ABC_Eladó
{
    public partial class Form2 : Form
    {
        public string connect, dbutvonal;
        public string kaz, kk, knev, kar, kdb, ajdi;
        public float osszar = 0, ideg = 0, ideg2 = 0;
        public string username, rang;
        OleDbConnection connection; //kapcsolat
        OleDbCommand command, command2; // sql parancs kuldo
        OleDbDataReader db; // ebbe lesz kiolvasva a cucc
        public int user_rang; // 1 - admin  , 0 - raktáros, 2 - pénztáros
        public string darab;
        Boolean gedi = true;
        Boolean eladasellenor = true;

        public Form2()
        {
            InitializeComponent();
            user();
            admin();
            euser();
            ruser();
        }

        public void admin()
        {
            if (user_rang == 1)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        public void euser() //penztaros/elado felhasznalo
        {
            if (user_rang == 2)
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        public void buttonegy()
        {
            if ((textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != "") && (comboBox1.SelectedIndex > -1))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        public void ruser() //raktaros felhasznalo
        {
            if (user_rang == 0)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
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
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Kategória.Név LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; //rb1 kategoria
                        textBox1.Enabled = true;
                    }
                    if (radioButton2.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Név LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb2 nev
                        textBox1.Enabled = true;
                    }
                    if (radioButton3.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Azonosító LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb3 azonosito
                        textBox1.Enabled = true;
                    }
                    if (radioButton4.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mértékegység LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 mertekegyseg
                        textBox1.Enabled = true;
                    }
                    if (radioButton5.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Kiszerelés LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 kiszereles
                        textBox1.Enabled = true;
                    }
                    if (radioButton6.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Hely LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 hely
                        textBox1.Enabled = true;
                    }
                    if (radioButton7.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Ár LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 ar
                        textBox1.Enabled = true;
                    }
                    if (radioButton8.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mennyiség LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 mennyiseg
                        textBox1.Enabled = true;
                    }
                    if (radioButton9.Checked == true)
                    {
                        command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Szavatosság LIKE '%" + textBox1.Text + "%' AND Áru.Mennyiség > 0"; // rb4 szavatossag
                        textBox1.Enabled = true;
                    }

                }
                if (textBox1.Text == "") command.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mértékegység, Áru.Kiszerelés, Áru.Hely, Áru.Ár, Áru.Mennyiség, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mennyiség > 0";



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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void adatbazis_utvonal()
        {

            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");

                dbutvonal = utvonal.ReadLine();

                connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

                utvonal.Close();
            

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            adatbazis_utvonal();
            frissites();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void user()
        {
            TextReader user = new StreamReader("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\user.txt");
            username = user.ReadLine();
            rang = user.ReadLine();
            user.Close();

            label1.Text = "Helló " + username + ", jó munkát!";
            label1.ForeColor = System.Drawing.Color.Green;

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

            label2.Text = "Rang: " + rang;
            label2.ForeColor = System.Drawing.Color.Red;

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);

                command.CommandText = "DELETE * FROM Eladás";
                command.ExecuteNonQuery();

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            frissites();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            comboBox1.Items.Clear();

            if (e.IsSelected)
            {
                textBox2.Text = listView1.Items[e.ItemIndex].SubItems[8].Text;
                textBox3.Text = listView1.Items[e.ItemIndex].SubItems[0].Text;
                textBox4.Text = listView1.Items[e.ItemIndex].SubItems[1].Text;
                textBox5.Text = listView1.Items[e.ItemIndex].SubItems[5].Text;
                for (int i = 1; i <= Convert.ToInt32(listView1.Items[e.ItemIndex].SubItems[6].Text); i++)
                {
                    comboBox1.Items.Add(i);
                }
                //comboBox1.Text = "1";
            }
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frissites();
        }

        private void eladellenor(int azonosit)
        {
            command.CommandText = "SELECT Áru_Azonosító FROM Eladás";
            db = command.ExecuteReader();

            while (db.Read())
            {
                if (Convert.ToInt32(db["Áru_Azonosító"]) == azonosit) eladasellenor = false;
            }

            db.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                eladasellenor = true;
                string kategoria, nev, az, ar;

                az = textBox2.Text;
                kategoria = textBox3.Text;
                nev = textBox4.Text;
                ar = textBox5.Text;
                darab = comboBox1.Text;

                eladellenor(Convert.ToInt32(az));

                if (eladasellenor == true)
                {
                    command.CommandText = "INSERT INTO Eladás (Áru_Azonosító, Kategória, Név, Ár, Mennyiség) VALUES ('" + az + "','" + kategoria + "','" + nev + "','" + ar + "','" + darab + "')";
                    command.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Ez a termék már rajta van az eladási listán, ha módosítani akarja, akkor törölje ki, és adja hozzá újra!","Üzenet",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }

                comboBox1.SelectedIndex = -1;

                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }

            frissites2();
            
        }

        public float osszegezes(float darab, float ar)
        {
            float sum;

            sum = darab * ar;

            return sum;
        }

        public void szamla()
        {
            string datum = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month + "." + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            TextWriter tw = new StreamWriter("szamla.txt");

            command.CommandText = "SELECT * FROM Eladás";
            db = command.ExecuteReader();

            tw.WriteLine("        SC.  ABC  SRL");
            tw.WriteLine("           Számla");
            tw.WriteLine("-------------------------------------");
            tw.WriteLine("Dátum és idő: " + datum);
            tw.WriteLine("-------------------------------------");
            tw.WriteLine("");

            tw.WriteLine("Megvásárolt Termékek");
            tw.WriteLine("-------------------------------------");

            while (db.Read())
            {
                tw.WriteLine(db["Mennyiség"] + " X " + db["Név"] + " (" + db["Kategória"] + ")");
                tw.WriteLine("          " + db["Mennyiség"] + " X " + db["Ár"] + " RON = " + (Convert.ToSingle(db["Mennyiség"]) * Convert.ToSingle(db["Ár"])) + " RON");
                tw.WriteLine("");
            }

            tw.WriteLine("-------------------------------------");
            tw.WriteLine("          Ár összesítve: " + ideg + " RON");

            tw.WriteLine("");
            tw.WriteLine("");

            tw.WriteLine("Köszönjük, hogy minket választott!");
            tw.WriteLine("      Várjuk máskor is! :)");

            db.Close();
            tw.Close();
        }
     

        public void frissites2()
        {
            try
            {

                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand(); //letrehozza a command obejktumot

                listView2.Items.Clear();


                command.CommandText = "SELECT * FROM Eladás";
                db = command.ExecuteReader();


                while (db.Read())
                {
                    osszar = Convert.ToSingle(db["Ár"]);
                    ListViewItem sor = new ListViewItem();
                    sor.Text = db["Áru_Azonosító"].ToString();
                    sor.SubItems.Add(db["Kategória"].ToString());
                    sor.SubItems.Add(db["Név"].ToString());
                    sor.SubItems.Add(db["Ár"].ToString());
                    sor.SubItems.Add(db["Mennyiség"].ToString());
                    sor.SubItems.Add(db["Id"].ToString());

                    listView2.Items.Add(sor);
                }

                if (eladasellenor == true)
                {
                    if (gedi == true) ideg = ideg + osszegezes(Convert.ToSingle(darab), osszar);
                    else ideg = ideg - ideg2;
                }

                gedi = true;

                db.Close();

                textBox7.Text = ideg.ToString();

                if ((textBox7.Text == "0") || (textBox7.Text == ""))
                {
                    button3.Enabled = false;
                    button4.Enabled = false;
                }
                else
                {
                    button3.Enabled = true;
                    if (kar == "") button4.Enabled = false;
                    else button4.Enabled = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void levono()
        {
            command2 = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Eladás";
            db = command.ExecuteReader();
            
            while (db.Read())
            {
                command2.CommandText = "UPDATE Áru SET Mennyiség = Mennyiség - " + Convert.ToInt32(db["Mennyiség"]) + " WHERE Azonosító = " + db["Áru_Azonosító"] +"";
                command2.ExecuteNonQuery();
            }

            db.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Végre akarja hajtani az eladást?","Kérdés",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    szamla();
                    levono();
                    textBox7.Text = "";
                    command.CommandText = "DELETE * FROM Eladás";
                    command.ExecuteNonQuery();

                   

                    System.Threading.Thread.Sleep(1000);

                    ideg = 0;
                    osszar = 0;

                    frissites2();
                    frissites();

                    Process.Start("szamla.txt");
                    Eladas_LOG();
                
                }
                catch (Exception y)
                {
                    MessageBox.Show(y.Message);
                }
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            button4.Enabled = true;
            if (e.IsSelected)
            {
                kaz = listView2.Items[e.ItemIndex].SubItems[0].Text;
                kk = listView2.Items[e.ItemIndex].SubItems[1].Text;
                knev = listView2.Items[e.ItemIndex].SubItems[2].Text;
                kar = listView2.Items[e.ItemIndex].SubItems[3].Text;
                kdb = listView2.Items[e.ItemIndex].SubItems[4].Text;
                ajdi = listView2.Items[e.ItemIndex].SubItems[5].Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Biztos, hogy törölni akarja az eladási listáról a következő árut: | " + kaz + " | " + kk + " | " + knev + " | " + kar + " | " + kdb + " | ?","Kérdés",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    ideg2 = (Convert.ToSingle(kar) * Convert.ToSingle(kdb));
                    command.CommandText = "DELETE FROM Eladás WHERE Áru_Azonosító =" + kaz + " AND Id =" + ajdi;
                    command.ExecuteNonQuery();
                    gedi = false;
         
                }
                catch (Exception y)
                {
                    MessageBox.Show(y.Message);
                }

                kk = "";
                kdb = "";
                knev = "";
                kaz = "";
                kar = "";

                System.Threading.Thread.Sleep(1000);

                frissites2();

                int listCount = listView2.Items.Count;

                if (listCount == 0)
                {
                    ideg = 0;
                    osszar = 0;
                    textBox7.Text = "0";
                }

                

                
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = e.KeyChar;
            if ((i > 48 && i <= 57) || (i == 8))
            {
            }
            else
            {
                MessageBox.Show("Ide csak nullánál nagyobb számot lehet beírni!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        public void Eladas_LOG()
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();
            string ora = DateTime.Now.Hour.ToString();
            string perc = DateTime.Now.Minute.ToString();
            string masodperc = DateTime.Now.Second.ToString();

            string datum = nap + "." + honap + "." + ev + " " + ora + ":" + perc + ":" + masodperc;

            command.CommandText = "INSERT INTO LOG (Dátum, Felhasználó, Rang, Esemény) VALUES ('" + datum + "', '" + username + "', '" + rang + "', 'Árut adott el.')";
            command.ExecuteNonQuery();

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonegy();
        }

 
    }
}

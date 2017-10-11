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

namespace ABC_Eladó
{
    public partial class Form1 : Form
    {
        public String connect, rang;
        static public OleDbConnection con;
        static public OleDbCommand myCommand;
        static public OleDbDataReader dr;
        public string username, password, dbutvonal;
        Boolean conn = false;

        public Form1()
        {
            InitializeComponent();
            adatbazis_utvonal();
        }

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");

                dbutvonal = utvonal.ReadLine();

                connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

                utvonal.Close();
        }

        public void hibas_utvonal()
        {
            TextWriter hibasutvonal = new StreamWriter("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");

            hibasutvonal.WriteLine("hiba");

            hibasutvonal.Close();
        }

        public void button()
        {
            if ((textBox1.Text != "") && (textBox2.Text != "")) button1.Enabled = true;
            else button1.Enabled = false;
        }

        private Boolean pass(string formuser, string formpass)
        {
            bool pass = false;

            while (dr.Read())
            {
                if ((dr["Felhasználónév"].ToString() == formuser) && (dr["Jelszó"].ToString() == formpass))
                {
                    pass = true;
                    rang = dr["Rang"].ToString();
                }
            }

            return pass;
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con = new OleDbConnection(connect);
                con.Open();
                conn = true;
                myCommand = con.CreateCommand();
                myCommand.CommandText = "Select * from Felhasználók";

                dr = Form1.myCommand.ExecuteReader();

                username = textBox1.Text;
                password = textBox2.Text;

                if (pass(username, password) == true)
                {
                    TextWriter user = new StreamWriter("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\user.txt");
                    user.WriteLine(username);
                    user.WriteLine(rang);
                    user.Close();


                    Form2 f2 = new Form2();
                    Hide();
                    f2.ShowDialog();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    Show();
                }
 
                else MessageBox.Show("Hibásan adta meg a felhasználónevét vagy a jelszavát!", "Hiba");

                dr.Close();
            }
            catch
            {
                MessageBox.Show("Hiba történt az adatbázis megnyitása során, kérjük válassza ki ismét!","Üzenet",MessageBoxButtons.OK, MessageBoxIcon.Error);
                hibas_utvonal();
                System.Diagnostics.Process.Start("..\\..\\..\\..\\ABC\\ABC\\bin\\Debug\\ABC.exe");
                Close();
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conn == true) con.Close();
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

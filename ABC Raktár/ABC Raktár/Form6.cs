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
    public partial class Form6 : Form
    {
        public string connect, dbutvonal;
        OleDbConnection connection, conn; //kapcsolat
        OleDbCommand command, cmd; // sql parancs kuldo
        OleDbDataReader db; // ebbe lesz kiolvasva a cucc

        public Form6()
        {
            InitializeComponent();
        }

        public void adatbazis_utvonal()
        {
            TextReader utvonal = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            dbutvonal = utvonal.ReadLine();

            connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbutvonal + "; Jet OLEDB:Database Password=be34syi92ld,d";

            utvonal.Close();
        }

        private void frissit()
        {
            try
            {
                adatbazis_utvonal();
                connection = new OleDbConnection(connect); //letrehozza az objektumot
                connection.Open(); //kapcsolodik
                command = connection.CreateCommand(); //letrehozza a command obejktumot

                listView1.Items.Clear();

                if (radioButton1.Checked == true) command.CommandText = "Select * From LOG WHERE Dátum LIKE '%" + textBox1.Text + "%'";
                if (radioButton2.Checked == true) command.CommandText = "Select * From LOG WHERE Felhasználó LIKE '%" + textBox1.Text + "%'";
                if (radioButton3.Checked == true) command.CommandText = "Select * From LOG WHERE Rang LIKE '%" + textBox1.Text + "%'";
                if (textBox1.Text == "") command.CommandText = "Select * From LOG";

                db = command.ExecuteReader();

                while (db.Read())
                {

                    ListViewItem sor = new ListViewItem();
                    sor.Text = db["Azonosító"].ToString();
                    sor.SubItems.Add(db["Dátum"].ToString());
                    sor.SubItems.Add(db["Felhasználó"].ToString());
                    sor.SubItems.Add(db["Rang"].ToString());
                    sor.SubItems.Add(db["Esemény"].ToString());

                    listView1.Items.Add(sor);

                }

                db.Close();
            }
            catch (Exception h)
            {
                MessageBox.Show(h.Message);
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            frissit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Biztos ki akarja üríteni a LOG-ot?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    conn = new OleDbConnection(connect); //letrehozza az objektumot
                    conn.Open(); //kapcsolodik
                    cmd = connection.CreateCommand();

                    cmd.CommandText = "DELETE * FROM LOG";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("LOG sikeresen ürítve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    System.Threading.Thread.Sleep(1000);

                    frissit();
                }
                catch (Exception l)
                {
                    MessageBox.Show(l.Message);
                }

            }
            else
            {
                MessageBox.Show("LOG nem lett kiürítve!", "Üzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissit();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            frissit();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            frissit();
        }
    }
}

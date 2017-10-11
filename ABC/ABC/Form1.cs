using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ABC
{
    public partial class Form1 : Form
    {
        Form2 f2 = new Form2();
        public string ut1,ut2;
        public Boolean dbellenorzes = false;

        public void adatbazis_ellenorzes_kiolvasas()
        {
            TextReader tr = new StreamReader("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");
            TextReader tr2 = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            ut1 = tr.ReadLine();
            ut2 = tr2.ReadLine();

            tr.Close();
            tr2.Close();
        }

        public void ellenorzes()
        {
            if (ut1 == ut2)
            {
                label7.ForeColor = System.Drawing.Color.Green;
                label7.Text = "Ok!"; //ha rendben van az adatbazis, akkor label7: OK
                dbellenorzes = true;
            }

            if ((ut1 != ut2) || (ut1 == "") || (ut2 == "")) //ha nincs rendben az adatbazis
            {
                label7.ForeColor = System.Drawing.Color.Red;
                label7.Text = "Hiba!";
                MessageBox.Show("Hibásan van megadva az adatbázis útvonala, vagy nincs megadva egyáltalán!\n                                          Kérjük tallóza az adatbázist!", "Üzenet");
                f2.ShowDialog();
                adatbazis_ellenorzes_kiolvasas();
                ellenorzes();
            }
        }

        public Form1()
        {
            InitializeComponent();
            adatbazis_ellenorzes_kiolvasas();
            ellenorzes();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (dbellenorzes == false)
            {
                MessageBox.Show("Hibásan van megadva az adatbázis útvonala, vagy nincs megadva egyáltalán!", "Üzenet");
            }
            else
            {
                System.Diagnostics.Process.Start("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\ABC Raktár.exe");
                Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (dbellenorzes == false)
            {
                MessageBox.Show("Hibásan van megadva az adatbázis útvonala, vagy nincs megadva egyáltalán!", "Üzenet");
            }
            else
            {
                System.Diagnostics.Process.Start("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\ABC Eladó.exe");
                Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (dbellenorzes == false)
            {
                MessageBox.Show("Hibásan van megadva az adatbázis útvonala, vagy nincs megadva egyáltalán!", "Üzenet");
            }
            else
            {
                System.Diagnostics.Process.Start("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\ABC Raktár.exe");
                Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (dbellenorzes == false)
            {
                MessageBox.Show("Hibásan van megadva az adatbázis útvonala, vagy nincs megadva egyáltalán!", "Üzenet");
            }
            else
            {
                System.Diagnostics.Process.Start("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\ABC Eladó.exe");
                Close();
            }
        }

        private void kilépéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void adatbázisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
        }

        private void aProgramrólToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();

            f3.ShowDialog();
        }

    }
}

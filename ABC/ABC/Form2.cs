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
    public partial class Form2 : Form
    {
        public string utvonal;
        string ut1, ut2;

        public Form2()
        {
            InitializeComponent();
        }

        public void adatbazis_ellenorzes_kiolvasas()
        {
            TextReader tr = new StreamReader("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");
            TextReader tr2 = new StreamReader("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            ut1 = tr.ReadLine();
            ut2 = tr2.ReadLine();

            tr.Close();
            tr2.Close();
        }

        private void button1_Click(object sender, EventArgs e) //adatbazis tallozasa
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName); //fajl megnyitas
                utvonal = openFileDialog1.FileName; //fajl utvonala az utvonal stringbe kerul
                textBox1.Text = utvonal; 

                if (utvonal != "") button2.Enabled = true; //ha oke az utvonal, az oke gomb aktiv lesz
                else button2.Enabled = false;
                sr.Close();
            }
   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextWriter tw = new StreamWriter("..\\..\\..\\..\\ABC Eladó\\ABC Eladó\\bin\\Debug\\db.ini");
            TextWriter tw2 = new StreamWriter("..\\..\\..\\..\\ABC Raktár\\ABC Raktár\\bin\\Debug\\db2.ini");

            tw.Write(utvonal);
            tw2.Write(utvonal);
            
            tw.Close();
            tw2.Close();

            MessageBox.Show("Adatbázis sikeresen kiválasztva!","Üzenet");



            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            adatbazis_ellenorzes_kiolvasas();
            if (ut1 == ut2) textBox2.Text = ut1;
            else textBox2.Text = "Nincs/hibás útvonal!";
        
            textBox1.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            adatbazis_ellenorzes_kiolvasas();
            if (ut1 != ut2)
            {
                DialogResult dialogResult = MessageBox.Show("Ki szeretnél lépni a programból?", "Kérdés", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    System.Environment.Exit(0);
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            
        }

    }
}

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
    public partial class Form10 : Form
    {
        public string dbutvonal, connect;
        static public OleDbConnection con;
        static public OleDbCommand myCommand;
        static public OleDbDataReader dr;

        public Form10()
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

        private void Form10_Load(object sender, EventArgs e)
        {
            adatbazis_utvonal();
            frissit();
            frissit2();
            frissit3();
            frissit4();
        }

        private void frissit()
        {
            try
            {
                listView1.Items.Clear();

                con = new OleDbConnection(connect);
                con.Open();
                myCommand = con.CreateCommand();

                myCommand.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mennyiség = 0";
                dr = myCommand.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = dr["Azonosító"].ToString();
                    sor.SubItems.Add(dr["Kategória"].ToString());
                    sor.SubItems.Add(dr["Név"].ToString());

                    listView1.Items.Add(sor);
                }

                dr.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void frissit3()
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();

            string datum = nap + "/" + honap + "/" + ev;


            try
            {
                listView3.Items.Clear();

                con = new OleDbConnection(connect);
                con.Open();
                myCommand = con.CreateCommand();

                myCommand.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Szavatosság < #" + datum + "#";
                dr = myCommand.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = dr["Azonosító"].ToString();
                    sor.SubItems.Add(dr["Kategória"].ToString());
                    sor.SubItems.Add(dr["Név"].ToString());

                    listView3.Items.Add(sor);
                }

                dr.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void frissit4()
        {
            string nap = DateTime.Now.Day.ToString();
            string honap = DateTime.Now.Month.ToString();
            string ev = DateTime.Now.Year.ToString();

            string datum = nap + "/" + honap + "/" + ev;


            try
            {
                listView4.Items.Clear();

                con = new OleDbConnection(connect);
                con.Open();
                myCommand = con.CreateCommand();

                myCommand.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Szavatosság From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Szavatosság > #" + datum + "# ORDER BY Áru.Szavatosság";
                dr = myCommand.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = dr["Azonosító"].ToString();
                    sor.SubItems.Add(dr["Kategória"].ToString());
                    sor.SubItems.Add(dr["Név"].ToString());
                    sor.SubItems.Add(dr["Szavatosság"].ToString());

                    listView4.Items.Add(sor);
                }

                dr.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void frissit2()
        {
            try
            {
                listView2.Items.Clear();

                con = new OleDbConnection(connect);
                con.Open();
                myCommand = con.CreateCommand();

                myCommand.CommandText = "Select Áru.Azonosító ,Kategória.Név as Kategória, Áru.Név, Áru.Mennyiség From Kategória INNER JOIN Áru ON Kategória.Azonosító = Áru.Kategória_Id WHERE Áru.Mennyiség < 11 AND Áru.Mennyiség > 0";
                dr = myCommand.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem sor = new ListViewItem();
                    sor.Text = dr["Azonosító"].ToString();
                    sor.SubItems.Add(dr["Kategória"].ToString());
                    sor.SubItems.Add(dr["Név"].ToString());
                    sor.SubItems.Add(dr["Mennyiség"].ToString());

                    listView2.Items.Add(sor);
                }

                dr.Close();
            }
            catch (Exception y)
            {
                MessageBox.Show(y.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

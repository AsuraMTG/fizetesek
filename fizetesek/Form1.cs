using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace fizetesek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class DolgozoAdatok
        {
            public string nev;
            public string neme;
            public string reszleg;
            public int belepes;
            public int ber;
        }
        public static string[] Beolvaso(string fileName)
        {
            string[] a = new string[170];

            FileStream folyam = new FileStream(fileName, FileMode.Open);
            StreamReader olvas = new StreamReader(folyam);

            string elso = olvas.ReadLine();

            int counter = 0;

            while (!olvas.EndOfStream)
            {
                elso = olvas.ReadLine();

                a[counter]= elso;
                counter++;
            }

            folyam.Close();

            return a;   
        }
        public DolgozoAdatok dolgozokFeltoltese = new DolgozoAdatok();
        public List<DolgozoAdatok> dolgozok = new List<DolgozoAdatok>();
        public void Rendezo()
        {
            string[] a = Beolvaso("fizetések.txt");
            string[] resz;
            for (int i = 0; i < a.Length; i++)
            {
                string seged = a[i];
                resz = a[i].Split(';');

                DolgozoAdatok dolgozokFeltoltese = new DolgozoAdatok();

                dolgozokFeltoltese.nev = resz[0];
                dolgozokFeltoltese.neme = resz[1];
                dolgozokFeltoltese.reszleg = resz[2];
                dolgozokFeltoltese.belepes = Convert.ToInt32(resz[3]);
                dolgozokFeltoltese.ber = Convert.ToInt32(resz[4]);

                dolgozok.Add(dolgozokFeltoltese);
            }
        }
        public string Max()
        {
            int seged = 0;
            int seged2 = 0;
            for (int i = 0; i < dolgozok.Count; i++)
            {
                if (dolgozok[i].reszleg == "karbantartás")
                {
                    if (seged < dolgozok[i].ber)
                    {
                        seged = dolgozok[i].ber;
                        seged2 = i;
                    }
                }
            }

            return dolgozok[seged2].nev;
        }
        public int Reszleg(string megnevezes)
        {
            int counter = 0;
            for (int i = 0; i < dolgozok.Count; i++)
            {
                if (dolgozok[i].reszleg == megnevezes)
                {
                    counter++;
                }
            }

            return counter;
        }

        public Dictionary<string, int> Reszlegek = new Dictionary<string, int>();
        public void MindenReszleg()
        {
            for (int i = 0; i < dolgozok.Count; i++)
            {
                int counter = 0;
                if (!Reszlegek.ContainsKey(dolgozok[i].reszleg))
                {
                    string a = dolgozok[i].reszleg;
                    for (int j = 0; j < dolgozok.Count; j++)
                    {
                        if (dolgozok[j].reszleg == a)
                        {
                            counter++;
                        }
                    }
                    Reszlegek.Add(a, counter);
                }
            }

            listBox1.DataSource = new BindingSource(Reszlegek, null);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Rendezo();
            label1.Text = "";

            var watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"{Max()}";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            label1.Text += $" Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"{Reszleg("pénzügy")}";
            elapsedMs = watch.ElapsedMilliseconds;
            label1.Text += $" Run Time: {elapsedMs}\n";

            MindenReszleg();

            
            
        }
    }
}

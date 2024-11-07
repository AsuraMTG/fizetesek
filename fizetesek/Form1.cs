using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

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

        public double Atlag()
        {
            return dolgozok.Average(x => x.ber);
        }

        public void GroupBy1()
        {
            dolgozok.GroupBy(x => x.reszleg).ToList().ForEach(x => listBox1.Items.Add(x.Key + " " + x.Count()));
        }

        public void GroupBy2()
        {
            var eredmeny = from x in dolgozok group x by x.reszleg;

            foreach (var x in eredmeny)
            {
                listBox1.Items.Add(x.Key + " " + x.Count());
            }
        }

        public void DoSumthing()
        {
            double fCounter = 0;
            double fAverage = 0;
            double nCounter = 0;
            double nAverage = 0;
            for (int i = 0; i < dolgozok.Count; i++)
            {
                if (dolgozok[i].reszleg == "asztalosműhely")
                {
                    if (dolgozok[i].neme == "férfi")
                    {
                        fCounter++;
                        fAverage += dolgozok[i].ber;
                    }
                    else
                    {
                        nCounter++;
                        nAverage += dolgozok[i].ber;
                    }

                }
            }
            label3.Text = $"F: {fAverage / fCounter}\nN: {nAverage / nCounter}\n{(fAverage / fCounter) / (nAverage / nCounter)}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Rendezo();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text = $"{Max()}\n";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            label2.Text = $"Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"{Reszleg("pénzügy")}\n";
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            label2.Text += $"Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"Átlag bér: {Atlag()}\n";
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            label2.Text += $"Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"MindenReszleg();\n";
            //MindenReszleg();
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            label2.Text += $"Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"GroupBy1();\n";
            //GroupBy1();
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            label2.Text += $"Run Time: {elapsedMs}\n";

            watch = System.Diagnostics.Stopwatch.StartNew();
            label1.Text += $"GroupBy2();\n";
            GroupBy2();
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            label2.Text += $"Run Time: {elapsedMs}\n";

            DoSumthing();
            
        }
    }
}

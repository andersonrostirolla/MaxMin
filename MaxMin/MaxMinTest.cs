using MaxMin.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxMin
{
    public partial class MaxMinTest : Form
    {
        public MaxMinTest()
        {
            InitializeComponent();
        }
        private List<Double> listDados = new List<Double>();
        private AlgoritmMaxMin alg = new AlgoritmMaxMin();
       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader rd = new StreamReader(@"C:\git\MaxMin\file2.csv");

                string linha = "";

                while ((linha = rd.ReadLine()) != null)
                    listDados.Add(Convert.ToDouble(linha));


                rd.Close();

                alg.trataAnomalia(listDados);

                Console.WriteLine("Lista tratada:");

                foreach (double row in listDados)
                    Console.WriteLine(row.ToString());
            }
            catch
            {
                Console.WriteLine("Erro ao executar Leitura do Arquivo");
            }
        }
    }
}
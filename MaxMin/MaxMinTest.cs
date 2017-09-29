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

        private List<Double> linhaseparada = new List<Double>();
        private List<String> classificacao = new List<String>();
        private int cont = 0, cont2 = 0;
        private AlgoritmMaxMin alg = new AlgoritmMaxMin();

        private void button1_Click(object sender, EventArgs e)
        {
            alg.setMaxMin(2.1,3.0);
            alg.setVisinhos(2);
            try
            {
                StreamReader rd = new StreamReader(@"C:\Users\Anderson\Desktop\file2.csv");
                string linha = "";
                while ((linha = rd.ReadLine()) != null)
                {
                    linhaseparada.Add(Convert.ToDouble(linha));
                }

                rd.Close();

                recursiveMethod();

                for (int i = 0; i < linhaseparada.Count(); i++)
                {
                    foreach (double row in linhaseparada)
                    {
                        alg.setClassificacao(row);
                        classificacao.Add(alg.getClassificacao());
                    }

                    if (classificacao.Contains("1"))
                    {
                        recursiveMethod();
                        classificacao.Clear();
                    }
                }

                Console.WriteLine("Lista refeita:");
                foreach (double row in linhaseparada)
                {
                    Console.WriteLine(row.ToString());
                }
            }
            catch
            {
                Console.WriteLine("Erro ao executar Leitura do Arquivo");
            }

        }

        public void recursiveMethod()
        {
            for (int i = 0; i <= linhaseparada.Count() - alg.getVisinhos(); i++)
            {
                if (i >= (linhaseparada.Count() - alg.getVisinhos()))
                {
                    for (int j = i; j < linhaseparada.Count(); j++)
                    {
                        cont = linhaseparada.Count() - j;
                        if ((linhaseparada[j] >= alg.getMin()) && (linhaseparada[j] <= alg.getMax()))
                        {
                            linhaseparada[j] = linhaseparada[i];
                        }
                        else
                        {
                            if (j == linhaseparada.Count() - cont && alg.getVisinhos() == 1 && linhaseparada.Count() >= 1)
                            {
                                linhaseparada[j] = linhaseparada[j - 1];
                            }
                            else if (j == linhaseparada.Count() - cont && alg.getVisinhos() == 2 && linhaseparada.Count() >= 2)
                            {
                                linhaseparada[j] = ((linhaseparada[j - 1] + linhaseparada[j - 2]) / alg.getVisinhos());
                            }
                            else if (j == linhaseparada.Count() - cont && alg.getVisinhos() == 3 && linhaseparada.Count() >= 3)
                            {
                                linhaseparada[j] = ((linhaseparada[j - 1] + linhaseparada[j - 2] + linhaseparada[j - 3]) / alg.getVisinhos());
                            }
                        }
                    }
                }
                else if (i >= alg.getVisinhos())
                {
                    if ((linhaseparada[i] >= alg.getMin()) && (linhaseparada[i] <= alg.getMax()))
                    {
                        linhaseparada[i] = linhaseparada[i];
                    }
                    else
                    {
                        switch (alg.getVisinhos())
                        {
                            case 1:
                                linhaseparada[i] = ((linhaseparada[i + 1] + linhaseparada[i - 1]) / (alg.getVisinhos() * 2));
                                break;
                            case 2:
                                linhaseparada[i] = ((linhaseparada[i - 2] + linhaseparada[i - 1] + linhaseparada[i + 1] + linhaseparada[i + 2]) / (alg.getVisinhos() * 2));
                                break;
                            case 3:
                                linhaseparada[i] = ((linhaseparada[i - 3] + linhaseparada[i - 2] + linhaseparada[i - 1] + linhaseparada[i + 1] + linhaseparada[i + 2] + linhaseparada[i + 3]) / (alg.getVisinhos() * 2));
                                break;
                            default:
                                Console.WriteLine("O Tratamento para visinhos foi implementado somente até 3.");
                                break;
                        }
                    }
                }
                else
                {
                    cont2 = ((i+alg.getVisinhos()) - alg.getVisinhos());
                    if ((linhaseparada[i] >= alg.getMin()) && (linhaseparada[i] <= alg.getMax()))
                    {
                        linhaseparada[i] = linhaseparada[i];
                    }
                    else
                    {
                        if (i == cont2 && alg.getVisinhos() == 1 && linhaseparada.Count() >= 1)
                        {
                            if (linhaseparada[i + 1] >= alg.getMin())
                                linhaseparada[i] = linhaseparada[i + 1];
                            else
                                linhaseparada[i] = alg.getMin();
                        }
                        else if (i == cont2 && alg.getVisinhos() == 2 && linhaseparada.Count() >= 2)
                        {
                            if (linhaseparada[i + 1] >= alg.getMin())
                                linhaseparada[i] = ((linhaseparada[i + 1] + linhaseparada[i + 2]) / alg.getVisinhos());
                            else
                                linhaseparada[i] = alg.getMin();
                        }
                        else if (i == cont2 && alg.getVisinhos() == 3 && linhaseparada.Count() >= 3)
                        {
                            if (linhaseparada[i + 1] >= alg.getMin())
                                linhaseparada[i] = ((linhaseparada[i + 1] + linhaseparada[i + 2] + linhaseparada[i + 3]) / alg.getVisinhos());
                            else
                                linhaseparada[i] = alg.getMin();
                        }
                    }
                }
            }


        }
    }
}

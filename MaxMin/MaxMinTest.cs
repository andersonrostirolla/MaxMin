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
        private List<String> classificacao = new List<String>();
        private int cont = 0, cont2 = 0;
        private AlgoritmMaxMin alg = new AlgoritmMaxMin();

        private void button1_Click(object sender, EventArgs e)
        {
            alg.setMaxMin(2.1,3.0);
            alg.setVizinhos(3);
            try
            {
                StreamReader rd = new StreamReader(@"C:\Users\Anderson\Desktop\file2.csv");
                string linha = "";
                while ((linha = rd.ReadLine()) != null)
                {
                    listDados.Add(Convert.ToDouble(linha));
                }

                rd.Close();

                recursiveMethod();

                for (int i = 0; i < listDados.Count(); i++)
                {
                    foreach (double row in listDados)
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

                Console.WriteLine("Lista tratada:");
                foreach (double row in listDados)
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
            for (int i = 0; i <= listDados.Count() - alg.getVizinhos(); i++)
            {
                if (i >= (listDados.Count() - alg.getVizinhos()))
                {
                    for (int j = i; j < listDados.Count(); j++)
                    {
                        cont = listDados.Count() - j;
                        if ((listDados[j] >= alg.getMin()) && (listDados[j] <= alg.getMax()))
                        {
                            listDados[j] = listDados[i];
                        }
                        else
                        {
                            if (j == listDados.Count() - cont && alg.getVizinhos() == 1 && listDados.Count() >= 1)
                            {
                                listDados[j] = listDados[j - 1];
                            }
                            else if (j == listDados.Count() - cont && alg.getVizinhos() == 2 && listDados.Count() >= 2)
                            {
                                if (j < listDados.Count()-1)
                                {
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j + 1]) / (alg.getVizinhos() - 1));
                                }
                                else
                                {
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2]) / alg.getVizinhos());
                                }
                            }
                            else if (j == listDados.Count() - cont && alg.getVizinhos() == 3 && listDados.Count() >= 3)
                            {
                                if (j < listDados.Count()-1)
                                {
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3] + listDados[j + 2] + listDados[j + 1]) / (alg.getVizinhos()-1));
                                }
                                else
                                {
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3]) / alg.getVizinhos());
                                }
                            }
                        }
                    }
                }
                else if (i >= alg.getVizinhos())
                {
                    if ((listDados[i] >= alg.getMin()) && (listDados[i] <= alg.getMax()))
                    {
                        listDados[i] = listDados[i];
                    }
                    else
                    {
                        switch (alg.getVizinhos())
                        {
                            case 1:
                                listDados[i] = ((listDados[i + 1] + listDados[i - 1]) / (alg.getVizinhos() * 2));
                                break;
                            case 2:
                                listDados[i] = ((listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2]) / (alg.getVizinhos() * 2));
                                break;
                            case 3:
                                listDados[i] = ((listDados[i - 3] + listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / (alg.getVizinhos() * 2));
                                break;
                            default:
                                Console.WriteLine("O Tratamento para vizinhos foi implementado somente até 3.");
                                break;
                        }
                    }
                }
                else
                {
                    cont2 = ((i+alg.getVizinhos()) - alg.getVizinhos());
                    if ((listDados[i] >= alg.getMin()) && (listDados[i] <= alg.getMax()))
                    {
                        listDados[i] = listDados[i];
                    }
                    else
                    {
                        if (i == cont2 && alg.getVizinhos() == 1 && listDados.Count() >= 1)
                        {
                            if (listDados[i + 1] >= alg.getMin())
                                listDados[i] = listDados[i + 1];
                            else
                                listDados[i] = alg.getMin();
                        }
                        else if (i == cont2 && alg.getVizinhos() == 2 && listDados.Count() >= 2)
                        {
                            if (listDados[i + 1] >= alg.getMin())
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2]) / alg.getVizinhos());
                            else
                                listDados[i] = alg.getMin();
                        }
                        else if (i == cont2 && alg.getVizinhos() == 3 && listDados.Count() >= 3)
                        {
                            if (listDados[i + 1] >= alg.getMin())
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / alg.getVizinhos());
                            else
                                listDados[i] = alg.getMin();
                        }
                    }
                }
            }


        }
    }
}

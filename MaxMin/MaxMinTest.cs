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
        //Lista carregada com os valores passados via CSV ou via Banco.
        private List<Double> listDados = new List<Double>();
        //Lista para controlar a classificação de cada anomalia da lista de valores.
        private List<String> classificacao = new List<String>();
        //variaveis de controle para o algoritmo do vizinho mais proximo.
        private int cont = 0;
        //declaração da classe que conterá o tratamento de anomalia.
        private AlgoritmMaxMin alg = new AlgoritmMaxMin();

        private void button1_Click(object sender, EventArgs e)
        {
            //Método responsável por setar o valor minimo e o valor máximo aceito para o controle de anomalia.
            alg.setMinMax(2.1, 4.0);
            //Método responsável por setar o range de dados para trás e para frente do dado com anomalia a ser tratado.
            //Ex: Uma lista com valores de: 1, 2, 3, 2, 1. Neste exemplo levaremos em consideração o minimo e o máximo como (1.0, 2.9) e os vizinhos (2).
            //Neste caso, o algoritmo encontrará um problema no terceiro elemento, este elemento passará pelo seguinte calculo de anomalia: 
            //((elemento[i-2] + elemento[i-1] + elemento[i+1] + elemento[i+2])/(vizinhos*2)) após passar pelo calculo: ((1+2+2+1)/4) = 1.5 (Dentro do valor) 
            //Caso o valor não chegasse dentro da faixa estabelecida, o algoritmo iria fazer recursões até que chegasse com todos os valores dentro da faixa.
            alg.setVizinhos(3);
            //utilizado para arquivos CSV.
            try
            {
                //inicializa o arquivo CSV.
                StreamReader rd = new StreamReader(@"C:\GIT\MaxMin\file.csv");
                //variavel de controle.
                string linha = "";
                //Enquanto houver linhas no arquivo a ser lido, continua
                while ((linha = rd.ReadLine()) != null)
                {
                    //Entrada de dados na lista que será submetida ao algoritmo de tratamento.
                    listDados.Add(Convert.ToDouble(linha));
                }
                //fechar o arquivo.
                rd.Close();

                //Método responsável pelo tratamento das anomalias.
                trataAnomalia();

                //For para realizar todas as recursões necessárias para correção da anomalia.
                for (int i = 0; i < listDados.Count(); i++)
                {
                    //For para pegar os valores da lista já submetida e realizar a verificação se os valores estão ou não dentro do alcance. 
                    foreach (double row in listDados)
                    {
                        //Método responsável por submeter o valor atual do For a classificação, ela dirá se é anomalia ou não.
                        alg.setClassificacao(row);
                        //Lista com todas as classificações que será utilizada para a validação de necessidade ou não de mais uma recursão.
                        classificacao.Add(alg.getClassificacao());
                    }

                    //verificação de necessidade de recursão, caso a lista contenha algum valor = "1" a recursão de tratamento de anomalia é chamada novamente.
                    if (classificacao.Contains("1"))
                    {
                        //recursão chamada por conta de ainda existirem anomalias após o primeiro tratamento.
                        trataAnomalia();
                        //Limpar a classificação para nova analise, já que a lista de valores foi resubmetida ao tratamento.
                        classificacao.Clear();
                    }
                }

                //Demonstrar a lista correta.
                Console.WriteLine("Lista tratada:");
                foreach (double row in listDados)
                {
                    Console.WriteLine(row.ToString());
                }
            }
            catch
            {
                //Algum erro encontrado.
                Console.WriteLine("Erro ao executar Leitura do Arquivo");
            }

        }

        public void trataAnomalia()
        {
            //o for irá somente até a quantidade de valores da lista de dados - o numero de vizinhos porque se ele passar disto o tratamento não pode ser feito.
            //logo, os proximos valores que este for não comporta são tratados de forma diferente dentro dele.
            for (int i = 0; i <= listDados.Count() - alg.getVizinhos(); i++)
            {
                //verificação se o valor atual é um dos ultimos valores.
                if (i >= (listDados.Count() - alg.getVizinhos()))
                {
                    //for para tratar valores que não são contemplados com tratamento igual aos demais por serem os ultimos.
                    for (int j = i; j < listDados.Count(); j++)
                    {
                        //variavel de controle utilizada para saber se é o ultimo, penultimo ou mesmo o antepenultimo valor da lista.
                        cont = listDados.Count() - j;
                        //Antes de entrar nas regras de tratamento, a verificação se o valor esta dentro do alcance estabelecido pelo usuário.
                        if ((listDados[j] >= alg.getMin()) && (listDados[j] <= alg.getMax()))
                        {
                            //caso estiver dentro do alcance, o valor se mantem o mesmo.
                            listDados[j] = listDados[j];
                        }
                        else
                        {
                            //verifica se o valor de j atual é igual ao (ultimo, penultimo ou antepenultimo) valor da lista, também verifica se é o tratamento de um unico vizinho e se existe mais de um valor na lista de tratamento.
                            if (j == listDados.Count() - cont && alg.getVizinhos() == 1 && listDados.Count() >= 1)
                            {
                                //caso atenda as regras, o valor atual recebe o valor anterior como correção.
                                listDados[j] = listDados[j - 1];
                            }
                            //verifica se o valor de j atual é igual ao (ultimo, penultimo ou antepenultimo) valor da lista, também verifica se é o tratamento de dois vizinhos e se existe mais de dois valores na lista de tratamento.
                            else if (j == listDados.Count() - cont && alg.getVizinhos() == 2 && listDados.Count() >= 2)
                            {
                                //verifica se é o penultimo valor.
                                if (j < listDados.Count() - 1)
                                {
                                    //caso o valor seja o penultimo valor, ele recebe a soma dos (dois vizinhos anteriores + o ultimo vizinho) / ((pelo valor de vizinhos * 2) -1)
                                    //o -1 serve para retirar o valor que não existe na lista.
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j + 1]) / ((alg.getVizinhos() * 2) - 1));
                                }
                                //se não é o penultimo valor então é o ultimo.
                                else
                                {
                                    //caso seja o ultimo valor, não existem valore a frente para o teste, logo deve pegar somente os 2 anteriores e dividir pela quantidade de vizinhos.
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2]) / alg.getVizinhos());
                                }
                            }
                            //verifica se o valor de j atual é igual ao (ultimo, penultimo ou antepenultimo) valor da lista, também verifica se é o tratamento de três vizinhos e se existe mais de três valores na lista de tratamento.
                            else if (j == listDados.Count() - cont && alg.getVizinhos() == 3 && listDados.Count() >= 3)
                            {
                                //faz a verificação para o antepenultimo valor.
                                if (j < listDados.Count() - 2)
                                {
                                    //caso seja o antepenultimo valor será necessário somar os 3 valores para trás + os 2 proximos valores e após isto, dividir pelo numero de (vizinhos * 2)-1.
                                    //o -1 serve para retirar o valor que não existe na lista.
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3] + listDados[j + 2] + listDados[j + 1]) / ((alg.getVizinhos() * 2) - 1));
                                }
                                //faz a verificação para o penultimo valor.
                                else if (j < listDados.Count() - 1)
                                {
                                    //caso seja o penultimo valor será necessário somar os 3 valores para trás + o ultimo valor e após isto, dividir pelo numero de (vizinhos * 2)-1.
                                    //o -2 serve para retirar o valor que não existe na lista.
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3] + listDados[j + 1]) / ((alg.getVizinhos() * 2) - 2));
                                }
                                // faz a verificação para o ultimo valor.
                                else
                                {
                                    //caso seja o ultimo valor será necessário somar os 3 valores para trás e dividir pelo numero de vizinhos.
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3]) / alg.getVizinhos());
                                }
                            }
                        }
                    }
                }
                //caso o valor seja maior que os vizinhos quer dizer que ele não precisa se preocupar com a questão valores nulos, ja que todos os valores do meio terão valores para
                //frente e para trás de acordo com o numero de vizinhos.
                else if (i >= alg.getVizinhos())
                {
                    //Antes de entrar nas regras de tratamento, a verificação se o valor esta dentro do alcance estabelecido pelo usuário.
                    if ((listDados[i] >= alg.getMin()) && (listDados[i] <= alg.getMax()))
                    {
                        //caso estiver dentro do alcance, o valor se mantem o mesmo.
                        listDados[i] = listDados[i];
                    }
                    else
                    {
                        //verifica a quantidade de vizinhos estabelecida pelo usuário.
                        switch (alg.getVizinhos())
                        {
                            //caso for 1 unico vizinho para frente e para trás, soma-os e divide pelo numero de vizinhos * 2.
                            case 1:
                                listDados[i] = ((listDados[i + 1] + listDados[i - 1]) / (alg.getVizinhos() * 2));
                                break;
                            //caso for 2 vizinhos para frente e para trás, soma-os e divide pelo numero de vizinhos * 2.
                            case 2:
                                listDados[i] = ((listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2]) / (alg.getVizinhos() * 2));
                                break;
                            //caso for 3 vizinhos para frente e para trás, soma-os e divide pelo numero de vizinhos * 2.
                            case 3:
                                listDados[i] = ((listDados[i - 3] + listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / (alg.getVizinhos() * 2));
                                break;
                            //caso for definido um valor para vizinhos maior que 3, não deve deixar, já que não existe tratamento.
                            default:
                                Console.WriteLine("O Tratamento para vizinhos foi implementado somente até 3.");
                                break;
                        }
                    }
                }
                //caso sejam os primeiros valores da lista de tratamento, deve entrar nesta regra pois não existem valores que antecedem os valores iniciais.
                else
                {
                    //Antes de entrar nas regras de tratamento, a verificação se o valor esta dentro do alcance estabelecido pelo usuário.
                    if ((listDados[i] >= alg.getMin()) && (listDados[i] <= alg.getMax()))
                    {
                        //caso estiver dentro do alcance, o valor se mantem o mesmo.
                        listDados[i] = listDados[i];
                    }
                    else
                    {
                        //caso o valor de vizinhos for = 1 e a lista contiver mais de um unico dado
                        if (alg.getVizinhos() == 1 && listDados.Count() > 2)
                        {
                            //se o proximo valor da lista for maior que o valor minimo estabelecido pelo usuário
                            if (listDados[i + 1] >= alg.getMin())
                                //O valor atual da lista recebe o proximo valor.
                                listDados[i] = listDados[i + 1];
                            //caso o proximo valor não seja maior que o valor minimo estabelecido pelo usuário
                            else
                                //O valor atual recebe o valor minimo.
                                listDados[i] = alg.getMin();
                        }
                        //ARRUMAR A QUESTÃO DO SEGUNDO VALOR E TERCEIRO PARA OS PROXIMOS CASOS QUE AINDA NÃO TINHA PENSADO NISSO.
                        //DEVE CONSIDERAR O PRIMEIRO VALOR E OS DOIS PROXIMOS CASO FOR O SEGUNDO VALOR E A QUANTIDADE DE VIZINHOS FOR = 2

                        //caso o numero de vizinhos seja dois e tiver mais de dois dados na lista
                        else if (alg.getVizinhos() == 2 && listDados.Count() > 4)
                        {
                            //se o proximo valor da lista for maior que o valor minimo estabelecido pelo usuário
                            if (listDados[i + 1] >= alg.getMin())
                                //o valor atual recebe a soma dos proximos 2 valores / pelo numero de vizinhos
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2]) / alg.getVizinhos());
                            //caso seja o segundo valor
                            else if (i == 1)
                            {
                                //o valor atual recebe a soma dos proximos 2 valores / pelo numero de vizinhos
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i - 1]) / ((alg.getVizinhos()*2) -1));
                            }
                            //caso o proximo valor não seja maior que o valor minimo estabelecido pelo usuário
                            else
                            {
                                //O valor atual recebe o valor minimo.
                                listDados[i] = alg.getMin();
                            }
                        }
                        //caso o numero de vizinhos seja três e tiver mais que três dados na lista.
                        else if (alg.getVizinhos() == 3 && listDados.Count() > 6)
                        {
                            //se o proximo valor da lista for maior que o valor minimo estabelecido pelo usuário
                            if (i == 0)
                            {
                                //o valor atual recebe a soma dos proximos 3 valores / pelo numero de vizinhos
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / alg.getVizinhos());
                            }
                            else if (i == 1)
                            {
                                //o valor atual recebe a soma dos proximos 3 valores / pelo numero de vizinhos -2 pois só tem um vizinho para tras
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3] + listDados[i - 1]) / ((alg.getVizinhos() * 2) - 2));
                            }
                            else if (i == 2)
                            {
                                //o valor atual recebe a soma dos proximos 3 valores / pelo numero de vizinhos -1 pois só tem dois vizinho para tras
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3] + listDados[i - 1] + listDados[i - 2]) / ((alg.getVizinhos() * 2) - 1));
                            }
                            //caso o proximo valor não seja maior que o valor minimo estabelecido pelo usuário
                            else
                            {
                                //O valor atual recebe o valor minimo.
                                listDados[i] = alg.getMin();
                            }
                        }
                    }
                }
            }


        }
    }
}
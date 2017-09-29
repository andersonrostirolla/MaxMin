using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxMin
{
    public class AlgoritmMaxMin
    {
        private string anomalia;
        private string valorFinal;
        private double max, min;
        private int vizinhos;

        public void setMinMax(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double getMax()
        {
            return this.max;
        }

        public double getMin()
        {
            return this.min;
        }

        public void setClassificacao(Double valor)
        {
            if (valor > this.max || valor < this.min)
            {
                this.anomalia = "1";
            }
            else
            {
                this.anomalia = "0";
            }
        }

        public void setVizinhos(int valor)
        {
            this.vizinhos = valor;
        }

        public int getVizinhos()
        {
            return this.vizinhos;
        }

        public String getClassificacao()
        {
            return anomalia;
        }

        public void trataAnomalia(List<String> listaValores)
        {
            string anomaliaClassificada = "";

            //tudo que ta na MaxMin vai vir pra ca.
        }

        public String valorTratado()
        {
            return valorFinal;
        }
    }
}
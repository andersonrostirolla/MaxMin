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
        private int visinhos;

        public void setMaxMin(double min, double max)
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

        public void setVisinhos(int valor)
        {
            this.visinhos = valor;
        }

        public int getVisinhos()
        {
            return this.visinhos;
        }

        public String getClassificacao()
        {
            return anomalia;
        }

        public void trataAnomalia(List<String> listaValores)
        {
            string anomaliaClassificada = "";

            /*foreach (double row in listaValores)
            {
                setClassificacao(row);
                anomaliaClassificada = getClassificacao();

                if (anomaliaClassificada == "1")
                {
                    if (Convert.ToDouble(row) == 0)
                    {
                        this.valorFinal = Convert.ToString(this.min);
                    }
                    else
                    {
                        this.valorFinal = Convert.ToString(Convert.ToDouble(row) / 2);
                    }
                }
                else
                {
                    this.valorFinal = row;
                }
            }*/
        }

        public String valorTratado()
        {
            return valorFinal;
        }
    }
}

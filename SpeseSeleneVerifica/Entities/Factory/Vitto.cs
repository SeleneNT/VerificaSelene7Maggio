using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.Factory
{
    public class Vitto: IRimborso
    {
        public double Rimborso(double importo)
        {
            return ((double)importo*70/100);
        }
    }
}

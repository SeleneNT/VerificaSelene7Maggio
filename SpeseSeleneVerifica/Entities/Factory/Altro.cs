using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.Factory
{
    public class Altro : IRimborso
    {
        public double Rimborso(double importo)
        {
            return ((double)importo * 10 / 100);
        }
    }
}

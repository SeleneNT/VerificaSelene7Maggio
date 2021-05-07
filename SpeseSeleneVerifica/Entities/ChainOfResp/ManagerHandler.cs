using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.ChainOfResp
{
    public class ManagerHandler: AbstractHandler
    {
        //Per ogni singola classe della Chain faccio l'override del metodo per accettare la richiesta o delegarla
        public override string Handle(double request)
        {
            if (request<=400 && request >=0)
            {
                return "Manager";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.ChainOfResp
{
    public class OperationalManagerHandler : AbstractHandler
    {
        public override string Handle(double request)
        {
            if (request <= 1000)
            {
                return "Operational Manager";
            }
            else
            {
                return base.Handle(request);
            }
        }

    }
}

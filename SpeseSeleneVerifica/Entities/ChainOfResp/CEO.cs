using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.ChainOfResp
{
    public class CEOHandler: AbstractHandler
    {
        public override string Handle(double request)
        {
            if (request>1000 && request<=2500)
            {
                return "CEO";
            }
            else
            {
                return base.Handle(request);
            }
        }

    }
}

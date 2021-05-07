using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.Factory
{
    public class RimborsoFactory
    {
        public static IRimborso FactoryRimborso(string categoria)
        {
            IRimborso CategoriaTipo = null;

            switch (categoria)
            {
                case "Viaggio":
                    CategoriaTipo = new Viaggio();
                    break;
                case "Alloggio":
                    CategoriaTipo = new Alloggio();
                    break;
                case "Vitto":
                    CategoriaTipo = new Vitto();
                    break;
                default:
                    CategoriaTipo = new Altro();
                    break;
            }

            return CategoriaTipo;

        }
    }
}

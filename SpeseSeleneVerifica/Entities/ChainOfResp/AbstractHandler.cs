using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities.ChainOfResp
{
    public abstract class AbstractHandler: IHandler
    {
        //Con questa classe astratta specifico le generiche proprietà dell'Handler astratto
        private IHandler _nextHandler;

        //Affido al primo Handler
        public IHandler SetNext(IHandler handler)
        {
            return this._nextHandler = handler;
        }

        //Demando al successivo e, se alla fine della catena nessuno ha preso in carica la richiesta, essa 
        //non è accettata da nessuno -> return null
        public virtual string Handle(double request)
        {
            if(this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }else
            {
                return null;
            }
        }
    }
}

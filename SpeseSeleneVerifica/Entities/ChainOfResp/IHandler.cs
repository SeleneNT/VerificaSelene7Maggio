using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeseSeleneVerifica.Entities
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        string Handle(double request);
    }
}

using SpeseSeleneVerifica.Entities;
using SpeseSeleneVerifica.Entities.ChainOfResp;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpeseSeleneVerifica
{
    class Program
    {
        static void Main(string[] args)
        {
            //Avvio il monitoraggio della cartella con il FileSystemWatcher
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Path = @"C:\Users\selene.c.tucciarone\Desktop\watcher";

            //filtro per formato .txt
            fsw.Filter = "*.txt";

            //Richiedo la notifica per una serie di eventi
            fsw.NotifyFilter =
                NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.DirectoryName
                | NotifyFilters.LastAccess;

            //Abilitiamo le notifiche
            fsw.EnableRaisingEvents = true;
            fsw.Created += GestioneEvento.HandleNewTextFile;



            Console.ReadLine();
        }
    }
}

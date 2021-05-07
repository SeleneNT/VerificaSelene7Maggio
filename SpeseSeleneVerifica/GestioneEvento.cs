using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SpeseSeleneVerifica.Entities.ChainOfResp;
using SpeseSeleneVerifica.Entities.Factory;

namespace SpeseSeleneVerifica.Entities
{
    class GestioneEvento
    {
        //Classe che mi permette di gestire le notifiche in merito alla creazione e alla lettura del file

        public static void HandleNewTextFile(object sender, FileSystemEventArgs e)
        {
            List<List<string>> valoriDaStampare = new List<List<string>>();

            //Controllo creazione di un nuovo File.txt
            Console.WriteLine($"Un nuovo file è stato creato col nome: {e.Name}");

            //Leggiamo il contenuto usando lo StreamReader
            using StreamReader reader = File.OpenText(e.FullPath);
            {
                Console.WriteLine("Lettura in corso...");
                Console.WriteLine("-------------------");
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    if (extractImport(line) != 0)
                    {

                        Console.WriteLine($"La spesa è Approvata. L'importo della spesa è {extractImport(line)} euro");
                        Console.WriteLine($"Il livello di approvazione è {LevelApprovation(extractImport(line))}");
                        Console.WriteLine($"L'importo rimborsato è di: {CalcolaRimborso(extractCategory(line), extractImport(line))} euro");

                        double Drimborso = CalcolaRimborso(extractCategory(line), extractImport(line));
                        var SRimborso = Drimborso.ToString();
                        string livello = (string)LevelApprovation(extractImport(line));
                        List<string> dati = new List<string> { line, SRimborso, livello };
                        valoriDaStampare.Add(dati);

                        Console.WriteLine("________________");
                    }
                    else
                    {
                        Console.WriteLine("La spesa non è approvata");

                        var SRimborso = "-";
                        string livello = "-";
                        List<string> datiNo = new List<string> { line, SRimborso, livello };
                        valoriDaStampare.Add(datiNo);

                        Console.WriteLine("________________");
                    }

                }
                //Chiusura dello StreamReader obbligata o la gestione diventa inaccessibile
                reader.Close();
                Console.WriteLine("-------------------");
                Console.WriteLine("Fine Contenuto");
                Console.WriteLine("-------------------");

            }

            SaveNewFile(valoriDaStampare);
        }

        public static double extractImport(string line)
        {
            double importo;
            double.TryParse(line.Split(";")[3], out importo);

            if (Approver(importo))
            {
                return importo;
            }
            return 0;
        }

        public static string extractCategory(string line)
        {
            //estraggo la categoria così da farla valutare dal Factory
            string categoria;
            return categoria = line.Split(";")[1];
        }

        public static bool Approver(double importo)
        {
            if (importo > 2500)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string LevelApprovation(double importo)
        {

            //Chain of Responsability per l'Handler
            var manager = new ManagerHandler();
            var OPmanager = new OperationalManagerHandler();
            var ceo = new CEOHandler();

            //Assumo una catena di comando passandogli l'importo della spesa. Vado salendo, parto dal
            //Manager->OpManager->CEO->richiesta cade nel vuoto se <0 o > di 2500
            manager.SetNext(OPmanager).SetNext(ceo);

            var LevelResult = manager.Handle(importo);  //prendo un importo 
            return LevelResult;                         //restituisco una stringa

        }

        public static double CalcolaRimborso(string categoria, double importo)
        {
            IRimborso rimborso = RimborsoFactory.FactoryRimborso(categoria);

            return rimborso.Rimborso(importo);
        }

        public static void SaveNewFile(List<List<string>> valori)
        {

            Console.WriteLine("Salvataggio in corso....");

            using StreamWriter writer = File.CreateText(@"C:\Users\selene.c.tucciarone\Desktop\watcher\speseElaborate\spese_elaborate.txt");

            {

                //Estrazione dati mancanti
                foreach (var lista in valori)
                {
                    string[] values = lista[0].Split(";");

                    string data = values[0];
                    string categoria = values[1];
                    string descrizione = values[2];

                    string livelloApprovazione = lista[2];
                    string importoRimborsato = lista[1];
                    string approvazione = "";

                    if (livelloApprovazione.Equals("-"))
                    {
                        approvazione = "RESPINTA";
                    }
                    else
                    {
                        approvazione = "APPROVATA";
                    }

                    writer.WriteLine($"{data};{categoria};{descrizione};{approvazione};{livelloApprovazione};{importoRimborsato}");
                    
                }

                Console.WriteLine("Salvataggio completato con successo");
                writer.Close();
            }

        }
    }
}

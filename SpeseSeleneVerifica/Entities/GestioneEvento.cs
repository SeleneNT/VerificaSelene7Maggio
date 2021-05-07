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
                    if(extractImport(line) != 0)
                    {

                        Console.WriteLine($"La spesa è Approvata. L'importo della spesa è {extractImport(line)} euro");
                        Console.WriteLine($"Il livello di approvazione è {LevelApprovation(extractImport(line))}");
                        Console.WriteLine($"L'importo rimborsato è di: {CalcolaRimborso(extractCategory(line), extractImport(line))} euro");

                        //NOTA!!!
                        //Non mi ero accorta di dover recuperare anche altre informazioni (Data e descrizione) e dovrei cambiare la struttura
                        //Inserendo tutta la lettura in un array ma in mancanza di tempo, conscia di rendere il codice verboso e ridondante
                        //Devo rieseguire l'estrazione.
                      
                        SaveNewFile
                            (
                             line,
                             LevelApprovation(extractImport(line)),
                             CalcolaRimborso(extractCategory(line), extractImport(line))
                            );
                        
                        Console.WriteLine("________________");
                    }
                    else
                    {
                        Console.WriteLine("La spesa non è approvata");
                        SaveNewFile
                           (
                            line,
                            "-",
                            0
                             );

                        Console.WriteLine("________________");
                    }
                                        
                }
                //Chiusura dello StreamReader obbligata o la gestione diventa inaccessibile
                reader.Close();
                Console.WriteLine("-------------------");
                Console.WriteLine("Fine Contenuto");
            }


        }
        public static double extractImport(string line)
        {
            double importo;
            double.TryParse(line.Split(";")[3], out importo);
           
            if (Approver(importo)){
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

        public static string LevelApprovation(double importo) { 

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

        public static void SaveNewFile(string line, string level, double rimborso)
        {
            //Estrazione dati mancanti
            string [] values = line.Split(";");
            string data= values[0];
            string categoria = values[1];
            string descrizione = values[2];
            
            string approvazione= null;
            string livelloApprovazione = level;
            double importoRimborsato = rimborso;
            
            if (level.Equals("-"))
            {
                approvazione = "RESPINTA";
            }else
            {
                approvazione = "APPROVATA";
            }

            Console.WriteLine("Salvataggio in corso....");

            using StreamWriter writer = File.CreateText(@"C:\Users\selene.c.tucciarone\Desktop\watcher\spese_elaborate.txt");
            {
                writer.WriteLine($"{data};{categoria};{descrizione};{approvazione};{livelloApprovazione};{importoRimborsato}");
                writer.Close();
            }

            Console.WriteLine("Salvataggio completato con successo");
            // TOFIX! L'azione non finisce di salvare perchè sto usando streamreader e streamwriter contemporaneamente.
            //Se mi fossi salvata tutto in un array a parte avrei potuto fare il salvataggio dopo aver chiuso il Reader

        }
    }
}

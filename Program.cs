using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordStats
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Program je namenjen uporabi znotraj ukazne vrstice.");
                Console.WriteLine("Za normalno analizo uporabite ukaz");
                Console.WriteLine("Wordstat [Ime_datoteke]");
                Console.WriteLine("Za filtrirano analizo pa uporabite ta ukaz");
                Console.WriteLine("Wordstat [Ime_datoteke] [datoteka z besedami za izločitev]");
            }
            else
            {
                ReadWords(args[0]);
            }
        }
        static string[] ReadWords(string location)
        {
            string[] lines = File.ReadAllLines(location);
            List<string> words = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i]=lines[i].ToLower();
                lines[i] = lines[i].Replace(",","");
                lines[i] = lines[i].Replace(".", "");
                lines[i] = lines[i].Replace("!", "");
                lines[i] = lines[i].Replace("?", "");
                lines[i] = lines[i].Replace("(", "");
                lines[i] = lines[i].Replace(")", "");
                lines[i] = lines[i].Replace("\"", "");
                lines[i] = lines[i].Replace("-", "");
                lines[i] = lines[i].Replace(";", "");
                string[] wordsa = lines[i].Split(' ');
                foreach (string worda in wordsa) 
                {
                    words.Add(worda);
                }
                for (int x=0;x< words.Count(); x++)
                {
                    if(words[x].All(char.IsDigit)) 
                    {
                        words.RemoveAt(x);
                        
                    }
                }
            }
            Array.ForEach(words.ToArray(), Console.WriteLine);
            return null;
        }
    }
}

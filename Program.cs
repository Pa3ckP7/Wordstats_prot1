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
            Dictionary<string, int> words = new Dictionary<string, int>();
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
                words=ReadWords(args[0]);
                int sum = 0;
                foreach (var word in words) sum += word.Value;
                Console.WriteLine($"Skupno število besed: {sum}");
                Console.WriteLine($"Število unikatnih besed: {words.Count}");
                Console.WriteLine($"10 najpogostejših besed: WIP");
                Console.WriteLine($"Povprečna dolžina besede: WIP");
                Console.WriteLine($"Število kratkih besed (manj kot 3 znaki): WIP");
                Console.WriteLine($"Število dolgih besed (več kot 3 znaki): WIP");
            }
        }
        static Dictionary<string,int> ReadWords(string location)
        {
            string[] lines = File.ReadAllLines(location);
            Dictionary<string,int> words = new Dictionary<string, int>();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].ToLower();
                lines[i] = lines[i].Replace(",","");
                lines[i] = lines[i].Replace(".", "");
                lines[i] = lines[i].Replace("!", "");
                lines[i] = lines[i].Replace("?", "");
                lines[i] = lines[i].Replace("(", "");
                lines[i] = lines[i].Replace(")", "");
                lines[i] = lines[i].Replace("\"", "");
                lines[i] = lines[i].Replace("-", "");
                lines[i] = lines[i].Replace(";", "");
                string[] temp = lines[i].Split(' ');
                foreach (string str in temp)
                {
                    if (str.All(char.IsDigit)) continue;
                    if (words.ContainsKey(str))
                    {
                        words[str]++;
                    }
                    else
                    {
                        words.Add(str, 1);
                    }
                }
            }
            return words;
        }
    }
}
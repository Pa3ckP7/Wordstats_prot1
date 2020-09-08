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
                Dictionary<string,int> top10 = Top10Lengths(words);
                int avgrLength=AvgrLength(words);
                int numowordless3 = WordsShorter3(words);
                int numowordmore3 = Words3orLonger(words);
                Console.WriteLine($"Skupno število besed: {sum}");
                Console.WriteLine($"Število unikatnih besed: {words.Count}");
                Console.WriteLine($"10 najpogostejših besed: ");
                int placement=0;
                foreach (var word in top10) 
                {
                    placement++;
                    Console.WriteLine(String.Format("{0,3}.{1,15}|{2,5}",placement,word.Key,word.Value));
                }
                Console.WriteLine($"Povprečna dolžina besede: {avgrLength}");
                Console.WriteLine($"Število kratkih besed (manj kot 3 znaki): {numowordless3}");
                Console.WriteLine($"Število dolgih besed (več kot 3 znaki): {numowordmore3}");
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
        static Dictionary<string,int> Top10Lengths(Dictionary<string,int> source) 
        {
            List<string> top10 = new List<string>();
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var word in source) 
            {
                if (top10.Count < 10)
                {
                    top10.Add(word.Key);
                }
                else 
                {
                    for (int i = 0; i < top10.Count; i++) 
                    {
                        int valueToCompare;
                        source.TryGetValue(top10[i], out valueToCompare);
                        if (word.Value > valueToCompare)
                        {
                            top10.Insert(i, word.Key);
                            break;
                        }
                    }
                    if (top10.Count > 10) 
                    {
                        top10.RemoveAt(top10.Count - 1);
                    }
                }
            }
            foreach(string word in top10)
            {
                int valueToAdd;
                source.TryGetValue(word, out valueToAdd);
                result.Add(word, valueToAdd);
            }
            return result;
        }
        static int AvgrLength(Dictionary<string, int> source) 
        {
            List<int> words = new List<int>();
            foreach (var src in source) 
            {
                words.Add(src.Key.Length);
            }
            return (int)words.Average();
        }
        static int WordsShorter3(Dictionary<string, int> source)
        {
            List<string> words = new List<string>();
            foreach (var src in source)
            {
                if (src.Key.Length < 3)
                {
                    words.Add(src.Key);
                }
            }
            return words.Count;
        }
        static int Words3orLonger(Dictionary<string, int> source)
        {
            List<string> words = new List<string>();
            foreach (var src in source)
            {
                if (src.Key.Length >= 3)
                {
                    words.Add(src.Key);
                }
            }
            return words.Count;
        }
    }
}
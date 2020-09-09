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
            Dictionary<string, int> words_filtered = new Dictionary<string, int>();
            string filename;
            string[] stopwords;

            if (args.Length < 1)
            {
                Console.WriteLine("Program je namenjen uporabi znotraj ukazne vrstice.");
                Console.WriteLine("Za normalno analizo uporabite ukaz");
                Console.WriteLine("Wordstat [Ime_datoteke]");
                Console.WriteLine("Za filtrirano analizo pa uporabite ta ukaz");
                Console.WriteLine("Wordstat [Ime_datoteke] [datoteka z besedami za izločitev]");
            }
            else if (args.Length == 1)
            {
                words = ReadWords(args[0]);
                filename = Path.GetFileNameWithoutExtension(args[0]);
                int sum = 0;
                foreach (var word in words) sum += word.Value;
                Dictionary<string, int> top10 = Sort(words, 10);
                Dictionary<string, int> words_sorted = Sort(words, words.Count);
                int avgrLength = AvgrLength(words);
                int numowordless3 = WordsShorter3(words);
                int numowordmore3 = Words3orLonger(words);
                List<string> towritelist = new List<string>();
                foreach (var word in words_sorted)
                {
                    towritelist.Add($"{word.Key}  {word.Value}");
                }
                String[] towrite = towritelist.ToArray();
                if (!File.Exists($"{filename}.besede.txt"))
                {
                    File.WriteAllLines($"{filename}.besede.txt", towrite);
                }
                else
                {
                    Console.WriteLine($"Datoteka {filename}.besede.txt že obstaja želite prepisati to datoteko (y/n) (privzeto y)");
                    char ans = (char)Console.Read();
                    if (ans == 'y' || ans == '\r')
                    {
                        File.WriteAllLines($"{filename}.besede.txt", towrite);
                    }
                    else if (ans == 'n')
                    {
                        Console.WriteLine("Izpis statistike brez shranjevanja seznama besed");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }
                Console.WriteLine($"Skupno število besed: {sum}");
                Console.WriteLine($"Število unikatnih besed: {words.Count}");
                Console.WriteLine($"10 najpogostejših besed: ");
                int placement = 0;
                foreach (var word in top10)
                {
                    placement++;
                    Console.WriteLine(String.Format("{0,3}.{1,15}|{2,5}", placement, word.Key, word.Value));
                }
                Console.WriteLine($"Povprečna dolžina besede: {avgrLength}");
                Console.WriteLine($"Število kratkih besed (manj kot 3 znaki): {numowordless3}");
                Console.WriteLine($"Število dolgih besed (več kot 3 znaki): {numowordmore3}");
            }
            else 
            {
                words = ReadWords(args[0]);
                stopwords = File.ReadAllLines(args[1]);
                words_filtered = Filter(words,stopwords);
                filename = Path.GetFileNameWithoutExtension(args[0]);
                int sum = 0;
                int sumf = 0;
                foreach (var word in words) sum += word.Value;
                foreach (var word in words_filtered) sumf += word.Value;
                Dictionary<string, int> top10 = Sort(words_filtered, 10);
                Dictionary<string, int> words_sorted = Sort(words_filtered, words_filtered.Count);
                int avgrLength = AvgrLength(words_filtered);
                int numowordless3 = WordsShorter3(words_filtered);
                int numowordmore3 = Words3orLonger(words_filtered);
                List<string> towritelist = new List<string>();
                foreach (var word in words_sorted)
                {
                    towritelist.Add($"{word.Key}  {word.Value}");
                }
                String[] towrite = towritelist.ToArray();
                if (!File.Exists($"{filename}.besede.txt"))
                {
                    File.WriteAllLines($"{filename}.besede.txt", towrite);
                }
                else
                {
                    Console.WriteLine($"Datoteka {filename}.besede.txt že obstaja želite prepisati to datoteko (y/n) (privzeto y)");
                    char ans = (char)Console.Read();
                    if (ans == 'y' || ans == '\r')
                    {
                        File.WriteAllLines($"{filename}.besede.txt", towrite);
                    }
                    else if (ans == 'n')
                    {
                        Console.WriteLine("Izpis statistike brez shranjevanja seznama besed");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }
                Console.WriteLine($"Skupno število besed: {sum}");
                Console.WriteLine($"Skupno število besed po filtriranju: {sumf}");
                Console.WriteLine($"Skupno število filtriranih besed: {sum-sumf}");
                Console.WriteLine($"Število unikatnih besed: {words.Count}");
                Console.WriteLine($"Število unikatnih besed filtriranih: {words.Count-words_filtered.Count}");
                Console.WriteLine($"10 najpogostejših besed: ");
                int placement = 0;
                foreach (var word in top10)
                {
                    placement++;
                    Console.WriteLine(String.Format("{0,3}.{1,15}|{2,5}", placement, word.Key, word.Value));
                }
                Console.WriteLine($"Povprečna dolžina besede: {avgrLength}");
                Console.WriteLine($"Število kratkih besed (manj kot 3 znaki): {numowordless3}");
                Console.WriteLine($"Število dolgih besed (več kot 3 znaki): {numowordmore3}");
            }
        }
        static Dictionary<string,int> ReadWords(string location)
        {
            var locila = new[] { ',', '?', '.', '!', ' ','(',')','"',';','-'};
            string[] lines = File.ReadAllLines(location);
            Dictionary<string,int> words = new Dictionary<string, int>();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].ToLower();
                string[] temp = lines[i].Split(locila, StringSplitOptions.RemoveEmptyEntries);
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
        static Dictionary<string,int> Sort(Dictionary<string,int> source, int size) 
        {
            List<string> top10 = new List<string>();
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var word in source) 
            {
                if (top10.Count == 0) 
                {
                    top10.Add(word.Key);
                }
                else if (top10.Count < size)
                {
                    bool inserted = false;
                    for (int i = 0; i < top10.Count; i++)
                    {
                        int valueToCompare;
                        source.TryGetValue(top10[i], out valueToCompare);
                        if (word.Value > valueToCompare)
                        {
                            top10.Insert(i, word.Key);
                            inserted = true;
                            break;
                        }
                        
                    }
                    if (!inserted) 
                    {
                        top10.Add(word.Key);
                    }
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
                    if (top10.Count > size)
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
        static Dictionary<string, int> Filter(Dictionary<string, int> unfiltered, string[] filter) 
        {
            Dictionary<string, int> wordsunfiltered= new Dictionary<string, int>();
            foreach (var word in unfiltered) wordsunfiltered.Add(word.Key, word.Value);
            for (int i = 0; i < filter.Length; i++) 
            {
                if (wordsunfiltered.ContainsKey(filter[i])) 
                {
                    wordsunfiltered.Remove(filter[i]);
                }
            }
            return wordsunfiltered;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordStats
{
    public class Morf
    {
        private Dictionary<string, string> Stems = new Dictionary<string, string>();
        private List<string> Words = new List<string>();
        private HashSet<string> Stopwordsmorf = new HashSet<string>();
        public Morf(Dictionary<string,int> source) 
        {
            foreach (var word in source) 
            {
                Words.Add(word.Key);
            }
            using (var fs = File.OpenRead("morfologija.txt"))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    var l = sr.ReadLine();
                    var arr = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!arr[2].StartsWith("K"))
                    {
                        if (!Stems.ContainsKey(arr[0])) Stems[arr[0]] = arr[1];
                    }
                }
            }
        }
        public Morf(string stopwords)
        {
            
            using (var fsSw = File.OpenRead("stopwords.txt"))
            using (var srSw = new StreamReader(fsSw))
            {
                while (!srSw.EndOfStream)
                {
                    Stopwordsmorf.Add(srSw.ReadLine());
                }
            }
            using (var fs = File.OpenRead("morfologija.txt"))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    var l = sr.ReadLine();
                    var arr = l.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!arr[2].StartsWith("K"))
                    {
                        if (!Stems.ContainsKey(arr[0])) Stems[arr[0]] = arr[1];
                    }
                }
            }
        }
        public KeyValuePair<string, int> Stemify(KeyValuePair<string,int> word) 
        {
            if (Stems.TryGetValue(word.Key, out var r))
            {
                return(new KeyValuePair<string,int>(r,word.Value));
            }
            else
            {
                return(word);
            }
        }
        public string Stemify(string line)
        {
            var entries = Regex.Replace(line, "[.,;:!?()]", "").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                if (Stopwordsmorf.Contains(entry)) continue;
                if (Stems.TryGetValue(entry, out var r))
                {
                    if (Stopwordsmorf.Contains(r)) continue;
                    return r+ " ";
                }
                else
                {
                    return entry+" ";
                }
            }
            return "\n";
        }

    }
}

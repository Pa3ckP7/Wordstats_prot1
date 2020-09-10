using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordStats
{
    public class Morf
    {
        private Dictionary<string, string> stems = new Dictionary<string, string>();
        private List<string> words = new List<string>();
        public Morf(String location, Dictionary<string,int> source) 
        {
            foreach (var word in source) 
            {
                words.Add(word.Key);
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
                        if (!stems.ContainsKey(arr[0])) stems[arr[0]] = arr[1];
                    }
                }
            }
        }
        public string Stemify(string word) 
        {
            if (stems.TryGetValue(word, out var r))
            {
                return(r);
            }
            else
            {
                return(word);
            }
        }

    }
}

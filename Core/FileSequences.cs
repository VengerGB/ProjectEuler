using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    using System.IO;

    public static class FileSequences
    {
        public static IEnumerable<string> Items(string filename, string[] delimiters)
        {
            using (TextReader tr = File.OpenText(filename))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    var items = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    foreach(var item in items)
                    {
                        yield return item.Trim();
                    }
                }
            }

            yield break;
        }
    }
}

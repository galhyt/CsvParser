using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvParser
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvParser csv = new CsvParser("myfile.csv");
            if (!csv.IsEmpty)
            {
                int count = csv.Count;
                for (int i = 0; i < csv.Count; ++i)
                {
                    dynamic row = csv[i];
                    Console.WriteLine("{0}: {1}", row.time, row.result);
                }
                foreach (CsvRow row in csv)
                {
                    Console.WriteLine("{0}: {1}", row["time"], row["result"]);
                }
            }
            List<CsvRow> matches = csv.WhereEquals("result", "OK");
            matches = csv.WhereGreaterThan("status", 200);
            matches = csv.WhereLessThan("status", 500);
        }
    }
}

using System;
using System.Linq;

namespace AsciiTable
{
    class Program
    {
        static void Main(string[] args)
        {
            var reportWriter = new AsciiTable<DataRow>();

            foreach (var item in Enumerable.Range(0, 10))
            {
                reportWriter.Add(new DataRow() { LineNo = item, Name = "Something cool" + item });
            }

            Console.WriteLine(reportWriter.GetOutput());
        }
    }
}

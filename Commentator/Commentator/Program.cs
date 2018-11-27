using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            string text = File.ReadAllText(fileName);
            Console.WriteLine(text);
        }
    }
}

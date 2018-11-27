using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GrEmit;

namespace AdditionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];

            var codeGenerator = new CodeGenerator("Addition", "Add", fileName);
            var assemblyBuilder = codeGenerator.Generate();

            assemblyBuilder.Save(fileName);
        }
    }
}
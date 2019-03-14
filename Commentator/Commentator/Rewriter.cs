using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commentator
{
    public class Rewriter
    {
        private string root;
        private readonly string simpleClass = "GroboIL";
        private readonly string shellClass = "Commentator.GroboILCollector";
        public Rewriter(string projectPath)
        {
            root = projectPath;
        }

        public void RewriteToShellName()
        {
            string[] allfiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allfiles)
            {
                Console.WriteLine("TO_SHELL: " + candidate);
                ReplaceShellNames(candidate, simpleClass, shellClass);
            }
        }

        public void RewriteFromShellName()
        {
            var allfiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allfiles)
            {
                Console.WriteLine("FROM_SHELL: " + candidate);
                ReplaceShellNames(candidate, shellClass, simpleClass);
            }
        }

        private void ReplaceShellNames(string fileName, string nameFrom, string nameTo)
        {
            var lines = File.ReadLines(fileName);
            File.WriteAllText(fileName, string.Empty);

            foreach (var line in lines)
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                    {
                        if (line.Contains(nameFrom+" ") || !line.Contains(nameFrom+"C"))
                            streamWriter.WriteLine(line.Replace(nameFrom, nameTo));
                        else
                            streamWriter.WriteLine(line);
                    }
                }
        }
    }
}

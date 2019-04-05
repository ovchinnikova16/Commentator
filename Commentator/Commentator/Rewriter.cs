using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Commentator
{
    public class Rewriter
    {
        private readonly string root;
        private readonly string simpleClass = "GroboIL";
        private readonly string shellClass = "Commentator.GroboILCollector";
        public Rewriter(string projectPath)
        {
            root = projectPath;
        }

        public void RewriteToShellName()
        {
            var allFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allFiles)
            {
                Console.WriteLine("TO_SHELL: " + candidate);
                ReplaceShellNames(candidate, simpleClass, shellClass);
            }
        }

        public void RewriteFromShellName()
        {
            var allFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allFiles)
            {
                Console.WriteLine("FROM_SHELL: " + candidate);
                ReplaceShellNames(candidate, shellClass, simpleClass);
            }
        }

        private void ReplaceShellNames(string fileName, string nameFrom, string nameTo)
        {
            var lines = File.ReadLines(fileName);
            var content = new StringBuilder();

            foreach (var line in lines)
                {
                    if (line.Contains(nameFrom+" ") || !line.Contains(nameFrom+"C"))
                        content.AppendLine(line.Replace(nameFrom, nameTo));
                    else
                        content.AppendLine(line);
                }
            File.WriteAllText(fileName, content.ToString().Trim(),Encoding.UTF8);
        }
    }
}

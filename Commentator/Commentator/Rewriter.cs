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

        public Rewriter(string projectPath)
        {
            root = projectPath;
        }

        public void RewriteToShellName()
        {
            string[] allfiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allfiles)
            {
                Console.WriteLine(candidate);
                ReplaceShellNames(candidate, "GroboIL", "GroboILCollector");
            }
        }

        public void RewriteFromShellName()
        {
            string[] allfiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allfiles)
            {
                Console.WriteLine(candidate);
                ReplaceShellNames(candidate, "GroboILCollector", "GroboIL");
            }
        }

        private void ReplaceShellNames(string fileName, string nameFrom, string nameTo)
        {
            var file = "temp.cs";
            File.Copy(fileName, file);
            File.WriteAllText(fileName, string.Empty);

            using (StreamReader streamReader = new StreamReader(file))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    using (StreamWriter streamWriter = new StreamWriter(fileName, true))
                    {
                        if (line.Contains(nameFrom+" ") || line.Contains(nameFrom+"("))
                            streamWriter.WriteLine(line.Replace(nameFrom, nameTo));
                        else
                            streamWriter.WriteLine(line);
                    }
                }
            }
            File.Delete("temp.cs");
        }
    }
}

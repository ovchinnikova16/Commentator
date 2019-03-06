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
                Console.WriteLine("TO_SHELL: " + candidate);
                ReplaceShellNames(candidate, "GroboIL", "Commentator.GroboILCollector");
            }
        }

        public void RewriteFromShellName()
        {
            string[] allfiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allfiles)
            {
                Console.WriteLine("FROM_SHELL: " + candidate);
                ReplaceShellNames(candidate, "Commentator.GroboILCollector", "GroboIL");
            }
        }

        private void ReplaceShellNames(string fileName, string nameFrom, string nameTo)
        {
            var file = "temp.cs";
            if (File.Exists(file))
                File.Delete(file);

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

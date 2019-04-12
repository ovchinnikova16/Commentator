using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commentator
{
    public class Replacer
    {
        private readonly string currentName;
        private readonly string targetName; 

        public Replacer(string currentName, string targetName)
        {
            this.currentName = currentName;
            this.targetName = targetName;
        }

        public void Replace(string root)
        {
            var allFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
            foreach (var candidate in allFiles)
            {
                Console.WriteLine("REPLACE: " + candidate);
                ReplaceTo(candidate);
            }
        }

        private void ReplaceTo(string fileName)
        {
            var lines = File.ReadLines(fileName);
            var content = new StringBuilder();

            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, @"\W" + currentName + @"\W"))
                    content.AppendLine(line.Replace( currentName, targetName));
                else
                    content.AppendLine(line);
            }
            File.WriteAllText(fileName, content.ToString().Trim(),Encoding.UTF8);
        }
    }
}

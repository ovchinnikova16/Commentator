using log4net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Commentator
{
    public class Replacer
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Replacer));

        private readonly string currentName;
        private readonly string targetName; 

        public Replacer(string currentName, string targetName)
        {
            this.currentName = currentName;
            this.targetName = targetName;
        }

        public void Replace(string root)
        {
            foreach (var candidate in Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories))
            {
                logger.Debug($"REPLACE: {candidate}");
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

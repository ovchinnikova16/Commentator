using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptCs.ReplCommands;

namespace Commentator
{
    class Commentator
    {
        private readonly string targetFileName;
        private readonly string infoFileName;
        private readonly string resultFileName;
        private readonly Dictionary<int, string> comments = new Dictionary<int, string>();


        public Commentator(string targetFileName, string infoFileName)
        {
            this.targetFileName = targetFileName;
            this.infoFileName = infoFileName;
            this.resultFileName = targetFileName.Remove(targetFileName.Length - 3) + "WithComments.cs";
            File.WriteAllText(resultFileName, string.Empty);
        }

        public void AddComments()
        {
            GetCommentsInfoFromFile();

            var strNumber = 1;

            using (StreamReader streamReader = new StreamReader(targetFileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    using (StreamWriter streamWriter = new StreamWriter(resultFileName, true))
                    {
                        if (comments.ContainsKey(strNumber))
                        {
                            var stackString = String.Join(", ", comments[strNumber].Split(' ').ToArray());
                            streamWriter.WriteLine("{0} // [{1}]", line, stackString);
                        }
                        else
                            streamWriter.WriteLine(line);
                    }

                    strNumber++;
                }
            }
        }

        private void GetCommentsInfoFromFile()
        {
            using (StreamReader streamReader = new StreamReader(infoFileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var file = streamReader.ReadLine();
                    var stringNumber = streamReader.ReadLine();
                    var stackInfo = streamReader.ReadLine();
                    int number;
                    if (file == targetFileName && int.TryParse(stringNumber, out number))
                    {
                        AddNewComment(number, stackInfo);
                    }
                    else
                    {
                        Console.WriteLine("Wrong target file or string number in stackInfo file");
                    }

                }
            }
        }

        private void AddNewComment(int stringNumber, string stackInfo)
        {
            var newStackValues = stackInfo.Split(' ').ToArray();

            if (!comments.ContainsKey(stringNumber))
            {
                comments.Add(stringNumber, stackInfo);
                return;
            }

            var currentStackValues = comments[stringNumber].Split(' ').ToArray();
            var len = Math.Min(newStackValues.Length, currentStackValues.Length);
            for (int i = 0; i < len; i++)
            {
                if (currentStackValues[i] != newStackValues[i])
                    newStackValues[i] = "Object";
            }

            comments[stringNumber] = String.Join(" ", newStackValues);

        }
    }
}

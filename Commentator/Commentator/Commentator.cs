using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.String;

namespace Commentator
{
    class Commentator
    {
        private readonly string targetFileName;
        private readonly string infoFileName;
        private readonly string resultFileName;
        private readonly Dictionary<int, string> comments = new Dictionary<int, string>();
        private readonly Dictionary<string, int> methodMinStackLength = new Dictionary<string, int>();
        private readonly Dictionary<int, string> metodNameByNumber = new Dictionary<int, string>();


        public Commentator(string targetFileName, string infoFileName)
        {
            this.targetFileName = targetFileName;
            this.infoFileName = infoFileName;
            this.resultFileName = targetFileName.Remove(targetFileName.Length - 3) + "WithComments.cs";
            File.WriteAllText(resultFileName, Empty);
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
                            streamWriter.WriteLine("{0} // [{1}]", line, GetStackString(strNumber));
                        else
                            streamWriter.WriteLine(line);
                    }
                    strNumber++;
                }
            }
        }

        private string GetStackString(int strNumber)
        {
            var stackValues = comments[strNumber].Split(' ').ToArray();
            var stackValuesArray = stackValues
                 .Reverse()
                 .Take(stackValues.Length - methodMinStackLength[metodNameByNumber[strNumber]] + 1)
                 .Reverse()
                 .ToArray();
            var stackString = Join(", ", stackValuesArray);

            return stackString;
        }

        private void GetCommentsInfoFromFile()
        {
            using (StreamReader streamReader = new StreamReader(infoFileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var file = streamReader.ReadLine();
                    var methodName = streamReader.ReadLine();
                    var stringNumber = streamReader.ReadLine();
                    var stackInfo = streamReader.ReadLine();

                    int number;
                    if (file == targetFileName && int.TryParse(stringNumber, out number))
                    {
                        AddNewComment(methodName, number, stackInfo);
                    }
                    else
                    {
                        Console.WriteLine("Wrong target file or string number in stackInfo file");
                    }

                }
            }
        }

        private void AddNewComment(string methodName, int stringNumber, string stackInfo)
        {
            var newStackValues = stackInfo.Split(' ').ToArray();

            if (!methodMinStackLength.ContainsKey(methodName))
                methodMinStackLength.Add(methodName, newStackValues.Length);

            if (!comments.ContainsKey(stringNumber))
            {
                comments.Add(stringNumber, stackInfo);
                metodNameByNumber.Add(stringNumber, methodName);
                return;
            }

            var currentStackValues = comments[stringNumber].Split(' ').ToArray();
            var len = Math.Min(newStackValues.Length, currentStackValues.Length);
            var newStack = new string[len]; 

            for (int i = len-1; i >= 0; i--)
                if (currentStackValues[currentStackValues.Length - i - 1] !=
                    newStackValues[newStackValues.Length - i - 1])
                    newStack[len - i - 1] = "Object";
                else
                    newStack[len - i - 1] = newStackValues[newStackValues.Length - i - 1];

            if (newStackValues.Length < methodMinStackLength[methodName])
                methodMinStackLength[methodName] = newStackValues.Length;

            comments[stringNumber] = Join(" ", newStack);
        }
    }
}

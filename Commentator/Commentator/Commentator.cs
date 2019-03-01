using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.String;

namespace Commentator
{
    class Commentator
    {
        private  Dictionary<int, string> comments;
        private  Dictionary<string, int> methodMinStackLength;
        private  Dictionary<int, string> methodNameByNumber;
        private readonly Dictionary<string, List<CommentInfo>> commentsByFile = new Dictionary<string, List<CommentInfo>>();
        private readonly string logFile;

        public Commentator(string infoFileName)
        {
            GetCommentsInfoFromFile(infoFileName);
            logFile = Directory.GetCurrentDirectory() + "ExistingAndNewCommentsLog.txt";

        }

        private void GetCommentsInfoFromFile(string infoFileName)
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
                    if (int.TryParse(stringNumber, out number))
                    {
                        if (!commentsByFile.ContainsKey(file))
                            commentsByFile.Add(file, new List<CommentInfo>());
                        commentsByFile[file].Add(new CommentInfo(file, methodName, number, stackInfo));
                    }
                }
            }
        }

        public void AddComments( )
        {
            foreach (var file in commentsByFile)
            {
                comments = new Dictionary<int, string>();
                methodMinStackLength = new Dictionary<string, int>();
                methodNameByNumber = new Dictionary<int, string>();

                foreach (var commentInfo in commentsByFile[file.Key])
                {
                    AddNewComment(commentInfo.MethodName, commentInfo.StringNumber, commentInfo.StackInfo);
                }

                RewriteFileWithComments(file.Key);
            }
        }

        private void RewriteFileWithComments(string targetFileName)
        {
            var strNumber = 1;

            var fileName = "temp.cs";
            File.Copy(targetFileName, fileName);
            File.WriteAllText(targetFileName, Empty);

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    using (StreamWriter streamWriter = new StreamWriter(targetFileName, true))
                    {
                        if (comments.ContainsKey(strNumber))
                            if (line.Contains("//"))
                            {
                                WriteCommentsLogFile(line, strNumber, targetFileName);
                                streamWriter.WriteLine(line);
                            }
                            else
                                streamWriter.WriteLine("{0} // [{1}]", line, GetStackString(strNumber));
                        else
                            streamWriter.WriteLine(line);
                    }

                    strNumber++;
                }
            }

            File.Delete("temp.cs");
        }


        private void WriteCommentsLogFile(string line, int strNumber, string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(logFile, true))
            {
                int ind = line.IndexOf("//");
                string str = line.Substring(0, ind);
                string comment = line.Substring(ind + 2, line.Length - ind - 2);
                    streamWriter.WriteLine("File: "+fileName);
                    streamWriter.WriteLine("String Number: "+strNumber);
                    streamWriter.WriteLine("String: "+str);
                    streamWriter.WriteLine("ExistingComment: "+comments[strNumber]);
                    streamWriter.WriteLine("NewComment: "+comment);
                    streamWriter.WriteLine("");
            }
        }

        private string GetStackString(int strNumber)
        {
            var stackValues = comments[strNumber].Split(' ').ToArray();
            var stackValuesArray = stackValues
                 .Reverse()
                 .Take(stackValues.Length - methodMinStackLength[methodNameByNumber[strNumber]] + 1)
                 .Reverse()
                 .ToArray();
            var stackString = Join(", ", stackValuesArray);

            return stackString;
        }

        private void AddNewComment(string methodName, int stringNumber, string stackInfo)
        {
            var newStackValues = stackInfo.Split(' ').ToArray();

            if (!methodMinStackLength.ContainsKey(methodName))
                methodMinStackLength.Add(methodName, newStackValues.Length);

            if (!comments.ContainsKey(stringNumber))
            {
                comments.Add(stringNumber, stackInfo);
                methodNameByNumber.Add(stringNumber, methodName);
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

    public class CommentInfo
    {
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public int StringNumber { get; set; }
        public string StackInfo { get; set; }

        public CommentInfo(string fileName, string methodName, int stringNumber, string stackInfo)
        {
            FileName = fileName;
            MethodName = methodName;
            StringNumber = stringNumber;
            StackInfo = stackInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commentator
{
    public class Commentator
    {
        private readonly string logFile;
        private readonly string infoFile;

        public Commentator(string infoFileName)
        {
            infoFile = infoFileName;
            logFile = Path.GetDirectoryName(infoFileName) + @"\ExistingAndNewCommentsLog.txt";
        }

        private Dictionary<string, List<CommentInfo>> GetCommentsInfoFromFile(string infoFileName)
        {
            var commentsByFile = new Dictionary<string, List<CommentInfo>>();

            try
            { 
                using (StreamReader streamReader = new StreamReader(infoFileName))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var file = streamReader.ReadLine();
                        var methodName = streamReader.ReadLine();
                        var stringNumber = streamReader.ReadLine();
                        var stackInfo = streamReader.ReadLine();

                        if (int.TryParse(stringNumber, out var number) && !string.IsNullOrEmpty(stackInfo) && stackInfo.Length > 1)
                        {
                            if (!commentsByFile.ContainsKey(file))
                                commentsByFile.Add(file, new List<CommentInfo>());
                            commentsByFile[file].Add(new CommentInfo(file, methodName, number, stackInfo));
                        }
                    }
                }
                return commentsByFile;
            }
            catch (Exception)
            {
                Console.WriteLine("Stack Info File Error");
                throw;
            }
        }

        public void AddComments()
        {
            var commentsByFile = GetCommentsInfoFromFile(infoFile);

            foreach (var file in commentsByFile)
            {
                var comments = new Dictionary<int, string>();
                var methodMinStackLength = new Dictionary<string, int>();
                var methodNameByNumber = new Dictionary<int, string>();

                foreach (var commentInfo in commentsByFile[file.Key])
                {
                    AddNewComment(
                        commentInfo.MethodName, 
                        commentInfo.StringNumber, 
                        commentInfo.StackInfo, 
                        comments, 
                        methodMinStackLength, 
                        methodNameByNumber);
                }

                RewriteFileWithComments(file.Key, comments, methodNameByNumber, methodMinStackLength);
                Console.WriteLine("COMMENT: "+file.Key);
            }
        }

        private void RewriteFileWithComments(
            string targetFileName, 
            Dictionary<int, string> comments, 
            Dictionary<int, string> methodNameByNumber, 
            Dictionary<string, int> methodMinStackLength)
        {
            var strNumber = 1;
            var lines = File.ReadLines(targetFileName);
            var content = new StringBuilder();

            foreach (var line in lines)
            {
                if (comments.ContainsKey(strNumber))
                    if (line.Contains("//"))
                    {
                        WriteCommentsLogFile(line, strNumber, targetFileName, comments);
                        content.AppendLine(line);
                    }
                    else
                        content.AppendLine(string.Format("{0} // [{1}]", line, GetStackString(strNumber, comments, methodMinStackLength, methodNameByNumber)));
                else
                    content.AppendLine(line);
                strNumber++;
            }
            File.WriteAllText(targetFileName, content.ToString());
        }


        private void WriteCommentsLogFile(
            string line, 
            int strNumber, 
            string fileName, 
            Dictionary<int, string> comments)
        {
            using (StreamWriter streamWriter = new StreamWriter(logFile, true))
            {
                var ind = line.IndexOf("//");
                var str = line.Substring(0, ind);
                var comment = line.Substring(ind + 2, line.Length - ind - 2);
                    streamWriter.WriteLine("File: "+fileName);
                    streamWriter.WriteLine("String Number: "+strNumber);
                    streamWriter.WriteLine("String: "+str);
                    streamWriter.WriteLine("ExistingComment: "+comment);
                    streamWriter.WriteLine("NewComment: ["+ comments[strNumber]+"]");
                    streamWriter.WriteLine("");
            }
        }

        private string GetStackString(int strNumber, 
            Dictionary<int, string> comments, 
            Dictionary<string, int> methodMinStackLength, 
            Dictionary<int, string> methodNameByNumber)
        {
            var stackValues = comments[strNumber].Split(' ').ToArray();
            var stackValuesArray = stackValues
                 .Reverse()
                 .Take(stackValues.Length - methodMinStackLength[methodNameByNumber[strNumber]] + 1)
                 .Reverse()
                 .ToArray();
            var stackString = string.Join(", ", stackValuesArray);

            return stackString;
        }

        private void AddNewComment(
            string methodName, 
            int stringNumber, 
            string stackInfo,
            Dictionary<int, string> comments,
            Dictionary<string, int> methodMinStackLength,
            Dictionary<int, string> methodNameByNumber)
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

            comments[stringNumber] = string.Join(" ", newStack);
        }
    }

    public class CommentInfo
    {
        public string FileName { get; }
        public string MethodName { get; }
        public int StringNumber { get; }
        public string StackInfo { get; }

        public CommentInfo(string fileName, string methodName, int stringNumber, string stackInfo)
        {
            FileName = fileName;
            MethodName = methodName;
            StringNumber = stringNumber;
            StackInfo = stackInfo;
        }
    }
}

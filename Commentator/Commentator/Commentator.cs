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
        private readonly string projectName;

        public Commentator(string infoFileName, string projectName)
        {
            infoFile = infoFileName;
            this.projectName = projectName;
            logFile = Path.GetDirectoryName(infoFileName) + @"\ExistingAndNewCommentsLog.txt";
            File.WriteAllText(logFile, String.Empty);
        }

        public void AddComments()
        {
            var commentsByFile = GetCommentsInfoFromFile(infoFile);

            foreach (var kvp in commentsByFile)
            {
                var comments = new Dictionary<int, string[]>();
                var methodMinStackHead = new Dictionary<string, int>();
                var methodNameByNumber = new Dictionary<int, string>();
                var stackHeadByLine = new Dictionary<int, int>();

                foreach (var commentInfo in commentsByFile[kvp.Key])
                {
                    AddNewCommentToComments(
                        commentInfo, 
                        comments, 
                        methodMinStackHead, 
                        methodNameByNumber, 
                        stackHeadByLine);
                }

                RewriteFileWithComments(kvp.Key, comments, methodNameByNumber, methodMinStackHead, stackHeadByLine);
                Console.WriteLine("COMMENT: "+ kvp.Key);
            }
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
                        var prevStackInfo = streamReader.ReadLine();
                        var stackInfo = streamReader.ReadLine();

                        if (int.TryParse(stringNumber, out var number) &&
                            !string.IsNullOrEmpty(file) &&
                            file.StartsWith(projectName))
                        {
                            if (!commentsByFile.ContainsKey(file))
                                commentsByFile.Add(file, new List<CommentInfo>());

                            commentsByFile[file]
                                .Add(new CommentInfo(file, methodName, number, prevStackInfo, stackInfo));
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

        private void RewriteFileWithComments(
            string targetFileName, 
            Dictionary<int, string[]> comments, 
            Dictionary<int, string> methodNameByNumber, 
            Dictionary<string, int> methodMinStackHead,
            Dictionary<int, int> stackHeadByLine)
        {
            var strNumber = 1;
            var lines = File.ReadLines(targetFileName);
            var content = new StringBuilder();

            ReplaceThisStackElements(comments, methodNameByNumber, stackHeadByLine, lines.ToArray());

            foreach (var line in lines)
            {
                if (comments.ContainsKey(strNumber))
                {
                    var comment = GetStackStringFromValues(
                        strNumber,
                        comments,
                        methodMinStackHead,
                        methodNameByNumber);

                    if (line.Contains("//"))
                    {
                        WriteCommentsLogFile(line, strNumber, targetFileName, comment);
                        content.AppendLine(line);
                    }
                    else
                        content.AppendLine(string.Format("{0} // {1}", line, comment));
                }
                else
                    content.AppendLine(line);
                strNumber++;
            }
            File.WriteAllText(targetFileName, content.ToString(), Encoding.UTF8);
        }

        private void ReplaceThisStackElements(
            Dictionary<int, string[]> comments,
            Dictionary<int, string> methodNameByNumber,
            Dictionary<int, int> stackHeadByLine,
            string[] lines)
        {
            var stringsByMethodName = new Dictionary<string, List<int>>();
            foreach (var e in methodNameByNumber)
            {   
                if (e.Key < 0) continue;
                if (!stringsByMethodName.ContainsKey(e.Value))
                    stringsByMethodName.Add(e.Value, new List<int>());
                stringsByMethodName[e.Value].Add(e.Key);
            }

            foreach (var e in stringsByMethodName)
            {
                e.Value.Sort();
                var prevStack = new string[0];
                
                foreach (var strNumber in e.Value)
                {
                    for (int i = 0; i < Math.Min(stackHeadByLine[strNumber], prevStack.Length); i++)
                        if (prevStack[i] == "this")
                            comments[strNumber][i] = "this";

                    var stackLen = comments[strNumber].Length;
                    if (lines[strNumber - 1].Contains("Dup()") && comments[strNumber][stackLen - 2] == "this")
                        comments[strNumber][stackLen - 1] = "this";

                    prevStack = comments[strNumber];
                }
            }
        }

        private void WriteCommentsLogFile(
            string line, 
            int strNumber, 
            string fileName, 
            string newComment)
        {
            using (StreamWriter streamWriter = new StreamWriter(logFile, true))
            {
                var ind = line.IndexOf("//");
                var str = line.Substring(0, ind);
                var comment = line.Substring(ind + 2, line.Length - ind - 2);
                    streamWriter.WriteLine("File: " + fileName);
                    streamWriter.WriteLine("String Number: " + strNumber);
                    streamWriter.WriteLine("String: " + str.Trim());
                    streamWriter.WriteLine("ExistingComment: " + comment);
                    streamWriter.WriteLine("NewComment: " + newComment);
                    streamWriter.WriteLine("");
            }
        }

        private string GetStackStringFromValues(int strNumber, 
            Dictionary<int, string[]> comments, 
            Dictionary<string, int> methodMinStackHead, 
            Dictionary<int, string> methodNameByNumber)
        {
            var stackValues = comments[strNumber];
            var stackValuesArray = stackValues.Skip(methodMinStackHead[methodNameByNumber[strNumber]]);

            var stackString = "[" + string.Join(", ", stackValuesArray) + "]";

            return stackString;
        }

        private void AddNewCommentToComments(
            CommentInfo commentInfo,
            Dictionary<int, string[]> comments,
            Dictionary<string, int> methodMinStackHead,
            Dictionary<int, string> methodNameByNumber, 
            Dictionary<int, int> stackHeadByLine)
        {

            var newStackValues = GetValuesFromStackString(commentInfo.StackInfo);
            var prevStackValues = GetValuesFromStackString(commentInfo.PrevStackInfo);

            UpdateStackHead(commentInfo, stackHeadByLine, newStackValues, prevStackValues);

            if (!methodMinStackHead.ContainsKey(commentInfo.MethodName))
                methodMinStackHead.Add(commentInfo.MethodName, stackHeadByLine[commentInfo.StringNumber]);
            if (stackHeadByLine[commentInfo.StringNumber] < methodMinStackHead[commentInfo.MethodName])
                methodMinStackHead[commentInfo.MethodName] = stackHeadByLine[commentInfo.StringNumber];

            if (!comments.ContainsKey(commentInfo.StringNumber))
            {
                comments.Add(commentInfo.StringNumber, newStackValues);
                methodNameByNumber.Add(commentInfo.StringNumber, commentInfo.MethodName);
                return;
            }

            UpdateCommentStackValues(commentInfo, comments, methodMinStackHead, newStackValues);
        }

        private static void UpdateCommentStackValues(CommentInfo commentInfo, Dictionary<int, string[]> comments,
            Dictionary<string, int> methodMinStackHead, string[] newStackValues)
        {
            var currentStackValues = comments[commentInfo.StringNumber];

            var len = Math.Min(currentStackValues.Length, newStackValues.Length);

            var newStack = MergeStackValues(
                newStackValues.Skip(newStackValues.Length - len).ToArray(), 
                currentStackValues.Skip(currentStackValues.Length - len).ToArray());

            comments[commentInfo.StringNumber] = newStack;
        }

        private static void UpdateStackHead(
            CommentInfo commentInfo, 
            Dictionary<int, int> stackHeadByLine, 
            string[] newStackValues,
            string[] prevStackValues)
        {
            var headLength = 0;
            for (int i = 0; i < Math.Min(newStackValues.Length, prevStackValues.Length); i++)
            {
                if (newStackValues[i] == prevStackValues[i])
                    headLength++;
            }

            if (!stackHeadByLine.ContainsKey(commentInfo.StringNumber))
                stackHeadByLine.Add(commentInfo.StringNumber, headLength);
            if (headLength < stackHeadByLine[commentInfo.StringNumber])
                stackHeadByLine[commentInfo.StringNumber] = headLength;
        }

        private static string[] MergeStackValues(string[] newStackValues, string[] currentStackValues)
        {
            var len = newStackValues.Length;
            var newStack = new string[len];

            for (int i = len - 1; i >= 0; i--)
            {
                var newV = newStackValues[newStackValues.Length - i - 1].Trim();
                var currentV = currentStackValues[currentStackValues.Length - i - 1].Trim();
                if (newV != currentV)
                {
                    if (newV != "null" && !newV.Contains("Nullable") && currentV != "null" && !currentV.Contains("Nullable"))
                        newStack[len - i - 1] = "Object";
                    else if (newV != "null" && !newV.Contains("Nullable"))
                        newStack[len - i - 1] = newV;
                    else
                        newStack[len - i - 1] = currentV;
                }
                else
                    newStack[len - i - 1] = newV;
            }

            return newStack;
        }

        private string[] GetValuesFromStackString(string stackInfo)
        {
            var counter = 0;
            var content = new StringBuilder();
            foreach (var e in stackInfo)
            {
                if (e == '<') counter++;
                if (e == '>') counter--;
                if (counter == 0)
                {
                    if (e == ' ') content.Append('*');
                    if (e != ',') content.Append(e);
                }
                else
                    content.Append(e);
            }
            return content.ToString().Split('*').Where(x => x != "").ToArray();
        }
    }

    public class CommentInfo
    {
        public string FileName { get; }
        public string MethodName { get; }
        public int StringNumber { get; }
        public string StackInfo { get; }
        public string PrevStackInfo { get; }

        public CommentInfo(string fileName, string methodName, int stringNumber, string prevStackInfo, string stackInfo)
        {
            FileName = fileName;
            MethodName = methodName;
            StringNumber = stringNumber;
            PrevStackInfo = prevStackInfo;
            StackInfo = stackInfo;
        }
    }
}

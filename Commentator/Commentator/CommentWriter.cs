using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commentator
{
    public class CommentWriter
    {
        private static ILog logger = LogManager.GetLogger(typeof(CommentWriter));

        private readonly Dictionary<string, List<CommentInfo>> commentsByFileName;


        private CommentWriter(Dictionary<string, List<CommentInfo>> commentsByFileName)
        {
            this.commentsByFileName = commentsByFileName;
        }

        public static CommentWriter Load(string stackInfoFileName)
        {
            return new CommentWriter(LoadComments(stackInfoFileName));
        }

        public void AddComments(string projectPath)
        {
            var projectPathUri = new Uri(Path.GetFullPath(projectPath));
            foreach (var kvp in commentsByFileName)
            {
                if (!projectPathUri.IsBaseOf(new Uri(kvp.Key)))
                    continue;

                var comments = new Dictionary<int, string[]>();
                var methodMinStackHead = new Dictionary<string, int>();
                var methodNameByNumber = new Dictionary<int, string>();
                var stackHeadByLine = new Dictionary<int, int>();

                foreach (var commentInfo in kvp.Value)
                {
                    AddNewCommentToComments(
                        commentInfo, 
                        comments, 
                        methodMinStackHead, 
                        methodNameByNumber, 
                        stackHeadByLine);
                }

                RewriteFileWithComments(kvp.Key, comments, methodNameByNumber, methodMinStackHead, stackHeadByLine);
                logger.Debug($"COMMENT: {kvp.Key}");
            }
        }

        private static Dictionary<string, List<CommentInfo>> LoadComments(string infoFileName)
        {
            var commentsByFile = new Dictionary<string, List<CommentInfo>>();
            if (!File.Exists(infoFileName))
            {
                logger.Warn($"Comments file {infoFileName} doesn't exist");
                return commentsByFile;
            }
            try
            {
                using (var streamReader = new StreamReader(infoFileName))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    jsonReader.SupportMultipleContent = true;
                    var serializer = new JsonSerializer();
                    while (jsonReader.Read())
                    {
                        var entry = serializer.Deserialize<CommentInfo>(jsonReader);
                        if (!commentsByFile.TryGetValue(entry.FileName, out List<CommentInfo> entryList))
                            commentsByFile[entry.FileName] = entryList = new List<CommentInfo>();
                        entryList.Add(entry);
                    }
                }
                return commentsByFile;
            }
            catch (Exception ex)
            {
                logger.Warn("Failed to read comments from file", ex);
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
                        logger.Debug($"Comment already exists at {targetFileName}:{strNumber}: {comment}");
                        content.AppendLine(line);
                    }
                    else
                    {
                        content.AppendLine(string.Format("{0} // {1}", line, comment));
                    }
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

            var newStackValues = GetValuesFromStackString(commentInfo.NewStackValues);
            var prevStackValues = GetValuesFromStackString(commentInfo.PreviousStackValues);

            UpdateStackHead(commentInfo, stackHeadByLine, newStackValues, prevStackValues);

            if (!methodMinStackHead.ContainsKey(commentInfo.MethodName))
                methodMinStackHead.Add(commentInfo.MethodName, stackHeadByLine[commentInfo.LineNumber]);
            if (stackHeadByLine[commentInfo.LineNumber] < methodMinStackHead[commentInfo.MethodName])
                methodMinStackHead[commentInfo.MethodName] = stackHeadByLine[commentInfo.LineNumber];

            if (!comments.ContainsKey(commentInfo.LineNumber))
            {
                comments.Add(commentInfo.LineNumber, newStackValues);
                methodNameByNumber.Add(commentInfo.LineNumber, commentInfo.MethodName);
                return;
            }

            UpdateCommentStackValues(commentInfo, comments, methodMinStackHead, newStackValues);
        }

        private static void UpdateCommentStackValues(CommentInfo commentInfo, Dictionary<int, string[]> comments,
            Dictionary<string, int> methodMinStackHead, string[] newStackValues)
        {
            var currentStackValues = comments[commentInfo.LineNumber];

            var len = Math.Min(currentStackValues.Length, newStackValues.Length);

            var newStack = MergeStackValues(
                newStackValues.Skip(newStackValues.Length - len).ToArray(), 
                currentStackValues.Skip(currentStackValues.Length - len).ToArray());

            comments[commentInfo.LineNumber] = newStack;
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

            if (!stackHeadByLine.ContainsKey(commentInfo.LineNumber))
                stackHeadByLine.Add(commentInfo.LineNumber, headLength);
            if (headLength < stackHeadByLine[commentInfo.LineNumber])
                stackHeadByLine[commentInfo.LineNumber] = headLength;
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
}

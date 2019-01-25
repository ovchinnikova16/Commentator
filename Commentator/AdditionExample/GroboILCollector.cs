using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using GrEmit;

namespace AdditionExample
{
    public class GroboILCollector : GroboIL
    {
        private static readonly FieldInfo stackFieldInfo = typeof(GroboIL).GetField("stack", BindingFlags.NonPublic | BindingFlags.Instance);

        private string stackInfoFileName = "stackInfo.txt";

        public GroboILCollector(MethodBuilder methodBuilder) : base(methodBuilder)
        {
            File.WriteAllText(@"stackInfo.txt", string.Empty);
        }

        public new void Ldarg(int index)
        {
            base.Ldarg(index);
            SaveStackInfo();
        }

        public new void Add()
        {
            base.Add();
            SaveStackInfo();
        }

        public new void Ret()
        {
            base.Ret(); 
            SaveStackInfo();
        }

        public new void Pop()
        {
            base.Pop();
            SaveStackInfo();
        }

        private void PrintStackInfo()
        {
            var stackTrace = new StackTrace(true);
            Console.WriteLine(stackTrace);
            Console.WriteLine(stackTrace.GetFrame(2).GetFileName());
            Console.WriteLine(stackTrace.GetFrame(2).GetFileLineNumber());
            Console.WriteLine(stackFieldInfo?.GetValue(this));
        }

        private void SaveStackInfo()
        {
            var stackTrace = new StackTrace(true);

            using (StreamWriter streamWriter = new StreamWriter(stackInfoFileName, true))
            {
                var stackValues = GetStackValues();
                if (stackValues == "") return;
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileName());
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetMethod().Name);
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileLineNumber());
                streamWriter.WriteLine(stackValues);
            }

        }

        private string GetStackValues()
        {
            var stackInfo = stackFieldInfo?.GetValue(this);
            if (stackInfo == null)
                return "";
            var stackValues = stackInfo.ToString().Remove(0, 1).Split(' ').Select(x => x.Remove(x.Length - 1)).ToArray();

            return  String.Join(" ", stackValues);
        }
    }
}

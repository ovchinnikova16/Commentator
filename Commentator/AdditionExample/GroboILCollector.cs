using System;
using System.Diagnostics;
using System.IO;
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
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileName());
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileLineNumber());
                streamWriter.WriteLine(stackFieldInfo?.GetValue(this));
            }

        }

    }
}

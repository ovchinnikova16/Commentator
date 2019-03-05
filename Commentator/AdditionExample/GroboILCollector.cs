using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using GrEmit;

namespace AdditionExample
{
    public class GroboILCollector : GroboIL
    {
        private static readonly FieldInfo stackFieldInfo = typeof(GroboIL).GetField("stack", BindingFlags.NonPublic | BindingFlags.Instance);

        private string stackInfoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";

        public GroboILCollector(MethodBuilder methodBuilder) : base(methodBuilder)
        {
            File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public GroboILCollector(MethodBuilder methodBuilder, ISymbolDocumentWriter symbolWriter) : base(methodBuilder, symbolWriter)
        {
            File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public GroboILCollector(DynamicMethod method, bool analyzeStack = true)
          : base(method, analyzeStack)
        {
            File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public GroboILCollector(MethodBuilder method, bool analyzeStack = true)
            : base(method, analyzeStack)
        {
            File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public GroboILCollector(ConstructorBuilder constructor, bool analyzeStack = true)
          : base(constructor, analyzeStack)
        {
            File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public GroboILCollector(ConstructorBuilder constructor, ISymbolDocumentWriter symbolDocumentWriter) 
            : base(constructor, symbolDocumentWriter)
        {
              File.WriteAllText(stackInfoFileName, string.Empty);
        }

        public new void Stfld(FieldInfo field)
        {
            base.Stfld(field);
            SaveStackInfo();
        }

        public new void Seal()
        {
            base.Seal();
            SaveStackInfo();
        }

        public new void Dispose()
        {
            base.Dispose();
            SaveStackInfo();
        }

        public new string GetILCode()
        {
            var value  = base.GetILCode();
            SaveStackInfo();
            return value;
        }

        public new GroboIL.Local DeclareLocal(Type localType, bool pinned = false)
        {
            var value = base.DeclareLocal(localType, pinned);
            SaveStackInfo();
            return value;
        }

        public new GroboIL.Label DefineLabel(string name, bool appendUniquePrefix = true)
        {
            var value = base.DefineLabel(name, appendUniquePrefix);
            SaveStackInfo();
            return value;
        }

        public new void MarkLabel(GroboIL.Label label)
        {
            base.MarkLabel(label);
            SaveStackInfo();
        }

        public new void WriteLine(string str)
        {
            base.WriteLine(str);
            SaveStackInfo();
        }

        public new void WriteLine(GroboIL.Local local)
        {
            base.WriteLine(local);
            SaveStackInfo();
        }

        public new void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            base.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
            SaveStackInfo();
        }

        // public new static void MarkSequencePoint(ILGenerator il, ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)

        public new void BeginExceptionBlock()
        {
            base.BeginExceptionBlock();
            SaveStackInfo();
        }

        public new void BeginCatchBlock(Type exceptionType)
        {
            base.BeginCatchBlock(exceptionType);
            SaveStackInfo();
        }

        public new void BeginExceptFilterBlock()
        {
            base.BeginExceptFilterBlock();
            SaveStackInfo();
        }

        public new void BeginFaultBlock()
        {
            base.BeginFaultBlock();
            SaveStackInfo();
        }

        public new void BeginFinallyBlock()
        {
            base.BeginFinallyBlock();
            SaveStackInfo();
        }

        public new void EndExceptionBlock()
        {
            base.EndExceptionBlock();
            SaveStackInfo();
        }

        public new void Break()
        {
            base.Break();
            SaveStackInfo();
        }

        public new void Nop()
        {
            base.Nop();
            SaveStackInfo();
        }

        public new void Throw()
        {
            base.Throw();
            SaveStackInfo();
        }

        public new void Rethrow()
        {
            base.Rethrow();
            SaveStackInfo();
        }

        public new void Switch(params GroboIL.Label[] labels)
        {
            base.Switch(labels);
            SaveStackInfo();
        }

        public new void Ret()
        {
            base.Ret();
            SaveStackInfo();
        }

        public new void Leave(GroboIL.Label label)
        {
            base.Leave(label);
            SaveStackInfo();
        }

        public new void Jmp(MethodInfo method)
        {
            base.Jmp(method);
            SaveStackInfo();
        }

        public new void Br(GroboIL.Label label)
        {
            base.Br(label);
            SaveStackInfo();
        }

        public new void Brfalse(GroboIL.Label label)
        {
            base.Brfalse(label);
            SaveStackInfo();
        }

        public new void Brtrue(GroboIL.Label label)
        {
            base.Brtrue(label);
            SaveStackInfo();
        }

        public new void Ble(GroboIL.Label label, bool unsigned)
        {
            base.Ble(label, unsigned);
            SaveStackInfo();
        }

        public new void Bge(GroboIL.Label label, bool unsigned)
        {
            base.Bge(label, unsigned);
            SaveStackInfo();
        }

        public new void Blt(GroboIL.Label label, bool unsigned)
        {
            base.Blt(label, unsigned);
            SaveStackInfo();
        }

        public new void Bgt(GroboIL.Label label, bool unsigned)
        {
            base.Bgt(label, unsigned);
            SaveStackInfo();
        }

        public new void Bne_Un(GroboIL.Label label)
        {
            base.Bne_Un(label);
            SaveStackInfo();
        }

        public new void Beq(GroboIL.Label label)
        {
            base.Beq(label);
            SaveStackInfo();
        }

        public new void Ceq()
        {
            base.Ceq();
            SaveStackInfo();
        }

        public new void Cgt(bool unsigned)
        {
            base.Cgt(unsigned);
            SaveStackInfo();
        }

        public new void Clt(bool unsigned)
        {
            base.Clt(unsigned);
            SaveStackInfo();
        }

        public new void Pop()
        {
            base.Pop();
            SaveStackInfo();
        }

        public new void Dup()
        {
            base.Dup();
            SaveStackInfo();
        }

        public new void Ldloca(GroboIL.Local local)
        {
            base.Ldloca(local);
            SaveStackInfo();
        }

        public new void Ldloc(GroboIL.Local local)
        {
            base.Ldloc(local);
            SaveStackInfo();
        }

        public new void Stloc(GroboIL.Local local)
        {
            base.Stloc(local);
            SaveStackInfo();
        }

        public new void Ldnull()
        {
            base.Ldnull();
            SaveStackInfo();
        }

        public new void Initobj(Type type)
        {
            base.Initobj(type);
            SaveStackInfo();
        }

        public new void Cpobj(Type type)
        {
            base.Cpobj(type);
            SaveStackInfo();
        }

        public new void Ldarg(int index)
        {
            base.Ldarg(index);
            SaveStackInfo();
        }

        public new void Starg(int index)
        {
            base.Starg(index);
            SaveStackInfo();
        }

        public new void Ldarga(int index)
        {
            base.Ldarga(index);
            SaveStackInfo();
        }

        public new void Arglist()
        {
            base.Arglist();
            SaveStackInfo();
        }

        public new void Ldc_I4(int value)
        {
            base.Ldc_I4(value);
            SaveStackInfo();
        }

        public new void Ldc_I8(long value)
        {
            base.Ldc_I8(value);
            SaveStackInfo();
        }

        public new void Ldc_R4(float value)
        {
            base.Ldc_R4(value);
            SaveStackInfo();
        }

        public new void Ldc_R8(double value)
        {
            base.Ldc_R8(value);
            SaveStackInfo();
        }

        public new void Ldc_IntPtr(IntPtr value)
        {
            base.Ldc_IntPtr(value);
            SaveStackInfo();
        }

        public new void FreePinnedLocal(GroboIL.Local local)
        {
            base.FreePinnedLocal(local);
            SaveStackInfo();
        }

        public new void Ldlen()
        {
            base.Ldlen();
            SaveStackInfo();
        }

        public new void Ldftn(MethodInfo method)
        {
            base.Ldftn(method);
            SaveStackInfo();
        }

        public new void Ldvirtftn(MethodInfo method)
        {
            base.Ldvirtftn(method);
            SaveStackInfo();
        }

        public new void Stfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            base.Stfld(field, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Ldfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            base.Ldfld(field, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Ldflda(FieldInfo field)
        {
            base.Ldflda(field);
            SaveStackInfo();
        }

        public new void Ldelema(Type elementType, bool asReadonly = false)
        {
            base.Ldelema(elementType, asReadonly);
            SaveStackInfo();
        }

        public new void Ldelem(Type elementType)
        {
            base.Ldelem(elementType);
            SaveStackInfo();
        }

        public new void Stelem(Type elementType)
        {
            base.Stelem(elementType);
            SaveStackInfo();
        }

        public new void Stind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            base.Stind(type, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Ldind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            base.Ldind(type, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Cpblk(bool isVolatile = false, int? unaligned = null)
        {
            base.Cpblk(isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Initblk(bool isVolatile = false, int? unaligned = null)
        {
            base.Initblk(isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Ldtoken(Type type)
        {
            base.Ldtoken(type);
            SaveStackInfo();
        }

        public new void Ldtoken(MethodInfo method)
        {
            base.Ldtoken(method);
            SaveStackInfo();
        }

        public new void Ldtoken(FieldInfo field)
        {
            base.Ldtoken(field);
            SaveStackInfo();
        }

        public new void Castclass(Type type)
        {
            base.Castclass(type);
            SaveStackInfo();
        }

        public new void Isinst(Type type)
        {
            base.Isinst(type);
            SaveStackInfo();
        }

        public new void Unbox_Any(Type type)
        {
            base.Unbox_Any(type);
            SaveStackInfo();
        }

        public new void Box(Type type)
        {
            base.Box(type);
            SaveStackInfo();
        }

        public new void Stobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            base.Stobj(type, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Ldobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            base.Ldobj(type, isVolatile, unaligned);
            SaveStackInfo();
        }

        public new void Newobj(ConstructorInfo constructor)
        {
            base.Newobj(constructor);
            SaveStackInfo();
        }

        public new void Newarr(Type type)
        {
            base.Newarr(type);
            SaveStackInfo();
        }

        public new void Ckfinite()
        {
            base.Ckfinite();
            SaveStackInfo();
        }

        public new void And()
        {
            base.And();
            SaveStackInfo();
        }

        public new void Or()
        {
            base.Or();
            SaveStackInfo();
        }

        public new void Xor()
        {
            base.Xor();
            SaveStackInfo();
        }

        public new void Add()
        {
            base.Add();
            SaveStackInfo();
        }

        public new void Add_Ovf(bool unsigned)
        {
            base.Add_Ovf(unsigned);
            SaveStackInfo();
        }

        public new void Sub()
        {
            base.Sub();
            SaveStackInfo();
        }

        public new void Sub_Ovf(bool unsigned)
        {
            base.Sub_Ovf(unsigned);
            SaveStackInfo();
        }

        public new void Mul()
        {
            base.Mul();
            SaveStackInfo();
        }

        public new void Mul_Ovf(bool unsigned)
        {
            base.Mul_Ovf(unsigned);
            SaveStackInfo();
        }

        public new void Div(bool unsigned)
        {
            base.Div(unsigned);
            SaveStackInfo();
        }

        public new void Rem(bool unsigned)
        {
            base.Rem(unsigned);
            SaveStackInfo();
        }

        public new void Shl()
        {
            base.Shl();
            SaveStackInfo();
        }

        public new void Shr(bool unsigned)
        {
            base.Shr(unsigned);
            SaveStackInfo();
        }

        public new void Neg()
        {
            base.Neg();
            SaveStackInfo();
        }

        public new void Not()
        {
            base.Not();
            SaveStackInfo();
        }

        public new void Ldstr(string value)
        {
            base.Ldstr(value);
            SaveStackInfo();
        }

        public new void Conv<T>()
        {
            base.Conv<T>();
            SaveStackInfo();
        }

        public new void Conv_R_Un()
        {
            base.Conv_R_Un();
            SaveStackInfo();
        }

        public new void Conv_Ovf<T>(bool unsigned)
        {
            base.Conv_Ovf<T>(unsigned);
            SaveStackInfo();
        }

        public new void Call(MethodInfo method, Type constrained = null, bool tailcall = false, Type[] optionalParameterTypes = null, bool isVirtual = false)
        {
            base.Call(method, constrained, tailcall, optionalParameterTypes, isVirtual);
            SaveStackInfo();
        }

        public new void Call(ConstructorInfo constructor)
        {
            base.Call(constructor);
            SaveStackInfo();
        }

        public new void Callnonvirt(MethodInfo method, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            base.Callnonvirt(method, tailcall, optionalParameterTypes);
            SaveStackInfo();
        }

        public new void Calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            base.Calli(callingConvention, returnType, parameterTypes, tailcall, optionalParameterTypes);
            SaveStackInfo();
        }

        public new void Calli(CallingConvention callingConvention, Type returnType, Type[] parameterTypes)
        {
            base.Calli(callingConvention, returnType, parameterTypes);
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

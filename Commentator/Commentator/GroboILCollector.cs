using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using GrEmit;

namespace Commentator
{
    public class GroboILCollector : GroboIL
    {
        private static readonly FieldInfo StackFieldInfo = typeof(GroboIL).GetField("stack", BindingFlags.NonPublic | BindingFlags.Instance);

        private string stackInfoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";

        public GroboILCollector(MethodBuilder methodBuilder) : base(methodBuilder)
        {
        }

        public GroboILCollector(MethodBuilder methodBuilder, ISymbolDocumentWriter symbolWriter) : base(methodBuilder, symbolWriter)
        {
        }

        public GroboILCollector(DynamicMethod method, bool analyzeStack = true)
          : base(method, analyzeStack)
        {
        }

        public GroboILCollector(MethodBuilder method, bool analyzeStack = true)
            : base(method, analyzeStack)
        {
        }

        public GroboILCollector(ConstructorBuilder constructor, bool analyzeStack = true)
          : base(constructor, analyzeStack)
        {
        }

        public GroboILCollector(ConstructorBuilder constructor, ISymbolDocumentWriter symbolDocumentWriter) 
            : base(constructor, symbolDocumentWriter)
        {
        }


        public new void Stfld(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Stfld(field);
            SaveStackInfo(prevStackValues);
        }

        public new void Seal()
        {
            var prevStackValues = GetStackValues();
            base.Seal();
            SaveStackInfo(prevStackValues);
        }

        public new void Dispose()
        {
            var prevStackValues = GetStackValues();
            base.Dispose();
            SaveStackInfo(prevStackValues);
        }

        public new string GetILCode()
        {
            var prevStackValues = GetStackValues();
            var value = base.GetILCode();
            SaveStackInfo(prevStackValues);
            return value;
        }

        public new GroboIL.Local DeclareLocal(Type localType, bool pinned = false)
        {
            var prevStackValues = GetStackValues();
            var value = base.DeclareLocal(localType, pinned);
            SaveStackInfo(prevStackValues);
            return value;
        }

        public new GroboIL.Label DefineLabel(string name, bool appendUniquePrefix = true)
        {
            var prevStackValues = GetStackValues();
            var value = base.DefineLabel(name, appendUniquePrefix);
            SaveStackInfo(prevStackValues);
            return value;
        }

        public new void MarkLabel(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.MarkLabel(label);
            SaveStackInfo(prevStackValues);
        }

        public new void WriteLine(string str)
        {
            var prevStackValues = GetStackValues();
            base.WriteLine(str);
            SaveStackInfo(prevStackValues);
        }

        public new void WriteLine(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.WriteLine(local);
            SaveStackInfo(prevStackValues);
        }

        public new void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            var prevStackValues = GetStackValues();
            base.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
            SaveStackInfo(prevStackValues);
        }

        // public new static void MarkSequencePoint(ILGenerator il, ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)

        public new void BeginExceptionBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginExceptionBlock();
            SaveStackInfo(prevStackValues);
        }

        public new void BeginCatchBlock(Type exceptionType)
        {
            var prevStackValues = GetStackValues();
            base.BeginCatchBlock(exceptionType);
            SaveStackInfo(prevStackValues);
        }

        public new void BeginExceptFilterBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginExceptFilterBlock();
            SaveStackInfo(prevStackValues);
        }

        public new void BeginFaultBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginFaultBlock();
            SaveStackInfo(prevStackValues);
        }

        public new void BeginFinallyBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginFinallyBlock();
            SaveStackInfo(prevStackValues);
        }

        public new void EndExceptionBlock()
        {
            var prevStackValues = GetStackValues();
            base.EndExceptionBlock();
            SaveStackInfo(prevStackValues);
        }

        public new void Break()
        {
            var prevStackValues = GetStackValues();
            base.Break();
            SaveStackInfo(prevStackValues);
        }

        public new void Nop()
        {
            var prevStackValues = GetStackValues();
            base.Nop();
            SaveStackInfo(prevStackValues);
        }

        public new void Throw()
        {
            var prevStackValues = GetStackValues();
            base.Throw();
            SaveStackInfo(prevStackValues);
        }

        public new void Rethrow()
        {
            var prevStackValues = GetStackValues();
            base.Rethrow();
            SaveStackInfo(prevStackValues);
        }

        public new void Switch(params GroboIL.Label[] labels)
        {
            var prevStackValues = GetStackValues();
            base.Switch(labels);
            SaveStackInfo(prevStackValues);
        }

        public new void Ret()
        {
            var prevStackValues = GetStackValues();
            base.Ret();
            SaveStackInfo(prevStackValues);
        }

        public new void Leave(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Leave(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Jmp(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Jmp(method);
            SaveStackInfo(prevStackValues);
        }

        public new void Br(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Br(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Brfalse(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Brfalse(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Brtrue(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Brtrue(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Ble(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Ble(label, unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Bge(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Bge(label, unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Blt(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Blt(label, unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Bgt(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Bgt(label, unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Bne_Un(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Bne_Un(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Beq(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Beq(label);
            SaveStackInfo(prevStackValues);
        }

        public new void Ceq()
        {
            var prevStackValues = GetStackValues();
            base.Ceq();
            SaveStackInfo(prevStackValues);
        }

        public new void Cgt(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Cgt(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Clt(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Clt(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Pop()
        {
            var prevStackValues = GetStackValues();
            base.Pop();
            SaveStackInfo(prevStackValues);
        }

        public new void Dup()
        {
            var prevStackValues = GetStackValues();
            base.Dup();
            SaveStackInfo(prevStackValues);
        }

        public new void Ldloca(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Ldloca(local);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldloc(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Ldloc(local);
            SaveStackInfo(prevStackValues);
        }

        public new void Stloc(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Stloc(local);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldnull()
        {
            var prevStackValues = GetStackValues();
            base.Ldnull();
            SaveStackInfo(prevStackValues);
        }

        public new void Initobj(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Initobj(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Cpobj(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Cpobj(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldarg(int index)
        {
            var prevStackValues = GetStackValues();
            base.Ldarg(index);
            SaveStackInfo(prevStackValues);
        }

        public new void Starg(int index)
        {
            var prevStackValues = GetStackValues();
            base.Starg(index);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldarga(int index)
        {
            var prevStackValues = GetStackValues();
            base.Ldarga(index);
            SaveStackInfo(prevStackValues);
        }

        public new void Arglist()
        {
            var prevStackValues = GetStackValues();
            base.Arglist();
            SaveStackInfo(prevStackValues);
        }

        public new void Ldc_I4(int value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_I4(value);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldc_I8(long value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_I8(value);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldc_R4(float value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_R4(value);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldc_R8(double value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_R8(value);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldc_IntPtr(IntPtr value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_IntPtr(value);
            SaveStackInfo(prevStackValues);
        }

        public new void FreePinnedLocal(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.FreePinnedLocal(local);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldlen()
        {
            var prevStackValues = GetStackValues();
            base.Ldlen();
            SaveStackInfo(prevStackValues);
        }

        public new void Ldftn(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldftn(method);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldvirtftn(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldvirtftn(method);
            SaveStackInfo(prevStackValues);
        }

        public new void Stfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stfld(field, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldfld(field, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldflda(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Ldflda(field);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldelema(Type elementType, bool asReadonly = false)
        {
            var prevStackValues = GetStackValues();
            base.Ldelema(elementType, asReadonly);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldelem(Type elementType)
        {
            var prevStackValues = GetStackValues();
            base.Ldelem(elementType);
            SaveStackInfo(prevStackValues);
        }

        public new void Stelem(Type elementType)
        {
            var prevStackValues = GetStackValues();
            base.Stelem(elementType);
            SaveStackInfo(prevStackValues);
        }

        public new void Stind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stind(type, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldind(type, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Cpblk(bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Cpblk(isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Initblk(bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Initblk(isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldtoken(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldtoken(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(method);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldtoken(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(field);
            SaveStackInfo(prevStackValues);
        }

        public new void Castclass(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Castclass(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Isinst(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Isinst(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Unbox_Any(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Unbox_Any(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Box(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Box(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Stobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stobj(type, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Ldobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldobj(type, isVolatile, unaligned);
            SaveStackInfo(prevStackValues);
        }

        public new void Newobj(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Newobj(constructor);
            SaveStackInfo(prevStackValues);
        }

        public new void Newarr(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Newarr(type);
            SaveStackInfo(prevStackValues);
        }

        public new void Ckfinite()
        {
            var prevStackValues = GetStackValues();
            base.Ckfinite();
            SaveStackInfo(prevStackValues);
        }

        public new void And()
        {
            var prevStackValues = GetStackValues();
            base.And();
            SaveStackInfo(prevStackValues);
        }

        public new void Or()
        {
            var prevStackValues = GetStackValues();
            base.Or();
            SaveStackInfo(prevStackValues);
        }

        public new void Xor()
        {
            var prevStackValues = GetStackValues();
            base.Xor();
            SaveStackInfo(prevStackValues);
        }

        public new void Add()
        {
            var prevStackValues = GetStackValues();
            base.Add();
            SaveStackInfo(prevStackValues);
        }

        public new void Add_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Add_Ovf(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Sub()
        {
            var prevStackValues = GetStackValues();
            base.Sub();
            SaveStackInfo(prevStackValues);
        }

        public new void Sub_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Sub_Ovf(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Mul()
        {
            var prevStackValues = GetStackValues();
            base.Mul();
            SaveStackInfo(prevStackValues);
        }

        public new void Mul_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Mul_Ovf(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Div(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Div(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Rem(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Rem(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Shl()
        {
            var prevStackValues = GetStackValues();
            base.Shl();
            SaveStackInfo(prevStackValues);
        }

        public new void Shr(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Shr(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Neg()
        {
            var prevStackValues = GetStackValues();
            base.Neg();
            SaveStackInfo(prevStackValues);
        }

        public new void Not()
        {
            var prevStackValues = GetStackValues();
            base.Not();
            SaveStackInfo(prevStackValues);
        }

        public new void Ldstr(string value)
        {
            var prevStackValues = GetStackValues();
            base.Ldstr(value);
            SaveStackInfo(prevStackValues);
        }

        public new void Conv<T>()
        {
            var prevStackValues = GetStackValues();
            base.Conv<T>();
            SaveStackInfo(prevStackValues);
        }

        public new void Conv_R_Un()
        {
            var prevStackValues = GetStackValues();
            base.Conv_R_Un();
            SaveStackInfo(prevStackValues);
        }

        public new void Conv_Ovf<T>(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Conv_Ovf<T>(unsigned);
            SaveStackInfo(prevStackValues);
        }

        public new void Call(MethodInfo method, Type constrained = null, bool tailcall = false, Type[] optionalParameterTypes = null, bool isVirtual = false)
        {
            var prevStackValues = GetStackValues();
            base.Call(method, constrained, tailcall, optionalParameterTypes, isVirtual);
            SaveStackInfo(prevStackValues);
        }

        public new void Call(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Call(constructor);
            SaveStackInfo(prevStackValues);
        }

        public new void SafeCall(MethodInfo method, Type constrained = null, bool tailcall = false, Type[] optionalParameterTypes = null, bool isVirtual = false)
        {
            var prevStackValues = GetStackValues();
            base.Call(method, constrained, tailcall, optionalParameterTypes, isVirtual);
            SaveStackInfo(prevStackValues);
        }

        public new void SafeCall(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Call(constructor);
            SaveStackInfo(prevStackValues);
        }

        public new void Callnonvirt(MethodInfo method, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            var prevStackValues = GetStackValues();
            base.Callnonvirt(method, tailcall, optionalParameterTypes);
            SaveStackInfo(prevStackValues);
        }

        public new void Calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            var prevStackValues = GetStackValues();
            base.Calli(callingConvention, returnType, parameterTypes, tailcall, optionalParameterTypes);
            SaveStackInfo(prevStackValues);
        }

        public new void Calli(CallingConvention callingConvention, Type returnType, Type[] parameterTypes)
        {
            var prevStackValues = GetStackValues();
            base.Calli(callingConvention, returnType, parameterTypes);
            SaveStackInfo(prevStackValues);
        }

        private void PrintStackInfo()
        {
            var stackTrace = new StackTrace(true);
            Console.WriteLine(stackTrace);
            Console.WriteLine(stackTrace.GetFrame(2).GetFileName());
            Console.WriteLine(stackTrace.GetFrame(2).GetFileLineNumber());
            Console.WriteLine(StackFieldInfo?.GetValue(this));
        }

        private void SaveStackInfo(string prevStackValues)
        {
            var stackTrace = new StackTrace(true);
            var stackValues = GetStackValues();
            if (string.IsNullOrEmpty(stackValues) || stackValues.Length < 2) return;

            using (StreamWriter streamWriter = new StreamWriter(stackInfoFileName, true))
            {
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileName());
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetMethod().Name);
                streamWriter.WriteLine(stackTrace.GetFrame(2).GetFileLineNumber()-1);
                streamWriter.WriteLine(prevStackValues);
                streamWriter.WriteLine(stackValues);
            }
        }

        private string GetStackValues()
        {
            var stackInfo = StackFieldInfo?.GetValue(this);
            if (stackInfo == null)
                return "";
            return stackInfo.ToString();
        }
    }
}

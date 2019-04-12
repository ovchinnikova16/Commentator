using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using GrEmit;

namespace Commentator
{
    public class GroboILCollector : GroboIL
    {
        private static readonly FieldInfo StackFieldInfo = typeof(GroboIL).GetField("stack", BindingFlags.NonPublic | BindingFlags.Instance);

        private string stackInfoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";

        private bool isStatic = false;
        public GroboILCollector(MethodBuilder methodBuilder) : base(methodBuilder)
        {
            isStatic = methodBuilder.IsStatic;
        }

        public GroboILCollector(MethodBuilder methodBuilder, ISymbolDocumentWriter symbolWriter) : base(methodBuilder, symbolWriter)
        {
            isStatic = methodBuilder.IsStatic;
        }

        public GroboILCollector(DynamicMethod method, bool analyzeStack = true)
          : base(method, analyzeStack)
        {
            isStatic = method.IsStatic;
        }

        public GroboILCollector(MethodBuilder method, bool analyzeStack = true)
            : base(method, analyzeStack)
        {
            isStatic = method.IsStatic;
        }

        public GroboILCollector(ConstructorBuilder constructor, bool analyzeStack = true)
          : base(constructor, analyzeStack)
        {
            isStatic = constructor.IsStatic;
        }

        public GroboILCollector(ConstructorBuilder constructor, ISymbolDocumentWriter symbolDocumentWriter) 
            : base(constructor, symbolDocumentWriter)
        {
            isStatic = constructor.IsStatic;
        }

        public new void Stfld(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Stfld(field);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Seal()
        {
            var prevStackValues = GetStackValues();
            base.Seal();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Dispose()
        {
            var prevStackValues = GetStackValues();
            base.Dispose();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new string GetILCode()
        {
            var prevStackValues = GetStackValues();
            var value = base.GetILCode();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
            return value;
        }

        public new GroboIL.Local DeclareLocal(Type localType, bool pinned = false)
        {
            var prevStackValues = GetStackValues();
            var value = base.DeclareLocal(localType, pinned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
            return value;
        }

        public new GroboIL.Label DefineLabel(string name, bool appendUniquePrefix = true)
        {
            var prevStackValues = GetStackValues();
            var value = base.DefineLabel(name, appendUniquePrefix);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
            return value;
        }

        public new void MarkLabel(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.MarkLabel(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void WriteLine(string str)
        {
            var prevStackValues = GetStackValues();
            base.WriteLine(str);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void WriteLine(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.WriteLine(local);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            var prevStackValues = GetStackValues();
            base.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        // public new static void MarkSequencePoint(ILGenerator il, ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)

        public new void BeginExceptionBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginExceptionBlock();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void BeginCatchBlock(Type exceptionType)
        {
            var prevStackValues = GetStackValues();
            base.BeginCatchBlock(exceptionType);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void BeginExceptFilterBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginExceptFilterBlock();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void BeginFaultBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginFaultBlock();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void BeginFinallyBlock()
        {
            var prevStackValues = GetStackValues();
            base.BeginFinallyBlock();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void EndExceptionBlock()
        {
            var prevStackValues = GetStackValues();
            base.EndExceptionBlock();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Break()
        {
            var prevStackValues = GetStackValues();
            base.Break();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Nop()
        {
            var prevStackValues = GetStackValues();
            base.Nop();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Throw()
        {
            var prevStackValues = GetStackValues();
            base.Throw();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Rethrow()
        {
            var prevStackValues = GetStackValues();
            base.Rethrow();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Switch(params GroboIL.Label[] labels)
        {
            var prevStackValues = GetStackValues();
            base.Switch(labels);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ret()
        {
            var prevStackValues = GetStackValues();
            base.Ret();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Leave(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Leave(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Jmp(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Jmp(method);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Br(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Br(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Brfalse(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Brfalse(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Brtrue(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Brtrue(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ble(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Ble(label, unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Bge(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Bge(label, unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Blt(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Blt(label, unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Bgt(GroboIL.Label label, bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Bgt(label, unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Bne_Un(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Bne_Un(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Beq(GroboIL.Label label)
        {
            var prevStackValues = GetStackValues();
            base.Beq(label);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ceq()
        {
            var prevStackValues = GetStackValues();
            base.Ceq();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Cgt(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Cgt(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Clt(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Clt(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Pop()
        {
            var prevStackValues = GetStackValues();
            base.Pop();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Dup()
        {
            var prevStackValues = GetStackValues();
            base.Dup();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldloca(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Ldloca(local);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldloc(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Ldloc(local);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Stloc(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.Stloc(local);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldnull()
        {
            var prevStackValues = GetStackValues();
            base.Ldnull();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Initobj(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Initobj(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Cpobj(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Cpobj(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldarg(int index)
        {
            var prevStackValues = GetStackValues();
            base.Ldarg(index);
            var newStackValues = GetStackValues();
            if (index == 0 && !isStatic)
            {
                var values = newStackValues.Split(' ');
                values[values.Length - 1] = "this";
                newStackValues = string.Join(" ", values);
            }

            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Starg(int index)
        {
            var prevStackValues = GetStackValues();
            base.Starg(index);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldarga(int index)
        {
            var prevStackValues = GetStackValues();
            base.Ldarga(index);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Arglist()
        {
            var prevStackValues = GetStackValues();
            base.Arglist();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldc_I4(int value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_I4(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldc_I8(long value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_I8(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldc_R4(float value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_R4(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldc_R8(double value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_R8(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldc_IntPtr(IntPtr value)
        {
            var prevStackValues = GetStackValues();
            base.Ldc_IntPtr(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void FreePinnedLocal(GroboIL.Local local)
        {
            var prevStackValues = GetStackValues();
            base.FreePinnedLocal(local);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldlen()
        {
            var prevStackValues = GetStackValues();
            base.Ldlen();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldftn(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldftn(method);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldvirtftn(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldvirtftn(method);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Stfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stfld(field, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldfld(FieldInfo field, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldfld(field, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldflda(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Ldflda(field);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldelema(Type elementType, bool asReadonly = false)
        {
            var prevStackValues = GetStackValues();
            base.Ldelema(elementType, asReadonly);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldelem(Type elementType)
        {
            var prevStackValues = GetStackValues();
            base.Ldelem(elementType);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Stelem(Type elementType)
        {
            var prevStackValues = GetStackValues();
            base.Stelem(elementType);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Stind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stind(type, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldind(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldind(type, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Cpblk(bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Cpblk(isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Initblk(bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Initblk(isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldtoken(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldtoken(MethodInfo method)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(method);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldtoken(FieldInfo field)
        {
            var prevStackValues = GetStackValues();
            base.Ldtoken(field);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Castclass(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Castclass(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Isinst(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Isinst(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Unbox_Any(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Unbox_Any(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Box(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Box(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Stobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Stobj(type, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldobj(Type type, bool isVolatile = false, int? unaligned = null)
        {
            var prevStackValues = GetStackValues();
            base.Ldobj(type, isVolatile, unaligned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Newobj(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Newobj(constructor);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Newarr(Type type)
        {
            var prevStackValues = GetStackValues();
            base.Newarr(type);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ckfinite()
        {
            var prevStackValues = GetStackValues();
            base.Ckfinite();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void And()
        {
            var prevStackValues = GetStackValues();
            base.And();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Or()
        {
            var prevStackValues = GetStackValues();
            base.Or();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Xor()
        {
            var prevStackValues = GetStackValues();
            base.Xor();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Add()
        {
            var prevStackValues = GetStackValues();
            base.Add();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Add_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Add_Ovf(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Sub()
        {
            var prevStackValues = GetStackValues();
            base.Sub();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Sub_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Sub_Ovf(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Mul()
        {
            var prevStackValues = GetStackValues();
            base.Mul();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Mul_Ovf(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Mul_Ovf(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Div(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Div(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Rem(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Rem(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Shl()
        {
            var prevStackValues = GetStackValues();
            base.Shl();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Shr(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Shr(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Neg()
        {
            var prevStackValues = GetStackValues();
            base.Neg();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Not()
        {
            var prevStackValues = GetStackValues();
            base.Not();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Ldstr(string value)
        {
            var prevStackValues = GetStackValues();
            base.Ldstr(value);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Conv<T>()
        {
            var prevStackValues = GetStackValues();
            base.Conv<T>();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Conv_R_Un()
        {
            var prevStackValues = GetStackValues();
            base.Conv_R_Un();
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Conv_Ovf<T>(bool unsigned)
        {
            var prevStackValues = GetStackValues();
            base.Conv_Ovf<T>(unsigned);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Call(MethodInfo method, Type constrained = null, bool tailcall = false, Type[] optionalParameterTypes = null, bool isVirtual = false)
        {
            var prevStackValues = GetStackValues();
            base.Call(method, constrained, tailcall, optionalParameterTypes, isVirtual);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Call(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Call(constructor);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void SafeCall(MethodInfo method, Type constrained = null, bool tailcall = false, Type[] optionalParameterTypes = null, bool isVirtual = false)
        {
            var prevStackValues = GetStackValues();
            base.Call(method, constrained, tailcall, optionalParameterTypes, isVirtual);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void SafeCall(ConstructorInfo constructor)
        {
            var prevStackValues = GetStackValues();
            base.Call(constructor);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Callnonvirt(MethodInfo method, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            var prevStackValues = GetStackValues();
            base.Callnonvirt(method, tailcall, optionalParameterTypes);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes, bool tailcall = false, Type[] optionalParameterTypes = null)
        {
            var prevStackValues = GetStackValues();
            base.Calli(callingConvention, returnType, parameterTypes, tailcall, optionalParameterTypes);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        public new void Calli(CallingConvention callingConvention, Type returnType, Type[] parameterTypes)
        {
            var prevStackValues = GetStackValues();
            base.Calli(callingConvention, returnType, parameterTypes);
            var newStackValues = GetStackValues();
            SaveStackInfo(prevStackValues, newStackValues);
        }

        private void SaveStackInfo(string prevStackValues, string newStackValues)
        {
            var stackTrace = new StackTrace(true);

            var sB = new StringBuilder();
            sB.AppendLine(stackTrace.GetFrame(2).GetFileName());
            sB.AppendLine(stackTrace.GetFrame(2).GetMethod().Name);
            sB.AppendLine((stackTrace.GetFrame(2).GetFileLineNumber()).ToString());
            sB.AppendLine(prevStackValues);
            sB.AppendLine(newStackValues);
            File.AppendAllText(stackInfoFileName, sB.ToString());
        }

        private string GetStackValues()
        {
            var stackInfo = StackFieldInfo?.GetValue(this);
            if (stackInfo == null)
                return "";
            var stackStr = stackInfo.ToString();
            return stackStr.Substring(1, stackStr.Length-2);
        }
    }
}

using System;
using System.Reflection;
using System.Reflection.Emit;
using Commentator;
using GrEmit;

namespace AdditionExample
{
    class CodeGenerator
    {
        private readonly string assemblyName;
        private readonly string methodName;
        private readonly string fileName;

        public CodeGenerator(string assemblyName, string methodName, string fileName)
        {
            this.assemblyName = assemblyName;
            this.methodName = methodName;
            this.fileName = fileName;
        }

        public AssemblyBuilder Generate()
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName { Name = assemblyName },
                AssemblyBuilderAccess.RunAndSave);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, fileName);

            var typeBuilder = moduleBuilder.DefineType(assemblyName+"Helper",
                TypeAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod(
                methodName,
                MethodAttributes.Public |
                MethodAttributes.Static,
                typeof(int),
                new[] { typeof(int), typeof(int), typeof(double), typeof(double) });

            var helper = new GroboIL(methodBuilder);

            using (var il = new GroboILCollector(methodBuilder))
            {
                GenerateAdditionWithMethodILCode(il);
            }

            typeBuilder.CreateType();
            return assemblyBuilder;
        }

        private static void GenerateSimpleAdditionILCode(GroboILCollector il)
        {
            il.Ldarg(0);
            il.Ldarg(1);
            il.Ldarg(2);
            il.Ldarg(3);
            il.Pop();
            il.Pop();
            il.Add();
            il.Ret();
        }

        private static void GenerateAdditionWithMethodILCode(GroboILCollector il)
        {
            CreateTwoArguments(il, 0, 1);
            CreateTwoArguments(il, 2, 3);
            AddNumbers(il);
            il.Pop();
            AddNumbers(il);
            il.Ret();
        }

        private static void CreateTwoArguments(GroboILCollector il, int firstNumber, int secondNumber)
        {
            il.Ldarg(firstNumber);
            il.Ldarg(secondNumber);
        }

        private static void AddNumbers(GroboILCollector il)
        {
            il.Add();
        }
    }
}
//функция, которая вызывается много раз
//если два типа, то object
//обрубать стек
//LdFld


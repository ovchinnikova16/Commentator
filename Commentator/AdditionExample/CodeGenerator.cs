using System;
using System.Reflection;
using System.Reflection.Emit;
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
                new[] { typeof(int), typeof(int) });

            using (var il = new GroboIL(methodBuilder))
            {
                GenerateILCode(il);
            }

            typeBuilder.CreateType();
            return assemblyBuilder;
        }

        private static void GenerateILCode(GroboIL il)
        {
            il.Ldarg(0);
            il.Ldarg(1);
            il.Add();
            il.Ret();
        }
    }
}

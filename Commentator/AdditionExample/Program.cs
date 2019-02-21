namespace AdditionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = args[0];
            string fileName = @"addition.dll";

            var codeGenerator = new CodeGenerator("Addition", "Add", fileName);
            var assemblyBuilder = codeGenerator.Generate();

            assemblyBuilder.Save(fileName);
        }
    }
}
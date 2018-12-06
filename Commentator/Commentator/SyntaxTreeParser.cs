using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Commentator
{
    class SyntaxTreeParser
    {
        private string fileName;

        public SyntaxTreeParser(string fileName)
        {
            this.fileName = fileName;
        }

        public void ParseFile()
        { 
            string text = File.ReadAllText(fileName);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var statementsCount = GetMethodDeclaration(root).Body.Statements.Count;
                for (int i = 0; i<statementsCount; i++)
            {
                var statement = GetMethodDeclaration(root).Body.Statements[i];
                var comment = SyntaxFactory.Comment($" // {statement}");
                var triviaList = statement.GetTrailingTrivia().Insert(0, comment);
                root = root.ReplaceNode(statement, statement.WithTrailingTrivia(triviaList));
            }

            Console.WriteLine(root);
        }

        private static MethodDeclarationSyntax GetMethodDeclaration(CompilationUnitSyntax root)
        {
        var namespaceDeclaration = (NamespaceDeclarationSyntax)root.Members[0];
        var classDeclaration = (ClassDeclarationSyntax)namespaceDeclaration.Members[0];
        var methodDeclaration = (MethodDeclarationSyntax)classDeclaration.Members[5];

            return methodDeclaration;
        }
    }
}

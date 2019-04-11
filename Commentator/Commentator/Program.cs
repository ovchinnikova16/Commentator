using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Engine;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            //string command = args[0];
            //string targetAssemblyPath = args[1];

            var infoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";

            var targetAssemblyPath = @"C:\Users\e.ovc\Commentator\project1\RequisitesReader";
            //var targetProjectPath = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector";
            //var targetAssemblyPath = @"C:\Users\e.ovc\Commentator\project1\flash.props";

            //var rewrite = new Rewriter(targetAssemblyPath);
            //rewrite.RewriteToShellName();
            //BuildTargetAssembly(targetAssemblyPath);

            //RunAllTests(targetAssemblyPath, infoFileName);

            //rewrite.RewriteFromShellName();

            AddCommentsToProject(infoFileName, targetAssemblyPath);
            //BuildTargetAssembly(targetAssemblyPath);
        }


        private static void BuildTargetAssembly(string targetAssemblyPath)
        {
            Process process = new Process();
	        //review: можно сеттить филды при создании
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c cd "
                                  + targetAssemblyPath
                                  + " && cm build";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private static void RunAllTests(string targetProjectPath, string infoFileName)
        {
            File.WriteAllText(infoFileName, string.Empty);
            ITestEngine engine = TestEngineActivator.CreateInstance();
            var allFiles = Directory
                .GetFiles(targetProjectPath, "*Test*.dll", SearchOption.AllDirectories)
                .Where(x => x.Contains("Release"))
                .ToArray();
            foreach (var candidate in allFiles)
            {
                Console.WriteLine("RUN_TESTS: "+Path.GetFileName(candidate));

                Directory.SetCurrentDirectory(Path.GetDirectoryName(candidate));
                TestPackage package = new TestPackage(candidate);
                ITestRunner runner = engine.GetRunner(package);
                XmlNode testResult = runner.Run(new NullListener(), TestFilter.Empty);
            }
        }


        public static string XmlNodeToString(XmlNode node, int indentation)
        {
            using (var sw = new System.IO.StringWriter())
            {
                using (var xw = new System.Xml.XmlTextWriter(sw))
                {
                    xw.Formatting = System.Xml.Formatting.Indented;
                    xw.Indentation = indentation;
                    node.WriteContentTo(xw);
                }
                return sw.ToString();
            }
        }

        private static void AddCommentsToProject(string infoFileName, string projectName)
        {
            var commentator = new Commentator(infoFileName, projectName);
            commentator.AddComments();
        }

        private class NullListener : ITestEventListener
        {
            public void OnTestEvent(string report)
            {
            }
        }
    }

}

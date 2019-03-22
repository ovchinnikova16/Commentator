using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using NUnit.Engine;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            //string targetFileName = args[0];
            //string infoFileName = args[1];

            var infoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";
            var targetAssemblyPath = @"C:\Users\e.ovc\Commentator\project1\flash.props";
            var helperPath = @"C:\Users\e.ovc\Commentator\project1\RequisitesReader";
            var targetProjectPath1 = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector.Benchmarks";
            var targetProjectPath2 = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector.UnitTests";
            var targetProjectPath3 = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector";

            // зависимости
            //BuildTargetAssembly(helperPath);
            //BuildTargetAssembly(targetAssemblyPath);
            //Thread.Sleep(30000);

            //var helperRewrite = new Rewriter(helperPath);
            //helperRewrite.RewriteToShellName();

            //var rewrite1 = new Rewriter(targetProjectPath1);
            //rewrite1.RewriteToShellName();
            //var rewrite2 = new Rewriter(targetProjectPath2);
            //rewrite2.RewriteToShellName();
            //var rewrite3 = new Rewriter(targetProjectPath3);
            //rewrite3.RewriteToShellName();

            ////BuildTargetAssembly(helperPath);
            //BuildTargetAssembly(targetAssemblyPath);
            //Thread.Sleep(30000);

            //RunAllTests(targetAssemblyPath, infoFileName);

            ////helperRewrite.RewriteFromShellName();

            //rewrite1.RewriteFromShellName();
            //rewrite2.RewriteFromShellName();
            //rewrite3.RewriteFromShellName();

            AddCommentsToProject(infoFileName, targetAssemblyPath);

            ////BuildTargetAssembly(helperPath);
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
        }

        private static void RunAllTests(string targetProjectPath, string infoFileName)
        {
            File.WriteAllText(infoFileName, string.Empty);
            ITestEngine engine = TestEngineActivator.CreateInstance();
            var allfiles = Directory.GetFiles(targetProjectPath, "*UnitTest*.dll", SearchOption.AllDirectories).Where(x => x.Contains("Release")).ToArray();
            foreach (var candidate in allfiles)
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

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
            var msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin";
            var targetAssembly = @"C:\Users\e.ovc\Commentator\project1\RequisitesReader\RequisitesReader.sln";
            //var targetProjectPath = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector";
            //var targetAssemblyPath = @"C:\Users\e.ovc\Commentator\project1\flash.props";

            var targetAssemblyPath = Path.GetDirectoryName(targetAssembly);

            var rewrite = new Rewriter(targetAssemblyPath);
            rewrite.RewriteToShellName();
            BuildTargetAssembly(targetAssembly, msbuildPath);

            RunAllTests(targetAssemblyPath, infoFileName);

            rewrite.RewriteFromShellName();

            AddCommentsToProject(infoFileName, targetAssemblyPath);
            BuildTargetAssembly(targetAssembly, msbuildPath);
        }


        private static void BuildTargetAssembly(string targetAssembly, string msbuildPath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c cd "
                                  + msbuildPath
                                  + @" && MSBuild.exe " 
                                  + targetAssembly 
                                  + " -property:Configuration=Debug";
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
                .Where(x => x.Contains("Debug"))
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

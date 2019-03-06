using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Engine;
using NUnit.Framework.Constraints;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            //string targetFileName = args[0];
            //string infoFileName = args[1];

            string infoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";
            var targetAssemblyPath = @"C:\Users\e.ovc\Commentator\project1\flash.props";
            var targetProjectPath =
                @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector\Compile\Emitters";


            var rewriter = new Rewriter(targetProjectPath);

            rewriter.RewriteToShellName();

            BuildTargetAssembly(targetAssemblyPath);

            RunAllTests(targetAssemblyPath, infoFileName);

            rewriter.RewriteFromShellName();

            AddCommentsToProject(infoFileName);
        }

        private static void BuildTargetAssembly(string targetAssemblyPath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c cd "
                                  + targetAssemblyPath
                                  + " && cm build-deps && cm build";
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void RunAllTests(string targetProjectPath, string infoFileName)
        {
            File.WriteAllText(infoFileName, string.Empty);
            ITestEngine engine = TestEngineActivator.CreateInstance();
            var allfiles = Directory.GetFiles(targetProjectPath, "*Test*.dll", SearchOption.AllDirectories).Where(x => x.Contains("Release")).ToArray();
            foreach (var candidate in allfiles)
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(candidate));
                TestPackage package = new TestPackage(candidate);
                ITestRunner runner = engine.GetRunner(package);
                XmlNode testResult = runner.Run(new NullListener(), TestFilter.Empty);

                Console.WriteLine(Path.GetFileName(candidate));
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

        private static void AddCommentsToProject(string infoFileName)
        {
            var commentator = new Commentator(infoFileName);
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

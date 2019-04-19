﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NUnit.Engine;
using Process = System.Diagnostics.Process;

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
            //var targetAssembly = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector.sln";

            var targetAssemblyPath = Path.GetDirectoryName(targetAssembly);

            AddReferences(targetAssemblyPath);

            var replacerTo = new Replacer("GroboIL", "Commentator.GroboILCollector");
            replacerTo.Replace(targetAssemblyPath);


            BuildTargetAssembly(targetAssembly, msbuildPath);

            RunAllTests(targetAssemblyPath, infoFileName);

            var replacerFrom = new Replacer("Commentator.GroboILCollector", "GroboIL");
            replacerFrom.Replace(targetAssemblyPath);

            var commentator = new Commentator(infoFileName, targetAssemblyPath);
            commentator.AddComments();

            BuildTargetAssembly(targetAssembly, msbuildPath);

        }

        private static void AddReferences(string targetAssemblyPath)
        {
            var allProjects = Directory
                .GetFiles(targetAssemblyPath, "*.csproj", SearchOption.AllDirectories);

            foreach (var project in allProjects)
            {
                var currentFile = Process.GetCurrentProcess().MainModule.FileName;
                var xml = new XmlDocument();
                xml.Load(project);

                var newNode = xml.CreateNode(XmlNodeType.Element, "Reference", null);
                newNode.Attributes.Append(xml.CreateAttribute("Include"));
                newNode.Attributes.Item(0).Value = Assembly.GetCallingAssembly().FullName;
                var firstChild = xml.CreateNode(XmlNodeType.Element, "HintPath", null);
                firstChild.InnerText = currentFile;
                newNode.AppendChild(firstChild);

                xml.GetElementsByTagName("Reference")
                    .Item(0)
                    .ParentNode
                    .AppendChild(newNode);
                xml.Save(project);
            }
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
                .Where(x => x.Contains("Debug"));
            foreach (var candidate in allFiles)
            {
                Console.WriteLine("RUN_TESTS: "+Path.GetFileName(candidate));

                Directory.SetCurrentDirectory(Path.GetDirectoryName(candidate));
                TestPackage package = new TestPackage(candidate);
                ITestRunner runner = engine.GetRunner(package);
                runner.Run(new NullListener(), TestFilter.Empty);
            }
        }

        private class NullListener : ITestEventListener
        {
            public void OnTestEvent(string report)
            {
            }
        }
    }

}

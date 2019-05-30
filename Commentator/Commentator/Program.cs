using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using CommandLine;
using NUnit.Engine;
using log4net.Config;
using Process = System.Diagnostics.Process;
using log4net;
using System.Collections.Generic;
using System;

namespace Commentator
{
    class Program
    {
        private static ILog logger = LogManager.GetLogger(typeof(Program));

        public class Options
        {
            [Option("gen-projects", HelpText = "Target projects that generate IL code.", Required = true)]
            public IEnumerable<string> GeneratorProjects { get; set; }

            [Option("test-projects", HelpText = "Projects with tests that run generators from target projects.", Required = true)]
            public IEnumerable<string> GeneratorTestsProjects { get; set; }

            [Option("msbuild-path", HelpText = "Path to MSBuild binary.", Required = true)]
            public string MsBuildPath { get; set; }
        }

        static void Main(string[] args)
        {
            var stackInfoFileName = Path.GetTempFileName();
            Environment.SetEnvironmentVariable(Constants.StackInfoFileNameEnvVariable,
                stackInfoFileName);
            BasicConfigurator.Configure();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Run(options, stackInfoFileName));
        }

        static void Run(Options options, string stackInfoFileName)
        {
            var generatorProjects = options.GeneratorProjects.Select(p => Path.GetFullPath(p)).ToList();
            var generatorTestsProjects = options.GeneratorTestsProjects.Select(p => Path.GetFullPath(p)).ToList();

            var forwardReplacer = new Replacer("GroboIL", "Commentator.GroboILCollector");
            foreach (var projectPath in generatorProjects)
            {
                forwardReplacer.Replace(projectPath);
                AddReference(projectPath);
            }

            foreach (var projectPath in generatorProjects.Concat(generatorTestsProjects))
                BuildProject(projectPath, options.MsBuildPath);

            foreach (var projectPath in generatorTestsProjects)
                RunAllTests(projectPath);

            var backwardReplacer = new Replacer("Commentator.GroboILCollector", "GroboIL");
            foreach (var projectPath in generatorProjects)
            {
                RemoveReference(projectPath);
                backwardReplacer.Replace(projectPath);
            }

            var commentWriter = CommentWriter.Load(stackInfoFileName);
            foreach (var projectPath in generatorProjects)
                commentWriter.AddComments(projectPath);
        }

        private static IEnumerable<string> FindProjectFiles(string projectPath)
        {
            var projectFiles = Directory.GetFiles(projectPath, "*.csproj");
            if (projectFiles.Length == 0)
            {
                logger.Warn($"Failed to find project file at {projectPath}");
            }
            return projectFiles;
        }

        private static void AddReference(string projectPath)
        {
            try
            {
                foreach (var projectFile in FindProjectFiles(projectPath))
                {
                    var xml = new XmlDocument();
                    xml.Load(projectFile);

                    var namespaceURI = xml.GetElementsByTagName("Project")
                        .Cast<XmlNode>()
                        .FirstOrDefault()
                        ?.NamespaceURI;

                    var newNode = xml.CreateElement("Reference", namespaceURI);
                    newNode.Attributes.Append(xml.CreateAttribute("Include"));
                    newNode.Attributes.Item(0).Value = Assembly.GetCallingAssembly().FullName;
                    var firstChild = xml.CreateElement("HintPath", namespaceURI);
                    firstChild.InnerText = Process.GetCurrentProcess().MainModule.FileName;
                    newNode.AppendChild(firstChild);

                    xml.GetElementsByTagName("Reference")
                        .Item(0)
                        .ParentNode
                        .AppendChild(newNode);
                    xml.Save(projectFile);
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to add reference to project {projectPath}", ex);
            }
        }

        private static void RemoveReference(string projectPath)
        {
            try
            {
                var processFileName = Process.GetCurrentProcess().MainModule.FileName;
                foreach (var projectFile in FindProjectFiles(projectPath))
                {
                    var xml = new XmlDocument();
                    xml.Load(projectFile);

                    var addedNode = xml.GetElementsByTagName("Reference")
                        .Cast<XmlNode>()
                        .FirstOrDefault(node =>
                        {
                            var hintPathNode = node.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.Name == "HintPath");
                            return hintPathNode?.InnerText == processFileName;
                        });
                    addedNode.ParentNode.RemoveChild(addedNode);
                    xml.Save(projectFile);
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to remove reference from project {projectPath}", ex);
            }
        }

        private static void BuildProject(string projectPath, string msBuildPath)
        {
            try
            {
                foreach (var projectFile in FindProjectFiles(projectPath))
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = @"/c cd "
                                          + Path.GetDirectoryName(msBuildPath)
                                          + @" && "
                                          + $"{Path.GetFileName(msBuildPath)} "
                                          + Path.GetFullPath(projectFile)
                                          + " -property:Configuration=Debug",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                    };
                    process.StartInfo = startInfo;
                    process.Start();

                    while (!process.StandardOutput.EndOfStream)
                        logger.Debug($"Build output: {process.StandardOutput.ReadLine()}");

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to build {projectPath}", ex);
            }
        }

        private static void RunAllTests(string targetProjectPath)
        {
            try
            {
                foreach (var testAssemblyPath in GetAssemblyPaths(targetProjectPath))
                {
                    ITestEngine engine = TestEngineActivator.CreateInstance();
                    logger.Debug($"Running tests from {testAssemblyPath} from project {targetProjectPath}");

                    Directory.SetCurrentDirectory(Path.GetDirectoryName(testAssemblyPath));
                    TestPackage package = new TestPackage(testAssemblyPath);
                    ITestRunner runner = engine.GetRunner(package);
                    runner.Run(new LoggingListener(testAssemblyPath), TestFilter.Empty);
                }
            }
            catch (Exception ex)
            {
                logger.Warn($"Failed to run tests for {targetProjectPath}", ex);
            }
        }

        private static IEnumerable<string> GetAssemblyPaths(string projectPath)
        {
            return FindProjectFiles(projectPath).Select(projectFile =>
            {
                var xml = new XmlDocument();
                xml.Load(projectFile);

                var assemblyName = xml.GetElementsByTagName("AssemblyName").Cast<XmlNode>().FirstOrDefault()?.InnerText;
                if (assemblyName == null)
                {
                    return null;
                }
                assemblyName = $"{assemblyName}.dll";

                return Directory.EnumerateFiles(projectPath, assemblyName, SearchOption.AllDirectories).FirstOrDefault();
            }).Where(o => o != null);
        }

        private class LoggingListener : ITestEventListener
        {
            private readonly string assemblyPath;

            public LoggingListener(string assemblyPath)
            {
                this.assemblyPath = assemblyPath;
            }
            
            public void OnTestEvent(string report)
            {
                logger.Debug($"Event from test runner for {assemblyPath}:\n{report}");
            }
        }
    }

}

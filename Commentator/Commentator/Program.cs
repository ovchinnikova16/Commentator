using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            //string targetFileName = args[0];
            //string infoFileName = args[1];

            string infoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";
            var targetProjectPath = @"C:\Users\e.ovc\Commentator\project1\flash.props\PropertiesCollector";

            BuildTargetProject(targetProjectPath);

            var rewriter = new Rewriter(targetProjectPath);
            rewriter.RewriteToShellName();

            RunAllTests(targetProjectPath);

            rewriter.RewriteFromShellName();

            //var commentator = new Commentator(infoFileName);
            //commentator.AddComments();



        }

        private static void BuildTargetProject(string targetProjectPath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c cd "
                                  + targetProjectPath
                                  + " && cm update && cm update-deps && cm build-deps && cm build";
            process.StartInfo = startInfo;
            process.Start();
        }

        private static void RunAllTests(string targetProjectPath)
        {
            var allfiles = Directory.GetFiles(targetProjectPath, "*Test*.dll", SearchOption.AllDirectories).Where(x => x.Contains("Release")).ToArray();
            foreach (var candidate in allfiles)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = @"/c cd C:\Users\e.ovc\Downloads\NUnit.Console-3.9.0 && NUGET3-CONSOLE " + candidate;
                process.StartInfo = startInfo;
                process.Start();
                Console.WriteLine("DONE " + candidate);
            }
        }
    }
}

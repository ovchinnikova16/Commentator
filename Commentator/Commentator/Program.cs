using System;

namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            //string targetFileName = args[0];
            //string infoFileName = args[1];

            //string targetFileName = @"C:\Users\e.ovc\Commentator\work\flash.props\PropertiesCollector\Compile\Emitters\ContextTypeEmitter.cs";
            //string infoFileName = @"C:\Users\e.ovc\Commentator\work\stackInfo.txt";

            //var commentator = new Commentator(targetFileName, infoFileName);
            //commentator.AddComments();
            var rewriter = new Rewriter(@"C:\Users\e.ovc\Commentator\example\Emitters");
            rewriter.RewriteFromShellName();
        }
    }
}

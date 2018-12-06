using System.Collections.Generic;
using System.IO;

namespace Commentator
{
    class Commentator
    {
        private readonly string targetFileName;
        private readonly string infoFileName;
        private readonly Dictionary<int, string> comments = new Dictionary<int, string>();

        private readonly string resultFileName = "result.cs";

        public Commentator(string targetFileName, string infoFileName)
        {
            this.targetFileName = targetFileName;
            this.infoFileName = infoFileName;
        }

        public void AddComments()
        {
            GetCommentsInfo();

            var strNumber = 1;

            using (StreamReader streamReader = new StreamReader(targetFileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();

                    using (StreamWriter streamWriter = new StreamWriter(resultFileName, true))
                    {
                        if (comments.ContainsKey(strNumber))
                            streamWriter.WriteLine("{0} // {1}", line, comments[strNumber]);
                        else
                            streamWriter.WriteLine(line);
                    }

                    strNumber++;
                }
            }
        }

        private void GetCommentsInfo()
        {
            using (StreamReader streamReader = new StreamReader(infoFileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var file = streamReader.ReadLine();
                    var stringNumber = streamReader.ReadLine();
                    var stackInfo = streamReader.ReadLine();
                    int number;
                    if (file == targetFileName && int.TryParse(stringNumber, out number))
                    {
                        comments.Add(number, stackInfo);
                    }
                }
            }
        }
    }
}

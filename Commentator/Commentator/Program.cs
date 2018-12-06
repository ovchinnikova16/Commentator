namespace Commentator
{
    class Program
    {
        static void Main(string[] args)
        {
            string targetFileName = args[0];
            string infoFileName = args[1];

            var commentator = new Commentator(targetFileName, infoFileName);
            commentator.AddComments();

        }
    }
}

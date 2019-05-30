namespace Commentator
{
    class CommentInfo
    {
        public string FileName { get; set; }

        public string MethodName { get; set; }

        public int LineNumber { get; set; }

        public string PreviousStackValues { get; set; }

        public string NewStackValues { get; set; }
    }
}

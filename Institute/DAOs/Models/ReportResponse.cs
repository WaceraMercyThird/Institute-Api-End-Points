namespace Institute.Actor
{
    internal class ReportResponse
    {
        public byte[] PdfBytes { get; internal set; }
        public string RecipientEmail { get; internal set; }
        public string To { get; internal set; }
        public string Subject { get; internal set; }
        public string Body { get; internal set; }
        public byte[] Attachment { get; internal set; }
    }
}
namespace AuthorizeTransaction.Models
{
    public struct Output
    {
        public Account Account { get; set; }
        public string[] Violations { get; set; }
    }
}

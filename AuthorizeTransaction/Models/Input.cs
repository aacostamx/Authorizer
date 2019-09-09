namespace AuthorizeTransaction.Models
{
    public struct Input
    {
        public Account Account { get; set; }
        public Transaction Transaction { get; set; }
    }
}

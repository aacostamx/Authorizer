namespace AuthorizeTransaction.Models
{
    public struct Account
    {
        public bool ActiveCard { get; set; }
        public int AvailableLimit { get; set; }
    }
}

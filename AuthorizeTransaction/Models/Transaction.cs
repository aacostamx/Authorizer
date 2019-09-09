using System;

namespace AuthorizeTransaction.Models
{
    public struct Transaction
    {
        public string Merchant { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }
    }
}

namespace GestionComercial.Domain.DTOs.Banks
{
    public class BankAndBoxViewModel
    {
        public int Id { get; set; }

        public string BankName { get; set; }

        public decimal FromDebit { get; set; }

        public decimal FromCredit { get; set; }

        public decimal Sold { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsBank { get; set; }
        public int SaleConditionId { get; set; }
        public int AccountId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int StateId { get; set; }
        public string City { get; set; }
        public string? Phone { get; set; }
        public string? Phone1 { get; set; }
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public string AccountNumber { get; set; }
        public string CBU { get; set; }
        public string? Alias { get; set; }
        public string SaleConditionString { get; set; }
    }
}

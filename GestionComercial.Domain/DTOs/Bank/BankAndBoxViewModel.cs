namespace GestionComercial.Domain.DTOs.Bank
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

    }
}

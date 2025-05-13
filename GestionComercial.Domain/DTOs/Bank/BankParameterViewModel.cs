namespace GestionComercial.Domain.DTOs.Bank
{
    public class BankParameterViewModel
    {
        public string SaleCondition { get; set; }

        public decimal Rate { get; set; }

        public int AcreditationDay { get; set; }

        public string BankName { get; set; }

        public int Id { get; set; }

        public int BankId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnabled { get; set; }
    }
}

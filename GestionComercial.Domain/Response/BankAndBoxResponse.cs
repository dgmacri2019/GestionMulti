using GestionComercial.Domain.DTOs.Banks;

namespace GestionComercial.Domain.Response
{
    public class BankAndBoxResponse : GeneralResponse
    {
        public BankViewModel? BankViewModel { get; set; }

        public BoxViewModel? BoxViewModel { get; set; }

        public BankParameterViewModel? BankParameterViewModel { get; set; }


    }
}

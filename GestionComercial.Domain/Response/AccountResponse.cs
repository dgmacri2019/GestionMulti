using GestionComercial.Domain.DTOs.Accounts;

namespace GestionComercial.Domain.Response
{
    public class AccountResponse : GeneralResponse
    {
        public AccountViewModel AccountViewModel { get; set; }
    }
}

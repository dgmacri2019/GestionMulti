using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Accounts
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFirstLevel { get; set; }
        public bool ForeignCurrency { get; set; }
        //public int AccountGroupNumber { get; set; }
        public int AccountSubGroupNumber1 { get; set; }
        public int AccountSubGroupNumber2 { get; set; }
        public int AccountSubGroupNumber3 { get; set; }
        public int AccountSubGroupNumber4 { get; set; }
        public int AccountSubGroupNumber5 { get; set; }
        public int AccountTypeId { get; set; }
        public bool IsReference { get; set; }
        public int AccountIdSubGroupNumber1 { get; set; }
        public int AccountIdSubGroupNumber2 { get; set; }
        public int AccountIdSubGroupNumber3 { get; set; }
        public int AccountIdSubGroupNumber4 { get; set; }
        public string FullDescription
        {
            get
            {
                return string.Format("{0:00}.{1:00}.{2:000}.{3:0000}.{4:00000}.{5:000000} - {6}",
                    AccountTypeId,
                    AccountSubGroupNumber1,
                    AccountSubGroupNumber2,
                    AccountSubGroupNumber3,
                    AccountSubGroupNumber4,
                    AccountSubGroupNumber5,
                    Name);
            }
        }


        public List<AccountViewModel> Children { get; set; } = [];
    }
}

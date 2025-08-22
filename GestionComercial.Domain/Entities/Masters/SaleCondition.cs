using GestionComercial.Domain.Entities.BoxAndBank;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class SaleCondition : CommonEntity
    {
        public required string Description { get; set; }

        public int AfipId { get; set; }



        [JsonIgnore]
        public virtual ICollection<BankParameter>? BankParameters { get; set; }
        public virtual ICollection<Box>? Boxes { get; set; }
        public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<Provider>? Providers { get; set; }
        //public virtual ICollection<Provider>? Providers { get; set; }
    }
}

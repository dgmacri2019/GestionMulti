using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class UserPermission : CommonEntity
    {
        // Clave foránea de ApplicationUser
        public required string UserId { get; set; }

        // Clave foránea de Permission
        public int PermissionId { get; set; }



        [JsonIgnore] 
        public virtual User? User { get; set; }

        [JsonIgnore] 
        public virtual Permission? Permission { get; set; }
    }
}

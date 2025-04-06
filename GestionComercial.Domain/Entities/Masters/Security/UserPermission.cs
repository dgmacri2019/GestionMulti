using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class UserPermission : CommonEntity
    {
        // Clave foránea de ApplicationUser
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }


        // Clave foránea de Permission
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}

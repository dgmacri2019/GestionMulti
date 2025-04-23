namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class UserPermission : CommonEntity
    {
        // Clave foránea de ApplicationUser
        public required string UserId { get; set; }

        // Clave foránea de Permission
        public int PermissionId { get; set; }



        public virtual User? User { get; set; }

        public virtual Permission? Permission { get; set; }
    }
}

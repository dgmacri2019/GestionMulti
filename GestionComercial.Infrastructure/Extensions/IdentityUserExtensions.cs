using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Infrastructure.Extensions
{
    public static class IdentityUserExtensions
    {
        /// <summary>
        /// Verifica si el usuario posee un permiso específico, combinando los permisos
        /// asignados directamente y los que se heredan a través de sus roles.
        /// </summary>
        /// <param name="user">El objeto IdentityUser</param>
        /// <param name="permissionName">Nombre del permiso a verificar</param>
        /// <param name="context">Contexto de datos (AppDbContext)</param>
        /// <returns>true si el usuario tiene el permiso; false en caso contrario</returns>
        public static bool HasPermission(this IdentityUser user, string permissionName, AppDbContext context)
        {
            // Permisos asignados directamente al usuario
            var directPermissions = context.UserPermissions
                .Where(up => up.UserId == user.Id)
                .Select(up => up.Permission.Name);

            // Obtener los roles del usuario desde la tabla intermedia de Identity (AspNetUserRoles)
            var roleIds = context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId);

            // Permisos asignados a través de los roles (desde la tabla RolePermissions)
            var rolePermissions = context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name);

            // Unir ambos conjuntos de permisos y verificar si se encuentra el permiso requerido
            var allPermissions = directPermissions.Union(rolePermissions);

            return allPermissions.Contains(permissionName);
        }
    }
}
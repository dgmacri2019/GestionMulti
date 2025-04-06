namespace GestionComercial.Applications.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);

        Task<bool> DeleteRoleAsync(string roleId);

        List<string> GetRoles();
    }
}

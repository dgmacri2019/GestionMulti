namespace GestionComercial.API.Mappers
{
    public static class PermissionNameMapper
    {
        public static string MapController(string controller)
        {
            return controller switch
            {
                "Articles" => "Articulos",
                "Clients" => "Clientes",
                "Users" => "Usuarios",
                _ => controller
            };
        }

        public static string MapAction(string action)
        {
            return action switch
            {
                "Add" or "AddAsync" => "Agregar",
                "Edit" or "EditAsync" or "Update" or "UpdateAsync" or "UpdatePrices" or "UpdatePricesAsync" or "GenerateNewBarCode" or "GenerateNewBarCodeAsync" or "ChangeRoleAsync" => "Editar",
                "Delete" or "DeleteAsync" => "Eliminar",
                "GetAll" or "GetAllAsync" or "GetById" or "GetByIdAsync" or "FindByBarCode" or "FindByBarCodeAsync"
                or "FindByCodeOrBarCode" or "FindByCodeOrBarCodeAsync" or "SearchToList" or "SearchToListAsync" => "Lectura",
                _ => action
            };
        }
    }
}
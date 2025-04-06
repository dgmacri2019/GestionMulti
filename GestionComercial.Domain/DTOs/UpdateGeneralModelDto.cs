namespace GestionComercial.Domain.DTOs
{
    public class UpdateGeneralModelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
    }
}

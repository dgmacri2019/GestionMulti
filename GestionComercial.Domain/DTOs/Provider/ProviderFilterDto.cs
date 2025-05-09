namespace GestionComercial.Domain.DTOs.Provider
{
    public class ProviderFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string FansatyName { get; set; } = string.Empty;
        public string FansatyDescription { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
    }
}

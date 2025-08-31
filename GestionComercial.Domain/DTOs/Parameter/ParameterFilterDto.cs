namespace GestionComercial.Domain.DTOs.Parameter
{
    public class ParameterFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public string PcName { get; set; } = string.Empty;
    }
}

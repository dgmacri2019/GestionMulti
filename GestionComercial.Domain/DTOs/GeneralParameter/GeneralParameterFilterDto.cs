namespace GestionComercial.Domain.DTOs.GeneralParameter
{
    public class GeneralParameterFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
    }
}

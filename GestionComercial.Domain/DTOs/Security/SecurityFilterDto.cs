using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionComercial.Domain.DTOs.Security
{
    public class SecurityFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
    }
}

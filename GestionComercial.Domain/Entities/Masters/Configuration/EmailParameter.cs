using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters.Configuration
{
    public class EmailParameter : CommonEntity
    {
        [Display(Name = "Direccion del servidor")]
        public string AddressServer { get; set; }

        [Display(Name = "Puerto SMTP")]
        public int SMTPPort { get; set; }

        [Display(Name = "User SSL")]
        public bool UseSSL { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remitente")]
        public string FromName { get; set; }

        [Display(Name = "Responser A")]
        public string RaplyTo { get; set; }
    }
}

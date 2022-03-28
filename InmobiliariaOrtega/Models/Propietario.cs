using System.ComponentModel.DataAnnotations;

namespace InmobiliariaOrtega.Models
{
    public class Propietario
    {
        public int IdPropietario { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(8, MinimumLength = 8, ErrorMessage = "Un DNI debe tener 8 dígitos")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(15, MinimumLength = 10, ErrorMessage = "Un número de teléfono debe tener entre 10 y 15 dígitos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            EmailAddress(ErrorMessage = "Debe ser una dirección de correo válida"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres"),
            DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}

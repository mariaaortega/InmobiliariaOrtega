
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaOrtega.Models
{
    public class Inmueble : Entidad
    {
        [Required(ErrorMessage = "Campo obligatorio"),
            ForeignKey("PropietarioId"),
            Display(Name = "Propietario")]
        public int PropietarioId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
#pragma warning disable CS8618 // El elemento propiedad "Direccion" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Direccion { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Direccion" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio")]
#pragma warning disable CS8618 // El elemento propiedad "Uso" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Uso { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Uso" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio")]
#pragma warning disable CS8618 // El elemento propiedad "Tipo" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Tipo { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Tipo" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(8, MinimumLength = 1, ErrorMessage = "Ingrese un precio válido")]
        public int Precio { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(2, MinimumLength = 0, ErrorMessage = "Ingrese un numero de ambientes válido")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(8, MinimumLength = 1, ErrorMessage = "Ingrese una superficie válida")]
        public int Superficie { get; set; }

        public int Visible { get; set; }

        [Display(Name = "Propietario")]
#pragma warning disable CS8618 // El elemento propiedad "Dueño" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public Propietario Dueño { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Dueño" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        public override string ToString()
        {
            return $"#{Id} {Direccion} | {Dueño.Nombre[0].ToString().ToUpper()}. {Dueño.Apellido}";
        }
    }
}

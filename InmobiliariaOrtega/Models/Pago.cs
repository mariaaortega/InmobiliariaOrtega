using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaOrtega.Models
{
    public class Pago : Entidad
    {
        [Required(ErrorMessage = "Campo obligatorio"),
            ForeignKey("ContratoId"),
            Display(Name = "Contrato")]
        public int ContratoId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(8, MinimumLength = 1, ErrorMessage = "Ingrese un importe válido")]
        public int Importe { get; set; }

        [Display(Name = "Contrato")]
#pragma warning disable CS8618 // El elemento propiedad "Contrato" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public Contrato Contrato { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Contrato" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        public override string ToString()
        {
            return $"#{Contrato.Id}/{Id} {Fecha.ToShortDateString()} ${Importe}";
        }
    }
}

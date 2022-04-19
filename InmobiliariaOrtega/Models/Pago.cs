﻿using System.ComponentModel.DataAnnotations;
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

        public Contrato Contrato { get; set; }


        public override string ToString()
        {
            return $"#{Contrato.Id}/{Id} {Fecha.ToShortDateString()} ${Importe}";
        }
    }
}

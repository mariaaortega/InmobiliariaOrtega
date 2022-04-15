﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace InmobiliariaOrtega.Models
{
    public enum Estados
    {
        Abierto = 1,
        Terminado = 2,
        Roto = 3
    }
    public class Contrato : Entidad
    {
        [Required(ErrorMessage = "Campo obligatorio"),
            ForeignKey("InmuebleId"),
            Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            ForeignKey("InquilinoId"),
            Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            Display(Name = "Fecha inicial")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"),
            Display(Name = "Fecha final")]
        public DateTime FechaHasta { get; set; }

        public int Precio { get; set; }

        [Display(Name = "Inquilino")]
#pragma warning disable CS8618 // El elemento propiedad "Inquilino" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public Inquilino Inquilino { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Inquilino" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Display(Name = "Inmueble")]
#pragma warning disable CS8618 // El elemento propiedad "Inmueble" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public Inmueble Inmueble { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Inmueble" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        public int Estado { get; set; }
        public string NombreEstado => ((Estados)Estado).ToString();
        public int CantidadPagos { get; set; }

        public int TotalMeses => Math.Max((FechaHasta.Year - FechaDesde.Year) * 12 + FechaHasta.Month - FechaDesde.Month, 1);
        public DateTime ProximoPago => new DateTime(Math.Min(FechaDesde.AddMonths(CantidadPagos).Ticks, FechaHasta.Ticks));
        public string ProximoPagoTexto => ProximoPago == FechaHasta ? "N/A" : ProximoPago.ToShortDateString();
        public string EstadoPagos => Estado == 1 && ProximoPago < FechaHasta ? (ProximoPago > DateTime.Now ? "Al día" : (ProximoPago.Month == DateTime.Now.Month ? "Pendiente" : "Atrasado")) : "Finalizados";
        public string ResumenPagos => $"{CantidadPagos}/{TotalMeses} mes{(TotalMeses > 1 ? "es" : "")} pagado{(TotalMeses > 1 ? "s" : "")}";
        public bool MitadContratoCumplida => CantidadPagos >= (TotalMeses / 2);

        public string EstadoPagos2 // Testear
        {
            get
            {
                if (Estado == 1 && ProximoPago < FechaHasta) // Si todavia no se hizo el ultimo pago del contrato
                {
                    if (ProximoPago > DateTime.Now) // Si ya se pago el mes actual
                        return "Al día";
                    else
                    {
                        if (ProximoPago.Month == DateTime.Now.Month) // Si se debe solo el mes actual
                            return "Pendiente";
                        else
                            return "Atrasado";
                    }
                }
                else
                {
                    return "Finalizados";
                }
            }
        }
        public static IDictionary<int, string> ObtenerEstados()
        {
            SortedDictionary<int, string> estados = new SortedDictionary<int, string>();
            Type tipoEnumEstado = typeof(Estados);
            foreach (var valor in Enum.GetValues(tipoEnumEstado))
            {
#pragma warning disable CS8604 // Posible argumento de referencia nulo para el parámetro "value" en "void SortedDictionary<int, string>.Add(int key, string value)".
                estados.Add((int)valor, Enum.GetName(tipoEnumEstado, valor));
#pragma warning restore CS8604 // Posible argumento de referencia nulo para el parámetro "value" en "void SortedDictionary<int, string>.Add(int key, string value)".
            }
            return estados;
        }

        public override string ToString()
        {
            return $"#{Id} - {Inmueble.Direccion} ({FechaDesde.Month}/{FechaDesde.Year} - {FechaHasta.Month}/{FechaHasta.Year})";
        }
    }
}

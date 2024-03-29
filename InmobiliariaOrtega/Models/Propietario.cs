﻿using System.ComponentModel.DataAnnotations;

namespace InmobiliariaOrtega.Models
{
    public class Propietario : Entidad
    {
        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
#pragma warning disable CS8618 // El elemento propiedad "Nombre" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Nombre { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Nombre" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
#pragma warning disable CS8618 // El elemento propiedad "Apellido" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Apellido { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Apellido" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(8, MinimumLength = 8, ErrorMessage = "Un DNI debe tener 8 dígitos")]
#pragma warning disable CS8618 // El elemento propiedad "Dni" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Dni { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Dni" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            StringLength(15, MinimumLength = 10, ErrorMessage = "Un número de teléfono debe tener entre 10 y 15 dígitos")]
#pragma warning disable CS8618 // El elemento propiedad "Telefono" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Telefono { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Telefono" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            EmailAddress(ErrorMessage = "Debe ser una dirección de correo válida"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
#pragma warning disable CS8618 // El elemento propiedad "Email" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Email { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Email" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        [Required(ErrorMessage = "Campo obligatorio"),
            MaxLength(50, ErrorMessage = "Máximo 50 caracteres"),
            DataType(DataType.Password)]
#pragma warning disable CS8618 // El elemento propiedad "Clave" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Clave { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Clave" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        public override string ToString()
        {
            return $"#{Id} {Nombre[0].ToString().ToUpper()}. {Apellido}";
        }
    }
}

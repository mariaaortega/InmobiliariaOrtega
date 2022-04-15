using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaOrtega.Models
{
    public enum Roles
    {
        SuperAdministrador = 1,
        Administrador = 2,
        Empleado = 3
    }

    public class Usuario : Entidad
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

#pragma warning disable CS8618 // El elemento propiedad "Avatar" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Avatar { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Avatar" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
#pragma warning disable CS8618 // El elemento propiedad "AvatarFile" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public IFormFile AvatarFile { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "AvatarFile" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public int Rol { get; set; }

        public string RolNombre => Rol > 0 ? ((Roles)Rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(Roles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
#pragma warning disable CS8604 // Posible argumento de referencia nulo para el parámetro "value" en "void SortedDictionary<int, string>.Add(int key, string value)".
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
#pragma warning restore CS8604 // Posible argumento de referencia nulo para el parámetro "value" en "void SortedDictionary<int, string>.Add(int key, string value)".
            }
            return roles;
        }

        public override string ToString()
        {
            return $"#{Id} {Nombre[0].ToString().ToUpper()}. {Apellido}";
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace InmobiliariaOrtega.Models
{
    public class LoginView
    {
        [DataType(DataType.EmailAddress)]
#pragma warning disable CS8618 // El elemento propiedad "Usuario" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Usuario { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Usuario" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        [DataType(DataType.Password)]
#pragma warning disable CS8618 // El elemento propiedad "Clave" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string Clave { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "Clave" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
    }
}

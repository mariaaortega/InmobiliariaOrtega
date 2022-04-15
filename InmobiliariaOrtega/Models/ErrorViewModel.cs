using System;

namespace InmobiliariaOrtega.Models
{
    public class ErrorViewModel
    {
#pragma warning disable CS8618 // El elemento propiedad "RequestId" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public string RequestId { get; set; }
#pragma warning restore CS8618 // El elemento propiedad "RequestId" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

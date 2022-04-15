

namespace InmobiliariaOrtega.Models
{
    public class RepositorioInquilino : RepositorioBase
    {
        public RepositorioInquilino(IConfiguration config) : base(config)
        {
            this.tabla = "Inquilinos";
            this.columnas = new string[11] { "Nombre", "Apellido", "Dni", "Telefono", "Email", "LugarTrabajo", "NombreGarante", "ApellidoGarante", "DniGarante", "TelefonoGarante", "EmailGarante" };
        }
    }
}

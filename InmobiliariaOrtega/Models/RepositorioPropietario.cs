using System.Data.SqlClient;

namespace InmobiliariaOrtega.Models
{
    public class RepositorioPropietario : RepositorioBase
    {
        public RepositorioPropietario(IConfiguration config) : base(config)
        {
            this.tabla = "Propietarios";
            this.columnas = new string[6] { "Nombre", "Apellido", "Dni", "Telefono", "Email", "Clave" };
        }
    }
}

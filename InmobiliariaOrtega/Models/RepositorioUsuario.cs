using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaOrtega.Models
{
    public class RepositorioUsuario : RepositorioBase
    {
        public RepositorioUsuario(IConfiguration config) : base(config)
        {
            this.tabla = "Usuarios";
            this.columnas = new string[5] { "Nombre", "Apellido", "Email", "Clave", "Rol" };
        }

        public Usuario ObtenerPorEmail(string email)
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            Usuario res = null;
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM {tabla} WHERE Email = '{email}';";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Usuario();
#pragma warning disable CS8601 // Posible asignación de referencia nula.
                        res.Nombre = reader["Nombre"].ToString();
#pragma warning restore CS8601 // Posible asignación de referencia nula.
#pragma warning disable CS8601 // Posible asignación de referencia nula.
                        res.Apellido = reader["Apellido"].ToString();
#pragma warning restore CS8601 // Posible asignación de referencia nula.
#pragma warning disable CS8601 // Posible asignación de referencia nula.
                        res.Avatar = reader["Avatar"].ToString();
#pragma warning restore CS8601 // Posible asignación de referencia nula.
#pragma warning disable CS8601 // Posible asignación de referencia nula.
                        res.Email = reader["Email"].ToString();
#pragma warning restore CS8601 // Posible asignación de referencia nula.
#pragma warning disable CS8601 // Posible asignación de referencia nula.
                        res.Clave = reader["Clave"].ToString();
#pragma warning restore CS8601 // Posible asignación de referencia nula.
                        res.Rol = (int)reader["Rol"];
                        res.Id = (int)reader["Id"];
                        connection.Close();
                    }
                }
            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo.
            return res;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo.
        }
    }
}

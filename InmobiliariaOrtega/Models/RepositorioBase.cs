
using System.Data.SqlClient;

namespace InmobiliariaOrtega.Models
{
    public abstract class RepositorioBase
    {
        protected readonly IConfiguration configuration;
        protected readonly string connectionString;
        protected string tabla;
        protected string[] columnas;

#pragma warning disable CS8618 // El elemento campo "columnas" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
#pragma warning disable CS8618 // El elemento campo "tabla" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
        public RepositorioBase(IConfiguration configuration)
#pragma warning restore CS8618 // El elemento campo "tabla" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
#pragma warning restore CS8618 // El elemento campo "columnas" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public int Eliminar(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM {tabla} WHERE Id = {id};";
                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    res = 1;
                }
            }
            return res;
        }

        public int Editar(Entidad e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {tabla} SET ";
                for (int i = 0; i < columnas.Length; i++)
                {
                    if (i == columnas.Length - 1)
                        sql += $"{columnas[i]} = @{columnas[i]}";
                    else
                        sql += $"{columnas[i]} = @{columnas[i]}, ";
                }
                sql += $" WHERE Id = {e.Id}";

                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var col in columnas)
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        command.Parameters.AddWithValue(col, e.GetType().GetProperty(col).GetValue(e));
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    res = 1;
                }
            }
            return res;
        }

        public int Alta(Entidad e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "", cols = "", vals = "";
                for (int i = 0; i < columnas.Length; i++)
                {
                    if (i == columnas.Length - 1)
                    {
                        cols += columnas[i];
                        vals += $"@{columnas[i]}";
                    }
                    else
                    {
                        cols += $"{columnas[i]}, ";
                        vals += $"@{columnas[i]}, ";
                    }
                }
                sql = $"INSERT INTO {tabla} ({cols}) VALUES ({vals}); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(sql, connection))
                {
                    foreach (var col in columnas)
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        command.Parameters.AddWithValue(col, e.GetType().GetProperty(col).GetValue(e));
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    e.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public T ObtenerPorId<T>(int id) where T : Entidad, new()
        {
            T res = new T();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, ";
                for (int i = 0; i < columnas.Length; i++)
                {
                    if (i == columnas.Length - 1)
                        sql += columnas[i];
                    else
                        sql += $"{columnas[i]}, ";
                }
                sql += $" FROM {tabla} WHERE Id = {id};";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    foreach (var col in columnas)
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        res.GetType().GetProperty(col).SetValue(res, reader[col]);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                    res.Id = reader.GetInt32(0);
                    connection.Close();
                }
            }
            return res;
        }

        public List<T> ObtenerTodos<T>() where T : Entidad, new()
        {
            List<T> res = new List<T>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, ";
                for (int i = 0; i < columnas.Length; i++)
                {
                    if (i == columnas.Length - 1)
                        sql += columnas[i];
                    else
                        sql += $"{columnas[i]}, ";
                }
                sql += $" FROM {tabla} ORDER BY Id DESC;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T item = new T();
                        foreach (var col in columnas)
                        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                            item.GetType().GetProperty(col).SetValue(item, reader[col]);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                            item.Id = reader.GetInt32(0);
                        }
                        res.Add(item);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}

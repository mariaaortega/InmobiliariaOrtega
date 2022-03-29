using System.Data.SqlClient;

namespace InmobiliariaOrtega.Models
{
    public class RepositorioPropietario
    {
        string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Inmobiliaria2022;Trusted_Connection=True;MultipleActiveResultSets=true";
        public RepositorioPropietario()
        { }
		public IList<Propietario> ObtenerTodos()
		{
			var res = new List<Propietario>();
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave" +
					$" FROM Propietarios;";
				using (SqlCommand comm = new SqlCommand(sql, connection))
				{
					connection.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							IdPropietario = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							Clave = reader.GetString(6),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		//{}
		public int Alta(Propietario p)
		{
			var res = -1;
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				 string sql = $"INSERT INTO Propietarios (Nombre, Apellido, Dni, Telefono, Email, Clave) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email, @clave);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand comm = new SqlCommand(sql, connection)) {
					comm.Parameters.AddWithValue("@nombre", p.Nombre);
					comm.Parameters.AddWithValue("@apellido", p.Apellido);
					comm.Parameters.AddWithValue("@dni", p.Dni);
					comm.Parameters.AddWithValue("@telefono", p.Telefono);
					comm.Parameters.AddWithValue("@email", p.Email);
					comm.Parameters.AddWithValue("@clave", p.Clave);
					connection.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
					p.IdPropietario = res;
					connection.Close();
				}
			}
			return res;
		}
		
	}
}

using System.Data.SqlClient;

namespace InmobiliariaOrtega.Models
{
    public class RepositorioInquilino
    {
        string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Inmobiliaria2022;Trusted_Connection=True;MultipleActiveResultSets=true";
        public RepositorioInquilino()
        { }
		public IList<Inquilino> ObtenerTodos()
		{
			var res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				string sql = $"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email, LugarTrabajo, NombreGarante, ApellidoGarante, DniGarante, TelefonoGarante, EmailGarante " +
					$" FROM Inquilinos;";
				using (SqlCommand comm = new SqlCommand(sql, connection))
				{
					connection.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							IdInquilino = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							LugarTrabajo = reader.GetString(6),
							NombreGarante = reader.GetString(7),
							ApellidoGarante = reader.GetString(8),
							DniGarante = reader.GetString(9),
							TelefonoGarante = reader.GetString(10),
							EmailGarante = reader.GetString(11),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		//{}
		public int Alta(Inquilino i)
		{
			var res = -1;
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				 string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Telefono, Email,  LugarTrabajo, NombreGarante, ApellidoGarante, DniGarante, TelefonoGarante, EmailGarante) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email,  @lugartrabajo, @nombregarante, @apellidogarante, @dnigarante, @telefonogarante, @emailgarante);" +
					"SELECT SCOPE_IDENTITY();";
				using (SqlCommand comm = new SqlCommand(sql, connection)) {
					comm.Parameters.AddWithValue("@nombre", i.Nombre);
					comm.Parameters.AddWithValue("@apellido", i.Apellido);
					comm.Parameters.AddWithValue("@dni", i.Dni);
					comm.Parameters.AddWithValue("@telefono", i.Telefono);
					comm.Parameters.AddWithValue("@email", i.Email);
					comm.Parameters.AddWithValue("@lugartrabajo", i.LugarTrabajo);
					comm.Parameters.AddWithValue("@nombregarante", i.NombreGarante);
					comm.Parameters.AddWithValue("@apellidogarante", i.ApellidoGarante);
					comm.Parameters.AddWithValue("@dnigarante", i.DniGarante);
					comm.Parameters.AddWithValue("@telefonogarante", i.TelefonoGarante);
					comm.Parameters.AddWithValue("@emailgarante", i.EmailGarante);
					connection.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
					i.IdInquilino = res;
					connection.Close();
				}
			}
			return res;
		}
		
	}
}

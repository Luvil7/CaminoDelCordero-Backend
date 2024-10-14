
using ApiClass.Models;
    using System.Data;
    using System.Data.SqlClient;

    namespace MiPrimeraApi.Repository
    {
        public static class UsuarioHandler
        {



        public const string ConnectionString =
            "Server=DESKTOP-LUCAS1\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
        public static List<Usuario> GetUsuarios()
            {
                List<Usuario> resultado = new List<Usuario>();
                using (SqlConnection sqlConecction = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Usuario", sqlConecction))
                    {
                        sqlConecction.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Usuario usuario = new Usuario();
                                    usuario.Id = Convert.ToInt32(reader["ID"]);
                                    usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                    usuario.Nombre = reader["Nombre"].ToString();
                                    usuario.Apellido = reader["Apellido"].ToString();
                                    usuario.Contraseña = reader["Contraseña"].ToString();
                                    usuario.Mail = reader["Mail"].ToString();
                                    resultado.Add(usuario);
                                }
                            }
                        }
                        sqlConecction.Close();
                    }
                }
                return resultado;
            }
            public static bool EliminarUsuario(int id)
            {
                bool resultado = false;
                using (SqlConnection sqlConecction = new SqlConnection(ConnectionString))
                {
                    string queryDelete = "DELETE FROM Usuario" +
                        "WHERE Id = @idUsuario";
                    SqlParameter sqlParameter = new SqlParameter("idUsuario", System.Data.SqlDbType.BigInt);
                    sqlParameter.Value = id;
                    sqlConecction.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConecction))
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                    }
                    sqlConecction.Close();
                }
                return resultado;
            }
        public static bool CrearUsuario(Usuario usuario)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryInsert = "INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Contraseña, Mail) VALUES " +
                    "(@Nombre, @Apellido, @NombreUsuario, @Contraseña, @Mail);";

                SqlParameter sqlParameterNombre = new SqlParameter("@Nombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                SqlParameter sqlParameterApellido = new SqlParameter("@Apellido", SqlDbType.VarChar) { Value = usuario.Apellido };
                SqlParameter sqlParameterNombreUsuario = new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter sqlParameterContraseña = new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña };
                SqlParameter sqlParameterMail = new SqlParameter("@Mail", SqlDbType.VarChar) { Value = usuario.Mail };

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameterNombre);
                    sqlCommand.Parameters.Add(sqlParameterApellido);
                    sqlCommand.Parameters.Add(sqlParameterNombreUsuario);
                    sqlCommand.Parameters.Add(sqlParameterContraseña);
                    sqlCommand.Parameters.Add(sqlParameterMail);

                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }

        public static bool UpdateNombre(Usuario usuario)
            {
                bool resultado = false;
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    string queryUpdate = "UPDATE Usuario SET Nombre = @nuevoNombre WHERE Id = @idUsuario";
                    SqlParameter parametroId = new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = usuario.Id };
                    SqlParameter parametroName = new SqlParameter("nuevoNombre", SqlDbType.VarChar) { Value = usuario.Nombre };
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(parametroId);
                        sqlCommand.Parameters.Add(parametroName);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            resultado = true;
                        }
                    }
                    sqlConnection.Close();
                }
                return resultado;
            }
    }
 }


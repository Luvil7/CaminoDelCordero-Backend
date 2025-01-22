namespace ProyectoFinal.Repository
{
    using ProyectoFinal.Models;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel.DataAnnotations;

    public static class LicenciaHandler
    {
        //public const string ConnectionString = @"Server=DESKTOP-LUCAS1\SQLEXPRESS;Database=GestionLicencias;Integrated Security=True;TrustServerCertificate=True;";
        public const string ConnectionString = @"Server=U3465-VPN011\SQLEXPRESS;Database=GestionLicencias;Integrated Security=True;TrustServerCertificate=True;";


        public static List<Licencia> GetLicencias()
        {
            List<Licencia> resultado = new List<Licencia>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = @"
                    SELECT 
                        l.Id AS LicenciaId,
                        l.FechaInicio,
                        l.FechaFin,
                        l.Tipo,
                        l.Provincia,
                        l.Localidad,
                        l.Dir,
                        l.OD,
                        s.Id AS SoldadoId,
                        s.Dni,
                        s.Nombre,
                        s.Apellido
                    FROM Licencias l
                    INNER JOIN Soldados s ON l.SoldadoDni = s.Dni;
                ";

                        sqlConnection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = sqlCommand;
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        foreach (DataRow row in table.Rows)
                        {
                            try
                            {
                                Soldado soldado = new Soldado
                                {
                                    Id = Convert.ToInt32(row["SoldadoId"]),
                                    Dni = Convert.ToInt32(row["Dni"]),
                                    Nombre = row["Nombre"].ToString(),
                                    Apellido = row["Apellido"].ToString()
                                };
                                Licencia licencia = new Licencia
                                {
                                    Id = Convert.ToInt32(row["LicenciaId"]),
                                    SoldadoDni = Convert.ToInt32(row["Dni"]),
                                    Soldado = soldado,
                                    FechaInicio = Convert.ToDateTime(row["FechaInicio"]).ToString("yyyy-MM-dd"),
                                    FechaFin = Convert.ToDateTime(row["FechaFin"]).ToString("yyyy-MM-dd"),
                                    Tipo = row["Tipo"].ToString(),
                                    Provincia = row["Provincia"].ToString(),
                                    Localidad = row["Localidad"].ToString(),
                                    Dir = row["Dir"].ToString(),
                                    OD = row["OD"].ToString()
                                };

                                resultado.Add(licencia);
                            }
                            catch (Exception rowEx)
                            {
                                Console.WriteLine($"Error procesando fila: {rowEx.Message}");
                            }
                        }
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetLicencias: {ex.Message}");
            }
            return resultado;
        }

        public static string DeleteLicencia(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM Licencias WHERE Id = @idLicencia";
                SqlParameter sqlParameter = new SqlParameter("idLicencia", System.Data.SqlDbType.Int);
                sqlParameter.Value = id;
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        string mensaje = $"La licencia con ID: {id} se eliminó correctamente";
                        Console.WriteLine(mensaje);
                        return mensaje;
                    }
                    return $"No se encontró ninguna licencia con ID: {id}";
                }
            }
        }

        public static string UpdateLicencia(Licencia licencia)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    // Verificar que el soldado existe
                    string checkDniQuery = "SELECT COUNT(1) FROM Soldados WHERE Dni = @Dni";
                    using (SqlCommand checkCommand = new SqlCommand(checkDniQuery, sqlConnection))
                    {
                        checkCommand.Parameters.AddWithValue("@Dni", licencia.SoldadoDni);
                        int exists = (int)checkCommand.ExecuteScalar();
                        if (exists == 0)
                        {
                            return $"Error de validación: No existe un soldado con el DNI {licencia.SoldadoDni}";
                        }
                    }
                    LicenciaValidationHandler.ValidateLicencia(licencia);

                    string queryUpdate = @"
                UPDATE Licencias 
                SET SoldadoDni = @SoldDni, 
                    FechaInicio = @FechaI, 
                    FechaFin = @FechaF, 
                    Tipo = @Tipo, 
                    Provincia = @Provincia,
                    Localidad = @Localidad,
                    Dir = @Dir,
                    OD = @OD 
                WHERE Id = @id;

                -- Verificar si se actualizó algún registro
                IF @@ROWCOUNT > 0
                    SELECT 'La licencia se actualizó correctamente' as Result;
                ELSE
                    SELECT 'No se encontró la licencia con el ID especificado' as Result;";

                    using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", licencia.Id);
                        sqlCommand.Parameters.AddWithValue("@SoldDni", licencia.SoldadoDni);
                        sqlCommand.Parameters.AddWithValue("@FechaI", licencia.FechaInicio);
                        sqlCommand.Parameters.AddWithValue("@FechaF", licencia.FechaFin);
                        sqlCommand.Parameters.AddWithValue("@Tipo", licencia.Tipo);
                        sqlCommand.Parameters.AddWithValue("@Provincia", licencia.Provincia);
                        sqlCommand.Parameters.AddWithValue("@Localidad", licencia.Localidad);
                        sqlCommand.Parameters.AddWithValue("@Dir", licencia.Dir);
                        sqlCommand.Parameters.AddWithValue("@OD", licencia.OD);

                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                                return reader["Result"].ToString();
                        }

                        return "Error al actualizar la licencia";
                    }
                }
            }
            catch (ValidationException ex)
            {
                return $"Error de validación: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la licencia: {ex.ToString()}");
                return $"Error al actualizar la licencia: {ex.Message}";
            }
        }



        public static string AddLicencia(Licencia licencia)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    string checkDniQuery = "SELECT COUNT(1) FROM Soldados WHERE Dni = @Dni";
                    using (SqlCommand checkCommand = new SqlCommand(checkDniQuery, sqlConnection))
                    {
                        checkCommand.Parameters.AddWithValue("@Dni", licencia.SoldadoDni);
                        int exists = (int)checkCommand.ExecuteScalar();
                        if (exists == 0)
                        {
                            return $"Error de validación: No existe un soldado con el DNI {licencia.SoldadoDni}";
                        }
                    }
                    LicenciaValidationHandler.ValidateLicencia(licencia);

                    string queryInsert = @"
                INSERT INTO Licencias 
                    (SoldadoDni, FechaInicio, FechaFin, Tipo, Provincia, Localidad, Dir, OD)
                    VALUES (@SoldDni, @FechaI, @FechaF, @Tipo, @Provincia,@Localidad,@Dir,@OD)
                

                SELECT SCOPE_IDENTITY() as Id, 'La licencia se agregó correctamente' as Result;";

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@SoldDni", licencia.SoldadoDni);
                        sqlCommand.Parameters.AddWithValue("@FechaI", licencia.FechaInicio);
                        sqlCommand.Parameters.AddWithValue("@FechaF", licencia.FechaFin);
                        sqlCommand.Parameters.AddWithValue("@Tipo", licencia.Tipo);
                        sqlCommand.Parameters.AddWithValue("@Provincia", licencia.Provincia);
                        sqlCommand.Parameters.AddWithValue("@Localidad", licencia.Localidad);
                        sqlCommand.Parameters.AddWithValue("@Dir", licencia.Dir);
                        sqlCommand.Parameters.AddWithValue("@OD", licencia.OD);

                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                                return reader["Result"].ToString();
                        }

                        return "Error al agregar la licencia";
                    }
                }
            }
            catch (ValidationException ex)
            {
                return $"Error de validación: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la licencia: {ex.ToString()}");
                return $"Error al agregar la licencia: {ex.Message}";
            }
        }
    }
}

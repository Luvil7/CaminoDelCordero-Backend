namespace ProyectoFinal.Repository
{

    using ProyectoFinal.Models;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel.DataAnnotations;

    public class SoldadoHandler
    {

        //public const string ConnectionString = @"Server=DESKTOP-LUCAS1\SQLEXPRESS;Database=GestionLicencias;Integrated Security=True;TrustServerCertificate=True;";
        public const string ConnectionString = @"Server=U3465-VPN011\SQLEXPRESS;Database=GestionLicencias;Integrated Security=True;TrustServerCertificate=True;";

        public static List<Soldado> GetSoldados()
        {
            List<Soldado> resultado = new List<Soldado>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = @"
                    SELECT 
                      *
                    FROM Soldados;
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
                                    Id = Convert.ToInt32(row["Id"]),
                                    Dni = Convert.ToInt32(row["Dni"]),
                                    Nombre = row["Nombre"].ToString(),
                                    Apellido = row["Apellido"].ToString()
                                };
                                

                                resultado.Add(soldado);
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
                Console.WriteLine($"Error en GetSoldados: {ex.Message}");
            }
            return resultado;
        }



        public static string DeleteSoldado(int dni)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM Soldados WHERE Dni = @idSoldado";
                SqlParameter sqlParameter = new SqlParameter("idSoldado", System.Data.SqlDbType.Int);
                sqlParameter.Value = dni;
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        string mensaje = $"El soldado con DNI: {dni} se eliminó correctamente";
                        Console.WriteLine(mensaje);
                        return mensaje;
                    }
                    return $"No se encontró ningun Soldado con DNI: {dni}";
                }
            }
        }

        public static string UpdateSoldado(Soldado soldado)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            string checkIdQuery = "SELECT Dni FROM Soldados WHERE Id = @id";
                            int oldDni;
                            using (SqlCommand checkCommand = new SqlCommand(checkIdQuery, sqlConnection, transaction))
                            {
                                checkCommand.Parameters.AddWithValue("@id", soldado.Id);
                                var result = checkCommand.ExecuteScalar();
                                if (result == null)
                                {
                                    return $"Error de validación: No existe un soldado con el ID {soldado.Id}";
                                }
                                oldDni = (int)result;
                            }

                            string disableConstraintQuery = "ALTER TABLE Licencias NOCHECK CONSTRAINT FK_Licencias_Soldados";
                            using (SqlCommand disableConstraintCommand = new SqlCommand(disableConstraintQuery, sqlConnection, transaction))
                            {
                                disableConstraintCommand.ExecuteNonQuery();
                            }

                            // actualizo el dni en ambas tablas
                            string updateSoldadoQuery = @"
                                UPDATE Soldados 
                                SET Dni = @newDni 
                                WHERE Id = @id";

                            using (SqlCommand updateSoldadoCommand = new SqlCommand(updateSoldadoQuery, sqlConnection, transaction))
                            {
                                updateSoldadoCommand.Parameters.AddWithValue("@id", soldado.Id);
                                updateSoldadoCommand.Parameters.AddWithValue("@newDni", soldado.Dni);
                                updateSoldadoCommand.ExecuteNonQuery();
                            }

                            string updateLicenciasQuery = @"
                                UPDATE Licencias 
                                SET SoldadoDni = @newDni 
                                WHERE SoldadoDni = @oldDni";

                            using (SqlCommand updateLicenciasCommand = new SqlCommand(updateLicenciasQuery, sqlConnection, transaction))
                            {
                                updateLicenciasCommand.Parameters.AddWithValue("@oldDni", oldDni);
                                updateLicenciasCommand.Parameters.AddWithValue("@newDni", soldado.Dni);
                                updateLicenciasCommand.ExecuteNonQuery();
                            }

                            string enableConstraintQuery = "ALTER TABLE Licencias WITH CHECK CHECK CONSTRAINT FK_Licencias_Soldados";
                            using (SqlCommand enableConstraintCommand = new SqlCommand(enableConstraintQuery, sqlConnection, transaction))
                            {
                                enableConstraintCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return "El soldado se actualizó correctamente";
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el soldado: {ex.ToString()}");
                return $"Error al actualizar el soldado: {ex.Message}";
            }
        }


        public static string AddSoldado(Soldado soldado)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();


                    string queryInsert = @"
                INSERT INTO Soldados
                    (Dni, Nombre, Apellido)
                    VALUES (@newDni, @newNombre, @newApellido)
                

                SELECT SCOPE_IDENTITY() as Id, 'El soldado se agregó correctamente' as Result;";

                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@newDni", soldado.Dni);
                        sqlCommand.Parameters.AddWithValue("@newNombre", soldado.Nombre);
                        sqlCommand.Parameters.AddWithValue("@newApellido", soldado.Apellido);
                   
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                                return reader["Result"].ToString();
                        }

                        return "Error al agregar al Soldado";
                    }
                }
            }
            catch (ValidationException ex)
            {
                return $"Error de validación: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el soldado: {ex.ToString()}");
                return $"Error al agregar el soldado: {ex.Message}";
            }
        }
    }
}

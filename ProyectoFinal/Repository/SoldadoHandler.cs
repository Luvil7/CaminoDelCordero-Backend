namespace ProyectoFinal.Repository
{

    using ProyectoFinal.Models;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel.DataAnnotations;

    public class SoldadoHandler
    {

        public const string ConnectionString = @"Server=DESKTOP-LUCAS1\SQLEXPRESS;Database=GestionLicencias;Integrated Security=True;TrustServerCertificate=True;";


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

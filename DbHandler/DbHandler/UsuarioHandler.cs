using EjemploClase;
using System.Data;
using System.Data.SqlClient;

public class Usuario
{
    public string nombre;
    public string apellido;
    public string NombreUsuario;
    public int id;
    public string Contraseña;
    public string Mail;

    public Usuario(string nombre, string apellido, string nombreUsuario, int id, string contraseña, string mail)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        NombreUsuario = nombreUsuario;
        this.id = id;
        Contraseña = contraseña;
        Mail = mail;
    }
}


public class UsuarioHandler : DbHandler
{
    public List<Usuario> GetUsuarios()

    {
        // esto es para que se libere el objeto SqlConnection
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            // command toma dos cosas, la query y el sqlConnection
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Usuario", sqlConnection))
            {


               
                sqlConnection.Open();
                // creamos el parametro que le vamos a pasar
                
                List<Usuario> usuarios = new List<Usuario>();

                //ejemplo de datareader
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    // para estar seguro que ahy filas o registros para leer
                    if (dataReader.HasRows)
                    {
                        // mientras haya para leer
                        while (dataReader.Read())
                        {
                            Usuario usuario = new Usuario();
                            usuario.id = Convert.ToInt32(dataReader["ID"]);
                            usuario.nombre = dataReader["Nombre"].ToString();
                            usuario.apellido = dataReader["Apellido"].ToString();


                        }

                    }

                }

                sqlConnection.Close();
                return usuarios;
            }
        }
    }
    // COMO MODIFICAR DATOS CON .NET
    public void Delete(Usuario usuario) 
    {
        try
        {

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {

                string queryDelete = "DELETE FROM Usuario WHERE Id = @idUsuario";
                
                SqlParameter parametro = new SqlParameter();
                parametro.ParameterName = "idUsuario";
                parametro.SqlDbType = System.Data.SqlDbType.BigInt; // por que debe coincidir con el tipo de dato de la Tabla
                parametro.Value = usuario.id;
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametro);
                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                }
                sqlConnection.Close();

            }

        }
        catch (Exception ex) // si algo falla controlamos la excepcion 
        {
            Console.WriteLine(ex.Message);

        }
    }

    public void UpdateContraseña(Usuario usuario)
    {
        try
        {

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {

                string queryUpdate = "UPDATE Usuario SET Contraseña = @nuevaContraseña WHERE Id = @idUsuario";
                string nuevaContraseña = "1234";
               

                SqlParameter parametroId = new SqlParameter();
                parametroId.ParameterName = "idUsuario";
                parametroId.SqlDbType = System.Data.SqlDbType.BigInt; // por que debe coincidir con el tipo de dato de la Tabla
                parametroId.Value = nuevaContraseña;

                SqlParameter parametroPass = new SqlParameter();
                parametroPass.ParameterName = "nuevaContraseña";
                parametroPass.SqlDbType = System.Data.SqlDbType.VarChar; // por que debe coincidir con el tipo de dato de la Tabla
                parametroPass.Value = usuario.id;
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(parametroId);
                    sqlCommand.Parameters.Add(parametroPass);

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                }
                sqlConnection.Close();
                
            }

        }
        catch (Exception ex) // si algo falla controlamos la excepcion 
        {
            Console.WriteLine(ex.Message);

        }
    }

    public void Insert(Usuario usuario)
    {
        try
        {

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {

                string queryInsert = "INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Constraseña, Mail) VALUES " +
                    "(@Nombre, @Apellido, @NombreUsuario, @Constraseña, @Mail);";


                SqlParameter sqlParameterNombre = new SqlParameter("Nombre", SqlDbType.VarChar) { Value = usuario.nombre };
                SqlParameter sqlParameterApellido = new SqlParameter("Apellido", SqlDbType.VarChar) { Value = usuario.apellido };
                SqlParameter sqlParameterNombreUsuario = new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter sqlParameterContraseña = new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña };
                SqlParameter sqlParameterMail = new SqlParameter("Mail", SqlDbType.Int) { Value =  usuario.Mail };


                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {


                    sqlCommand.Parameters.Add(sqlParameterNombre);
                    sqlCommand.Parameters.Add(sqlParameterApellido);
                    sqlCommand.Parameters.Add(sqlParameterNombreUsuario);
                    sqlCommand.Parameters.Add(sqlParameterContraseña);
                    sqlCommand.Parameters.Add(sqlParameterMail);
                    sqlCommand.ExecuteNonQuery(); //eejcuta el insert

                }

                sqlConnection.Close();
            }

        }
        catch (Exception ex) // si algo falla controlamos la excepcion 
        {
            Console.WriteLine(ex.Message);

        }
    }


}


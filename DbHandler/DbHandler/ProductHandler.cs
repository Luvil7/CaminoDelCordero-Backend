using EjemploClase;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;


public class Producto
{

    public int id;
    // definimos setters y getters, es una manera que se puede usar en C#
    public int Codigo
    {
        // creamos una tributo publico que setea y te da el valor tambien del metodo privado sin ceder el control de ese metodo
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }
    public string descripcion;
    public double precioCompra;
    public  double precioVenta;
    public int Stock;
    public int idUsuario;
    // Tenemos el constructor por defecto sin aprametros del C# o el constructor definido por nosotros.
    public Producto() // constructor por defecto que se llama cuando instanciamos un Producto.
    {
        // le podemos dar valores por defecto
        id = 0;
        descripcion = string.Empty;
        precioCompra = 0;
        precioVenta = 0;
        Stock = 0;
        idUsuario = 0; // ya le damos el valor
    }

    //Constructor parametrizado para definir los valores que queremos que tenga a la hora de inicializar
    public Producto(int codigo, string descripcion, double precioCompra, double precioVenta, int stock, int idUsuario)
    {
        // this para acceder al atributo
        this.id = codigo;
        this.descripcion = descripcion;
        this.precioCompra = precioCompra;
        this.precioVenta = precioVenta;
        this.Stock = stock;
        this.idUsuario = idUsuario;
    }

    // geters y setters, sirve para la encapsulamiento para que los atributos sean privados y no puedan ser modificados
    public string GetDescripcion()
    {
        return descripcion;
    }

    public bool HayPrecioDeVenta()
    {
        return precioVenta > 0;
    }
}



public class ProductHandler : DbHandler
{

    public List<string> GetDescripcionesReader()

    {
        // esto es para que se libere el objeto SqlConnection
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            // command toma dos cosas, la query y el sqlConnection
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Producto", sqlConnection))
            {


                int idProducto = 3;
                sqlConnection.Open();
                // creamos el parametro que le vamos a pasar
                SqlParameter parametro = new SqlParameter();

                parametro.ParameterName = "IdProducto";
                parametro.SqlDbType = System.Data.SqlDbType.Int;
                parametro.Value = idProducto;

                sqlCommand.Parameters.Add(parametro);
                List<string> descripciones = new List<string>();

                //ejemplo de datareader
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    // para estar seguro que ahy filas o registros para leer
                    if (dataReader.HasRows)
                    {
                        // mientras haya para leer
                        while (dataReader.Read())
                        {
                            // me da el value de una fila en especifico y de la columna indicada por parametro
                            descripciones.Add(dataReader.GetString(1));

                        }

                    }

                }

                sqlConnection.Close();
                return descripciones;
            }
        }
    }


    public DataSet GetDescripcionesAdapter()

    {
        // using, esto es para que se libere el objeto SqlConnection
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Producto", sqlConnection);


            sqlConnection.Open();

            DataSet resultado = new DataSet();

            adapter.Fill(resultado); // aca se llama a la base de datos, con  fill nos lleva los resultados que devuelve la query en la variable

            sqlConnection.Close();

            return resultado;
        }
    }

    public List<Producto> GetProdcutos()

    {
        // esto es para que se libere el objeto SqlConnection
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            // command toma dos cosas, la query y el sqlConnection
            using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Producto", sqlConnection))
            {


                int idProducto = 3;
                sqlConnection.Open();
                // creamos el parametro que le vamos a pasar
                SqlParameter parametro = new SqlParameter();

                parametro.ParameterName = "IdProducto";
                parametro.SqlDbType = System.Data.SqlDbType.Int;
                parametro.Value = idProducto;

                sqlCommand.Parameters.Add(parametro);
                List<Producto> productos = new List<Producto>();

                //ejemplo de datareader
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    // para estar seguro que ahy filas o registros para leer
                    if (dataReader.HasRows)
                    {
                        // mientras haya para leer
                        while (dataReader.Read())
                        {
                            Producto producto = new Producto();
                            producto.id = Convert.ToInt32(dataReader["ID"]); // me traigo solo los id de los productos

                            productos.Add(producto);

                        }

                    }

                }

                sqlConnection.Close();
                return productos;
            }
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString)) 
        {
            string queryDelete = "DELETE FROM Producto WHERE Id = @idProdcuto";

            SqlParameter sqlParameter = new SqlParameter("id", SqlDbType.BigInt);
            sqlParameter.Value = id;

            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
            { 
                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.ExecuteScalar(); // ejecuto delete
            
            }
            sqlConnection.Close();
        
        }



    }

    public void Add(Producto producto)
    {
        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            string queryInsert = "INSERT INTO Producto " +
                "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario)" +
                "VALUES (@Descripciones, @Costo, @PrecioVenta, @Stock, @IdUsuario)";

            SqlParameter sqlParameterDescripcion = new SqlParameter("Descripciones", SqlDbType.VarChar) {Value = producto.descripcion};
            SqlParameter sqlParameterCosto = new SqlParameter("Costo", SqlDbType.Int) {Value = producto.precioCompra};
            SqlParameter sqlParameterVenta = new SqlParameter("Venta", SqlDbType.Int) {Value = producto.precioVenta};
            SqlParameter sqlParameterStock = new SqlParameter("Stock", SqlDbType.Int) {Value = producto.Stock};
            SqlParameter sqlParameterIdUsuario = new SqlParameter("IdUsuario", SqlDbType.Int) {Value = producto.idUsuario};



            sqlConnection.Open();

            using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
            {
                sqlCommand.Parameters.Add(sqlParameterDescripcion);
                sqlCommand.Parameters.Add(sqlParameterCosto);
                sqlCommand.Parameters.Add (sqlParameterVenta);
                sqlCommand.Parameters.Add(sqlParameterStock);
                sqlCommand.Parameters.Add(sqlParameterIdUsuario);
                sqlCommand.ExecuteNonQuery(); //eejcuta el insert


            }
            sqlConnection.Close();

        }



    }


}
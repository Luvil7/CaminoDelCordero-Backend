namespace MiPrimeraApi.Handler
{
    using ApiClass.Models;
    using System.Data;
    using System.Data.SqlClient;

    namespace MiPrimeraApi.Repository
    {
        public static class ProductoHandler
        {

            public const string ConnectionString =
            "Server=DESKTOP-LUCAS1\\SQLEXPRESS;Database=SistemaGestion;Trusted_Connection=True";
            public static List<Producto> GetProductos()
            {
                List<Producto> resultado = new List<Producto>();
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = "SELECT * FROM Producto;";
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = sqlCommand;
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        sqlConnection.Close();
                        foreach (DataRow row in table.Rows)
                        {
                            Producto producto = new Producto();
                            producto.Id = Convert.ToInt32(row["Id"]);
                            producto.PrecioVenta = Convert.ToInt32(row["PrecioVenta"]);
                            producto.Costo = Convert.ToInt32(row["Costo"]);
                            producto.Stock = Convert.ToInt32(row["Stock"]);
                            producto.Descripciones = row["Descripciones"].ToString();
                            producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);
                            resultado.Add(producto);
                        }
                    }
                    return resultado;
                }
            }
        }
    }
}

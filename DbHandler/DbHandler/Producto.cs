using System.Data;

namespace Ejemplo 
{
    public class ProbarObjetos
    {
        static void Main(string[] args) 
        {
            ProductHandler productHandler = new ProductHandler();
            List<string> list = productHandler.GetDescripcionesReader();

            foreach (string item in list)
            {
                Console.WriteLine(item);
            }


            DataSet ds = productHandler.GetDescripcionesAdapter();
            // para printear el dataset
            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine("Tabla: " + dt.TableName);
                Console.WriteLine("---------------");

                foreach (DataColumn column in dt.Columns)
                {
                    Console.Write(column.ColumnName + "\t");
                }
                Console.WriteLine();

                
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write(item + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }


            List<Producto> productos =  productHandler.GetProdcutos();
            foreach (Producto item in productos)
            {
                Console.WriteLine(item.id);
        

            }

            UsuarioHandler usuarioHandler = new UsuarioHandler();
            List<Usuario> usuarios = usuarioHandler.GetUsuarios();
            foreach (Usuario item in usuarios)
            {
                Console.WriteLine(item.id);
                Console.WriteLine(item.nombre);
                Console.WriteLine(item.apellido);




            }
        }




        
        
    }
}
/* COMO se declara una clase
// por ejemplo public class Producto

// Modificadores de acceso, private, public, protected(solo accede la misma clase o hijas), internal(solo las clases del mismo proyecto tiene acceso)
// C# tiene un garbage collector que se hace cargo de llamar al destructor y liberar los recursos ocupados por la clase.

namespace EjemploClase // para llamar a todas las clases dentro de un namespace, es para agrupar clases que tiene que ver con lo mismo
{

    class ProbarObjetos // tengo que crear una clase para indicar donde esta el main y ahi llamar a mi codigo
    {
        static void Main(string[] args) // creo el amin como Java
        {
            Producto productoPorDefect = new Producto();
            
            Producto productoParam = new Producto(2, "Detergente ala", 2.37, 4.37, "Limpieza", false);

            Console.WriteLine(productoPorDefect);
            Console.WriteLine(productoParam);

            Console.WriteLine(productoPorDefect.GetDescripcion());


            if (productoPorDefect.HayPrecioDeVenta())
            {
                Console.WriteLine("hay precio de venta");
            }
            else { 
                Console.WriteLine("NO hay precio de venta");

            }

            Console.WriteLine(productoPorDefect.Codigo); // atraves de codigo modifico y obtengo el codigo
            productoPorDefect.Codigo = 1234;
            Console.WriteLine(productoPorDefect.Codigo);


        }
    }

    public class Producto
    {

        private int codigo;
        // definimos setters y getters, es una manera que se puede usar en C#
        public int Codigo
        {   
            // creamos una tributo publico que setea y te da el valor tambien del metodo privado sin ceder el control de ese metodo
            get {
                return codigo;
            }
            set {
                codigo= value;            
            }
        }
        private string descripcion;
        private double precioCompra;
        private double precioVenta;
        private string categoria;
        private bool estaVencido;

        // Tenemos el constructor por defecto sin aprametros del C# o el constructor definido por nosotros.
        public Producto() // constructor por defecto que se llama cuando instanciamos un Producto.
        {
            // le podemos dar valores por defecto
            codigo = 0;
            descripcion = string.Empty;
            precioCompra = 0;
            precioVenta = 0;
            categoria = string.Empty;
            estaVencido = false; // ya le damos el valor
        }

        //Constructor parametrizado para definir los valores que queremos que tenga a la hora de inicializar
        public Producto(int codigo, string descripcion, double precioCompra, double precioVenta, string categoria, bool estaVencido)
        {
            // this para acceder al atributo
            this.codigo = codigo;
            this.descripcion = descripcion;
            this.precioCompra = precioCompra;
            this.precioVenta = precioVenta;
            this.categoria = categoria;
            this.estaVencido = estaVencido;
        }

        // geters y setters, sirve para la encapsulamiento para que los atributos sean privados y no puedan ser modificados
        public string GetDescripcion()
        {
            return descripcion;
        }

        public bool HayPrecioDeVenta() { 
            return precioVenta > 0;
        }
    }
}

*/

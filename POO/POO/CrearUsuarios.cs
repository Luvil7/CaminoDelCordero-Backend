//EJERCICIO DE LA CLASE
namespace CrearUsuarios {
    public class Usuario
    {
        private string nombre;
        private string apellido;
        private int dni;
        private string mail;
        private int edad;
        private string domicilio;

        public Usuario() { 
            nombre = string.Empty;
            apellido = string.Empty;
            dni = 0;
            mail = string.Empty;
            edad = 0;
            domicilio = string.Empty;  
        }
        public Usuario(string nombre, string apellido, int dni, string mail, int edad, string domicilio)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
            this.mail = mail;
            this.edad = edad;
            this.domicilio = domicilio;
        }

        public bool EsMayorDeEdad() {
            return edad >= 18;
        
        }

        public void CambiarDireccion(string nuevoDomicilio) { 
            domicilio = nuevoDomicilio;
        }

        public bool TieneMail () {
            return mail != null;
        }

        public string GetNombre() { 
            return nombre;
        }

        public int GetDni() { 
            return dni;
        }


    }


    public class CrearUsuarios
    {
        static void Main(string[] args)
        {
            Usuario usuario1 = new Usuario("Lucas", "Vilas", 43667843, "lucas@gmail", 23, "lanus");

            Console.WriteLine(usuario1.GetNombre());

            if (usuario1.EsMayorDeEdad())
            {
                Console.WriteLine("Es mayor de edad");
            }
            else
            {
                Console.WriteLine("Es menor");
            }

            Console.WriteLine("DNI:" + usuario1.GetDni());

        }

    }

}
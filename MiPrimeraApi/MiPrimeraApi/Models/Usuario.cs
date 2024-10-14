namespace ApiClass.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }
        public Usuario()
        {
            Id = 0;
            Nombre = String.Empty;
            Apellido = String.Empty;
            Contraseña = String.Empty;
            NombreUsuario = String.Empty;
            Mail = String.Empty;
        }
        public Usuario(int id, string nombre, string apellido, string contraseña, string nombreUsuario, string mail)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Contraseña = contraseña;
            NombreUsuario = nombreUsuario;
            Mail = mail;
        }
    }
}
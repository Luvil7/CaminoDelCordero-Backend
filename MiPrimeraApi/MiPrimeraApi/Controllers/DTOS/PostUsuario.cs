namespace MiPrimeraApi.Controllers.DTOS
{
    // CREO EL DTO POST USUARIO PARA PODER CREAR AHORA UN USUARIO
    public class PostUsuario
    {
        // aca van los atributos que se van a LLENAR con el nuevo usuario

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }

    }
}
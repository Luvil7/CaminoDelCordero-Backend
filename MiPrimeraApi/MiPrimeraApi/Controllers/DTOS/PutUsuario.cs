namespace MiPrimeraApi.Controllers.DTOS
{
    //ESTO VA DENTRO DE LA CARPETA DTOS
    // ESTO SIRVE PARA DATA QUE SE TRANSFIERE DEL OBJETO, ES PARA LLEVAR INFO A OTR LUGAR DEL SISTEMA
    // Y FIN. PARA PUT Y POST PEDIMOS UN DTO ESPECIFICO PARA ESE OBJETO
    public class PutUsuario
    {
        // aca van los atributos que se van a modificar con el put
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
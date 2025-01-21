namespace ProyectoFinal.Controllers.DTOS
{
    public class PutLicencia
    {
        public int Id { get; set; }
        public int SoldadoDni { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Tipo { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Dir { get; set; }
        public string OD { get; set; }
    }
}
namespace ProyectoFinal.Models
{
    public class Licencia
    {
        public int Id { get; set; }
        public int SoldadoDni { get; set; }  // FK 
        public Soldado Soldado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Tipo { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Dir { get; set; }
        public string OD { get; set; }

    }
}

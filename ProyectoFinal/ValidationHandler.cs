using ProyectoFinal.Models;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Repository
{
    public class ValidationHandler : Exception
    {
        public ValidationHandler(string message) : base(message) { }
    }

    public static class LicenciaValidationHandler
    {
        public static void ValidateLicencia(Licencia licencia)
        {
            var missingFields = new List<string>();

            if (string.IsNullOrEmpty(licencia.FechaInicio)) missingFields.Add("Fecha de Inicio");
            if (string.IsNullOrEmpty(licencia.FechaFin)) missingFields.Add("Fecha de Fin");
            if (string.IsNullOrEmpty(licencia.Tipo)) missingFields.Add("Tipo");
            if (string.IsNullOrEmpty(licencia.Provincia)) missingFields.Add("Provincia");
            if (string.IsNullOrEmpty(licencia.Localidad)) missingFields.Add("Localidad");
            if (string.IsNullOrEmpty(licencia.Dir)) missingFields.Add("Dirección");
            if (string.IsNullOrEmpty(licencia.OD)) missingFields.Add("OD");
            if (licencia.SoldadoDni <= 0) missingFields.Add("ID del Soldado");

            if (missingFields.Any())
            {
                throw new ValidationException($"Faltan los siguientes campos obligatorios: {string.Join(", ", missingFields)}");
            }

            if (licencia.OD.Length < 6 || licencia.OD.Length > 10)
            {
                throw new ValidationException("La Orden del Día (OD) debe tener entre 6 y 10 caracteres");
            }

            if (!DateTime.TryParse(licencia.FechaInicio, out _))
            {
                throw new ValidationException("El formato de la Fecha de Inicio no es válido");
            }

            if (!DateTime.TryParse(licencia.FechaFin, out _))
            {
                throw new ValidationException("El formato de la Fecha de Fin no es válido");
            }
            string dniStr = licencia.SoldadoDni.ToString();
            if (dniStr.Length < 8 || dniStr.Length > 9)
            {
                throw new ValidationException("El DNI debe tener entre 8 y 9 caracteres");
            }
        }

    }
}

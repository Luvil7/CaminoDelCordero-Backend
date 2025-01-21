using ProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;
using ProyectoFinal.Repository;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenciaController : ControllerBase
    {
        [HttpGet(Name = "GetLicencias")]
        public List<Licencia> GetLicencias()
        {
            try
            {
                return LicenciaHandler.GetLicencias();
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                return new List<Licencia>();

            }

        }


        [HttpDelete(Name = "DeleteLicencia")]
        public ActionResult<string> DeleteLicencia([FromBody] int id)
        {
            try
            {
                string resultado = LicenciaHandler.DeleteLicencia(id);
                return Ok(resultado);  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("No se pudo eliminar la licencia debido a un error en el servidor");  
            }
        }


        [HttpPut]
        public ActionResult<string> ModifyLicencia([FromBody] PutLicencia licencia)
        {
            try
            {
                var licenciaToUpdate = new Licencia
                {
                    Id = licencia.Id,
                    SoldadoDni = licencia.SoldadoDni,
                    FechaInicio = licencia.FechaInicio,
                    FechaFin = licencia.FechaFin,
                    Tipo = licencia.Tipo,
                    Provincia = licencia.Provincia,
                    Localidad = licencia.Localidad,
                    Dir = licencia.Dir,
                    OD = licencia.OD
                };

                string resultado = LicenciaHandler.UpdateLicencia(licenciaToUpdate);

                if (resultado.StartsWith("Error de validación"))
                {
                    return BadRequest(resultado);  
                }
                if (resultado.StartsWith("La licencia"))
                {
                    return Ok(resultado);  // 200 OK con mensaje de éxito
                }

                return NotFound(resultado);  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");  
            }
        }
        

        [HttpPost]
        public ActionResult<string> AddLicencia([FromBody] PostLicencia licencia)
        {
            try
            {
                return LicenciaHandler.AddLicencia(new Licencia
                {
                    SoldadoDni=licencia.SoldadoDni,
                    FechaFin=licencia.FechaFin,
                    FechaInicio=licencia.FechaInicio,
                    Tipo=licencia.Tipo,
                    Provincia=licencia.Provincia,
                    Localidad=licencia.Localidad,
                    Dir = licencia.Dir,
                    OD=licencia.OD,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        } 

    }
}

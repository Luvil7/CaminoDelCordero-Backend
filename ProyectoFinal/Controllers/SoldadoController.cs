using ProyectoFinal.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Controllers.DTOS;
using ProyectoFinal.Repository;



namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoldadoController : Controller
    {

        [HttpPut]
        public ActionResult<string> ModifySoldado([FromBody] PutSoldado soldado)
        {
            try
            {
                var soldadoToUpdate = new Soldado
                {
                    Id = soldado.Id,
                    Dni = soldado.Dni,
                    
                };

                string resultado = SoldadoHandler.UpdateSoldado(soldadoToUpdate);

                if (resultado.StartsWith("Error de validación"))
                {
                    return BadRequest(resultado);
                }
                if (resultado.StartsWith("El soldado"))
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
                    SoldadoDni = licencia.SoldadoDni,
                    FechaFin = licencia.FechaFin,
                    FechaInicio = licencia.FechaInicio,
                    Tipo = licencia.Tipo,
                    Provincia = licencia.Provincia,
                    Localidad = licencia.Localidad,
                    Dir = licencia.Dir,
                    OD = licencia.OD,
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

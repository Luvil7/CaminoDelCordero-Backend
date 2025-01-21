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

        [HttpGet(Name = "GetSoldados")]
        public List<Soldado> GetSoldados()
        {
            try
            {
                return SoldadoHandler.GetSoldados();
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                return new List<Soldado>();

            }

        }


        [HttpDelete(Name = "DeleteSoldado")]
        public ActionResult<string> DeleteSoldado([FromBody] int dni)
        {
            try
            {
                string resultado = SoldadoHandler.DeleteSoldado(dni);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("No se pudo eliminar la licencia debido a un error en el servidor");
            }
        }

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
        public ActionResult<string> AddSoldado([FromBody] PostSoldado soldado)
        {
            try
            {
                return SoldadoHandler.AddSoldado(new Soldado
                {
                    Dni = soldado.Dni,
                    Nombre = soldado.Nombre,
                    Apellido = soldado.Apellido,
                    
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

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
        /*
         GetLicencias
            Devuelve el listado de las licencias disponibles
            La respuesta obtenida sera:
       [
         {
        "id": 1,
        "soldadoDni": 47667843,
        "soldado": {
            "id": 1,
            "dni": 47667843,
            "nombre": "Juan",
            "apellido": "Perez"
        },
        "fechaInicio": "2025-01-15",
        "fechaFin": "2025-01-30",
        "tipo": "Ordinaria",
        "provincia": "Tucuman",
        "localidad": "San Miguel",
        "dir": "Catamarca 123",
        "od": "OD250001"
         },
        {
        "id": 2,
        "soldadoDni": 40123789,
        "soldado": {
            "id": 2,
            "dni": 40123789,
            "nombre": "Carlos",
            "apellido": "González"
        },
        "fechaInicio": "2025-02-01",
        "fechaFin": "2025-02-15",
        "tipo": "Ordinaria",
        "provincia": "Mendoza",
        "localidad": "Ciudad",
        "dir": "Av. Las Heras 789",
        "od": "OD250002"
         }
       ]
          
       */

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
        /*
         DeleteLicencia
            Elimina la licencia segun el id que se le indique
            Si se encuentra la Licencia el resultado es:
                
                La licencia con ID: 2 se eliminó correctamente
         
            Si el id de la licencia que se quiere eliminar no se encuentra o no existe el resutado es:

                No se encontró ninguna licencia con ID: 2
         */

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

        /*
         * ModifyLicencia
         * Put/Licencia
         * Modifica la licencia que se le envia alterando los campos que se hayan modificado en el input
         * Por ejemplo se envia como input

            "id": 2,
        "soldadoDni": 43667843,
        "fechaInicio": "2025-03-01",
        "fechaFin": "2025-04-15",
        "tipo": "Ordinaria",
        "provincia": "Mendoza",
        "localidad": "Ciudad",
        "dir": "Av. Las Heras 789",
        "od": "OD250002"
         }
        
        Para la modificacion del DNI se hace a traves del endpoint del soldado, modeficando el dni que se necesita en la base de datos de soldados.
        Luego se cambia el dni tambien en la licencia.

        La licencia con id 2 modifica los campos segun como esta el input

        Si pudo modificarse correctamente el resultado es:
            La licencia se actualizó correctamente

        Si no se encontro una licencia con ese id el reultado es:
            No se encontró la licencia con el ID especificado

        Si no se encuentra una licencia con el dni indicado el resultado es:
            Error de validación: No existe un soldado con el DNI xxxxxxxx

        Si uno de los campos no esta completo, se notifica el campo faltante, por ejemplo:
            {
                "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                "title": "One or more validation errors occurred.",
                "status": 400,
                "errors": {
                "Dir": [
                    "The Dir field is required."
                ]
            },
            }

        Si alguno de los campos no cumple con alguna restriccion como DNI o la OD se indica el error.

         */

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

        /*
         * POST/Licencia
         * 
         * Se agrega a la base la licencia enviada en el input, haciendo las validaciones necesarias
         * para chequear que no haya un id o dni repetido. El soldado cuyo dni se quiere agregar en la licencia
         * debe existir en la base de datos.
         * 
         * Un input por ejemplo:
         * {   
                "id": 2,
                "soldadoDni": 43667843,
                "fechaInicio": "2025-04-01",
                "fechaFin": "2025-05-15",
                "tipo": "Extraordinaria",
                "provincia": "Cordoba",
                "localidad": "Ciudad America",
                "dir": "Cochabamba 123",
                "od": "OD202567"
           }
         * Si la licencia se pudo agregar correctamente el resultado es:
         *  La licencia se agregó correctamente
         * 
         * Si el dni que se indica en la Licencia es de un soldado que no existe en la base se muestra:
         *  Error de validación: No existe un soldado con el DNI 43667843
         *  
         *  Si la OD no cumple los requisitos:
         *   Error de validación: La Orden del Día (OD) debe tener entre 6 y 10 caracteres
         *   
         *  Si el Dni no cumple los requisistos:
         *   Error de validación: El DNI debe tener entre 8 y 9 caracteres
         *   
         *   Si hay algun campo faltante se indica por ejemplo:
         *      {
                "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                "title": "One or more validation errors occurred.",
                "status": 400,
                "errors": {
                "Dir": [
                    "The Dir field is required."
                ]
            },
            }
         *  
         */

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


using ApiClass.Models;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Controllers.DTOS;
using MiPrimeraApi.Repository;
using System.Data.SqlClient;


namespace MiPrimeraApi.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class UsuarioController : ControllerBase
        {
            [HttpGet(Name = "GetUsuarios")]
            public List<Usuario> GetUsuarios()
            {
               return UsuarioHandler.GetUsuarios();
            }
            [HttpDelete(Name = "BorrarUsuarios")] // con esto le decimos al endpoint que es un delete
            public bool BorrarUsuario([FromBody] int id)
            {
                try
                {
                    return UsuarioHandler.EliminarUsuario(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            [HttpPut]
            // mando por parametro el dto put usuario que se usa solo para el put
            public bool ModificarUsuario([FromBody] PutUsuario usuario)
            {
                try
                {
                    return UsuarioHandler.UpdateNombre(new Usuario
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            [HttpPost]
            // aca ya no puedo usar PutUsuario tengo que usar Post
            public bool CrearUsuario([FromBody] PostUsuario usuario)
            {
                try
                {
                    return UsuarioHandler.CrearUsuario(new Usuario
                    {
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        NombreUsuario = usuario.NombreUsuario,
                        Contraseña = usuario.Contraseña,
                        Mail = usuario.Mail,
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }
}


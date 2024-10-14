using ApiClass.Models;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi.Controllers.DTOS;
using MiPrimeraApi.Handler.MiPrimeraApi.Repository;
using MiPrimeraApi.Repository;
using System.Data.SqlClient;


namespace MiPrimeraApi.Controllers
{

        [ApiController]
        [Route("[controller]")]
        public class ProductoController : ControllerBase
        {
            [HttpGet(Name = "GetProductos")]
            public List<Producto> GetProductos()
            {
                return ProductoHandler.GetProductos();
            }
        }
 }

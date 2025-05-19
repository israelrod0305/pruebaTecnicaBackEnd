using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService;
using PruebaTecnica.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class ProductoController : PruebaTecnicaControllerBase
    {
        private readonly IProductoAppService _productoService;


        public ProductoController(IProductoAppService productoService)
        {
            _productoService = productoService;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<ResponseModel<string>>> Crear([FromBody] ProductoDto producto)
        {


            try
            {
                var resultado = await _productoService.Crear(producto, _connectionString);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Codigo = 500,
                    Mensaje = "Error interno del servidor.",
                    Data = null,
                    tabla = ex.Message
                });
            }
        }

        [HttpPut("actualizar")]
        public async Task<ActionResult<ResponseModel<string>>> Actualizar([FromBody] ProductoDto producto)
        {

            try
            {
                var resultado = await _productoService.Actualizar(producto, _connectionString);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Codigo = 500,
                    Mensaje = "Error interno del servidor.",
                    Data = null,
                    tabla = ex.Message
                });
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult<ResponseModel<string>>> Eliminar(int id, [FromQuery] string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Codigo = 400,
                    Mensaje = "El campo 'usuario' es obligatorio.",
                    Data = null
                });
            }

            try
            {
                var resultado = await _productoService.Eliminar(id, usuario, _connectionString);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Codigo = 500,
                    Mensaje = "Error interno del servidor.",
                    Data = null,
                    tabla = ex.Message
                });
            }
        }

        [HttpGet("consultar")]
        public async Task<ActionResult<ResponseModel<List<ProductoDto>>>> Consultar(int? id = null, string codigo = null, string nombre = null)
        {
            try
            {
                var resultado = await _productoService.Consultar(_connectionString, id, codigo, nombre);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<List<ProductoDto>>
                {
                    Codigo = 500,
                    Mensaje = "Error interno del servidor.",
                    Data = null,
                    tabla = ex.Message
                });
            }
        }
    }
}

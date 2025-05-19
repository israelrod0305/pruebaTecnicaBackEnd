using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Core;
using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaTecnica.Web.Controllers.Inventario
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class LoteController : PruebaTecnicaControllerBase
    {
        private readonly ILoteAppService _loteAppService;

        public LoteController(ILoteAppService loteAppService)
        {
            _loteAppService = loteAppService;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<ResponseModel<string>>> Crear([FromBody] LoteDto lote)
        {


            try
            {
                var resultado = await _loteAppService.Crear(lote, _connectionString);
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
        public async Task<ActionResult<ResponseModel<string>>> Actualizar([FromBody] LoteDto lote)
        {

            try
            {
                var result = await _loteAppService.Actualizar(lote, _connectionString);
                return Ok(result);
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
                var result = await _loteAppService.Eliminar(id, usuario ?? "sistema", _connectionString);
                return Ok(result);
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
        public async Task<ActionResult<ResponseModel<List<ProductoDto>>>> Consultar(int productoId, int? id = null, string codigoLote = null)
        {
            try
            {
                var result = await _loteAppService.Consultar(_connectionString, id, productoId, codigoLote);

                return Ok(result);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class UnidadMedidaController : PruebaTecnicaControllerBase
    {
        private readonly IUnidadMedidaAppService _iunidadMedidaService;


        public UnidadMedidaController(IUnidadMedidaAppService iunidadMedidaService)
        {
            _iunidadMedidaService = iunidadMedidaService;
        }
        [HttpGet("consultar")]
        public async Task<ActionResult<ResponseModel<List<ProductoDto>>>> Consultar(int? id = null, string codigo = null, string nombre = null, string descripcion = null)
        {
            try
            {
                var resultado = await _iunidadMedidaService.Consultar(_connectionString, id, codigo, nombre,descripcion);
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

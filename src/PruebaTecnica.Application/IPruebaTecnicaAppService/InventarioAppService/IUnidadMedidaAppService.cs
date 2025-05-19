using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService
{
    public interface IUnidadMedidaAppService
    {
        Task<ResponseModel<List<UnidadMedidaDto>>> Consultar(string connectionString, int? id = null, string codigo = null, string nombre = null, string descripcion = null);
    }
}

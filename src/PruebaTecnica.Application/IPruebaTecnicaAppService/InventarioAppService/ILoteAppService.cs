using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService
{
    public interface ILoteAppService
    {
        Task<ResponseModel<string>> Crear(LoteDto input, string connectionString);
        Task<ResponseModel<string>> Actualizar(LoteDto input, string connectionString);
        Task<ResponseModel<string>> Eliminar(int loteId, string usuario, string connectionString);
        Task<ResponseModel<List<LoteDto>>> Consultar(string connectionString, int? id = null, int? productoId = null, string codigoLote = null);
    }

}

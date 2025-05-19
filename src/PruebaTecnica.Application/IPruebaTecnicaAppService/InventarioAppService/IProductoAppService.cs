using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService
{
    public interface IProductoAppService
    {
        Task<ResponseModel<string>> Crear(ProductoDto producto, string connectionString);
        Task<ResponseModel<string>> Actualizar(ProductoDto producto, string connectionString);
        Task<ResponseModel<string>> Eliminar(int id, string usuario, string connectionString);
        Task<ResponseModel<List<ProductoDto>>> Consultar(string connectionString, int? id = null, string codigo = null, string nombre = null);
    }
}

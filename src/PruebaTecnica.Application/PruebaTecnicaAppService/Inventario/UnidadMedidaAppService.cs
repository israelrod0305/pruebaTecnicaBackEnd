using Microsoft.Data.SqlClient;
using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.PruebaTecnicaAppService.Inventario
{
    public class UnidadMedidaAppService : PruebaTecnicaAppServiceBase, IUnidadMedidaAppService
    {
        private SqlCommand CrearComando(SqlConnection conn, UnidadMedidaDto unidad, string accion)
        {
            SqlCommand cmd = new SqlCommand("sp_UnidadMedida", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UnidadMedidaID", unidad.id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Codigo", unidad.Codigo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Nombre", unidad.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", unidad.Descripcion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Accion", accion);

            return cmd;
        }

        public async Task<ResponseModel<List<UnidadMedidaDto>>> Consultar(string connectionString, int? id = null, string codigo = null, string nombre = null, string descripcion = null)
        {
            var response = new ResponseModel<List<UnidadMedidaDto>>();
            var UnidadMedida = new List<UnidadMedidaDto>();

            var UnidadMedidaDto = new UnidadMedidaDto
            {
                id = id,
                Codigo = codigo,
                Nombre = nombre,
                Descripcion = descripcion
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, UnidadMedidaDto, "SEL"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            UnidadMedida.Add(new UnidadMedidaDto
                            {
                                id = Convert.ToInt32(reader["id"]),
                                Codigo = reader["Codigo"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                            });
                        }
                    }
                }

                response.Codigo = 1;
                response.Mensaje = "Consulta exitosa";
                response.Data = UnidadMedida;
                response.tabla = UnidadMedida;
            }
            catch (Exception ex)
            {
                response.Codigo = -1;
                response.Mensaje = "Error: " + ex.Message;
            }

            return response;
        }

    }
}

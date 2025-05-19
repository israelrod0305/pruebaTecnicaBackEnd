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
    public class LoteAppService : PruebaTecnicaAppServiceBase, ILoteAppService
    {
        private SqlCommand CrearComando(SqlConnection conn, LoteDto lote, string accion)
        {
            SqlCommand cmd = new SqlCommand("sp_Lote", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@LoteID", lote.LoteID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@NumeroLote", lote.CodigoLote ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductoID", lote.ProductoID);
            cmd.Parameters.AddWithValue("@CantidadInicial", lote.CantidadInicial);
            cmd.Parameters.AddWithValue("@FechaFabricacion", lote.FechaFabricacion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaVencimiento", lote.FechaVencimiento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Usuario", lote.Usuario ?? "sistema");
            cmd.Parameters.AddWithValue("@Estado", lote.Estado);
            cmd.Parameters.AddWithValue("@Accion", accion);

            return cmd;
        }

        public async Task<ResponseModel<string>> Crear(LoteDto lote, string connectionString)
        {
            var response = new ResponseModel<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, lote, "INS"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var resultado = reader["Resultado"].ToString();
                            var mensaje = reader["Mensaje"].ToString();

                            response.Codigo = resultado == "OK" ? 1 : 0;
                            response.Mensaje = mensaje;
                            response.Data = resultado == "OK" ? "Lote creado correctamente" : null;
                        }
                        else
                        {
                            response.Codigo = 0;
                            response.Mensaje = "No se recibió respuesta del SP";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Codigo = -1;
                response.Mensaje = "Error: " + ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel<string>> Actualizar(LoteDto lote, string connectionString)
        {
            var response = new ResponseModel<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, lote, "UP"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var resultado = reader["Resultado"].ToString();
                            var mensaje = reader["Mensaje"].ToString();

                            response.Codigo = resultado == "OK" ? 1 : 0;
                            response.Mensaje = mensaje;
                            response.Data = resultado == "OK" ? "Lote actualizado correctamente" : null;
                        }
                        else
                        {
                            response.Codigo = 0;
                            response.Mensaje = "No se recibió respuesta del SP";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Codigo = -1;
                response.Mensaje = "Error: " + ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel<string>> Eliminar(int id, string usuario, string connectionString)
        {
            var response = new ResponseModel<string>();

            var lote = new LoteDto
            {
                LoteID = id,
                CodigoLote = null,
                ProductoID = 0,
                Cantidad = 0,
                FechaVencimiento = null,
                Usuario = usuario,
                Estado = 0
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, lote, "DEL"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var resultado = reader["Resultado"].ToString();
                            var mensaje = reader["Mensaje"].ToString();

                            response.Codigo = resultado == "OK" ? 1 : 0;
                            response.Mensaje = mensaje;
                            response.Data = resultado == "OK" ? "Lote eliminado correctamente" : null;
                        }
                        else
                        {
                            response.Codigo = 0;
                            response.Mensaje = "No se recibió respuesta del SP";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Codigo = -1;
                response.Mensaje = "Error: " + ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel<List<LoteDto>>> Consultar(string connectionString, int? id = null, int? productoId = null, string codigoLote = null)
        {
            var response = new ResponseModel<List<LoteDto>>();
            var lotes = new List<LoteDto>();

            var lote = new LoteDto
            {
                LoteID = id,
                CodigoLote = codigoLote,
                ProductoID = productoId ?? 0,
                Cantidad = 0,
                FechaVencimiento = null,
                Usuario = "sistema",
                Estado = 1
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, lote, "SEL"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lotes.Add(new LoteDto
                            {
                                LoteID = Convert.ToInt32(reader["ID"]),
                                CodigoLote = reader["NumeroLote"].ToString(),
                                ProductoID = Convert.ToInt32(reader["ProductoId"].ToString()),
                                Producto = reader["NombreProducto"].ToString(),
                                Cantidad = Convert.ToInt32(reader["CantidadDisponible"]),
                                CantidadInicial = Convert.ToInt32(reader["CantidadInicial"]),
                                FechaFabricacion = reader["FechaFabricacion"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["FechaFabricacion"])
                                    : (DateTime?)null,
                                FechaVencimiento = reader["FechaVencimiento"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["FechaVencimiento"])
                                    : (DateTime?)null,
                                Estado = Convert.ToInt32(reader["Estado"].ToString())
                            });
                        }
                    }
                }   

                response.Codigo = 1;
                response.Mensaje = "Consulta exitosa";
                response.Data = lotes;
                response.tabla = lotes;
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

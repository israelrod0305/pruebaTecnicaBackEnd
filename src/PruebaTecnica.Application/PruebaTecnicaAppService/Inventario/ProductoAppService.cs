using Microsoft.Data.SqlClient;
using PruebaTecnica.Dto;
using PruebaTecnica.Entities;
using PruebaTecnica.IPruebaTecnicaAppService;
using PruebaTecnica.IPruebaTecnicaAppService.InventarioAppService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.PruebaTecnicaAppService.Inventario
{
    public class ProductoAppService : PruebaTecnicaAppServiceBase, IProductoAppService
    {
        private SqlCommand CrearComando(SqlConnection conn, ProductoDto producto, string accion)
        {
            SqlCommand cmd = new SqlCommand("sp_Producto", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductoID", producto.ProductoID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoProducto", producto.CodigoProducto ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UnidadMedidaID", producto.UnidadMedidaID);
            cmd.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);
            cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
            cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);
            cmd.Parameters.AddWithValue("@StockMaximo", producto.StockMaximo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Usuario", producto.Usuario ?? "sistema");
            cmd.Parameters.AddWithValue("@Estado", producto.Estado);
            cmd.Parameters.AddWithValue("@Accion", accion);

            return cmd;
        }

        public async Task<ResponseModel<string>> Crear(ProductoDto producto, string connectionString)
        {
            var response = new ResponseModel<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = CrearComando(conn, producto, "INS"))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var resultado = reader["Resultado"].ToString();
                                var mensaje = reader["Mensaje"].ToString();

                                response.Codigo = resultado == "OK" ? 1 : 0;
                                response.Mensaje = mensaje;
                                response.Data = resultado == "OK" ? "Producto creado correctamente" : null;
                            }
                            else
                            {
                                response.Codigo = 0;
                                response.Mensaje = "No se recibió respuesta del SP";
                            }
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

        public async Task<ResponseModel<string>> Actualizar(ProductoDto producto, string connectionString)
        {
            var response = new ResponseModel<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, producto, "UP"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var resultado = reader["Resultado"].ToString();
                            var mensaje = reader["Mensaje"].ToString();

                            response.Codigo = resultado == "OK" ? 1 : 0;
                            response.Mensaje = mensaje;
                            response.Data = resultado == "OK" ? "Producto actualizado correctamente" : null;
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

            var producto = new ProductoDto
            {
                ProductoID = id,
                CodigoProducto = null,
                NombreProducto = null,
                Descripcion = null,
                UnidadMedidaID = 0,
                PrecioCompra = 0,
                PrecioVenta = 0,
                StockMinimo = 0,
                StockMaximo = null,
                Usuario = usuario,
                Estado = 0
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, producto, "DEL"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var resultado = reader["Resultado"].ToString();
                            var mensaje = reader["Mensaje"].ToString();

                            response.Codigo = resultado == "OK" ? 1 : 0;
                            response.Mensaje = mensaje;
                            response.Data = resultado == "OK" ? "Producto eliminado correctamente" : null;
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


        public async Task<ResponseModel<List<ProductoDto>>> Consultar(string connectionString, int? id = null, string codigo = null, string nombre = null)
        {
            var response = new ResponseModel<List<ProductoDto>>();
            var productos = new List<ProductoDto>();

            var producto = new ProductoDto
            {
                ProductoID = id,
                CodigoProducto = codigo,
                NombreProducto = nombre,
                UnidadMedidaID = 0,
                PrecioCompra = 0,
                PrecioVenta = 0,
                StockMinimo = 0,
                Usuario = "sistema",
                Estado = 1
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = CrearComando(conn, producto, "SEL"))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productos.Add(new ProductoDto
                            {
                                ProductoID = Convert.ToInt32(reader["Id"]),
                                CodigoProducto = reader["CodigoProducto"].ToString(),
                                NombreProducto = reader["NombreProducto"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                UnidadMedidaID = Convert.ToInt32(reader["UnidadMedidaID"]), // ← Ajustado aquí
                                PrecioCompra = Convert.ToDecimal(reader["PrecioCompra"]),
                                PrecioVenta = Convert.ToDecimal(reader["PrecioVenta"]),
                                StockMinimo = Convert.ToInt32(reader["StockMinimo"]),
                                StockMaximo = reader["StockMaximo"] != DBNull.Value ? Convert.ToInt32(reader["StockMaximo"]) : (int?)null,
                                Estado = Convert.ToBoolean(reader["Activo"]) ? 1 : 0
                            });
                        }
                    }
                }

                response.Codigo = 1;
                response.Mensaje = "Consulta exitosa";
                response.Data = productos;
                response.tabla = productos;
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

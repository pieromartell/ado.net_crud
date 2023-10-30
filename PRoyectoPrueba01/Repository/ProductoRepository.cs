using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using PRoyectoPrueba01.Models;
using PRoyectoPrueba01.Models.ViewModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace PRoyectoPrueba01.Repository
{
    public class ProductoRepository: IProductoRepository
    {
        private readonly string _conexion;

        public ProductoRepository(IConfiguration configuration)
        {
            _conexion = configuration.GetConnectionString("Conexion");
        }

        public async Task<bool> addProducts(ProductoViewModel producto)
        {
            using (SqlConnection conexion = new(_conexion))
            {
                using (SqlCommand cmd = new("sp_crearproducto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nameproducto", producto.nameproduct);
                    conexion.Open();
                    int evaluador = cmd.ExecuteNonQuery();
                    if (evaluador > 0)
                        return true;
                    else
                        return false;
                }
            }
        }

        public async Task<bool> deleteProduct(int id)
        {
            ProductoModel model = await getProducts(id);
            if (model == null) return false;
            using (SqlConnection conexion = new(_conexion))
            {
                using (SqlCommand cmd = new("sp_Eliminarproduct", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproducto", id);
                    conexion.Open();
                    int evaluador = cmd.ExecuteNonQuery();
                    if (evaluador > 0)
                        return true;
                    else
                        return false;
                }
            }
        }

        public async Task<List<ProductoModel>>getAllProducts()
        {
            using(SqlConnection conexion = new(_conexion))
            {
                using(SqlCommand cmd = new("sp_listarProductos", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        List<ProductoModel> productos= new List<ProductoModel>();
                        while (reader.Read()) {
                            ProductoModel producto = new ProductoModel
                            {
                                idproducto = Convert.ToInt32(reader["idproducto"]),
                                nameproduct = (reader["nameproduct"].ToString()),
                                state = Convert.ToBoolean(reader["state"].ToString()),
                            };
                           
                        productos.Add(producto);
                        }
                        return productos;
                    }

                }
            }
        }


        public async Task<bool> updateProduct(int id, ProductoModel producto)
        {
            ProductoModel model = await getProducts(id);
            if(model != null)
            {
                using (SqlConnection conexion = new(_conexion))
                {
                    using (SqlCommand cmd = new("sp_updateproduct", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idproducto", id);
                        cmd.Parameters.AddWithValue("@nameproducto", producto.nameproduct);
                        cmd.Parameters.AddWithValue("@State", producto.state);
                        conexion.Open();
                        int evaluador = cmd.ExecuteNonQuery();
                        if (evaluador > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            else
            {
                return false;
            }


        }

        public async Task<ProductoModel> getProducts(int id)
        {
            using (SqlConnection conexion = new(_conexion))
            {
                using (SqlCommand cmd = new("sp_listarProductosPorId", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idproduct", id);
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            ProductoModel producto = new ProductoModel();
                            producto.nameproduct = reader["nameproduct"].ToString();
                            producto.state = Convert.ToBoolean(reader["state"]);
                            producto.idproducto = Convert.ToInt32(reader["idproducto"]);
                            return producto;

                        }
                        return null;
                    }
                }
            }
        }

        public async Task<bool> UpdateProductState(int id)
        {
            var valorVerificar = true;
            ProductoModel productoModel = await getProducts(id);
            if (productoModel.state == true)
            {
                valorVerificar = false;
            }

            using (SqlConnection conexion = new(_conexion))
            {
                using (SqlCommand cmd = new("UPDATE producto SET state = @newState WHERE idproducto = @id", conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@newState", valorVerificar);
                    conexion.Open();
                   int valor= await cmd.ExecuteNonQueryAsync();
                    if (valor > 0)
                        return true;
                    else
                        return false;
                   
                }
            }
        }

        public async Task<List<ProductoModel>>FiltrarPorNombre(int difernciador,string valor)
        {
            if(difernciador == 0 ) { return null; }
            if(difernciador ==1) { if (!(int.TryParse(valor, out int valorEntero))) { return null; } }
            
            using (SqlConnection conexion = new(_conexion))
            {
                using (SqlCommand cmd = new("sp_filtrarPorIdyName", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@I", difernciador);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    conexion.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<ProductoModel> productos = new List<ProductoModel>();
                        while (reader.Read())
                        {
                            ProductoModel producto = new ProductoModel
                            {
                                idproducto = Convert.ToInt32(reader["idproducto"]),
                                nameproduct = (reader["nameproduct"].ToString()),
                                state = Convert.ToBoolean(reader["state"].ToString()),
                            };

                            productos.Add(producto);
                        }
                        return productos;
                    }
                }
            }
        }
    }
}

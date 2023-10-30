using PRoyectoPrueba01.Models;
using PRoyectoPrueba01.Models.ViewModel;

namespace PRoyectoPrueba01.Repository
{
    public interface IProductoRepository
    {
        Task<List<ProductoModel>> getAllProducts();
        Task<ProductoModel> getProducts(int id);
        Task<bool> addProducts(ProductoViewModel producto);
        Task<bool> updateProduct(int id, ProductoModel producto);
        Task<bool> deleteProduct(int id);
        Task<bool> UpdateProductState(int id);
        Task<List<ProductoModel>> FiltrarPorNombre(int difernciador, string valor);
    }
}

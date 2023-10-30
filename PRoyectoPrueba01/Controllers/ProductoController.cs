using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PRoyectoPrueba01.Models;
using PRoyectoPrueba01.Models.ViewModel;
using PRoyectoPrueba01.Repository;

namespace PRoyectoPrueba01.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IProductoRepository _repository;

        public ProductoController(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(int filtrar, string buscar)
        {
            if(filtrar == 0 || buscar == null)
            {
                try
                {
                    List<ProductoModel> productos = await _repository.getAllProducts();
                    return View(productos);
                }
                catch (Exception ex)
                {
                    return View(new ErrorViewModel { });
                }
            }
            else{
                try
                {
                    List<ProductoModel> productos = await _repository.FiltrarPorNombre(filtrar, buscar);
                    if(productos == null)
                    {
                        List<ProductoModel> productos2 = await _repository.getAllProducts();
                        return View(productos2);
                    }
                        
                    return View(productos);
                }
                catch (Exception ex)
                {
                    return View(new ErrorViewModel { });
                }
            }
        }

        public IActionResult crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> crear(ProductoViewModel producto)
        {
            try
            {
                bool productocreado = await _repository.addProducts(producto);
                if (productocreado)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }

            } catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<IActionResult> edit(int id)
        {
            try
            {
                ProductoModel product = await _repository.getProducts(id);
                return View(product);
            }catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> edit(int id,ProductoModel producto)
        {

            bool evaluador = await _repository.updateProduct(id ,producto);
            if (evaluador)
                return RedirectToAction("Index");
            else
                return View(producto);
        }

        public async Task<IActionResult> delete(int id)
        {
            bool evaluador = await _repository.deleteProduct(id);
            if (evaluador)
                return RedirectToAction("Index");
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> updateState(int id)
        {
            bool evaluador = await _repository.UpdateProductState(id);
            if (!evaluador)
                return RedirectToAction("Index"); 
            else 
                return RedirectToAction("Index");
        }

     //   public async Task<IActionResult> FiltrarNYI(int i,int id, string nameproduct)
     //   {
     //       try
     //       {
     //           List<ProductoModel> productos = await _repository.FiltrarPorNombre(i,id,nameproduct);
     //           return RedirectToAction("Index",productos);
     //       }
     //       catch (Exception ex)
     //       {
     //           return View(new ErrorViewModel { });
      //      }
     //   }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Infrastructure.Interface;
using OrderManagementSystem.Infrastructure.Service;


namespace OrderManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;


        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _product.GetAllProductsAsync();
            return Json(products); 
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.Product;
using Puppy_Project.Services.Products;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProducts _products;
        public ProductsController(IProducts products)
        {
            _products = products;
        }
        [HttpGet]
        public IActionResult ListProducts()
        {
            return Ok(_products.GetProducts());
        }


        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody] AddProductDTO product)
        {
            bool isAdded = _products.AddProduct(product);
            return Ok(isAdded);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateProducts(int id,[FromBody] AddProductDTO product)
        {
            bool isUpdated = _products.UpdateProduct(id, product);
            return Ok(isUpdated);
        }
    }
}

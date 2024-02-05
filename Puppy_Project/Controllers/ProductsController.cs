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



        [HttpGet(Name ="ListProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ListProducts()
        {
            try
            {
                var productslist = _products.GetProducts();
                return productslist.Count !=0 ? Ok(productslist):BadRequest("Products list is empty");
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }


        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddProduct([FromBody] AddProductDTO product)
        {
            try
            {
                bool isAdded = _products.AddProduct(product);
                if(!isAdded) 
                {
                    return BadRequest("Category_id is not valid");
                }
                return CreatedAtRoute("ListProducts",new {id=0}, product);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProducts(int id,[FromBody] AddProductDTO product)
        {
            try
            {
                bool isUpdated = _products.UpdateProduct(id, product);
                if (!isUpdated)
                {
                    return NotFound("item not found");
                }
                return Ok(isUpdated);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }   
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(int id)
        {
            try
            {
                bool isDeleted = _products.DeleteProduct(id);
                if (!isDeleted)
                {
                    return NotFound(id);
                }
                return Ok(isDeleted);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

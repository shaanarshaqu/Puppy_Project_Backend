using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.ProductDTO;
using Puppy_Project.Services.Products;
using static System.Net.Mime.MediaTypeNames;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _products;
        public ProductsController(IProductsService products)
        {
            _products = products;
        }



        [HttpGet(Name ="ListProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListProducts()
        {
            try
            {
                var productslist = await _products.GetProducts();
                return productslist.Count !=0 ? Ok(productslist):BadRequest("Products list is empty");
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _products.GetProductById(id);
                if(product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }




        [HttpGet("ByCategory/{id:int}")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            try
            {
                var list = await _products.GetProductsByCategory(id);
                return Ok(list);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }
        




        [HttpGet("Page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListProductByPage([FromQuery]int pageNo,int pageSize)
        {
            try
            {
                var productlist = await _products.GetProductsByPage(pageNo, pageSize);
                return Ok(productlist);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        [HttpPost("Add")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO product,IFormFile image)
        {
            try
            {
                bool isAdded =await _products.AddProduct(product, image);
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

        [HttpPut("Update/{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProducts([FromForm] AddProductDTO product, int id,IFormFile image)
        {
            try
            {
                bool isUpdated = await _products.UpdateProduct(id, product, image);
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


        [HttpDelete("Remove/{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                bool isDeleted =await _products.DeleteProduct(id);
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

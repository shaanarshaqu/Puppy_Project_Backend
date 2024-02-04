using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.Category;
using Puppy_Project.Services.Category;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        public CategoryController(ICategory category) 
        { 
            _category = category;
        }

        [HttpGet]
        public ActionResult GetCategories() 
        { 
            return Ok(_category.DisplayCategories());
        }



        [HttpPost("AddNewCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCategory([FromBody] AddCategoryDTO category) 
        {
            try
            {
                bool isAdded = _category.AddCategory(category);
                if (!isAdded) 
                {
                    return BadRequest();
                }
                return Ok(category);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                bool isDeleted = _category.DeleteCategory(id);
                return isDeleted ? Ok(isDeleted) : BadRequest(isDeleted);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

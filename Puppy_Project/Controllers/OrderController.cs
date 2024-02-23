using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Models.RazorPay;
using Puppy_Project.Secure;
using Puppy_Project.Services.Orders;

namespace Puppy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _order;
        public OrderController(IOrderService order) 
        {
            _order = order;
        }



        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> ListUserOrder(int id)
        {
            try
            {
                var order_list = await _order.ListUserOrder(id);
                return Ok(order_list);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }




        [HttpPost("order-create")]
        [Authorize]
        public async Task<ActionResult> createOrder([FromBody] TakePriceDTO priceDto)
        {
            try
            {
                if (priceDto.price <= 0)
                {
                    return BadRequest("enter a valid money ");
                }
                var orderId = await _order.OrderCreate(priceDto.price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }


        [HttpPost("payment")]
        [Authorize]
        public ActionResult Payment(RazorpayDTO razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("razorpay details must not null here");
                }
                var con = _order.Payment(razorpay);
                return Ok(con);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpPost("place-Order")]
        [Authorize]
        public async Task<ActionResult> PlaceOrder(inputOrderDTO orderRequests)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var Token = token.Split(' ')[1];
                var UserId = TokenDecoder.DecodeToken(Token);
                if (orderRequests == null || UserId != orderRequests.User_Id)
                {
                    return BadRequest();
                }
                var status = await _order.CreateOrder(orderRequests);
                return Ok(status);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }






        [HttpDelete("CancelUserOrder/{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                bool isDeleted = await _order.RemoveAllorders(id);
                return isDeleted ? Ok(isDeleted) : BadRequest(isDeleted);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        [HttpGet("TotalParchase")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> TotalProductParchased()
        {
            try
            {
                int total = await _order.TotalPurchase();
                return Ok(total);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        [HttpGet("TotalRevenue")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> TotalRevenueGenerated()
        {
            try
            {
                string total = await _order.TotalRevenueGenerated();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}

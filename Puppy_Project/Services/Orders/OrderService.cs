using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Models.RazorPay;
using Razorpay.Api;


namespace Puppy_Project.Services.Orders
{
    public class OrderService:IOrderService
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        private readonly IConfiguration _Configuration;
        public OrderService(PuppyDb puppyDb, IConfiguration configuration)
        {
            _puppyDb = puppyDb;
            _Configuration = configuration;
        }

        public async Task<List<outOrderDTO>> ListUserOrder(int id)
        {
            try
            {
                var order = _puppyDb.UsersTb
                    .Include(u=>u.userorder)
                        .ThenInclude(o=>o.orderItems)
                            .ThenInclude(oi=>oi.product)
                    .SingleOrDefault(u=>u.Id == id);
                if(order == null || order.userorder == null)
                {
                    return new List<outOrderDTO>();
                }
                var orderList = order.userorder.orderItems.Select(oi=>new outOrderDTO
                {
                    Id = oi.Id,
                    Product_Id = oi.Product_Id,
                    Qty = oi.Qty,
                    Img = $"{_Configuration["HostUrl:images"]}/Products/{oi.product.Img}",
                    Price = oi.Total / oi.Qty,
                    Total = oi.Total,
                    User_Id = order.Id
                }).ToList();
                return orderList;

            }
            catch(Exception ex)
            {
                return new List<outOrderDTO>();
            }
            
        }


        public async Task<List<outOrderDTO>> AllOrders()
        {
            try
            {
                var order =await _puppyDb.UsersTb
                    .Include(u => u.userorder)
                        .ThenInclude(o => o.orderItems)
                            .ThenInclude(oi => oi.product)
                .ToListAsync();

                if (order == null || order.Count == 0)
                {
                    return new List<outOrderDTO>();
                }
                var orderList = order.Select(u=>new outOrderDTO
                {
                    Id =u.userorder.orderItems.FirstOrDefault().Id,
                    Product_Id = u.userorder.orderItems.FirstOrDefault().Product_Id,
                    Qty = u.userorder.orderItems.FirstOrDefault().Qty,
                    Img = $"{_Configuration["HostUrl:images"]}/Products/{u.userorder.orderItems.FirstOrDefault().product.Img}",
                    Price = u.userorder.orderItems.FirstOrDefault().Price / u.userorder.orderItems.FirstOrDefault().Qty,
                    Total = u.userorder.orderItems.FirstOrDefault().Total,
                    User_Id = u.userorder.Id
                }).ToList();
                return orderList;

            }
            catch (Exception ex)
            {
                return new List<outOrderDTO>();
            }

        }


        public async Task<string> OrderCreate(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TrasactionId = random.Next(0, 1000).ToString();
            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TrasactionId);

            string key = _Configuration["Razorpay:KeyId"];
            string secret = _Configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            var OrderId = order["id"].ToString();

            return OrderId;
        }


        public bool Payment(RazorpayDTO razorpay)
        {
            if (
                razorpay == null ||
                razorpay.razorpay_payment_id == null ||
                razorpay.razorpay_order_id == null ||
                razorpay.razorpay_signature == null
                )
            {
                return false;
            }
            RazorpayClient client = new RazorpayClient(_Configuration["Razorpay:KeyId"], _Configuration["Razorpay:KeySecret"]);
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("razorpay_payment_id", razorpay.razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay.razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay.razorpay_signature);
            Utils.verifyPaymentSignature(attributes);
            return true;
        }



        public async Task<bool> CreateOrder(inputOrderDTO userDetails)
        {
            try
            {
                var cartitems = await _puppyDb.UsersTb
                    .Include(u => u.cartuser)
                        .ThenInclude(c => c.cartItemDTOs)
                            .ThenInclude(ci=>ci.product)
                    .SingleOrDefaultAsync(u => u.Id == userDetails.User_Id);

                if ( cartitems.cartuser == null )
                {
                    throw new Exception("User have no cart");
                }
                var isUserHaveOrder = await _puppyDb.OrderTb.SingleOrDefaultAsync(o => o.User_Id == userDetails.User_Id);
                if (isUserHaveOrder == null)
                {
                    await _puppyDb.OrderTb.AddAsync(new Models.Order { User_Id = userDetails.User_Id }); 
                    await _puppyDb.SaveChangesAsync();
                    isUserHaveOrder = await _puppyDb.OrderTb.SingleOrDefaultAsync(o => o.User_Id == userDetails.User_Id);
                }
                var orderitemlist =cartitems.cartuser.cartItemDTOs.Select(ci=> new OrderItem
                {
                    Order_Id = isUserHaveOrder.Id,
                    Product_Id = ci.Product_Id,
                    Qty=ci.Qty,
                    Price=ci.product.Price,
                    Total =ci.product.Price/ci.Qty,
                    DelivaryAddress = userDetails.DelivaryAddress,
                    OrderDate=DateTime.Now
                });
                 await _puppyDb.OrderItemTb.AddRangeAsync(orderitemlist);
                await _puppyDb.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                 throw new Exception(ex.Message);
            }
        }








        public async Task<bool> RemoveAllorders(int userid)
        {
            try
            {
                var userHasOrder = await _puppyDb.OrderTb.SingleOrDefaultAsync(o => o.User_Id == userid);
                if (userHasOrder == null)
                {
                    return false;
                }
                _puppyDb.OrderTb.Remove(userHasOrder);
                _puppyDb.SaveChanges() ;
                return true;
            }catch( Exception ex) 
            {
                return false;
            }
            
        }

        public async Task<int> TotalPurchase()
        {
            try
            {
                int total = _puppyDb.OrderItemTb.ToList().Count();
                return total;
            }catch(Exception ex)
            {
                return 0;
            }
           
        }


        public async Task<string> TotalRevenueGenerated()
        {
            try
            {
                decimal totalravenue = await _puppyDb.OrderItemTb.SumAsync(x => x.Total);
                return totalravenue.ToString();
            }catch(Exception ex)
            {
                return "0";
            }
        }


    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Models;
using Puppy_Project.Models.OrderDTO;


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
                /*var user_order = await _puppyDb.OrderTb.Include(o => o.orderItems).FirstOrDefaultAsync(o => o.User_Id == id);
                Console.WriteLine(user_order);
                if (user_order == null || user_order.orderItems == null)
                {
                    return new List<outOrderDTO>();
                }
                var outordertype = user_order.orderItems.Select(o => new outOrderDTO
                {
                    Id = o.Id,
                    Product_Id = o.Product_Id,
                    Qty = o.Qty,
                    Img = $"{_Configuration["HostUrl:images"]}/Products/{o.product.Img}",
                    Price = o.Total / o.Qty,
                    Total = o.Total,
                    User_Id = user_order.User_Id
                }
                ).ToList();
                return outordertype;*/
            }
            catch(Exception ex)
            {
                return new List<outOrderDTO>();
            }
            
        }


        public async Task<bool> AddUserOrder(inputOrderDTO order)
        {
            try
            {
                bool isUserValid = await _puppyDb.UsersTb.AnyAsync(u => u.Id == order.User_Id);
                var isProductValid = await _puppyDb.ProductsTb.FindAsync(order.Product_Id);
                if (!isUserValid || isProductValid == null)
                {
                    return false;
                }
                var isUserhasOrder = await _puppyDb.OrderTb.SingleOrDefaultAsync(o => o.User_Id == order.User_Id);
                if (isUserhasOrder == null)
                {
                    _puppyDb.OrderTb.Add(new Order { User_Id = order.User_Id, orderItems = new List<OrderItem>() });
                    _puppyDb.SaveChanges();
                    isUserhasOrder = await _puppyDb.OrderTb.SingleOrDefaultAsync(o => o.User_Id == order.User_Id);
                }
                var inOrderItemshasItem = await _puppyDb.OrderItemTb.SingleOrDefaultAsync(oi => oi.Product_Id == order.Product_Id && oi.Order_Id == isUserhasOrder.Id);
                if (inOrderItemshasItem == null)
                {
                    OrderItem orderitem = new OrderItem();
                    orderitem.Order_Id = isUserhasOrder.Id;
                    orderitem.Product_Id = order.Product_Id;
                    orderitem.Qty = order.Qty;
                    orderitem.Total = orderitem.Qty * isProductValid.Price;
                    _puppyDb.OrderItemTb.Add(orderitem);
                    _puppyDb.SaveChanges();
                    return true;
                }
                inOrderItemshasItem.Qty += order.Qty;
                inOrderItemshasItem.Total = inOrderItemshasItem.Qty * isProductValid.Price;
                _puppyDb.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
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

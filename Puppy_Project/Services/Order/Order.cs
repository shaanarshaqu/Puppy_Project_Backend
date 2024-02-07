using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Depandancies;
using Puppy_Project.Models;
using Puppy_Project.Models.Order;
using Puppy_Project.Services.Products;

namespace Puppy_Project.Services.Order
{
    public class Order:IOrder
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public Order(PuppyDb puppyDb)
        {
            _puppyDb = puppyDb;
        }

        public List<outOrderDTO> ListUserOrder(int id)
        {
            var user_order = _puppyDb.OrderTb.Include(o=>o.orderItems).FirstOrDefault(o=>o.User_Id == id);
            if (user_order == null)
            {
                return new List<outOrderDTO>();
            }
            var outordertype = user_order.orderItems.Select(o => new outOrderDTO
            {
                Id=o.Id,
                Product_Id = o.Product_Id,
                Qty=o.Qty,
                Price=o.Total/o.Qty,
                Total=o.Total,
                User_Id = user_order.User_Id
            }
            ).ToList();
            return outordertype;
        }


        public bool AddUserOrder(inputOrderDTO order)
        {
            bool isUserValid = _puppyDb.UsersTb.Any(u => u.Id == order.User_Id);
            var isProductValid = _puppyDb.ProductsTb.Find(order.Product_Id);
            if (!isUserValid || isProductValid==null)
            {
                return false;
            }
            var isUserhasOrder = _puppyDb.OrderTb.SingleOrDefault(o=>o.User_Id==order.User_Id);
            if(isUserhasOrder == null)
            {
                _puppyDb.OrderTb.Add(new OrderDTO { User_Id= order.User_Id, orderItems =new List<OrderItemDTO>() });
                _puppyDb.SaveChanges();
                isUserhasOrder = _puppyDb.OrderTb.SingleOrDefault(o => o.User_Id == order.User_Id);
            }
            var inOrderItemshasItem = _puppyDb.OrderItemTb.SingleOrDefault(oi => oi.Product_Id == order.Product_Id && oi.Order_Id == isUserhasOrder.Id);
            if(inOrderItemshasItem == null) 
            {
                OrderItemDTO orderitem = new OrderItemDTO();
                orderitem.Order_Id = isUserhasOrder.Id;
                orderitem.Product_Id = order.Product_Id;
                orderitem.Qty = order.Qty;
                orderitem.Total = orderitem.Qty * isProductValid.Price;
                _puppyDb.OrderItemTb.Add(orderitem);
                _puppyDb.SaveChanges();
                return true;
            }
            inOrderItemshasItem.Qty++;
            inOrderItemshasItem.Total = inOrderItemshasItem.Qty * isProductValid.Price;
            _puppyDb.SaveChanges();
            return true;           
        }

        /*public bool RemoveAllorders(int userid)
        {

        }*/
    }
}

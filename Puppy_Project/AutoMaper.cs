using AutoMapper;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Models;
using Puppy_Project.Models.CartDTO;
using Puppy_Project.Models.CategoryDTO;
using Puppy_Project.Models.Input_OutputDTOs;
using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Models.ProductDTO;


namespace Puppy_Project
{
    public class AutoMaper:Profile
    {
        public AutoMaper() 
        {
            CreateMap<User, inputUserDTO>().ReverseMap();
            CreateMap<outUserDTO,User>().ReverseMap();
            CreateMap<outUserDTO,inputUserDTO>().ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<Category,AddCategoryDTO>().ReverseMap();
            CreateMap<outCategoryDTO,Category>().ReverseMap();
            CreateMap<Product,AddProductDTO>().ReverseMap();
            CreateMap<outProductDTO, Product>().ReverseMap();
            CreateMap<Cart, AddCartDTO>().ReverseMap();
            CreateMap<outOrderDTO,Order>().ReverseMap();
            CreateMap<Order, inputOrderDTO>().ReverseMap();
            CreateMap<OrderItem, inputOrderDTO>().ReverseMap();

        }


    }
    
}

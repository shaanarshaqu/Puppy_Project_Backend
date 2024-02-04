using AutoMapper;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Models;
using Puppy_Project.Models.Category;
using Puppy_Project.Models.Input_OutputDTOs;
using Puppy_Project.Models.Product;

namespace Puppy_Project
{
    public class AutoMaper:Profile
    {
        public AutoMaper() 
        {
            CreateMap<UserDTO, inputUserDTO>().ReverseMap();
            CreateMap<outUserDTO,UserDTO>().ReverseMap();
            CreateMap<outUserDTO,inputUserDTO>().ReverseMap();
            CreateMap<UserDTO, RegisterDTO>().ReverseMap();
            CreateMap<CategoryDTO,AddCategoryDTO>().ReverseMap();
            CreateMap<outCategoryDTO,CategoryDTO>().ReverseMap();
            CreateMap<ProductDTO,AddProductDTO>().ReverseMap();
            CreateMap<outProductDTO, ProductDTO>().ReverseMap();

        }
    }
}

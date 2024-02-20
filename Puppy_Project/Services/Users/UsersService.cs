using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Interfaces;
using Puppy_Project.Migrations;
using Puppy_Project.Models.Input_OutputDTOs;
using Puppy_Project.Models.OrderDTO;
using Puppy_Project.Secure;
using System.Data;
using System.Numerics;

namespace Puppy_Project.Models
{
    public class UsersService:IUsersService
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public UsersService(PuppyDb puppyDb,IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration) 
        {
            _puppyDb= puppyDb;
            _mapper= mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration= configuration;
        }


        public async Task<List<outUserDTO>> ListUsers()
        {
            var usersFromDb = await _puppyDb.UsersTb.Include(u => u.userorder).ThenInclude(o => o.orderItems).ThenInclude(oi=>oi.product).ToListAsync();
            var userslist = usersFromDb.Select(u=>new outUserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Profile_Photo = $"{_configuration["HostUrl:images"]}/Users/{u.Profile_Photo}",
                Place = u.Place,
                Phone = u.Phone,
                Role = u.Role,
                Status = u.Status,
                orders =u.userorder?.orderItems?.Select(uo=> new
                outOrderDTO
                {
                    Id = uo.Id,
                    Product_Id = uo.Product_Id,
                    Img = $"{_configuration["HostUrl:images"]}/Products/{uo.product.Img}",
                    Qty = uo.Qty,
                    Price = uo.product.Price,
                    Total = uo.Total,
                    User_Id = u.Id
                }).ToList()
                }).ToList();
            if(usersFromDb == null  || userslist == null)
            {
                return new List<outUserDTO>();
            }
            return userslist;
        }



        public async Task<outUserDTO> GetUser(int id)
        {
            var user = await _puppyDb.UsersTb.Select(u => new outUserDTO
                {
                    Id=u.Id,
                    Name=u.Name,
                    Email=u.Email,
                    Profile_Photo = $"{_configuration["HostUrl:images"]}/Users/{u.Profile_Photo}",
                    Place = u.Place,
                    Phone = u.Phone,
                    Role = u.Role,
                    Status = u.Status
            }).SingleOrDefaultAsync(u=>u.Id == id);
            if(user == null)
            {
                return new outUserDTO();
            }
            return user;
        }


        public async Task<outUserDTO> GetUserforAdmin(int id)
        {
            var usersFromDb = await _puppyDb.UsersTb.Include(u => u.userorder).ThenInclude(o => o.orderItems).ThenInclude(oi => oi.product).SingleOrDefaultAsync(u => u.Id == id);
            var Current_user = new outUserDTO
            {
                Id = usersFromDb.Id,
                Name = usersFromDb.Name,
                Email = usersFromDb.Email,
                Profile_Photo = $"{_configuration["HostUrl:images"]}/Users/{usersFromDb.Profile_Photo}",
                Place = usersFromDb.Place,
                Phone = usersFromDb.Phone,
                Role = usersFromDb.Role,
                Status = usersFromDb.Status,
                orders = usersFromDb.userorder?.orderItems?.Select(uo => new outOrderDTO
                {
                    Id = uo.Id,
                    Product_Id = uo.Product_Id,
                    Img = $"{_configuration["HostUrl:images"]}/Products/{uo.product.Img}",
                    Qty = uo.Qty,
                    Price = uo.product.Price,
                    Total = uo.Total,
                    User_Id= usersFromDb.Id
                }).ToList()
            };
            if (usersFromDb == null || Current_user == null)
            {
                return new outUserDTO();
            }
            return Current_user;
        }




        public async Task<bool> Register(RegisterDTO user,IFormFile image)
        {
            using (var transaction = await _puppyDb.Database.BeginTransactionAsync())
            {
                try
                {
                    var isUserExist = await _puppyDb.UsersTb.SingleOrDefaultAsync(u => u.Email == user.Email);
                    if (isUserExist != null )
                    {
                        return false;
                    }
                    string productImage = null;
                    if (image != null && image.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Users", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        productImage = fileName;
                    }
                    if (productImage == null)
                    {
                        productImage = _configuration["Default:UserLogo"];
                    }
                    var userDtoConverted = _mapper.Map<User>(user);
                    userDtoConverted.Password = PasswordSecure.HashPassword(user.Password);
                    userDtoConverted.Profile_Photo = productImage;
                    await _puppyDb.UsersTb.AddAsync(userDtoConverted);
                    await _puppyDb.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> AddNewAdmin(RegisterDTO user)
        {
            using (var transaction = _puppyDb.Database.BeginTransaction())
            {
                try
                {
                    var isUserExist = await _puppyDb.UsersTb.SingleOrDefaultAsync(u => u.Email == user.Email);
                    if (isUserExist != null)
                    {
                        return false;
                    }
                    var userDtoConverted = _mapper.Map<User>(user);
                    userDtoConverted.Password = PasswordSecure.HashPassword(user.Password);
                    userDtoConverted.Role = "admin";
                    _puppyDb.UsersTb.Add(userDtoConverted);
                    _puppyDb.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }


        public async Task<bool> BlockUser(int id)
        {
            try
            {
                var user = await _puppyDb.UsersTb.FindAsync(id);
                if(user == null)
                {
                    return false;
                }
                user.Status = "Blocked";
                await _puppyDb.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UnBlockUser(int id)
        {
            try
            {
                var user = await _puppyDb.UsersTb.FindAsync(id);
                if (user == null)
                {
                    return false;
                }
                user.Status = "Active";
                await _puppyDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }





        public async Task<outUserDTO> Login(inputUserDTO user)
        {
            try
            {
                var isUser = await _puppyDb.UsersTb.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (isUser == null || !PasswordSecure.VerifyPassword(user.Password, isUser.Password))
                {
                    return null;
                }
                return _mapper.Map<outUserDTO>(isUser);
            }catch(Exception ex)
            {
                return null;
            }
            
        }
    }
}

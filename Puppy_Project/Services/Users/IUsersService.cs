using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Models;
using Puppy_Project.Models.Input_OutputDTOs;

namespace Puppy_Project.Interfaces
{
    public interface IUsersService
    {
        Task<List<outUserDTO>> ListUsers();
        Task<outUserDTO> GetUserforAdmin(int id);
        Task<outUserDTO> GetUser(int id);
        Task<bool> Register(RegisterDTO user, IFormFile image);
        Task<outUserDTO> Login(inputUserDTO user);
        Task<bool> AddNewAdmin(RegisterDTO user);
        Task<bool> BlockUser(int id);
        Task<bool> UnBlockUser(int id);
    }
}

using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Models;
using Puppy_Project.Models.Input_OutputDTOs;

namespace Puppy_Project.Interfaces
{
    public interface IUsersService
    {
        Task<List<outUserDTO>> ListUsers();
        Task<bool> Register(RegisterDTO user);
        Task<outUserDTO> Login(inputUserDTO user);
    }
}

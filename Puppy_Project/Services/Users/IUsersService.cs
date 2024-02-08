using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Models;
using Puppy_Project.Models.Input_OutputDTOs;

namespace Puppy_Project.Interfaces
{
    public interface IUsersService
    {
        List<outUserDTO> ListUsers();
        bool Register(RegisterDTO user);
        outUserDTO Login(inputUserDTO user);
    }
}

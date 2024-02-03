using AutoMapper;
using Puppy_Project.Dbcontext;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Interfaces;
using Puppy_Project.Models.Input_OutputDTOs;
using Puppy_Project.Secure;

namespace Puppy_Project.Models
{
    public class Users:IUsers
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public Users(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper= mapper;
        }


        public List<outUserDTO> ListUsers()
        {
            var usersFromDb = _puppyDb.UsersTb.ToList();
            if(usersFromDb == null || usersFromDb.Count == 0)
            {
                return null;
            }
            var userDtolist= _mapper.Map<List<outUserDTO>>(usersFromDb);
            return userDtolist;
        }

        public bool Register(RegisterDTO user)
        {
            using (var transaction = _puppyDb.Database.BeginTransaction())
            {
                try
                {
                    var isUserExist = _puppyDb.UsersTb.SingleOrDefault(u => u.Email == user.Email);
                    if (isUserExist != null)
                    {
                        return false;
                    }
                    var userDtoConverted = _mapper.Map<UserDTO>(user);
                    userDtoConverted.Password = PasswordSecure.HashPassword(user.Password);
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

        public outUserDTO Login(inputUserDTO user)
        {
            var isUser = _puppyDb.UsersTb.FirstOrDefault(u => u.Email == user.Email);
  
            if (isUser == null || !PasswordSecure.VerifyPassword(user.Password, isUser.Password))
            {
                return null;
            }
            return _mapper.Map<outUserDTO>(isUser);
        }
    }
}

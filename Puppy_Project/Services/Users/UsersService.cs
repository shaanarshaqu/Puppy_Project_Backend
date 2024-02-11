using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy_Project.Dbcontext;
using Puppy_Project.Depandancies;
using Puppy_Project.InputDTOs;
using Puppy_Project.Interfaces;
using Puppy_Project.Models.Input_OutputDTOs;
using Puppy_Project.Secure;

namespace Puppy_Project.Models
{
    public class UsersService:IUsersService
    {
        private readonly PuppyDb _puppyDb;
        private readonly IMapper _mapper;
        public UsersService(PuppyDb puppyDb,IMapper mapper) 
        {
            _puppyDb= puppyDb;
            _mapper= mapper;
        }


        public async Task<List<outUserDTO>> ListUsers()
        {
            var usersFromDb = await _puppyDb.UsersTb.ToListAsync();
            if(usersFromDb == null || usersFromDb.Count == 0)
            {
                return null;
            }
            var userDtolist= _mapper.Map<List<outUserDTO>>(usersFromDb);
            return userDtolist;
        }


        public async Task<outUserDTO> GetUser(int id)
        {
            var tmpuser = await _puppyDb.UsersTb.FindAsync(id);
            if (tmpuser == null )
            {
                return null;
            }
            var user = _mapper.Map<outUserDTO>(tmpuser);
            return user;
        }

        public async Task<bool> Register(RegisterDTO user)
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
                user.Role = "Blocked";
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
                user.Role = "user";
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

using Puppy_Project.Dbcontext;
using Puppy_Project.Interfaces;

namespace Puppy_Project.Models
{
    public class Users:IUsers
    {
        private readonly PuppyDb _puppyDb;
        public Users(PuppyDb puppyDb) 
        {
            _puppyDb= puppyDb;
        }

    }
}

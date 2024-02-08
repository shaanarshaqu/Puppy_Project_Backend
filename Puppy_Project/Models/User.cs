using Puppy_Project.Models;

namespace Puppy_Project.Depandancies
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public Cart cartuser { get; set; }
        public Order userorder { get; set; }
        public WishList wishList { get; set; }
    }
}

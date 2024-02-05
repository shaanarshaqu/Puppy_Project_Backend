using Puppy_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Depandancies
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public CartDTO cartuser { get; set; }
    }
}

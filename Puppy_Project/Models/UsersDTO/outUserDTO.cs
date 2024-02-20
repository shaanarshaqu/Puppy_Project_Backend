using Puppy_Project.Models.OrderDTO;

namespace Puppy_Project.Models
{
    public class outUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Profile_Photo { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public List<outOrderDTO> orders { get; set; }
    }
}

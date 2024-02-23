using System.ComponentModel.DataAnnotations;

namespace Puppy_Project.Models.OrderDTO
{
    public class inputOrderDTO
    {
        [Required]
        public int User_Id { get; set; }
        public string DelivaryAddress { get; set; }
    }
}

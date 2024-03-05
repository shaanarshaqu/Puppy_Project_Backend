namespace Puppy_Project.Models.OrderDTO
{
    public class outOrderDTO
    {
        public int Id {  get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Qty { get; set; }
        public int Price {  get; set; }
        public int Total { get; set; }
        public DateTime OrderDate { get; set; }
        public int User_Id { get; set; }
        public string Img { get; set;}
    }
}

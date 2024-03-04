namespace Test.Models
{

    public class Order
    {
        //OrderID, UserID, Products, TotalPrice
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Products { get; set; }
        public float TotalPrice { get; set; }
    }
}
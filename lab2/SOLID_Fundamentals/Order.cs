namespace SOLID_Fundamentals
{
    // Change: Reduced responcibility in Order class
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; private set; }
        public List<string> Items { get; } = new List<string>();
        public required Customer Customer { get; set; }
        public required IPaymentMethod PaymentMethod { get; set; }
    }
}

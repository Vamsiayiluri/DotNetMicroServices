namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public Guid UserId { get; set; }   // ✅ ADD THIS

        public string Status { get; set; } = "Created";  // ✅ ADD THIS

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
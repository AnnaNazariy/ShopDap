namespace ShopDap.Entities
{
    public class Product : BaseEntity
    {
        public required string ProductName { get; set; } 
        public decimal Price { get; set; }
        public required string Description { get; set; } 
    }

}

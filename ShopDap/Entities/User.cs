namespace ShopDap.Entities
{
    public class User : BaseEntity
    {
        public required string UserName { get; set; } 
        public required string Email { get; set; } 
        public required string PasswordHash { get; set; } 
    }

}

namespace Domain.DTO
{
    public class RestaurantDto
    {
        public int RestaurantId { get; set; }
        public bool Status { get; set; }

        public string?  OwnerName { get; set; }
        public bool IsValid { get; set; }
       
        public string? Phoneno { get; set; }
        public string? Address { get; set; }
    }
}

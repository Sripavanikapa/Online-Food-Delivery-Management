namespace Domain.DTO
{
    public class RestaurantCreateDto
    {
        public bool Status { get; set; }

        public int UserId { get; set; } // references from user table
    }
}

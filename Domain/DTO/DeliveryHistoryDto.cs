namespace Application.DTOs
{
    public class DeliveryHistoryDto
    {
        //public int DeliveryId { get; set; }
        //public int OrderId { get; set; }
        //public bool DeliveryStatus { get; set; }
        public string? CustomerName { get; set; }
        public string? RestaurantName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? DeliveryAddress { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
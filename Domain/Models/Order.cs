using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
  
    
    public  int UserId { get; set; }
    
    public  int RestaurantId { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public int CustAddressId { get; set; }
}
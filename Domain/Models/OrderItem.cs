using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models;

public partial class OrderItem
{
    [Key, ForeignKey("Order")]
    public required int OrderId { get; set; }

    public required int ItemId { get; set; }

    public required int Quantity { get; set; }

    public  virtual FoodItem? Item { get; set; }

    public  virtual Order? Order { get; set; }
}

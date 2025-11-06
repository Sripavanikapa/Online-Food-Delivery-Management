using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
 
namespace Domain.Models;
 
[Table("OrderItems")]
public partial class OrderItem
{
   
    public int OrderId { get; set; }

   
    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public virtual FoodItem? Item { get; set; }
    public virtual Order? Order { get; set; }
}

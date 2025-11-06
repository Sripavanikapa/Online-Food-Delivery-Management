using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models;

public partial class FoodItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

   
    public int ItemId { get; set; }

    public  int RestaurantId { get; set; }
    public string? ImageUrl { get; set; }

    public  string ItemName { get; set; }

    public  decimal Price { get; set; }

    public  decimal Rating { get; set; }

    public  int CategoryId { get; set; }

    public  bool Status { get; set; }

    public  string Description { get; set; }

    public  string Keywords {  get; set; }

    public virtual Category? Category { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

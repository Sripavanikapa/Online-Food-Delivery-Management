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

    public required int RestaurantId { get; set; }

    public required string ItemName { get; set; }

    public required decimal Price { get; set; }

    public required decimal Rating { get; set; }

    public required int CategoryId { get; set; }

    public required bool Status { get; set; }

    public required string Description { get; set; }

    public required string Keywords {  get; set; }

    public virtual Category? Category { get; set; }

    public virtual Restaurant? Restaurant { get; set; }


}

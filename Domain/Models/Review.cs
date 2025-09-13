using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models;

public partial class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    public required int UserId { get; set; }

    public required int RestaurantId { get; set; }

    public required int Rating { get; set; }

    public required string Comment { get; set; }

    public  virtual Restaurant? Restaurant { get; set; }

    public  virtual User? User { get; set; }
}

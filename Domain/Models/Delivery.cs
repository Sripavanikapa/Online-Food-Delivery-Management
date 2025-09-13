using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models;

public partial class Delivery
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeliveryId { get; set; }

    public required int OrderId { get; set; }

    public required int AgentId { get; set; }

    public required bool Status { get; set; }

    public virtual DeliveryAgent? Agent { get; set; }

    public virtual Order? Order { get; set; }
}

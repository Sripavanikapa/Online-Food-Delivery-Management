using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models;

public partial class DeliveryAgent
{
    [Key, ForeignKey("User")]
    public int AgentId { get; set; }

    public required bool Status { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}

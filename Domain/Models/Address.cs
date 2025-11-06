using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public partial class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AddressId { get; set; }

    public required int CustId { get; set; }

    public required string Address1 { get; set; }
   

    public  virtual User? Cust { get; set; }
}

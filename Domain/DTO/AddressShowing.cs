using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddressShowing
    {
        public int AddressId { get; set; }
        public required string Address { get; set; }
    }
}

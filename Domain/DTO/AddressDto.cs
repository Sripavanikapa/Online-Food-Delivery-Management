using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddressDto
    {

        public required int CustId { get; set; }

        public required string Address1 { get; set; }

    }
}

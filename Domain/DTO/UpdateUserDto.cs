using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UpdateUserDto
    {
        public string Name { get; set; }

        public string Password { get; set; }
        public string Phoneno { get; set; }
    }
}

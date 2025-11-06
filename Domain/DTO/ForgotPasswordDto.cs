using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ForgotPasswordDto
    {
        public string Phoneno { get; set; }
        public string NewPassword { get; set; }
    }
}

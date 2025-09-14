using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string PhnoPasswordCredential { get; set; }
        public int ExpiresIn { get; set; }
        public string Role { get; set; }           
        public int Id { get; set; }

        public string Phoneno { get; set; }
    }
}

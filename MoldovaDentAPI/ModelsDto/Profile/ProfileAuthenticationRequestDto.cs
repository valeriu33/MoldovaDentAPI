using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.ModelsDto
{
    public class ProfileAuthenticationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

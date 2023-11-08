using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD.Jwt
{
    public class RefreshTokenRequest
    {
//public int ID { get; set; }
        public string PhoneNumber { get; set; } 
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        
       
    }

}

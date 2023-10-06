using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    /*public class JwtConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }*/
    /* public class checkjwtmod
     {
         int isActive { get; set; }
         string ChucNang { get; set; }

         bool Xem { get; set; }

         bool Them { get; set; }
         bool Sua { get; set; }
         bool Xoa { get; set; }
     }*/
    public class jwtmod
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int ID { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TimeOut { get; set; }

        public List<string> ChucNangVaQuyen { get; set; }
        public class ChucNangQuyen
        {
            public string TenChucNang { get; set; }
            public List<string> Quyen { get; set; }
        }
    }
}

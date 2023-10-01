using System;
using System.Collections.Generic;
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
    public class ChucNangQuyen
    {
        public string TenChucNang { get; set; }
        public List<string> Quyen { get; set; }
    }
}

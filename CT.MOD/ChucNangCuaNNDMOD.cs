using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class ChucNangCuaNNDMOD
    {
        public int idChucNangCuaNND { get; set; }
        public int ChucNang { get; set; }
        public int NNDID { get; set; }
        public bool Xem { get; set; }
        public bool Them { get; set; }
        public bool Sua { get; set; }

        public bool Xoa { get; set; }

    }
    public class ChucNangCuaNNDMOD2
    {
        public int idChucNangCuaNND { get; set; }
        public int NNDID { get; set; }
        public string TenNND { get; set; }
        public int ChungNangid { get; set; }
        public string TenChucNang { get; set; }
       
        public bool Xem { get; set; }
        public bool Them { get; set; }
        public bool Sua { get; set; }

        public bool Xoa { get; set; }

    }
    public class ThemChucNangCuaNNDMOD
    {
     
        public int ChucNang { get; set; }
        public int NNDID { get; set; }
        public bool Xem { get; set; }
        public bool Them { get; set; }
        public bool Sua { get; set; }

        public bool Xoa { get; set; }

    }
}

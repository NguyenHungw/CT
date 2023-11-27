using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class DanhSachNhaCungCapMOD
    {
        public int id_NhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public DateTime NgayHopTac { get; set; }
    }
    public class ThemMoiNhaCC
    {
        public string TenNhaCungCap { get; set; }
        public DateTime NgayHopTac { get; set; }
    }
}

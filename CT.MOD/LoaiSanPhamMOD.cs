using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class LoaiSanPhamMOD
    {
        public int ID_LoaiSanPham { get; set; }
        public string TenLoaiSP { get; set; }
        public string? MoTaLoaiSP { get; set; }
        public bool? TrangThai { get; set; }
    }
    public class ThemMoiLoaiSP
    {
        public string TenLoaiSP { get; set; }
        public string? MoTaLoaiSP { get; set; }
        public bool? TrangThai { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class idPhieuNhapMOD
    {
        public int ID_PhieuNhap { get; set; }
    }
    public class PhieuNhapMOD
    {
        public int ID_PhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NguoiNhapHang { get; set; }
        public int ID_DonVi { get; set; }
    }
    public class DanhSachPhieuNhapMOD
    {
        public int ID_PhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NguoiNhapHang { get; set; }
        public string TenDonVi { get; set; }
    }

    public class ThemMoiPhieuNhapMOD
    {
     
        public DateTime NgayNhap { get; set; }
        public string NguoiNhapHang { get; set; }
        public int ID_DonVi { get; set; }
    }
}

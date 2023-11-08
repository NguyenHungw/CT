using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class ChiTietNhapMOD
    {
        public int ID_ChiTietNhap { get; set; }
        public int ID_PhieuNhap { get; set; }
        public string MSanPham { get; set; }
        public int SoLuong { get; set; }
        public Decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public decimal GiaBan { get; set; }

    }
    public class ThemChiTietNhap
    {
        public int ID_PhieuNhap { get; set; }
        public string MSanPham { get; set; }
        public int SoLuong { get; set; }
        public Decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public decimal GiaBan { get; set; }

    }
    public class ThemChiTietNhap2
    {
        public int ID_PhieuNhap { get; set; }
        public string MSanPham { get; set; }
        public int SoLuong { get; set; }
        public Decimal DonGia { get; set; }
        public decimal GiaBan { get; set; }


    }
    public class SuaChiTietNhapMOD
    {
        public int ID_ChiTietNhap { get; set; }
        public int ID_PhieuNhap { get; set; }
        public string MSanPham { get; set; }
        public int SoLuong { get; set; }
        public Decimal DonGia { get; set; }
        public decimal GiaBan { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class DanhGiaSanPhamMOD
    {
        public int id { get; set; }
        public string MSanPham { get; set; }
        public int idUser { get; set; }
        public int DiemDanhGia { get; set; }
        public string NhanXet { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }
    public class ThemMoiDanhGiaSanPhamMOD
    {
        public string MSanPham { get; set; }
        public int idUser { get; set; }
        public int DiemDanhGia { get; set; }
        public string NhanXet { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }
    public class SuaDanhGiaSanPhamMOD
    {
        public int id { get; set; }
        public string MSanPham { get; set; }
        public int idUser { get; set; }
        public int DiemDanhGia { get; set; }
        public string NhanXet { get; set; }
    }
    public class ChiTietDGSanPhamMOD
    {
        public int id { get; set; }
        public string MSanPham { get; set; }
     
        public int idUser { get; set; }
        public string AvatarUser { get; set; }
        public string Username { get; set; }
        public int DiemDanhGia { get; set; }
        public string NhanXet { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }
    public class DiemDanhGiaTB
    {
        public Decimal Diem { get; set; }
    }
}

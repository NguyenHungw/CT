using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD.TrangChuMOD
{
    public class TrangChu_DSSPMOD
    {
        public int id { get; set; }
        public string MaSanPham { get; set; }
        public string Picture { get; set; }
        public string TenSanPham { get; set; }
        public string LoaiSanPham { get; set; }
        
        public int? DiemDanhGia { get; set; }
        public Decimal? Giaban { get; set; }
    }
   public class TrangChu_CTSPMOD
    {
        public int id { get; set; }
        public string MaSanPham { get; set; }
        public string Picture { get; set; }
        public string TenSanPham { get; set; }
        public int? idUser { get; set; }
        public string? Username { get; set; }
        public int? DiemDanhGia { get; set; }
        public string? NhanXet { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public Decimal? GiaBan { get; set; }
    }
}

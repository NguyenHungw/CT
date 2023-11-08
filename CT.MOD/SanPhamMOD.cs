using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class DanhSachModel
    {
   
    
        public string MSanPham { get; set; }
        public string Picture { get; set; }

        public string TenSP { get; set; }
        public int LoaiSanPham { get; set; }


    }
    public class SanPhamMOD
    {
        public string MSanPham { get; set; }
        public string TenSP { get; set; }
        public int LoaiSanPham { get; set; }

        public IFormFile file;

    }
    public class SanPhamMOD2 : SanPhamMOD
    {
        public string Picture { set; get; }
    }
    public class DanhSachSP
    {
        public string MSanPham { get; set; }
        public string Picture {get; set; }

        public string TenSP { get; set; }
        public string TenLoaiSP { get; set; }

      

    }

    public class TimSp
    {
        public string MSanPham { get; set; }
        public string Picture { get; set; }

        public string TenSP { get; set; }
        public string LoaiSanPham { get; set; }

        public int SoLuong { get; set; }
        public float DonGia { get; set; }

    }
    public class ChiTietSP
    {
        public int id { get; set; }
        public string MSanPham { get; set; }

        public string Picture { get; set; }

        public string TenSP { get; set; }
        public string LoaiSanPham { get; set; }

        public int SoLuong { get; set; }
        public float DonGia { get; set; }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD.ThongKeVaLichSuMOD
{
    public class LichSuDonHangUserMOD
    {
        public int ID_DonHang { get; set; }
        public string OrderID { get; set; }
        public string MsanPham { get; set; }
        public string TenSanPham { get; set; } 
        public string Picture { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public DateTime NgayMua { get; set; }
        public string Status { get; set; }

    }
}

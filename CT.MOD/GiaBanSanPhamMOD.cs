using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class GiaBanSanPhamMOD
    {
        public int ID_GiaBan { get; set; }
        public string MSanPham { get; set; }
        public DateTime NgayBatDau { get; set; }
        public Decimal GiaBan { get; set; }
    }
    public class ThemGiaBanSanPham
    {
        public string MSanPham { get; set; }
        public DateTime NgayBatDau { get; set; }
        public Decimal GiaBan { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class DanhSachGioHang
    {
        public int ID_GioHang { get; set; }
        public int idUser { get; set; }
        public string MSanPham { get; set; }
    }
    public class ThemSP_Gio
    {
        public int idUser { get; set; }
        public string MSanPham { get; set; }

    }
}

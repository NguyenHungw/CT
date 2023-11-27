using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class DonViMOD
    {
        public int ID_DonVi { get; set; }
        public string TenDonVi { get; set; }
        public string GhiChu { get; set; }
    }
    public class ThemMoiDonVi
    {
        public string TenDonVi { get; set; }
        public string? GhiChu { get; set; }
    }
}

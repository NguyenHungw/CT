using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuMuaHang_UserController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachLichSuDonHang")]
        public IActionResult DanhSachLichSuDonHang(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new LichSuMuaHangBUS().DanhSachLSMuaHang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("ChiTietLichSuDonHang")]
        public IActionResult ChiTietLichSuDonHang( int page ,string iduser)
        {
            if (iduser == null) return BadRequest();
            else
            {
                var Result = new LichSuMuaHangBUS().ChiTLSDonhang(page, iduser);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
       
    }
}


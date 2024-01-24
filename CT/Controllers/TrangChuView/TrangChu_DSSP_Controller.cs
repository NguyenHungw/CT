using CT.BUS;
using CT.DAL;
using CT.MOD;
using CT.ULT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrangChuDSSP : ControllerBase
    {
  
        
        [HttpGet]
        [Route("DanhSachSP")]
        [AllowAnonymous]
        public IActionResult DanhSachSP(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new TrangChu_DSSPBUS().DanhSachSP(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("DanhSachSP-2-")]
        public IActionResult DanhSachSPKP(int page)
        {
            var result = new SanPhamBUS().DanhSachSPKP(page);

            if (result.Status == 1)
            {
                return Ok(result.Data); // Trả về dữ liệu nếu thành công
            }
            else if (result.Status == 0)
            {
                return NotFound(result.Message); // Trả về thông báo lỗi nếu không tìm thấy dữ liệu
            }
            else
            {
                return BadRequest(result.Message); // Trả về thông báo lỗi nếu có lỗi xảy ra
            }
        }


        [HttpPost]
        [Route("TimKiemSP {name}")]
        [AllowAnonymous]
        public IActionResult TimKiemSP(string name)
        {
            if (name == null || name == " ") return BadRequest();
            var Result = new SanPhamBUS().TimKiemByName(name);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("ChiTietSP {msp}")]
        [AllowAnonymous]
        public IActionResult ChiTietSP(string msp)
        {
            if (msp == null || msp == " ") return BadRequest();
            var Result = new TrangChu_DSSPBUS().ChiTSP(msp);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("PhanLoaiSP")]
        [AllowAnonymous]
        public IActionResult PhanLoaiSP(string loaisp, int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new SanPhamBUS().PhanLoaiSP(loaisp, page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}

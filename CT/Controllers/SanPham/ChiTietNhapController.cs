using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietPhieuNhapController : ControllerBase
    {
        [HttpGet]
        [Route("ChiTietNhap")]
        public IActionResult DanhSachChiTietPhieuNHap(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().DanhSachChiTietNhap(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaChiTietNhap")]
        public IActionResult SuaChiTietNhap([FromBody] SuaChiTietNhapMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().SuaChiTietNhap(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemChiTietNhap")]
        public IActionResult ThemDonVi([FromBody] ThemChiTietNhap2 item)
        {
            if (item == null) return BadRequest();
            var Result = new ChiTietNhapBUS().ThemChiTietNhap(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaChiTietNhap")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null) return BadRequest();
            var Result = new ChiTietNhapBUS().XoaChiTietNhap(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}


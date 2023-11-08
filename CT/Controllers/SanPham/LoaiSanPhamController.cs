using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachLoaiSP")]
        public IActionResult DanhSachSP(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new LoaiSanPhamBUS().DanhSachLoaiSP(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaLoaiSP")]
        public IActionResult SuaLoaiSP([FromBody] LoaiSanPhamMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new LoaiSanPhamBUS().SuaLoaiSP(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemLoaiSP")]
        public IActionResult ThemLoaiSP([FromBody] ThemMoiLoaiSP item)
        {
            if (item == null) return BadRequest();
            var Result = new LoaiSanPhamBUS().ThemLoaiSP(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaLoaiSP")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null ) return BadRequest();
            var Result = new LoaiSanPhamBUS().XoaLSP(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}

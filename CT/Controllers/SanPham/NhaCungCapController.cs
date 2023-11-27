using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNCC")]
        public IActionResult DanhSachDonVi(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new NhaCungCapBUS().DanhSachNCC(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaNCC")]
        public IActionResult SuaLoaiSP([FromBody] DanhSachNhaCungCapMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new NhaCungCapBUS().SuaDonVi(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemNCC")]
        public IActionResult ThemDonVi([FromBody] ThemMoiNhaCC item)
        {
            if (item == null) return BadRequest();
            var Result = new NhaCungCapBUS().ThemDonVi(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaNCC")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null ) return BadRequest();
            var Result = new NhaCungCapBUS().XoaDonVi(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}

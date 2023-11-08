using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonViController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachDonvi")]
        public IActionResult DanhSachDonVi(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new DonViBUS().DanhSachDonVi(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaDonvi")]
        public IActionResult SuaLoaiSP([FromBody] DonViMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new DonViBUS().SuaDonVi(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemDonvi")]
        public IActionResult ThemDonVi([FromBody] ThemMoiDonVi item)
        {
            if (item == null) return BadRequest();
            var Result = new DonViBUS().ThemDonVi(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaDonvi")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null ) return BadRequest();
            var Result = new DonViBUS().XoaDonVi(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}

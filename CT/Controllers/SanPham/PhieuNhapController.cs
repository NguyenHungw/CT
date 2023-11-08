using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuNhapController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachPhieuNhap")]
        public IActionResult DanhSachPhieuNhap(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new PhieuNhapBUS().DanhSachPhieuNhap(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaPhieuNhap")]
        public IActionResult SuaPhieuNhap([FromBody] PhieuNhapMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new PhieuNhapBUS().SuaPhieuNhap(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
           [Route("ThemPhieuNhap")]
           public IActionResult ThemDonVi([FromBody] ThemMoiPhieuNhapMOD item)
           {
               if (item == null) return BadRequest();
               var Result = new PhieuNhapBUS().ThemMoiPhieuNhap(item);
               if (Result != null) return Ok(Result);
               else return NotFound();
           }

        [HttpDelete]
        [Route("XoaPhieuNhap")]
        public IActionResult XoaPhieuNhap([FromBody] int id)
        {
            if (id == null) return BadRequest();
            var Result = new PhieuNhapBUS().XoaPhieuNhap(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
    }


using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaBanSanPhamController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachGiaBanSP")]
        public IActionResult DanhSachDonVi(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new GiaBanSanPhamBUS().DanhSachGiaBan(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaGiaBan")]
        public IActionResult SuaLoaiSP([FromBody] ThemGiaBanSanPham item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new GiaBanSanPhamBUS().SuaDonGiaBan(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemGiaBan")]
        public IActionResult ThemGiaBan([FromBody] ThemGiaBanSanPham item)
        {
            if (item == null) return BadRequest();
            var Result = new GiaBanSanPhamBUS().ThemGiaBan(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaGiaBan")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null ) return BadRequest();
            var Result = new GiaBanSanPhamBUS().XoaGiaBan(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
    }
}

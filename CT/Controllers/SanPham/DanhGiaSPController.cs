using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaSanPhamController : ControllerBase
    {
        [HttpGet]
        [Route("DanhGiaSP")]
        public IActionResult DanhSachDonVi(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new DanhGiaSanPhamBUS().DanhSachDGSP(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaDanhGiaSP")]
        public IActionResult SuaLoaiSP([FromBody] SuaDanhGiaSanPhamMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var Result = new DanhGiaSanPhamBUS().SuaDanhGia(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpPost]
        [Route("ThemDanhGiaSP")]
        public IActionResult ThemDonVi([FromBody] ThemMoiDanhGiaSanPhamMOD item)
        {
            if (item == null) return BadRequest();
            var Result = new DanhGiaSanPhamBUS().ThemDanhGia(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaDanhGiaSP")]
        public IActionResult XoaLoaiSP([FromBody] int id)
        {
            if (id == null) return BadRequest();
            var Result = new DanhGiaSanPhamBUS().XoaDonVi(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpGet]
        [Route("ChiTietDanhSachDGSP")]
        public IActionResult DanhSachDGSP(int page,string msp)
        {
            if(page <0) return BadRequest();
            else
            {
                var Result = new DanhGiaSanPhamBUS().ChiTietDSDGSP(page,msp);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
           

        }
    }
}

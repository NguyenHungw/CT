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
        [Route("DanhSachChiTietNhap")]
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
        public IActionResult ThemDonVi([FromBody] ThemChiTietNhap item)
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

        [HttpGet]
        [Route("DanhSachKho")]
        public IActionResult DanhSachKho(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().DanhSachKho(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("DanhSachKhoSapHetHang")]
        public IActionResult DanhSachKhoSapHetHang(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().DanhSachKhoSapHetHang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("DanhSachKhoDaHetHang")]
        public IActionResult DanhSachKhoDaHetHang(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().DanhSachKhoDaHetHang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("DanhSachPhieuNhapKho")]
        public IActionResult DanhSachPhieuNhapKho(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietNhapBUS().DanhSachPhieuNhapKho(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}


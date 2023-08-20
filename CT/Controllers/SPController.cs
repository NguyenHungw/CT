using CT.BUS;
using CT.DAL;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SPController : ControllerBase
    {
        [HttpPost]
        [Route("ThemSP")]
        public IActionResult ThemSP(IFormFile file, [FromForm] SanPhamMOD item)
        {
            if (item == null) return BadRequest();
            if (file.Length > 0)
            {
                var Result = new SanPhamBUS().ThemSP(item, file);
                if(Result != null) return Ok(Result);
                else return NotFound();

            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut]
        [Route("SuaSP")]
        public IActionResult SuaSP(IFormFile file, [FromForm] SanPhamMOD item)
        {
            if(item == null) return BadRequest();
            if (file.Length > 0)
            {
                var Result = new SanPhamBUS().SuaSP(item, file);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete]
        [Route("XoaSP")]
        public IActionResult XoaSP(string msp)
        {
            if(msp == null || msp =="") return BadRequest();
            var Result = new SanPhamBUS().XoaSP(msp);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpDelete]
        [Route("XoaALLSP")]
        public IActionResult XoaAllSP()
        {
         
            var Result = new SanPhamBUS().XoaAllSP();
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpGet]
        [Route("DanhSachSP")]
        public IActionResult DanhSachSP (int page)
        {
            if(page<1) return BadRequest();
            else
            {
                var Result = new SanPhamBUS().DanhSachSP(page);
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
                return NotFound(result.Messeage); // Trả về thông báo lỗi nếu không tìm thấy dữ liệu
            }
            else
            {
                return BadRequest(result.Messeage); // Trả về thông báo lỗi nếu có lỗi xảy ra
            }
        }

        [HttpPost]
        [Route("TimKiemSPModal")]
        public IActionResult TimKiemSPModal(string name)
        {
            if (name == null || name == " ") return BadRequest();
            var Result = new SanPhamDAL().TBNM(name);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("TimKiemSP")]
        public IActionResult TimKiemSP(string name)
        {
            if (name == null || name == " ") return BadRequest();
            var Result = new SanPhamBUS().TimKiemByName(name);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPost]
        [Route("PhanLoaiSP")]
        public IActionResult PhanLoaiSP(string loaisp, int page)
        {
            if(page<1) return BadRequest();
            else
            {
                var Result = new SanPhamBUS().PhanLoaiSP(loaisp, page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}
